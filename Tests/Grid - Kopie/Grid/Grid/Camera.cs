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
    class Camera
    {
        private Vector3 cameraPosition;
        private float cameraRotation;
        
        Vector3 cameraTarget;
        Vector3 upVector;
        KeyboardState keyboard = Keyboard.GetState();

        float counter;
        int planelength;
        float hexagonsidelength;

        private Vector3 baseCameraReference = new Vector3(0,0,1);
        private bool needViewResync = true;

        private Matrix cachedViewMatrix;

        #region Properties
        public Matrix Projection { get; private set; }

        public Vector3 Position
        {
            get
            {
                return cameraPosition;
            }
            set
            {
                cameraPosition = value;
            }
        }

        public float Rotation
        {
            get
            {
                return cameraRotation;
            }
            set
            {
                cameraRotation = value;
            }
        }

        public Matrix View
        {
            get
            {
                if (needViewResync)
                    cachedViewMatrix = Matrix.CreateLookAt(this.cameraPosition, this.cameraTarget, this.upVector);

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

            Projection = Matrix.CreatePerspectiveFieldOfView( MathHelper.PiOver4, aspectRatio, nearClip, farClip);
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
                    Vector3 positionchange = (getCameraTarget() - getCameraPosition()) + Vector3.UnitZ * getCameraPosition().Z;
                    positionchange.Normalize();
                    Vector3 newPosition = getCameraPosition() + positionchange;
                    Vector3 newTarget = getCameraTarget() + positionchange;
                    setCameraPosition(newPosition);
                    setCameraTarget(newTarget);
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.S))
                {
                    Vector3 positionchange = -((getCameraTarget() - getCameraPosition()) + Vector3.UnitZ * getCameraPosition().Z);
                    positionchange.Normalize();
                    Vector3 newPosition = getCameraPosition() + positionchange;
                    Vector3 newTarget = getCameraTarget() + positionchange;
                    setCameraPosition(newPosition);
                    setCameraTarget(newTarget);
                    counter = 0;         
                }
                else if (keyboard.IsKeyDown(Keys.A))
                {
                    Vector3 positionchange = (getCameraTarget() - getCameraPosition()) + Vector3.UnitZ * getCameraPosition().Z;
                    positionchange.Normalize();
                    positionchange = new Vector3(-positionchange.Y, positionchange.X, positionchange.Z);
                    Vector3 newPosition = getCameraPosition() + positionchange;
                    Vector3 newTarget = getCameraTarget() + positionchange;
                    setCameraPosition(newPosition);
                    setCameraTarget(newTarget);
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.D))
                {
                    Vector3 positionchange = (getCameraTarget() - getCameraPosition()) + Vector3.UnitZ * getCameraPosition().Z;
                    positionchange.Normalize();
                    positionchange = new Vector3(positionchange.Y, -positionchange.X, positionchange.Z);
                    Vector3 newPosition = getCameraPosition() + positionchange;
                    Vector3 newTarget = getCameraTarget() + positionchange;
                    setCameraPosition(newPosition);
                    setCameraTarget(newTarget);
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.E))
                {
                    Vector3 newCameraPosition = Vector3.Transform(getCameraPosition() - getCameraTarget(), Matrix.CreateFromAxisAngle(Vector3.UnitZ, gameTime.ElapsedGameTime.Milliseconds * 0.01f)) + getCameraTarget();
                    setCameraPosition(newCameraPosition);
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.Q))
                {
                    Vector3 newCameraPosition = Vector3.Transform(getCameraPosition() - getCameraTarget(), Matrix.CreateFromAxisAngle(Vector3.UnitZ, -gameTime.ElapsedGameTime.Milliseconds * 0.01f)) + getCameraTarget();
                    setCameraPosition(newCameraPosition);
                    counter = 0;
                }


                if (getCameraTarget().Y >= planelength * 1.75f * hexagonsidelength)
                {
                    setCameraPosition(getCameraPosition() - planelength * 1.75f * hexagonsidelength * Vector3.UnitY);
                    setCameraTarget(getCameraTarget() - planelength * 1.75f * hexagonsidelength * Vector3.UnitY);
                }
                if (getCameraTarget().Y < 0)
                {
                    setCameraPosition(getCameraPosition() + planelength * 1.75f * hexagonsidelength * Vector3.UnitY);
                    setCameraTarget(getCameraTarget() + planelength * 1.75f * hexagonsidelength * Vector3.UnitY);
                }
                if (getCameraTarget().X >= planelength * 1.5f * hexagonsidelength)
                {
                    setCameraPosition(getCameraPosition() - planelength * 1.5f * hexagonsidelength * Vector3.UnitX);
                    setCameraTarget(getCameraTarget() - planelength * 1.5f * hexagonsidelength * Vector3.UnitX);
                }
                if (getCameraTarget().X < 0)
                {
                    setCameraPosition(getCameraPosition() + planelength * 1.5f * hexagonsidelength * Vector3.UnitX);
                    setCameraTarget(getCameraTarget() + planelength * 1.5f * hexagonsidelength * Vector3.UnitX);
                }

            }
        }

        public Vector3 getCameraPosition() { return cameraPosition; }
        public void setCameraPosition(Vector3 position) { cameraPosition = position; }
        public void changeCameraPosition(Vector3 position) { cameraPosition += position; }

        public Vector3 getCameraTarget() { return cameraTarget; }
        public void setCameraTarget(Vector3 target) { cameraTarget = target; }
        public void changeCameraTarget(Vector3 target) { cameraTarget += target; }

        public Vector3 getUpVector() { return upVector; }
        public void setUpVector(Vector3 vector) { upVector = vector; }
    }
}
