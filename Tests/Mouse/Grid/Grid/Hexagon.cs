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
        Color standartColor;
        int indexNumber;

        public Hexagon(Vector3 position, float sidelength, Color color, int indexNumber)
        {
            standartColor = color;
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
        public void setColor(Color color) 
        {
            for (int i = 0; i < 6; ++i)
            {
                vertices[i].Color = color;
            }
        }
        public Color getStColor() { return standartColor; }

        public void Draw(GameTime gameTime, GraphicsDevice graphics)
        {
            graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, 4);
        }
    }
}
