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
        Vector3 cameraPosition;
        Vector3 cameraTarget;
        Vector3 upVector;
        KeyboardState keyboard = Keyboard.GetState();
        float counter;

        public Camera(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 upVector)
        {
            this.cameraPosition = cameraPosition;
            this.cameraTarget = cameraTarget;
            this.upVector = upVector;
        }

        public void Update(GameTime gameTime, float timeSinceLastUpdate)
        {
            counter += timeSinceLastUpdate;
            keyboard = Keyboard.GetState();
            if (counter > 100)
            {
                if (keyboard.IsKeyDown(Keys.Up))
                {
                    changeCameraPosition(Vector3.UnitY); changeCameaTarget(Vector3.UnitY); counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.Down))
                {
                    changeCameraPosition(-Vector3.UnitY); changeCameaTarget(-Vector3.UnitY); counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.Left))
                {
                    changeCameraPosition(-Vector3.UnitX); changeCameaTarget(-Vector3.UnitX); counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.Right))
                {
                    changeCameraPosition(Vector3.UnitX); changeCameaTarget(Vector3.UnitX); counter = 0;
                }
            }
        }

        public Vector3 getCameraPosition() { return cameraPosition; }
        public void setCameraPosition(Vector3 position) { cameraPosition = position; }
        public void changeCameraPosition(Vector3 position) { cameraPosition += position; }

        public Vector3 getCameraTarget() { return cameraTarget; }
        public void setCameaTarget(Vector3 target) { cameraTarget = target; }
        public void changeCameaTarget(Vector3 target) { cameraTarget += target; }

        public Vector3 getUpVector() { return upVector; }
        public void setUpVector(Vector3 vector) { upVector = vector; }
    }
}
