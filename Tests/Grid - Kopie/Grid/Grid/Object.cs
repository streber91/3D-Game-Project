using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Grid
{
    class Object
    {
        Vector2 position;
        int type;
        Color drawcolor;

        Model objectModel;
        private Matrix[] boneTransforms;

        public Object(Vector2 position, int type, Color color, Model model, Plane plane)
        {
            this.position = position;
            this.type = type;
            drawcolor = color;
            plane.getHexagonAt(position.X, position.Y).setObjekt(this);
            objectModel = model;
            boneTransforms = new Matrix[this.objectModel.Bones.Count];
        }

        public void DrawModel(Camera camera, Vector3 drawPosition)
        {
            this.objectModel.Root.Transform = Matrix.Identity *

            Matrix.CreateScale(0.875f) *
            Matrix.CreateRotationX(MathHelper.PiOver2) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);
            this.objectModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in this.objectModel.Meshes)
            {
                foreach (BasicEffect basicEffect in mesh.Effects)
                {
                    basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    basicEffect.EnableDefaultLighting();
                    basicEffect.World = boneTransforms[mesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;

                    basicEffect.AmbientLightColor = new Vector3(this.drawcolor.R, this.drawcolor.G, this.drawcolor.B);
                }
                mesh.Draw();
            }
        }
    }
}
