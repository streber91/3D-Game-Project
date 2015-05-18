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
    class Hexagon
    {
        VertexPositionColor[] vertices = new VertexPositionColor[6];
        Vector3 position;
        int indexNumber;

        public Hexagon(Vector3 position, float sidelength, Color color, int indexNumber)
        {
            /*vertices[0] = new VertexPositionColor(position + new Vector3(-sidelength / 2 - sidelength / (float)Math.Sqrt(2), 0.0f, 0.0f), color);
            vertices[1] = new VertexPositionColor(position + new Vector3(-sidelength / 2, sidelength / (float)Math.Sqrt(2), 0.0f), color);
            vertices[2] = new VertexPositionColor(position + new Vector3(-sidelength / 2, -sidelength / (float)Math.Sqrt(2), 0.0f), color);
            vertices[3] = new VertexPositionColor(position + new Vector3(sidelength / 2, sidelength / (float)Math.Sqrt(2), 0.0f), color);
            vertices[4] = new VertexPositionColor(position + new Vector3(sidelength / 2, -sidelength / (float)Math.Sqrt(2), 0.0f), color);
            vertices[5] = new VertexPositionColor(position + new Vector3(sidelength / 2 + sidelength / (float)Math.Sqrt(2), 0.0f, 0.0f), color);*/

            this.position = position;
            this.indexNumber = indexNumber;
            vertices[0] = new VertexPositionColor(position + new Vector3(-sidelength, 0.0f, 0.0f), color);
            vertices[1] = new VertexPositionColor(position + new Vector3(-sidelength / 2, sidelength * 7 / 8, 0.0f), color);
            vertices[2] = new VertexPositionColor(position + new Vector3(-sidelength / 2, -sidelength * 7 / 8, 0.0f), color);
            vertices[3] = new VertexPositionColor(position + new Vector3(sidelength / 2, sidelength * 7 / 8, 0.0f), color);
            vertices[4] = new VertexPositionColor(position + new Vector3(sidelength / 2, -sidelength * 7 / 8, 0.0f), color);
            vertices[5] = new VertexPositionColor(position + new Vector3(sidelength, 0.0f, 0.0f), color);
        }

        public int getIndexNumber() { return indexNumber; }
        public Vector2 get2DPosition() { return new Vector2(position.X, position.Z); }
        public Vector3 get3DPosition() { return position; }

        public void Draw(GameTime gameTime, GraphicsDevice graphics)
        {
            graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, 4);
        }
    }
}
