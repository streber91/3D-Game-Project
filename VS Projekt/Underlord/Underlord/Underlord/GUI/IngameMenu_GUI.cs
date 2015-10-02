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
    class IngameMenu_GUI
    {
        static List<GUI_Element> all = new List<GUI_Element>();
        static List<GUI_Element> elements = new List<GUI_Element>();
        static List<GUI_Element> buttons = new List<GUI_Element>();
        static List<GUI_Element> frames = new List<GUI_Element>();

        static GUI_Element background, currentSelectedButton = null, pressedButton = null;
        static MouseState oldMouseState;
        static int buttonCounter = 0;
        static bool keyIsDown = false, cleanUp = false, dontDraw = false, updateReady = false;

        #region Initialize
        public static void createGUI()
        {
            background = new GUI_Element(new Rectangle(0, 0, 1366, 768), "", Vars_Func.GUI_ElementTyp.BlackBackgoundHUD);
            background.SpriteColor = new Color(0.7f, 0.7f, 0.7f, 0.2f);
            elements.Add(background);
            all.Add(background);

            GUI_Element newGameButton = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 180, 384, 96), "      Continue", Vars_Func.GUI_ElementTyp.TextFieldMiddle, Vars_Func.GUI_Typ.NewGameButton);
            newGameButton.Highlightable = true;
            newGameButton.YBonus = 10;
            buttons.Add(newGameButton);
            all.Add(newGameButton);

            GUI_Element settingsFrame = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 296 - 32, 384, 84), "", Vars_Func.GUI_ElementTyp.ChainMiddle);
            frames.Add(settingsFrame);
            all.Add(settingsFrame);

            GUI_Element settingsButton = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 296, 384, 96), "       Settings", Vars_Func.GUI_ElementTyp.TextFieldMiddle, Vars_Func.GUI_Typ.SettingsButton);
            settingsButton.Highlightable = true;
            settingsButton.YBonus = 10;
            buttons.Add(settingsButton);
            all.Add(settingsButton);

            GUI_Element highscoreFrame = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 412 - 32, 384, 84), "", Vars_Func.GUI_ElementTyp.ChainMiddle);
            frames.Add(highscoreFrame);
            all.Add(highscoreFrame);

            GUI_Element highscoreButton = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 412, 384, 96), "      Highscore", Vars_Func.GUI_ElementTyp.TextFieldMiddle, Vars_Func.GUI_Typ.HighScoreButton);
            highscoreButton.Highlightable = true;
            highscoreButton.YBonus = 10;
            buttons.Add(highscoreButton);
            all.Add(highscoreButton);

            GUI_Element quitFrame = new GUI_Element(new Rectangle(1366 / 2 - (288 / 2), 528 - 32, 288, 64), "", Vars_Func.GUI_ElementTyp.ChainSmall);
            frames.Add(quitFrame);
            all.Add(quitFrame);

            GUI_Element quitButton = new GUI_Element(new Rectangle(1366 / 2 - (288 / 2), 528, 288, 72), "      Exit", Vars_Func.GUI_ElementTyp.TextFieldSmall, Vars_Func.GUI_Typ.StartButton);
            quitButton.Highlightable = true;
            quitButton.YBonus = 10;
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

            foreach (GUI_Element e in elements)
            {
                e.Update(time, null, mouseState);
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

                if (currentSelectedButton == null)
                {
                    if ((keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S)) && !keyIsDown)
                    {
                        currentSelectedButton = buttons[0];
                        buttonCounter = 0;
                        keyIsDown = true;
                    }

                    if ((keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W)) && !keyIsDown)
                    {
                        currentSelectedButton = buttons[buttons.Count - 1];
                        buttonCounter = buttons.Count - 1;
                        keyIsDown = true;
                    }
                }
                else
                {
                    if ((keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S)) && !keyIsDown)
                    {
                        buttonCounter++;
                        buttonCounter %= (buttons.Count);
                        currentSelectedButton = buttons[buttonCounter];
                        keyIsDown = true;
                    }

                    if ((keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W)) && !keyIsDown)
                    {
                        buttonCounter--;
                        if (buttonCounter < 0)
                        {
                            buttonCounter = buttons.Count - 1;
                        }
                        buttonCounter %= (buttons.Count);
                        currentSelectedButton = buttons[buttonCounter];
                        keyIsDown = true;
                    }
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
                if (!keyboard.IsKeyDown(Keys.Up) && !keyboard.IsKeyDown(Keys.W) && !keyboard.IsKeyDown(Keys.Down) && !keyboard.IsKeyDown(Keys.S))
                {
                    keyIsDown = false;
                }
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
                foreach (GUI_Element e in elements)
                {
                    e.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaTextField));
                }
                background.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold2));
                foreach (GUI_Element f in frames)
                {
                    f.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaTextField));
                }
                foreach (GUI_Element b in buttons)
                {
                    b.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaTextField));
                }
            }
        }
        #endregion
    }
}
