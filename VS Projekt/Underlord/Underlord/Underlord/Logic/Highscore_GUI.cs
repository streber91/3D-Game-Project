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
    class Highscore_GUI
    {
        static List<GUI_Element> all = new List<GUI_Element>();
        static List<GUI_Element> elements = new List<GUI_Element>();
        static List<GUI_Element> frames = new List<GUI_Element>();

        static GUI_Element dummy, headLine, returnButton, backgroundFrame, book;
        static GUI_Element pressedButton = null;
        static bool cleanUp = false, dontDraw = false, updateReady = false;

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

            headLine = new GUI_Element(new Rectangle(1366 / 2 - 256, 32, 512, 128), "      Highscore", Vars_Func.GUI_ElementTyp.TextFieldBig);
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

            GUI_Element bookFrame = new GUI_Element(new Rectangle(1366 / 2 - (911 / 2), -(237/2) + 100, 911, 237), "", Vars_Func.GUI_ElementTyp.ChainLarge);
            dummy.Children.Add(bookFrame);
            frames.Add(bookFrame);
            all.Add(bookFrame);
            
            string startSpace = "          ";
            string text = "\n" +startSpace;
            String[] highscore = Player.loadScore();

            for (int i = 0; i < 10; i++)
            {
                int number = i + 1;
                text += number.ToString() + ". " + highscore[i];
                text += "\n";
                text += startSpace;
            }


            book = new GUI_Element(new Rectangle(1366 / 2 - (911 / 2), 180, 911, 512), text, Vars_Func.GUI_ElementTyp.BookField);
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
        }
        #endregion

        #region Update
        public static void update(GameTime time, MouseState mouseState, KeyboardState keyboard)
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
                backgroundFrame.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold2));
            }
        }
        #endregion
    }
}
