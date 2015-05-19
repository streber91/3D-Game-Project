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
        int planelength;
        float hexagonsidelength;

        public Camera(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 upVector, int planelength, float hexagonsidelength)
        {
            this.cameraPosition = cameraPosition;
            this.cameraTarget = cameraTarget;
            this.upVector = upVector;
            this.planelength = planelength;
            this.hexagonsidelength = hexagonsidelength;
        }

        public void Update(GameTime gameTime, float timeSinceLastUpdate)
        {
            counter += timeSinceLastUpdate;
            keyboard = Keyboard.GetState();
            if (counter > 100)
            {
                if (keyboard.IsKeyDown(Keys.Up))
                {
                    //changeCameraPosition(Vector3.UnitY); changeCameraTarget(Vector3.UnitY); counter = 0;
                    float yPosition = (getCameraPosition().Y + 1 + planelength * 1.75f * hexagonsidelength) % (planelength * 1.75f * hexagonsidelength);
                    setCameraPosition(new Vector3(getCameraPosition().X, yPosition, getCameraPosition().Z));
                    setCameraTarget(new Vector3(getCameraTarget().X, yPosition, getCameraTarget().Z));
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.Down))
                {
                    /*changeCameraPosition(-Vector3.UnitY); changeCameraTarget(-Vector3.UnitY); counter = 0;
                    if (getCameraPosition().Y <= 0)
                    {
                        setCameraPosition(new Vector3(getCameraPosition().X, planelength, getCameraPosition().Z));
                        setCameraTarget(new Vector3(getCameraTarget().X, planelength, getCameraTarget().Z));
                    }*/

                    float yPosition = (getCameraPosition().Y - 1 + planelength * 1.75f * hexagonsidelength) % (planelength * 1.75f * hexagonsidelength);
                    setCameraPosition(new Vector3(getCameraPosition().X, yPosition, getCameraPosition().Z));
                    setCameraTarget(new Vector3(getCameraTarget().X, yPosition, getCameraTarget().Z));
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.Left))
                {
                    /*changeCameraPosition(-Vector3.UnitX); changeCameraTarget(-Vector3.UnitX); counter = 0;
                    if (getCameraPosition().X <= 0)
                    {
                        setCameraPosition(new Vector3(planelength, getCameraPosition().Y, getCameraPosition().Z));
                        setCameraTarget(new Vector3(planelength, getCameraTarget().Y, getCameraTarget().Z));
                    }*/

                    float xPosition = (getCameraPosition().X - 1 + planelength * 1.5f * hexagonsidelength) % (planelength * 1.5f * hexagonsidelength);
                    setCameraPosition(new Vector3(xPosition, getCameraTarget().Y, getCameraPosition().Z));
                    setCameraTarget(new Vector3(xPosition, getCameraTarget().Y, getCameraTarget().Z));
                    counter = 0;
                }
                else if (keyboard.IsKeyDown(Keys.Right))
                {
                    /*changeCameraPosition(Vector3.UnitX); changeCameraTarget(Vector3.UnitX); counter = 0;
                    if (getCameraPosition().X >= planelength * 1.5 * hexagonsidelength)
                    {
                        setCameraPosition(new Vector3(-planelength, getCameraPosition().Y, getCameraPosition().Z));
                        setCameraTarget(new Vector3(-planelength, getCameraTarget().Y, getCameraTarget().Z));
                    }*/

                    float xPosition = (getCameraPosition().X + 1 + planelength * 1.5f * hexagonsidelength) % (planelength * 1.5f * hexagonsidelength);
                    setCameraPosition(new Vector3(xPosition, getCameraTarget().Y, getCameraPosition().Z));
                    setCameraTarget(new Vector3(xPosition, getCameraTarget().Y, getCameraTarget().Z));
                    counter = 0;
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
