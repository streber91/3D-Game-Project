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
        
        Vector3 position;
        Vector2 indexNumber;
        Vector2[] neighbors = new Vector2[6]; //[up,right-up,right-down,down,left-down,left-up]
        Color standardcolor;
        Color drawcolor;
        float sidelength;

        Model hexagonModel;
        private Matrix[] boneTransforms;

        public Hexagon(Vector3 position, float sidelength, Color color, Vector2 indexNumber, Vector2[] neighbors, Model model)
        {
            this.position = position;
            this.indexNumber = indexNumber;
            this.neighbors = neighbors;
            standardcolor = color;
            drawcolor = color;
            this.sidelength = sidelength;

            this.hexagonModel = model;
            boneTransforms = new Matrix[this.hexagonModel.Bones.Count];
        }

        public Vector2 getIndexNumber() { return indexNumber; }
        public Vector2 get2DPosition() { return new Vector2(position.X, position.Z); }
        public Vector3 get3DPosition() { return position; }
        public Vector2[] getNeighbors() { return neighbors; }
        public Color getStdColor() { return standardcolor; }

        public void Draw(GraphicsDevice graphics, Vector3 drawPosition)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[6];
            vertices[0] = new VertexPositionColor(drawPosition + new Vector3(-sidelength, 0.0f, 0.0f), drawcolor);
            vertices[1] = new VertexPositionColor(drawPosition + new Vector3(-sidelength / 2, sidelength * 7 / 8, 0.0f), drawcolor);
            vertices[2] = new VertexPositionColor(drawPosition + new Vector3(-sidelength / 2, -sidelength * 7 / 8, 0.0f), drawcolor);
            vertices[3] = new VertexPositionColor(drawPosition + new Vector3(sidelength / 2, sidelength * 7 / 8, 0.0f), drawcolor);
            vertices[4] = new VertexPositionColor(drawPosition + new Vector3(sidelength / 2, -sidelength * 7 / 8, 0.0f), drawcolor);
            vertices[5] = new VertexPositionColor(drawPosition + new Vector3(sidelength, 0.0f, 0.0f), drawcolor);
            graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, 4);

        }

        public void DrawModel(Camera camera, Vector3 drawPosition)
        {
                this.hexagonModel.Root.Transform = Matrix.Identity *
           
                Matrix.CreateScale(0.85f) *
                Matrix.CreateRotationX(MathHelper.PiOver2) *
                Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(0) *
                Matrix.CreateTranslation(drawPosition);
                this.hexagonModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in  this.hexagonModel.Meshes)
            {
                foreach (BasicEffect basicEffect in mesh.Effects)
                {
                    basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    basicEffect.EnableDefaultLighting();
                    basicEffect.World = boneTransforms[mesh.ParentBone.Index];
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;

                    basicEffect.AmbientLightColor = new Vector3 ( this.drawcolor.R, this.drawcolor.G, this.drawcolor.B) ;
                }
                mesh.Draw();
            }


        }

        public void setColor(Color color) 
        {
            drawcolor = color;
        }
    }
}
