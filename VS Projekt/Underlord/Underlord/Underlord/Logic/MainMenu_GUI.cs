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
    class MainMenu_GUI
    {
        static List<GUI_Element> all = new List<GUI_Element>();
        static List<GUI_Element> elements = new List<GUI_Element>();
        static List<GUI_Element> buttons = new List<GUI_Element>();
        static List<GUI_Element> frames = new List<GUI_Element>();

        static GUI_Element dummy, headLine, backgroundFrame;
        static GUI_Element currentSelectedButton = null, pressedButton = null;
        static MouseState oldMouseState;
        static int buttonCounter = 0;
        static bool keyIsDown = false, cleanUp = false, dontDraw = false, updateReady = false;
        static float visibleLerpCounter = 1; 

        #region Initialize
        public static void createGUI()
        {
            dummy = new GUI_Element(new Rectangle(0, -600, 1366, 768), "", Vars_Func.GUI_ElementTyp.Dummy, 1f);
            dummy.MoveAlongX = false;
            dummy.MoveAlongY = true;
            dummy.Selectable = false;

            GUI_Element background = new GUI_Element(new Rectangle(0, 0, 1366, 768), "", Vars_Func.GUI_ElementTyp.StartBackgroundHUD);
            elements.Add(background);
            all.Add(background);

            GUI_Element headLineFrame = new GUI_Element(new Rectangle(1366 / 2 - 256, 0-50, 512, 100), "", Vars_Func.GUI_ElementTyp.ChainBig);
            dummy.Children.Add(headLineFrame);
            frames.Add(headLineFrame);
            all.Add(headLineFrame);

            headLine = new GUI_Element(new Rectangle(1366 / 2 - 256, 32, 512, 128), "     Main Menu", Vars_Func.GUI_ElementTyp.TextFieldBig);
            headLine.YBonus = 20;
            dummy.Children.Add(headLine);
            all.Add(headLine);

            GUI_Element newGameFrame = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 180 - 32, 384, 84), "", Vars_Func.GUI_ElementTyp.ChainMiddle);
            dummy.Children.Add(newGameFrame);
            frames.Add(newGameFrame);
            all.Add(newGameFrame);

            GUI_Element newGameButton = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 180, 384, 96), "      New Game", Vars_Func.GUI_ElementTyp.TextFieldMiddle, Vars_Func.GUI_Typ.NewGameButton);
            newGameButton.Highlightable = true;
            newGameButton.YBonus = 10;
            dummy.Children.Add(newGameButton);
            buttons.Add(newGameButton);
            all.Add(newGameButton);

            //GUI_Element howToFrame = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 296 - 32, 384, 84), "", Vars_Func.GUI_ElementTyp.ChainMiddle);
            //dummy.Children.Add(howToFrame);
            //frames.Add(howToFrame);

            //GUI_Element howToButton = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 296, 384, 96), " How To Play", Vars_Func.GUI_ElementTyp.TextFieldMiddle);
            //howToButton.Highlightable = true;
            //dummy.Children.Add(howToButton);
            //buttons.Add(howToButton);

            GUI_Element settingsFrame = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 296 - 32, 384, 84), "", Vars_Func.GUI_ElementTyp.ChainMiddle);
            dummy.Children.Add(settingsFrame);
            frames.Add(settingsFrame);
            all.Add(settingsFrame);

            GUI_Element settingsButton = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 296, 384, 96), "       Settings", Vars_Func.GUI_ElementTyp.TextFieldMiddle, Vars_Func.GUI_Typ.SettingsButton);
            settingsButton.Highlightable = true;
            settingsButton.YBonus = 10;
            dummy.Children.Add(settingsButton);
            buttons.Add(settingsButton);
            all.Add(settingsButton);

            GUI_Element highscoreFrame = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 412 - 32, 384, 84), "", Vars_Func.GUI_ElementTyp.ChainMiddle);
            dummy.Children.Add(highscoreFrame);
            frames.Add(highscoreFrame);
            all.Add(highscoreFrame);

            GUI_Element highscoreButton = new GUI_Element(new Rectangle(1366 / 2 - (384 / 2), 412, 384, 96), "      Highscore", Vars_Func.GUI_ElementTyp.TextFieldMiddle, Vars_Func.GUI_Typ.HighScoreButton);
            highscoreButton.Highlightable = true;
            highscoreButton.YBonus = 10;
            dummy.Children.Add(highscoreButton);
            buttons.Add(highscoreButton);
            all.Add(highscoreButton);

            GUI_Element quitFrame = new GUI_Element(new Rectangle(1366 / 2 - (288 / 2), 528 - 32, 288, 64), "", Vars_Func.GUI_ElementTyp.ChainSmall);
            dummy.Children.Add(quitFrame);
            frames.Add(quitFrame);
            all.Add(quitFrame);

            GUI_Element quitButton = new GUI_Element(new Rectangle(1366 / 2 - (288 / 2), 528, 288, 72), "       Quit", Vars_Func.GUI_ElementTyp.TextFieldSmall, Vars_Func.GUI_Typ.QuitButton);
            quitButton.Highlightable = true;
            quitButton.YBonus = 10;
            dummy.Children.Add(quitButton);
            buttons.Add(quitButton);
            all.Add(quitButton);

            backgroundFrame = new GUI_Element(new Rectangle(0, 0, 1366, 768), "", Vars_Func.GUI_ElementTyp.BackgroundHUD);
            all.Add(backgroundFrame);
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
            visibleLerpCounter = 1;
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
            if(!dummy.Move && !cleanUp){
                dummy.Move = true;
            }
            headLine.Update(time, null, mouseState);
            dummy.Update(time, null, mouseState);

            foreach (GUI_Element e in elements)
            {
                e.Update(time, null, mouseState);
            }

            if (cleanUp)
            {
                if (currentSelectedButton.Typ == Vars_Func.GUI_Typ.NewGameButton)
                {
                    FadeOut(time);
                }
                else
                {
                    if (dummy.Children.Last().CurrentRectangle.Y <= -500 && pressedButton == null)
                    {
                        pressedButton = currentSelectedButton;
                    }
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
                    dummy.Move = false;
                    cleanUp = true;
                }
            }
        }
        public static void FadeOut(GameTime time)
        {
            visibleLerpCounter -= (float)time.ElapsedGameTime.Milliseconds / 1000;
            if (visibleLerpCounter < 0 && dummy.Children.Last().CurrentRectangle.Y <= -500 && pressedButton == null)
            {
                visibleLerpCounter = 0;
                pressedButton = currentSelectedButton;
                dontDraw = true;
            }
            foreach (GUI_Element a in all)
            {
                a.SpriteColor = new Color(visibleLerpCounter, visibleLerpCounter, visibleLerpCounter);
            }
        }
        #endregion

        #region Draw
        public static void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (!dontDraw && updateReady)
            {
                dummy.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold2));
                foreach (GUI_Element e in elements)
                {
                    e.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaTextField));
                }
                foreach (GUI_Element f in frames)
                {
                    f.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaTextField));
                }
                foreach (GUI_Element b in buttons)
                {
                    b.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaTextField));
                }
                headLine.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaHeadline));
                backgroundFrame.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold2));
            }
        }
        #endregion
    }
}
