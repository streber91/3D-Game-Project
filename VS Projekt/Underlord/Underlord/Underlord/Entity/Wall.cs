using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Underlord.Logic;

namespace Underlord.Entity
{
    class Wall : Thing
    {
        Vector2 indexPosition;
        Vars_Func.WallTyp typ;
        int hp;
        float initHP;
        int gold;
        // private Matrix[] boneTransforms;

        #region Properties
        public Vars_Func.WallTyp Typ
        {
            set { typ = value; }
            get { return typ; }
        }
        public int HP
        {
            set { hp = value; }
            get { return hp; }
        }
        public int Gold
        {
            set { gold = value; }
            get { return gold; }
        }
        public Texture2D Texture
        {
            set { Logic.Vars_Func.getWallModell(typ).Texture = value; }
        }
        #endregion

        #region Constructor
        public Wall(Vector2 indexPosition, Vars_Func.WallTyp typ, int hp, Environment.Map map)
        {
            thingTyp = Vars_Func.ThingTyp.Wall;
            this.indexPosition = indexPosition;
            this.typ = typ;
            this.hp = hp;
            this.initHP = hp;
            map.getHexagonAt(indexPosition).Obj = this;
            if (typ == Vars_Func.WallTyp.Gold) gold = 100;
            //boneTransforms = new Matrix[Entity.Vars_Func.getWallModell(typ).Model.Bones.Count];
        }
        #endregion

        override public void update(GameTime gameTime, Environment.Map map)
        {

        }

        override public void DrawModel(Renderer.Camera camera, Vector3 drawPosition, Color drawColor, bool isEnlightend, float lightPower)
        {
            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(1) *
            Matrix.CreateRotationX(0/*MathHelper.PiOver2*/) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition);

            Logic.Vars_Func.getWallModell(typ).Color = drawColor;
            Texture2D externalText = null;

            float percent = ((float)hp / initHP);

            if (percent <= 0.25f)
            {
                switch (typ)
                {
                    case Vars_Func.WallTyp.Stone:
                        externalText = Vars_Func.getWall_RockTexture(2);
                        break;
                    case Vars_Func.WallTyp.Gold:
                        externalText = Vars_Func.getWall_GoldTexture(2);
                        break;
                    case Vars_Func.WallTyp.Diamond:
                        externalText = Vars_Func.getWall_DiamondTexture(2);
                        break;
                }
            }
            if (percent <= 0.5f)
            {
                switch (typ)
                {
                    case Vars_Func.WallTyp.Stone:
                        externalText = Vars_Func.getWall_RockTexture(1);
                        break;
                    case Vars_Func.WallTyp.Gold:
                        externalText = Vars_Func.getWall_GoldTexture(1);
                        break;
                    case Vars_Func.WallTyp.Diamond:
                        externalText = Vars_Func.getWall_DiamondTexture(1);
                        break;
                }
            }
            if (percent <= 0.75f)
            {
                switch (typ)
                {
                    case Vars_Func.WallTyp.Stone:
                        externalText = Vars_Func.getWall_RockTexture(0);
                        break;
                    case Vars_Func.WallTyp.Gold:
                        externalText = Vars_Func.getWall_GoldTexture(0);
                        break;
                    case Vars_Func.WallTyp.Diamond:
                        externalText = Vars_Func.getWall_DiamondTexture(0);
                        break;
                }
            }
            Logic.Vars_Func.getWallModell(typ).DrawTexture(camera, modelMatrix, externalText, !(drawColor.Equals(Color.White)), isEnlightend, lightPower);
        }

        //override public void DrawModel(Renderer.Camera camera, Vector3 drawPosition, Color drawColor)
        //{
        //    Matrix modelMatrix = Matrix.Identity *
        //    Matrix.CreateScale(1) *
        //    Matrix.CreateRotationX(0/*MathHelper.PiOver2*/) *
        //    Matrix.CreateRotationY(0) *
        //    Matrix.CreateRotationZ(0) *
        //    Matrix.CreateTranslation(drawPosition);

        //    Logic.Vars_Func.getWallModell(typ).Color = drawColor;
        //    Logic.Vars_Func.getWallModell(typ).Draw(camera, modelMatrix);


        //    //Entity.Vars_Func.getWallModell(typ).Model.Root.Transform = Matrix.Identity *
        //    //Matrix.CreateScale(1) *
        //    //Matrix.CreateRotationX(0/*MathHelper.PiOver2*/) *
        //    //Matrix.CreateRotationY(0) *
        //    //Matrix.CreateRotationZ(0) *
        //    //Matrix.CreateTranslation(drawPosition);
        //    //Entity.Vars_Func.getWallModell(typ).Model.CopyAbsoluteBoneTransformsTo(boneTransforms);

        //    //foreach (ModelMesh mesh in Entity.Vars_Func.getWallModell(typ).Model.Meshes)
        //    //{
        //    //    foreach (BasicEffect basicEffect in mesh.Effects)
        //    //    {
        //    //        basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        //    //        basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        //    //        basicEffect.EnableDefaultLighting();
        //    //        basicEffect.World = boneTransforms[mesh.ParentBone.Index];
        //    //        basicEffect.View = camera.View;
        //    //        basicEffect.Projection = camera.Projection;
        //    //        basicEffect.AmbientLightColor = new Vector3(drawColor.R, drawColor.G, drawColor.B);
        //    //    }
        //    //    mesh.Draw();
        //    //}
        //}
    }
}
