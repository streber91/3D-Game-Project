using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Underlord.Renderer;
using Underlord.Animation;
using Underlord.Logic;
using Underlord.Environment;
using Underlord.Entity;

namespace Underlord.Basic
{
    class FireBallModel : BasicModel
    {
        float speed, zPosition, alpha, scale, rotation;
        float timeCounter;
        float rotationSpeed1, rotationSpeed2, rotationSpeed3;
        float firePosition1, firePosition2, firePosition3;
        bool  burneHasWorke;
        Random randomValue;

        List<FireBallModel> fires = new List<FireBallModel>();
        Map map;
        Vector2 mapPosition;

        #region Properties
        public float Z
        {
            get { return zPosition; }
            set { zPosition = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public float RotationZ
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public List<FireBallModel> Fires
        {
            get { return fires; }
        }

        public void Remove()
        {
            if (map.FireBalls.Contains(this))
            {
                map.FireBalls.Remove(this);
            }
        }
        #endregion

        #region Constructor
        public FireBallModel(Model model)
            : base(model)
        {
            speed = 0.6f;
            zPosition = 0;
            alpha = 1;
            scale = 1;
            timeCounter = 0;
            randomValue = new Random();
        }

        public FireBallModel(Model model, List<FireBallModel> fires, Map map, Vector2 positon) : this(model)
        {
            this.fires = fires;
            this.map = map;
            this.mapPosition = positon;
            burneHasWorke = false;
            this.map.FireBalls.Add(this);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update the model.
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        public void Update(GameTime gameTime)
        {
            timeCounter += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

            zPosition = MathHelper.Lerp(5f, 0f, timeCounter * speed);
            float value = (float)(0.5f * Math.Cos(timeCounter / 1000 * speed + MathHelper.PiOver2)) + 0.5f;
            scale = 1 - (value / 2);
            rotation += (timeCounter / 500) * 10;

            firePosition1 += (timeCounter / 1000) * 8;
            fires[0].Z = firePosition1;
            fires[0].Scale = MathHelper.Lerp(1f, 0f, MathHelper.Clamp(firePosition1 / 10, 0, 1));
            fires[0].RotationZ += (timeCounter / 700) * 10;

            firePosition2 += (timeCounter / 1000) * 6;
            fires[1].Z = firePosition2;
            fires[1].Scale = MathHelper.Lerp(1f, 0f, MathHelper.Clamp(firePosition1 / 12, 0, 1));
            fires[1].RotationZ -= (timeCounter / 900) * 10;

            firePosition3 += (timeCounter / 1000) * 4;
            fires[2].Z = firePosition3;
            fires[2].Scale = MathHelper.Lerp(1f, 0f, MathHelper.Clamp(firePosition1 / 15, 0, 1));
            fires[2].RotationZ += (timeCounter / 1100) * 10;

            if (this.Z < 0.05f && !burneHasWorke)
            {
                burneHasWorke = true;
                this.Burne();
            }            
        }

        private void Burne()
        {
            System.Diagnostics.Debug.WriteLine("Burne");
            // is the target a creature?
            if (map.getHexagonAt(mapPosition).Obj != null && (map.getHexagonAt(mapPosition).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.DungeonCreature ||
                    map.getHexagonAt(mapPosition).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.NeutralCreature || map.getHexagonAt(mapPosition).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.HeroCreature))
            {
                //damage creature
                Creature target = (Creature)map.getHexagonAt(mapPosition).Obj;
                target.takeDamage(100);
                //is creature dead?
                if (target.DamageTaken >= target.HP) map.DyingCreatures.Add(target);
            }
            // effects neighbors
            foreach (Vector2 hex in map.getHexagonAt(mapPosition).Neighbors)
            {
                // is the target a creature?
                if (map.getHexagonAt(hex).Obj != null && (map.getHexagonAt(hex).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.DungeonCreature ||
                    map.getHexagonAt(hex).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.NeutralCreature || map.getHexagonAt(hex).Obj.getThingTyp() == Logic.Vars_Func.ThingTyp.HeroCreature))
                {
                    //damage creature
                    Creature target = (Creature)map.getHexagonAt(hex).Obj;
                    target.takeDamage(60);
                    //is creature dead?
                    if (target.DamageTaken >= target.HP) map.DyingCreatures.Add(target);
                }
            }
        }
        #endregion

        #region Drawing
        /// <summary>
        /// Draw the model
        /// </summary>
        /// <param name="camera">A camera to determine the view</param>
        /// <param name="world">A world matrix to place the model</param>
        public void Draw(Camera camera, Matrix world)
        {
            if (model == null)
                return;
            //
            // Compute all of the bone absolute transforms
            //
            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.Root.Transform = world;
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            // Draw the model.
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (BasicEffect basicEffect in modelMesh.Effects)
                {
                    basicEffect.World = boneTransforms[modelMesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;
                    basicEffect.Alpha = alpha;

                    basicEffect.EmissiveColor = new Vector3(1, 1, 1);
                    basicEffect.SpecularColor = new Vector3(1, 1, 1);
                    basicEffect.SpecularPower = 0.8f;
                    basicEffect.DiffuseColor = new Vector3(0.7f, 0.4f, 0.1f);
                    basicEffect.AmbientLightColor = new Vector3(1, 1, 1);
                    basicEffect.LightingEnabled = true;
                    basicEffect.DirectionalLight0.Enabled = true;
                    basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.4f, 0.1f, 0.2f);
                    basicEffect.DirectionalLight0.Direction = new Vector3(1, 0, 1);
                }
                modelMesh.Draw();
            }

            foreach (FireBallModel f in fires)
            {
                f.Draw(camera, world);
            }
        }
        #endregion
    }
}
