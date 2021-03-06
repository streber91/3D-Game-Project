﻿using System;
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
    class Setting_GUI
    {
        static List<GUI_Element> all = new List<GUI_Element>();
        static List<GUI_Element> elements = new List<GUI_Element>();
        static List<GUI_Element> frames = new List<GUI_Element>();

        static GUI_Element dummy, headLine, returnButton, fullscreenButton, brightnessUp, brightnessDown, backgroundFrame, book,tutorialButton;
        static GUI_Element pressedButton = null;
        static bool cleanUp = false, dontDraw = false, updateReady = false, fullscreen = false, help = true/*, buttonPressed = false*/;
        static int brightness; 

        #region Initialize
        public static void createGUI()
        {
            brightness = 5;
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

            headLine = new GUI_Element(new Rectangle(1366 / 2 - 256, 32, 512, 128), "        Settings", Vars_Func.GUI_ElementTyp.TextFieldBig);
            headLine.YBonus = 20;
            dummy.Children.Add(headLine);
            all.Add(headLine);

            GUI_Element returnFrame = new GUI_Element(new Rectangle(128 - (128 / 2), 0, 256, 84), "", Vars_Func.GUI_ElementTyp.ChainMiddle);
            dummy.Children.Add(returnFrame);
            frames.Add(returnFrame);
            all.Add(returnFrame);

            returnButton = new GUI_Element(new Rectangle(128 - (128 / 2), 42, 256, 64), "                 Back", Vars_Func.GUI_ElementTyp.TextArrow, Vars_Func.GUI_Typ.StartButton);
            returnButton.Highlightable = true;
            returnButton.YBonus = 15;
            dummy.Children.Add(returnButton);
            all.Add(returnButton);

            GUI_Element bookFrame = new GUI_Element(new Rectangle(1366 / 2 - (911 / 2), -(237 / 2) + 100, 911, 237), "", Vars_Func.GUI_ElementTyp.ChainLarge);
            dummy.Children.Add(bookFrame);
            frames.Add(bookFrame);
            all.Add(bookFrame);

            string spaceSmall = "     ", spaceBig = "        ";
            book = new GUI_Element(new Rectangle(1366 / 2 - (911 / 2), 180, 911, 512+48), "\n" + spaceBig + "Fullscreen:                                       Brightness:          "+brightness+"\n\n\n"
                                                                                            + spaceBig + "Show Help:  \n\n"
                                                                                            + spaceBig + "Menu:" + spaceSmall + "       Tab\n"
                                                                                            + spaceBig + "Confirm:" + spaceSmall + "  Enter/LMT\n"
                                                                                            + spaceBig + "Refuse:" + spaceSmall + "     Esc/RMT\n"
                                                                                            + spaceBig + "Close Help:  Esc\n"
                                                                                            + spaceBig + "Mine:" + spaceSmall + "        M \n"
                                                                                            + spaceBig + "Room:" + spaceSmall + "      R\n"
                                                                                            + spaceBig + "Merge:" + spaceSmall + "     T\n"
                                                                                            + spaceBig + "Delete:" + spaceSmall + "     D\n"
                                                                                            + spaceBig + "Upgrade:" + spaceSmall + "U\n"
                                                                                            + spaceBig + "Build:" + spaceSmall + "      N\n"
                                                                                            , Vars_Func.GUI_ElementTyp.BookField);
            book.YBonus = 10;
            dummy.Children.Add(book);
            all.Add(book);

            brightnessUp = new GUI_Element(new Rectangle(1366 / 2 + 215, 180 + 15 + 16, 32, 32), "", Vars_Func.GUI_ElementTyp.BrightnessUp);
            brightnessUp.Highlightable = true;
            brightnessUp.YBonus = 20;
            dummy.Children.Add(brightnessUp);
            all.Add(brightnessUp);

            brightnessDown = new GUI_Element(new Rectangle(1366 / 2 + 215, 180 + 15 + 16 + 32, 32, 32), "", Vars_Func.GUI_ElementTyp.BrightnessDown);
            brightnessDown.Highlightable = true;
            brightnessDown.YBonus = 20;
            dummy.Children.Add(brightnessDown);
            all.Add(brightnessDown);

            fullscreenButton = new GUI_Element(new Rectangle(1366 / 2 - 200, 180 + 20, 76, 76), "  Off", Vars_Func.GUI_ElementTyp.FullScreenButton);
            fullscreenButton.Highlightable = true;
            fullscreenButton.YBonus = 20;
            dummy.Children.Add(fullscreenButton);
            all.Add(fullscreenButton);

            tutorialButton = new GUI_Element(new Rectangle(1366 / 2 - 200, 230 + 60, 76, 76), "  Off", Vars_Func.GUI_ElementTyp.FullScreenButton);
            tutorialButton.Highlightable = true;
            tutorialButton.YBonus = 20;
            dummy.Children.Add(tutorialButton);
            all.Add(tutorialButton);

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
        public static bool UseFullscreen
        {
            get { return fullscreen; }
        }
        public static bool ShowHelp
        {
            set { help = value; }
            get { return help; }
        }
        public static void restGUI()
        {
            pressedButton = null;
            cleanUp = false;
            dontDraw = false;
            updateReady = false;
            //buttonPressed = false;
        }
        public static int getBrigthness()
        {
            return brightness;
        }
        #endregion

        #region Update
        public static void update(GameTime time, MouseState mouseState, MouseState lastMouseState, KeyboardState keyboard)
        {
            if (!updateReady)
            {
                updateReady = true;
            }
            if (!dummy.Move && !cleanUp)
            {
                dummy.Move = true;
            }
            headLine.Update(time, null, mouseState);
            dummy.Update(time, null, mouseState);

            foreach (GUI_Element e in elements)
            {
                e.Update(time, null, mouseState);
            }

            if (fullscreenButton.Rectangle.Contains(mouseState.X, mouseState.Y) 
                && lastMouseState.LeftButton == ButtonState.Released 
                && mouseState.LeftButton == ButtonState.Pressed)
            {
                fullscreen = !fullscreen;
                //buttonPressed = true;
            }

            if (tutorialButton.Rectangle.Contains(mouseState.X, mouseState.Y) 
                && lastMouseState.LeftButton == ButtonState.Released 
                && mouseState.LeftButton == ButtonState.Pressed)
            {
                help = !help;
                //buttonPressed = true;
            }

            if (brightnessUp.Rectangle.Contains(mouseState.X, mouseState.Y) 
                && lastMouseState.LeftButton == ButtonState.Released 
                && mouseState.LeftButton == ButtonState.Pressed)
            {

                if (brightness < 10)
                {
                    brightness++;
                    UpdateBook();
                }
            }

            if (brightnessDown.Rectangle.Contains(mouseState.X, mouseState.Y) 
                && lastMouseState.LeftButton == ButtonState.Released 
                && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (brightness > 1)
                {
                    brightness--;
                    UpdateBook();
                }
            }



            //if (!(fullscreenButton.Rectangle.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed) &&
            //   !keyboard.IsKeyDown(Keys.Enter) && buttonPressed)
            //{
            //    buttonPressed = false;
            //}
            //if (!(tutorialButton.Rectangle.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed) &&
            //   !keyboard.IsKeyDown(Keys.Enter) && buttonPressed)
            //{
            //    buttonPressed = false;
            //}


            if (fullscreen)
            {
                fullscreenButton.Text = "  On";
            }
            else
            {
                fullscreenButton.Text = "  Off";
            }
            if (help)
            {
                tutorialButton.Text = "  On";
            }
            else
            {
                tutorialButton.Text = "  Off";
            }

            if ((returnButton.Rectangle.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed) ||
                keyboard.IsKeyDown(Keys.Back) || (keyboard.IsKeyDown(Keys.Escape)))
            {
                dummy.Move = false;
                cleanUp = true;
                returnButton.SpriteColor = Color.Gray;
            }

            if (cleanUp && book.CurrentRectangle.Y <= -500 && pressedButton == null)
            {
                pressedButton = returnButton;
            }

        }
        #endregion

        private static void UpdateBook()
        {
            dummy.Children.Remove(book);
            all.Remove(book);
            
            string spaceSmall = "     ", spaceBig = "        ";
            book = new GUI_Element(new Rectangle(1366 / 2 - (911 / 2), 180, 911, 512 + 48), "\n" + spaceBig + "Fullscreen:                                       Brightness:          " + brightness + "\n\n\n"
                                                                                            + spaceBig + "Show Help:  \n\n"
                                                                                            + spaceBig + "Menu:" + spaceSmall + "       Tab\n"
                                                                                            + spaceBig + "Confirm:" + spaceSmall + "  Enter/LMT\n"
                                                                                            + spaceBig + "Refuse:" + spaceSmall + "     Esc/RMT\n"
                                                                                            + spaceBig + "Close Help:  Esc\n"
                                                                                            + spaceBig + "Mine:" + spaceSmall + "        M \n"
                                                                                            + spaceBig + "Room:" + spaceSmall + "      R\n"
                                                                                            + spaceBig + "Merge:" + spaceSmall + "     T\n"
                                                                                            + spaceBig + "Delete:" + spaceSmall + "     D\n"
                                                                                            + spaceBig + "Upgrade:" + spaceSmall + "U\n"
                                                                                            + spaceBig + "Build:" + spaceSmall + "      N\n"
                                                                                            , Vars_Func.GUI_ElementTyp.BookField);
            book.YBonus = 10;
            dummy.Children.Add(book);
            all.Add(book);
        }

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
                returnButton.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                book.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                headLine.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaHeadline));
                fullscreenButton.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                tutorialButton.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                brightnessUp.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                brightnessDown.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaText));
                backgroundFrame.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold2));
            }
        }
        #endregion
    }
}
