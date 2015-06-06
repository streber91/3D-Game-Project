using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Underlord.Renderer
{
    class Camera
    {
        private Vector3 cameraPosition;
        private float cameraRotation;
        Vector3 cameraTarget;
        Vector3 upVector;
        KeyboardState keyboard = Keyboard.GetState();
        float counter, hexagonsidelength;
        int planelength;
        private Vector3 baseCameraReference = new Vector3(0, 0, 1);
        private bool needViewResync = true;
        private Matrix cachedViewMatrix;

        #region Properties
        public Matrix Projection { get; private set; }

        public Vector3 Position
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }

        public Vector3 Target
        {
            get { return cameraTarget; }
            set { cameraTarget = value; }
        }

        public float Rotation
        {
            get { return cameraRotation; }
            set { cameraRotation = value; }
        }

        public Matrix View
        {
            get
            {
                if (needViewResync) cachedViewMatrix = Matrix.CreateLookAt(this.cameraPosition, this.cameraTarget, this.upVector);
                return cachedViewMatrix;
            }
        }
        #endregion

        #region Constructor
        public Camera(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 upVector, float aspectRatio, float nearClip, float farClip, int planelength, float hexagonsidelength)
        {
            this.cameraPosition = cameraPosition;
            this.cameraTarget = cameraTarget;
            this.upVector = upVector;
            this.planelength = planelength;
            this.hexagonsidelength = hexagonsidelength;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, nearClip, farClip);
        }
        #endregion

        public void Update(GameTime gameTime, float timeSinceLastUpdate, MouseState mouseState)
        {
            counter += timeSinceLastUpdate;
            keyboard = Keyboard.GetState();
            if (counter > 100)
            {


                /*if (mouseState.X < 5)
                {
                    System.Diagnostics.Debug.WriteLine("Output");
                }*/

                if (keyboard.IsKeyDown(Keys.W))
                {
                    Vector3 positionchange = (cameraTarget - cameraPosition) + Vector3.UnitZ * cameraPosition.Z;
                    positionchange.Normalize();
                    Vector3 newPosition = cameraPosition + positionchange;
                    Vector3 newTarget = cameraTarget + positionchange;
                    cameraPosition = newPosition;
                    cameraTarget = newTarget;
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.S))
                {
                    Vector3 positionchange = -((cameraTarget - cameraPosition) + Vector3.UnitZ * cameraPosition.Z);
                    positionchange.Normalize();
                    Vector3 newPosition = cameraPosition + positionchange;
                    Vector3 newTarget = cameraTarget + positionchange;
                    cameraPosition = newPosition;
                    cameraTarget = newTarget;
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.A))
                {
                    Vector3 positionchange = (cameraTarget - cameraPosition) + Vector3.UnitZ * cameraPosition.Z;
                    positionchange.Normalize();
                    positionchange = new Vector3(-positionchange.Y, positionchange.X, positionchange.Z);
                    Vector3 newPosition = cameraPosition + positionchange;
                    Vector3 newTarget = cameraTarget + positionchange;
                    cameraPosition = newPosition;
                    cameraTarget = newTarget;
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.D))
                {
                    Vector3 positionchange = (cameraTarget - cameraPosition) + Vector3.UnitZ * cameraPosition.Z;
                    positionchange.Normalize();
                    positionchange = new Vector3(positionchange.Y, -positionchange.X, positionchange.Z);
                    Vector3 newPosition = cameraPosition + positionchange;
                    Vector3 newTarget = cameraTarget + positionchange;
                    cameraPosition = newPosition;
                    cameraTarget = newTarget;
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.E))
                {
                    Vector3 newCameraPosition = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateFromAxisAngle(Vector3.UnitZ, gameTime.ElapsedGameTime.Milliseconds * 0.01f)) + cameraTarget;
                    cameraPosition = newCameraPosition;
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.Q))
                {
                    Vector3 newCameraPosition = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateFromAxisAngle(Vector3.UnitZ, -gameTime.ElapsedGameTime.Milliseconds * 0.01f)) + cameraTarget;
                    cameraPosition = newCameraPosition;
                    counter = 0;
                }


                if (cameraTarget.Y >= planelength * 1.75f * hexagonsidelength)
                {
                    cameraPosition = (cameraPosition - planelength * 1.75f * hexagonsidelength * Vector3.UnitY);
                    cameraTarget = (cameraTarget - planelength * 1.75f * hexagonsidelength * Vector3.UnitY);
                }
                if (cameraTarget.Y < 0)
                {
                    cameraPosition = (cameraPosition + planelength * 1.75f * hexagonsidelength * Vector3.UnitY);
                    cameraTarget = (cameraTarget + planelength * 1.75f * hexagonsidelength * Vector3.UnitY);
                }
                if (cameraTarget.X >= planelength * 1.5f * hexagonsidelength)
                {
                    cameraPosition = (cameraPosition - planelength * 1.5f * hexagonsidelength * Vector3.UnitX);
                    cameraTarget = (cameraTarget - planelength * 1.5f * hexagonsidelength * Vector3.UnitX);
                }
                if (cameraTarget.X < 0)
                {
                    cameraPosition = (cameraPosition + planelength * 1.5f * hexagonsidelength * Vector3.UnitX);
                    cameraTarget = (cameraTarget + planelength * 1.5f * hexagonsidelength * Vector3.UnitX);
                }

            }
        }
    }
}
