using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Underlord.Environment
{
    class Hexagon
    {
        Vector3 position;
        Vector2 indexNumber;
        Vector2[] neighbors = new Vector2[6]; //[up,right-up,right-down,down,left-down,left-up]
        List<Logic.Imp> imps;
        Logic.Thing obj;
        int roomNumber;
        bool building;
        bool nest;
        bool visited; //for breadth-first search
        Vector2 parent; //for breadth-first search
        Color drawColor;
        Logic.Vars_Func.HexTyp typ;
       
        private Matrix[] boneTransforms;

        #region Properties 
        public Color Color
        {
            get { return drawColor; }
            set { drawColor = value; }
        }
        public Logic.Thing Obj
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
        public List<Logic.Imp> Imps
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
        #endregion

        public Vector3 get3DPosition() { return position; }

        #region Constructor
        public Hexagon(Vector3 position, Vector2 indexNumber, Vector2[] neighbors, Logic.Vars_Func.HexTyp typ)
        {
            this.position = position;
            this.indexNumber = indexNumber;
            this.neighbors = neighbors;
            this.typ = typ;
            drawColor = Color.White;
            building = false;
            roomNumber = 0;
            parent = indexNumber;
            boneTransforms = new Matrix[Logic.Vars_Func.getHexagonModell(typ).Model.Bones.Count];
        }
        #endregion

        public void DrawModel(Renderer.Camera camera, Vector3 drawPosition)
        {
            Matrix modelMatrix = Matrix.Identity *
            Matrix.CreateScale(1) *
            Matrix.CreateRotationX(0) *
            Matrix.CreateRotationY(0) *
            Matrix.CreateRotationZ(0) *
            Matrix.CreateTranslation(drawPosition + new Vector3(0.0f, 0.0f, 0));

            Logic.Vars_Func.getHexagonModell(typ).Color = drawColor;
            Logic.Vars_Func.getHexagonModell(typ).Draw(camera, modelMatrix);

            if(obj != null) obj.DrawModel(camera, drawPosition, drawColor);
        }
    }
}