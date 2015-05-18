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


        public Hexagon(Vector3 position, float sidelength, Color color)
        {
            /*vertices[0] = new VertexPositionColor(position + new Vector3(-sidelength / 2 - sidelength / (float)Math.Sqrt(2), 0.0f, 0.0f), color);
            vertices[1] = new VertexPositionColor(position + new Vector3(-sidelength / 2, sidelength / (float)Math.Sqrt(2), 0.0f), color);
            vertices[2] = new VertexPositionColor(position + new Vector3(-sidelength / 2, -sidelength / (float)Math.Sqrt(2), 0.0f), color);
            vertices[3] = new VertexPositionColor(position + new Vector3(sidelength / 2, sidelength / (float)Math.Sqrt(2), 0.0f), color);
            vertices[4] = new VertexPositionColor(position + new Vector3(sidelength / 2, -sidelength / (float)Math.Sqrt(2), 0.0f), color);
            vertices[5] = new VertexPositionColor(position + new Vector3(sidelength / 2 + sidelength / (float)Math.Sqrt(2), 0.0f, 0.0f), color);*/

            vertices[0] = new VertexPositionColor(position + new Vector3(-sidelength, 0.0f, 0.0f), Color.Green);
            vertices[1] = new VertexPositionColor(position + new Vector3(-sidelength / 2, sidelength * 7 / 8, 0.0f), Color.Red);
            vertices[2] = new VertexPositionColor(position + new Vector3(-sidelength / 2, -sidelength * 7 / 8, 0.0f), Color.Blue);
            vertices[3] = new VertexPositionColor(position + new Vector3(sidelength / 2, sidelength * 7 / 8, 0.0f), Color.Yellow);
            vertices[4] = new VertexPositionColor(position + new Vector3(sidelength / 2, -sidelength * 7 / 8, 0.0f), Color.Orange);
            vertices[5] = new VertexPositionColor(position + new Vector3(sidelength, 0.0f, 0.0f), Color.Purple);
        }

        public void Draw(GameTime gameTime, GraphicsDevice graphics)
        {
            graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, 4);
        }
    }
}
