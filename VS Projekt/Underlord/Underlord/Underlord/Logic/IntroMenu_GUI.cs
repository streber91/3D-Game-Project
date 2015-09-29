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
    class IntroMenu_GUI
    {
        static List<GUI_Element> all = new List<GUI_Element>();
        static List<GUI_Element> elements = new List<GUI_Element>();
        static List<GUI_Element> frames = new List<GUI_Element>();
        static List<GUI_Element> buttons = new List<GUI_Element>();

        static GUI_Element dummy, headLine, backgroundFrame, book, background, currentSelectedButton = null, pressedButton = null;
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

            GUI_Element headLineFrame = new GUI_Element(new Rectangle(1366 / 2 - 256, 0 - 50, 512, 100), "", Vars_Func.GUI_ElementTyp.ChainBig);
            dummy.Children.Add(headLineFrame);
            frames.Add(headLineFrame);
            all.Add(headLineFrame);

            headLine = new GUI_Element(new Rectangle(1366 / 2 - 256, 32, 512, 128), "            Welcome new Lord...", Vars_Func.GUI_ElementTyp.TextFieldBig);
            headLine.YBonus = 45;
            dummy.Children.Add(headLine);
            all.Add(headLine);

            GUI_Element withFrame = new GUI_Element(new Rectangle(1366 / 2 - 911/2, 180+384-20, 256, 64), "", Vars_Func.GUI_ElementTyp.ChainMiddle);
            dummy.Children.Add(withFrame);
            frames.Add(withFrame);
            all.Add(withFrame);

            GUI_Element withButton = new GUI_Element(new Rectangle(1366 / 2 - 911 / 2, 180+384+10, 256, 76), "   With Help", Vars_Func.GUI_ElementTyp.TextFieldMiddle, Vars_Func.GUI_Typ.NewGameButton);
            withButton.Highlightable = true;
            withButton.YBonus = 15;
            dummy.Children.Add(withButton);
            buttons.Add(withButton);
            all.Add(withButton);

            GUI_Element bookFrame = new GUI_Element(new Rectangle(1366 / 2 - (911 / 2), -(237 / 2) + 100, 911, 237), "", Vars_Func.GUI_ElementTyp.ChainLarge);
            dummy.Children.Add(bookFrame);
            frames.Add(bookFrame);
            all.Add(bookFrame);

            GUI_Element withOutFrame = new GUI_Element(new Rectangle(1366 / 2 + 911 / 2-256, 180 + 384 - 20, 256, 64), "", Vars_Func.GUI_ElementTyp.ChainMiddle);
            dummy.Children.Add(withOutFrame);
            frames.Add(withOutFrame);
            all.Add(withOutFrame);

            GUI_Element withOutButton = new GUI_Element(new Rectangle(1366 / 2 + 911 / 2-256, 180 + 384 + 10, 256, 76), "  Without Help", Vars_Func.GUI_ElementTyp.TextFieldMiddle, Vars_Func.GUI_Typ.QuitButton);
            withOutButton.Highlightable = true;
            withOutButton.YBonus = 15;
            dummy.Children.Add(withOutButton);
            buttons.Add(withOutButton);
            all.Add(withOutButton);

            book = new GUI_Element(new Rectangle(1366 / 2 - (911 / 2), 180, 911, 384), Player.loadString("Content/Tutorials/MainTutorial.txt"), Vars_Func.GUI_ElementTyp.BookField);
            book.YBonus = 10;
            dummy.Children.Add(book);
            all.Add(book);

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
            pressedButton = null;
            cleanUp = false;
            dontDraw = false;
            updateReady = false;
            visibleLerpCounter = 1;
            keyIsDown = false;
            buttonCounter = 0;
            currentSelectedButton = null;
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
            if (!dummy.Move && !cleanUp)
            {
                dummy.Move = true;
            }
            headLine.Update(time, null, mouseState);
            dummy.Update(time, null, mouseState);

            if (cleanUp)
            {
                    FadeOut(time);
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
                    dummy.Move = false;
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
                    e.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                }
                foreach (GUI_Element f in frames)
                {
                    f.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                }
                foreach (GUI_Element b in buttons)
                {
                    b.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                }
                book.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.Augusta));
                headLine.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                backgroundFrame.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold2));
            }
        }
        #endregion
    }
}
