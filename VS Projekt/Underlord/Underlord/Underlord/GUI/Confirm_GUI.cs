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
    class Confirm_GUI
    {
        static List<GUI_Element> all = new List<GUI_Element>();
        static List<GUI_Element> buttons = new List<GUI_Element>();

        static GUI_Element background, headLine, currentSelectedButton = null, pressedButton = null;
        static MouseState oldMouseState;
        static int buttonCounter = 0;
        static bool keyIsDown = false, cleanUp = false, dontDraw = false, updateReady = false;

        #region Initialize
        public static void createGUI()
        {
            background = new GUI_Element(new Rectangle(0, 0, 1366, 768), "", Vars_Func.GUI_ElementTyp.BlackBackgoundHUD);
            background.SpriteColor = new Color(0.7f, 0.7f, 0.7f, 0.2f);
            all.Add(background);

            headLine = new GUI_Element(new Rectangle(1366 / 2 - 256, 450 - 74 - 10, 512, 74), "     Are you sure you want to quit?", Vars_Func.GUI_ElementTyp.TextFieldBig);
            headLine.YBonus = 20;
            all.Add(headLine);

            GUI_Element yesButton = new GUI_Element(new Rectangle(1366 / 2 - 256, 450, 96, 64), "   No", Vars_Func.GUI_ElementTyp.TextFieldMiddle, Vars_Func.GUI_Typ.NewGameButton);
            yesButton.Highlightable = true;
            yesButton.YBonus = 12;
            buttons.Add(yesButton);
            all.Add(yesButton);

            GUI_Element noButton = new GUI_Element(new Rectangle(1366 / 2 + 256 - 96, 450, 96, 64), "  Yes", Vars_Func.GUI_ElementTyp.TextFieldMiddle, Vars_Func.GUI_Typ.QuitButton);
            noButton.Highlightable = true;
            noButton.YBonus = 12;
            buttons.Add(noButton);
            all.Add(noButton);

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

                //if (currentSelectedButton == null)
                //{
                //    if ((keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S)) && !keyIsDown)
                //    {
                //        currentSelectedButton = buttons[0];
                //        buttonCounter = 0;
                //        keyIsDown = true;
                //    }

                //    if ((keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W)) && !keyIsDown)
                //    {
                //        currentSelectedButton = buttons[buttons.Count - 1];
                //        buttonCounter = buttons.Count - 1;
                //        keyIsDown = true;
                //    }
                //}
                //else
                //{
                //    if ((keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S)) && !keyIsDown)
                //    {
                //        buttonCounter++;
                //        buttonCounter %= (buttons.Count);
                //        currentSelectedButton = buttons[buttonCounter];
                //        keyIsDown = true;
                //    }

                //    if ((keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W)) && !keyIsDown)
                //    {
                //        buttonCounter--;
                //        if (buttonCounter < 0)
                //        {
                //            buttonCounter = buttons.Count - 1;
                //        }
                //        buttonCounter %= (buttons.Count);
                //        currentSelectedButton = buttons[buttonCounter];
                //        keyIsDown = true;
                //    }
                //}
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
                headLine.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                foreach (GUI_Element b in buttons)
                {
                    b.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                }
            }
        }
        #endregion
    }
}
