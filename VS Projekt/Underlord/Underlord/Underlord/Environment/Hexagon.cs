using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Underlord.Entity;
using Underlord.Basic;

namespace Underlord.Environment
{
    class Hexagon
    {
        Vector3 position;
        Vector2 indexNumber;
        Vector2[] neighbors = new Vector2[6]; //[up,right-up,right-down,down,left-down,left-up]
        List<Imp> imps;
        Thing obj;
        int roomNumber;
        bool building;
        bool nest;
        bool visited; //for breadth-first search
        Vector2 parent; //for breadth-first search
        Color drawColor;
        Logic.Vars_Func.HexTyp typ;
        Logic.Vars_Func.GrowObject growObject;
        bool targetFlag;           
       
        private Matrix[] boneTransforms;

        FireModel[] fireModels;
        bool isEnlightend, isEntrance, isHQ, isStartLight;
        float lightPower;
        Vector3 currentDrawPosition;
        Random randomValue;
        FireBallModel fireBall;

        #region Properties
        public bool TargetFlag
        {
            get { return targetFlag; }
            set { targetFlag = value; }
        }
        public Logic.Vars_Func.GrowObject GrowObject
        {
            get { return growObject; }
            set { growObject = value; }
        }
        public Color Color
        {
            get { return drawColor; }
            set { drawColor = value; }
        }
        public Thing Obj
        {
            get { return obj; }
            set { obj = value; }
        }
        public int RoomNumber
        {
            get { return roomNumber; }
            set { roomNumber = value; }
        }
        public bool Visited
        {
            get { return visited; }
            set { visited = value; }
        }
        public Vector2 Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        public bool Building
        {
            get { return building; }
            set { building = value; }
        }
        public bool Nest
        {
            get { return nest; }
            set { nest = value; }
        }
        public Logic.Vars_Func.HexTyp Typ
        {
            get { return typ; }
            set { typ = value; }
        }
        public List<Imp> Imps
        {
            get { return imps; }
        }
        public Vector2[] Neighbors
        {
            get { return neighbors; }
        }
        public Vector2 IndexNumber
        {
            get { return indexNumber; }
        }
        public bool IsEnlightend
        {
            set { isEnlightend = value; }
        }
        public bool IsEntrance
        {
            set { isEntrance = value; }
        }
        public bool IsHQ
        {
            set { isHQ = value; }
        }
        public float LightPower
        {
            set { lightPower = value; }
        }
        public bool IsStartLight
        {
            get { return isStartLight; }
        }
        public FireBallModel Fireball
        {
            get { return fireBall; }
            set { fireBall = value; }
        }
        #endregion

        public Vector3 get3DPosition() { return position; }

        public Vector3 getDrawPosition() { return currentDrawPosition; }

        #region Constructor
        public Hexagon(Vector3 position, Vector2 indexNumber, Vector2[] neighbors, Logic.Vars_Func.HexTyp typ)
        {
            imps = new List<Imp>();
            this.position = position;
            this.indexNumber = indexNumber;
            this.neighbors = neighbors;
            this.typ = typ;
            drawColor = Color.White;
            building = false;
            roomNumber = 0;
            parent = indexNumber;
            boneTransforms = new Matrix[Logic.Vars_Func.getHexagonModell(typ).Model.Bones.Count];
            currentDrawPosition = Vector3.Zero;
            isEnlightend = false;
            isStartLight = false;
            isEntrance = false;
            isHQ = false;
            lightPower = 0;
            fireModels = new FireModel[3];
            randomValue = new Random();
            growObject = Logic.Vars_Func.GrowObject.length;
            targetFlag = false;
        }
        #endregion

        #region Lighting
        public void EnlightendHexagon(Map map)
        {
            for (int index = 0; index < fireModels.Length; ++index)
            {
                float randomBonus = randomValue.Next(0, 100);
                fireModels[index] = new FireModel(Logic.Vars_Func.getTorchFireModel(), 0.01f + randomBonus / 3000);
                map.Fires.Add(fireModels[index]);
            }

            for (int i = 0; i < 6; ++i)
            {
                Vector2 neighbor = neighbors[i];
                map.getHexagonAt(neighbor).isEnlightend = true;
                map.getHexagonAt(neighbor).lightPower += 0.1f;

                for (int j = 0; j < 6; ++j)
                {
                    Vector2 secondDegreeNeighbor = map.getHexagonAt(neighbor).Neighbors[j];
                    map.getHexagonAt(secondDegreeNeighbor).isEnlightend = true;
                    map.getHexagonAt(secondDegreeNeighbor).lightPower += 0.07f;
                    for (int k = 0; k < 6; ++k)
                    {
                        Vector2 thirdDegreeNeighbor = map.getHexagonAt(secondDegreeNeighbor).Neighbors[k];
                        map.getHexagonAt(thirdDegreeNeighbor).isEnlightend = true;
                        map.getHexagonAt(thirdDegreeNeighbor).lightPower += 0.05f;
                        if (map.getHexagonAt(thirdDegreeNeighbor).lightPower > 0.7f)
                        {
                            map.getHexagonAt(thirdDegreeNeighbor).lightPower = 0.7f;
                        }

                    }
                    if (map.getHexagonAt(secondDegreeNeighbor).lightPower > 0.7f)
                    {
                        map.getHexagonAt(secondDegreeNeighbor).lightPower = 0.7f;
                    }
                }
                if (map.getHexagonAt(neighbor).lightPower > 0.7f)
                {
                    map.getHexagonAt(neighbor).lightPower = 0.7f;
                }
            }
            isStartLight = true;
            isEnlightend = true;
            lightPower += 0.3f;
            if (lightPower > 0.9f)
            {
                lightPower = 0.9f;
            }
        }
        #endregion

        #region Drawing
        public void DrawModel(Renderer.Camera camera, Vector3 drawPosition)
        {
            if (currentDrawPosition != drawPosition)
            {
                currentDrawPosition = drawPosition;
            }
            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(1) *
            Matrix.CreateRotationX(0) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition + new Vector3(0.0f, 0.0f, 0));

            Logic.Vars_Func.getHexagonModell(typ).Color = drawColor;
            Logic.Vars_Func.getHexagonModell(typ).Draw(camera, modelMatrix, !(drawColor.Equals(Color.White)), isEnlightend, lightPower);

            if (obj != null) obj.DrawModel(camera, drawPosition, drawColor, isEnlightend, lightPower);
            if (obj == null && imps.Count > 0) imps[0].DrawModel(camera, drawPosition, drawColor, isEnlightend, lightPower);
            //if (obj != null && imps.Count > 0) imps[0].DrawModel(camera, drawPosition, drawColor, isEnlightend, lightPower);

            if (targetFlag)
            {
                Matrix modelMatrix2 = Matrix.Identity *
                Matrix.CreateScale(1) *
                Matrix.CreateRotationX(0) *
                Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(0) *
                Matrix.CreateTranslation(drawPosition);
                Logic.Vars_Func.getTargetFlag().Color = drawColor;
                Logic.Vars_Func.getTargetFlag().Draw(camera, modelMatrix2, false, isEnlightend, lightPower);
            }

            switch (growObject)
            {
                case Logic.Vars_Func.GrowObject.Farm:
                    drawPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + Logic.Vars_Func.getGrowObjectParams(growObject).X);
                    Matrix farmModelMatrix = Matrix.Identity *
                    Matrix.CreateScale(Logic.Vars_Func.getGrowObjectParams(growObject).Y) *
                    Matrix.CreateRotationX(Logic.Vars_Func.getGrowObjectParams(growObject).Z) *
                    Matrix.CreateRotationY(0) *
                    Matrix.CreateRotationZ(0) *
                    Matrix.CreateTranslation(drawPosition);
                    Logic.Vars_Func.getGrowModel(growObject).Color = drawColor;
                    Logic.Vars_Func.getGrowModel(growObject).Draw(camera, farmModelMatrix, false, isEnlightend, lightPower/2);
                    break;
                case Logic.Vars_Func.GrowObject.Temple:
                    //drawPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + Logic.Vars_Func.getGrowObjectParams(growObject).X);
                    //Matrix templeModelMatrix = Matrix.Identity *
                    //Matrix.CreateScale(Logic.Vars_Func.getGrowObjectParams(growObject).Y) *
                    //Matrix.CreateRotationX(Logic.Vars_Func.getGrowObjectParams(growObject).Z) *
                    //Matrix.CreateRotationY(0) *
                    //Matrix.CreateRotationZ(0) *
                    //Matrix.CreateTranslation(drawPosition);
                    //Logic.Vars_Func.getGrowModel(growObject).Color = drawColor;
                    //Logic.Vars_Func.getGrowModel(growObject).Draw(camera, templeModelMatrix, false, isEnlightend, lightPower/2);
                    break;
                case Logic.Vars_Func.GrowObject.Graveyard:
                    drawPosition = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + Logic.Vars_Func.getGrowObjectParams(growObject).X);
                    Matrix graveyardModelMatrix = Matrix.Identity *
                    Matrix.CreateScale(Logic.Vars_Func.getGrowObjectParams(growObject).Y) *
                    Matrix.CreateRotationX(Logic.Vars_Func.getGrowObjectParams(growObject).Z) *
                    Matrix.CreateRotationY(0) *
                    Matrix.CreateRotationZ(0) *
                    Matrix.CreateTranslation(drawPosition);
                    Logic.Vars_Func.getGrowModel(growObject).Color = drawColor;
                    Logic.Vars_Func.getGrowModel(growObject).Draw(camera, graveyardModelMatrix, false, isEnlightend, lightPower/2);
                    break;
                case Logic.Vars_Func.GrowObject.length:
                    break;
            }

            #region Draw Fireball
            if (fireBall != null)
            {
                // Draw the fireball
                Vector3 fireBallPostion = new Vector3(drawPosition.X, drawPosition.Y, drawPosition.Z + fireBall.Z);
                Matrix fireBallMatrix = Matrix.Identity *
                Matrix.CreateScale(fireBall.Scale) *
                Matrix.CreateRotationX(0) *
                Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(fireBall.RotationZ) *
                Matrix.CreateTranslation(fireBallPostion);
                fireBall.Color = drawColor;
                fireBall.Draw(camera, fireBallMatrix);

                // Draw the all the ball's fire
                foreach (FireBallModel fire in fireBall.Fires)
                {
                    Vector3 firePositon = new Vector3(fireBallPostion.X, fireBallPostion.Y, fireBallPostion.Z + fire.Z);
                    Matrix fireMatrix = Matrix.Identity *
                    Matrix.CreateScale(fire.Scale) *
                    Matrix.CreateRotationX(0) *
                    Matrix.CreateRotationY(0) *
                    Matrix.CreateRotationZ(fire.RotationZ) *
                    Matrix.CreateTranslation(firePositon);
                    fire.Color = drawColor;
                    fire.Draw(camera, fireMatrix);
                }
                if (fireBallPostion.Z < 0)
                {
                    fireBall = null;
                }
            }
            #endregion

            if (!isEntrance && !isHQ)
            {
                foreach (FireModel fire in fireModels)
                {
                    if (fire != null)
                    {
                        Matrix fireModelMatrix = Matrix.Identity *
                        Matrix.CreateScale(1) *
                        Matrix.CreateRotationX(0) *
                        Matrix.CreateRotationY(0) *
                        Matrix.CreateRotationZ(0) *
                        Matrix.CreateTranslation(drawPosition + new Vector3(0.0f, 0.0f, fire.Z));
                        fire.Draw(camera, fireModelMatrix);
                    }
                }
                if (isStartLight)
                {
                    Logic.Vars_Func.getTorchModel().Draw(camera, modelMatrix, !(drawColor.Equals(Color.White)), isEnlightend, lightPower / 2);
                }
            }
            else if (isEntrance)
            {
                //Draw God's Ray
                Matrix rayModelMatrix = Matrix.Identity *
                Matrix.CreateScale(Logic.Vars_Func.getEntranceRayModel().Scale) *
                Matrix.CreateRotationX(0) *
                Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(camera.Rotation + Logic.Vars_Func.getEntranceRayModel().RotationZ) *
                Matrix.CreateTranslation(drawPosition + new Vector3(0.0f, 0.0f, Logic.Vars_Func.getEntranceRayModel().PositionZ));
                Logic.Vars_Func.getEntranceRayModel().Draw(camera, rayModelMatrix);
            }
        }
        #endregion
    }
}