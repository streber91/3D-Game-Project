using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Animation;

namespace Pipeline
{
    [ContentProcessor(DisplayName = "Animation Import Pipeline")]
    public class AnimationImportPipeline : ModelProcessor
    {
        #region Fields

        private ModelContent model;
        private AnimationData animationData = new AnimationData();

        // Dictionary that remembers material changes from BasicEffect to SkinnedEffect 
        private Dictionary<MaterialContent, SkinnedMaterialContent> toSkinnedMaterial = new Dictionary<MaterialContent, SkinnedMaterialContent>();

        #endregion

        #region Process

        // Function to process a model from normal content to a model with animation data 
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            SetSkinnedEffect(input);

            BoneContent skeleton = ImportSkeleton(input);
            
            model = base.Process(input, context);

            AnimationDataImport(model, input, context);

            model.Tag = animationData;

            return model;
        }

        #endregion

        #region Skeleton Import

        // Import the skeleton for Skeleton-Animation
        private BoneContent ImportSkeleton(NodeContent input)
        {
            BoneContent skeleton = MeshHelper.FindSkeleton(input);

            if (skeleton == null)
                return null;

            FlattenTransforms(input, skeleton);
            List<NodeContent> nodes = FlattenHeirarchy(input);
            IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);

            // Create a dictionary to convert a node to an index into the array of nodes
            Dictionary<NodeContent, int> nodeToIndex = new Dictionary<NodeContent, int>();
            for (int i = 0; i < nodes.Count; i++)
            {
                nodeToIndex[nodes[i]] = i;
            }

            foreach (BoneContent bone in bones)
            {
                animationData.Skeleton.Add(nodeToIndex[bone]);
            }

            return skeleton;
        }

        private List<NodeContent> FlattenHeirarchy(NodeContent item)
        {
            List<NodeContent> nodes = new List<NodeContent>();
            nodes.Add(item);
            foreach (NodeContent child in item.Children)
            {
                FlattenHeirarchy(nodes, child);
            }

            return nodes;
        }


        private void FlattenHeirarchy(List<NodeContent> nodes, NodeContent item)
        {
            nodes.Add(item);
            foreach (NodeContent child in item.Children)
            {
                FlattenHeirarchy(nodes, child);
            }
        }

        void FlattenTransforms(NodeContent node, BoneContent skeleton)
        {
            foreach (NodeContent child in node.Children)
            {
                // Don't process the skeleton, because that is special.
                if (child == skeleton)
                    continue;

                // This is important: Don't bake in the transforms except
                // for geometry that is part of a skinned mesh
                if(IsSkinned(child))
                {
                    FlattenAllTransforms(child);
                }
            }
        }

        void FlattenAllTransforms(NodeContent node)
        {
            // Bake the local transform into the actual geometry.
            MeshHelper.TransformScene(node, node.Transform);

            // Having baked it, we can now set the local
            // coordinate system back to identity.
            node.Transform = Matrix.Identity;

            foreach (NodeContent child in node.Children)
            {
                FlattenAllTransforms(child);
            }
        }
        #endregion

        #region Set Skinned Effect

        void SetSkinnedEffect(NodeContent node)
        {
            MeshContent mesh = node as MeshContent;
            if (mesh != null)
            {
                foreach (GeometryContent geometry in mesh.Geometry)
                {
                    bool swap = false;
                    foreach (VertexChannel vchannel in geometry.Vertices.Channels)
                    {
                        if (vchannel is VertexChannel<BoneWeightCollection>)
                        {
                            swap = true;
                            break;
                        }
                    }

                    if (swap)
                    {
                        if (toSkinnedMaterial.ContainsKey(geometry.Material))
                        {
                            geometry.Material = toSkinnedMaterial[geometry.Material];
                        }
                        else
                        {
                            SkinnedMaterialContent smaterial = new SkinnedMaterialContent();
                            BasicMaterialContent bmaterial = geometry.Material as BasicMaterialContent;

                            smaterial.Alpha = bmaterial.Alpha;
                            smaterial.DiffuseColor = bmaterial.DiffuseColor;
                            smaterial.EmissiveColor = bmaterial.EmissiveColor;
                            smaterial.SpecularColor = bmaterial.SpecularColor;
                            smaterial.SpecularPower = bmaterial.SpecularPower;
                            smaterial.Texture = bmaterial.Texture;
                            smaterial.WeightsPerVertex = 4;

                            toSkinnedMaterial[geometry.Material] = smaterial;
                            geometry.Material = smaterial;
                        }
                    }
                }
            }

            foreach (NodeContent child in node.Children)
            {
                SetSkinnedEffect(child);
            }
        }

        bool IsSkinned(NodeContent node)
        {
            MeshContent mesh = node as MeshContent;
            if (mesh != null)
            {
                foreach (GeometryContent geometry in mesh.Geometry)
                {
                    foreach (VertexChannel vchannel in geometry.Vertices.Channels)
                    {
                        if (vchannel is VertexChannel<BoneWeightCollection>)
                            return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Animation Import

        private Dictionary<string, int> bones = new Dictionary<string, int>();
        private Matrix[] boneTransforms;
        private Dictionary<string, Clip> clips = new Dictionary<string, Clip>();

        private void AnimationDataImport(ModelContent model, NodeContent input, ContentProcessorContext context)
        {
            for (int i = 0; i < model.Bones.Count; i++)
            {
                bones[model.Bones[i].Name] = i;
            }

            boneTransforms = new Matrix[model.Bones.Count];
            AnimationDataImportRec(input);

            if (animationData.Clips.Count == 0)
            {
                Clip clip = new Clip();
                animationData.Clips.Add(clip);

                string clipName = "Take 001";

                clips[clipName] = clip;

                clip.Name = clipName;
                foreach (ModelBoneContent bone in model.Bones)
                {
                    Clip.Bone clipBone = new Clip.Bone();
                    clipBone.Name = bone.Name;

                    clip.Bones.Add(clipBone);
                }
            }

            foreach (Clip clip in animationData.Clips)
            {
                for (int b = 0; b < bones.Count; b++)
                {
                    List<Clip.Keyframe> keyframes = clip.Bones[b].Keyframes;
                    if (keyframes.Count == 0 || keyframes[0].Time > 0)
                    {
                        Clip.Keyframe keyframe = new Clip.Keyframe();
                        keyframe.Time = 0;
                        keyframe.Transform = boneTransforms[b];
                        keyframes.Insert(0, keyframe);
                    }
                }
            }
        }

        private void AnimationDataImportRec(NodeContent input)
        {
            int inputBoneIndex;
            if (bones.TryGetValue(input.Name, out inputBoneIndex))
            {
                boneTransforms[inputBoneIndex] = input.Transform;
            }


            foreach (KeyValuePair<string, AnimationContent> animation in input.Animations)
            {
                Clip clip;
                string clipName = animation.Key;

                if (!clips.TryGetValue(clipName, out clip))
                {
                    clip = new Clip();
                    animationData.Clips.Add(clip);

                    clips[clipName] = clip;

                    clip.Name = clipName;
                    foreach (ModelBoneContent bone in model.Bones)
                    {
                        Clip.Bone clipBone = new Clip.Bone();
                        clipBone.Name = bone.Name;

                        clip.Bones.Add(clipBone);
                    }
                }

                if (animation.Value.Duration.TotalSeconds > clip.Duration)
                    clip.Duration = animation.Value.Duration.TotalSeconds;

                foreach (KeyValuePair<string, AnimationChannel> channel in animation.Value.Channels)
                {
                    int boneIndex;
                    if (!bones.TryGetValue(channel.Key, out boneIndex))
                        continue;           // Ignore if not a named bone

                    if (AnimationTest(boneIndex))
                        continue;

                    LinkedList<Clip.Keyframe> keyframes = new LinkedList<Clip.Keyframe>();
                    foreach (AnimationKeyframe keyframe in channel.Value)
                    {
                        Matrix transform = keyframe.Transform;      

                        Clip.Keyframe newKeyframe = new Clip.Keyframe();
                        newKeyframe.Time = keyframe.Time.TotalSeconds;
                        newKeyframe.Transform = transform;

                        keyframes.AddLast(newKeyframe);
                    }

                    LinearKeyframeReduction(keyframes);
                    foreach (Clip.Keyframe keyframe in keyframes)
                    {
                        clip.Bones[boneIndex].Keyframes.Add(keyframe);
                    }
                }
            }

            foreach (NodeContent child in input.Children)
            {
                AnimationDataImportRec(child);
            }
        }

        private const float TinyLength = 1e-8f;
        private const float TinyCosAngle = 0.9999999f;

        private void LinearKeyframeReduction(LinkedList<Clip.Keyframe> keyframes)
        {
            if (keyframes.Count < 3)
                return;

            for (LinkedListNode<Clip.Keyframe> node = keyframes.First.Next; ; )
            {
                LinkedListNode<Clip.Keyframe> next = node.Next;
                if (next == null)
                    break;

                Clip.Keyframe a = node.Previous.Value;
                Clip.Keyframe b = node.Value;
                Clip.Keyframe c = next.Value;

                float t = (float)((node.Value.Time - node.Previous.Value.Time) /
                                   (next.Value.Time - node.Previous.Value.Time));

                Vector3 translation = Vector3.Lerp(a.Translation, c.Translation, t);
                Quaternion rotation = Quaternion.Slerp(a.Rotation, c.Rotation, t);

                if ((translation - b.Translation).LengthSquared() < TinyLength &&
                   Quaternion.Dot(rotation, b.Rotation) > TinyCosAngle)
                {
                    keyframes.Remove(node);
                }

                node = next;
            }
        }

        bool AnimationTest(int boneId)
        {
            foreach (ModelMeshContent mesh in model.Meshes)
            {
                if (mesh.ParentBone.Index == boneId)
                    return false;
            }

            foreach (int b in animationData.Skeleton)
            {
                if (boneId == b)
                    return false;
            }
            return true;
        }

        #endregion
    }
}