using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Underlord.Logic
{
    class InputController
    {
        Keys lastKey = Keys.None;
        private string input = "";
        private string drawInput = "";
        private bool isActive;
        private float timeCounter = 0.0f;

        #region Properties
        public string Name
        {
            get { return input; }
        }
        #endregion


        #region Constructor
        public void InputConrtoller()
        {
            isActive = false;
        }
        #endregion

        #region Update
        public void Update(GameTime time)
        {
            timeCounter += time.ElapsedGameTime.Milliseconds;

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyUp(lastKey))
            {
                lastKey = Keys.None;
            }

            if (keyState.GetPressedKeys().Length > 0 && lastKey == Keys.None)
            {
                lastKey = keyState.GetPressedKeys()[0];
                if (lastKey == Keys.Back)
                {
                    if (input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                    }
                }
                else if (input.Length < 10)
                {                 
                    char value = (char)lastKey.GetHashCode();
                    if (Char.IsLetterOrDigit(value))
                    {
                        input += value;
                    }
                }
            }
            if (timeCounter > 150)
            {
                isActive = !isActive;
                timeCounter = 0.0f;
            }
            if (isActive)
            {
                drawInput = input.Substring(0, input.Length) + "|";
            }
            else
            {
                drawInput = input.Substring(0, input.Length) + "";
            }
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position)
        {
            spriteBatch.DrawString(font, drawInput, position, Color.Black);
        }
        #endregion
    }
}
