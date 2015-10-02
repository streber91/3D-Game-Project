using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Underlord.Entity;
using Underlord.Environment;

namespace Underlord.Logic
{
    class InsertNameGUI
    {
        static List<GUI_Element> all = new List<GUI_Element>();
        static List<GUI_Element> buttons = new List<GUI_Element>();

        static GUI_Element background, headLine,underLine, currentSelectedButton = null, pressedButton = null;
        static MouseState oldMouseState;
        static int buttonCounter = 0;
        static bool keyIsDown = false, cleanUp = false, dontDraw = false, updateReady = false;

        static InputController input = new InputController();

        #region Initialize
        public static void createGUI()
        {
            background = new GUI_Element(new Rectangle(0, 0, 1366, 768), "", Vars_Func.GUI_ElementTyp.BlackBackgoundHUD);
            background.SpriteColor = new Color(0.7f, 0.7f, 0.7f, 0.2f);
            all.Add(background);

            underLine = new GUI_Element(new Rectangle(1366 / 2 - 256, 450 - 96 - 10, 512, 96), "                                          Please insert you name:", Vars_Func.GUI_ElementTyp.TextFieldBig);
            underLine.YBonus = 10;
            all.Add(underLine);

            GUI_Element quitButton = new GUI_Element(new Rectangle(1366 / 2 -128 +32, 450, 256-64, 64), "      Confirm", Vars_Func.GUI_ElementTyp.TextFieldMiddle, Vars_Func.GUI_Typ.QuitButton);
            quitButton.Highlightable = true;
            quitButton.YBonus = 12;
            buttons.Add(quitButton);
            all.Add(quitButton);

        }
        #endregion

        #region Properties
        public static GUI_Element getGUI_Button()
        {
            return pressedButton;
        }
        public static bool UpdateReady
        {
            get { return updateReady; }
        }

        public static string getPlayerName()
        {
            return input.Name;
        }

        public static void restGUI()
        {
            currentSelectedButton = null;
            pressedButton = null;
            buttonCounter = 0;
            cleanUp = false;
            dontDraw = false;
            updateReady = false;
        }
        #endregion

        #region Update
        public static void update(GameTime time, MouseState mouseState, KeyboardState keyboard)
        {
            input.Update(time);

            if (!updateReady)
            {
                updateReady = true;
            }

            foreach (GUI_Element b in buttons)
            {
                b.Update(time, null, mouseState);
            }

            if (cleanUp)
            {
                if (pressedButton == null)
                {
                    pressedButton = currentSelectedButton;
                }
            }
            else
            {
                bool mouseMoving = false;
                if ((oldMouseState.X != mouseState.X) || (oldMouseState.Y != mouseState.Y))
                {
                    mouseMoving = true;
                    oldMouseState = mouseState;
                }

                GUI_Element mouseSelection = null;
                int buttonNumer = 0, mouseNumer = 0;
                foreach (GUI_Element b in buttons)
                {
                    if (b.SpriteColor == Color.Gray)
                    {
                        mouseSelection = b;
                        mouseNumer = buttonNumer;
                    }
                    buttonNumer++;
                }
                if (mouseSelection != null && mouseSelection != currentSelectedButton && mouseMoving)
                {
                    buttonCounter = mouseNumer;
                    currentSelectedButton = mouseSelection;
                }
                if (mouseSelection != null && mouseSelection != currentSelectedButton && !mouseMoving)
                {
                    mouseSelection.SpriteColor = Color.White;
                }
                if (mouseSelection == null && mouseSelection != currentSelectedButton && mouseMoving)
                {
                    currentSelectedButton.SpriteColor = Color.White;
                    currentSelectedButton = null;
                }
                if (currentSelectedButton != null && currentSelectedButton.SpriteColor != Color.Gray)
                {
                    currentSelectedButton.SpriteColor = Color.Gray;
                }
                //if (!keyboard.IsKeyDown(Keys.Up) && !keyboard.IsKeyDown(Keys.W) && !keyboard.IsKeyDown(Keys.Down) && !keyboard.IsKeyDown(Keys.S))
                //{
                //    keyIsDown = false;
                //}
                if (currentSelectedButton != null &&
                   (mouseState.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter)))
                {
                    cleanUp = true;
                }
            }
        }
        #endregion

        #region Draw
        public static void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (!dontDraw && updateReady)
            {
                background.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold2));
                underLine.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall));
                foreach (GUI_Element b in buttons)
                {
                    b.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                }
                input.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold2), new Vector2(1366 / 2 - 100, 450 - 96 / 2 - 20));
            }
        }
        #endregion
    }
}
