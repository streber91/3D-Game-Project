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
    class StartMenu_GUI
    {
        static List<GUI_Element> all = new List<GUI_Element>();
        static List<GUI_Element> elements = new List<GUI_Element>();
        static GUI_Element startButton, textFrame, fireLogo, burnedLogo, dummy, pressedButton = null;
   
        static bool visible = false, fadeOut = false, cleanUp = false;
        static float visibleLerpCounter, fireLerpCounter, burnedLerpCounter;

        #region Initialize
        public static void createGUI()
        {
            visibleLerpCounter = 0;
            fireLerpCounter = 0;
            burnedLerpCounter = 0;
            visible = false;

            dummy = new GUI_Element(new Rectangle(0, -600, 1366, 768), "", Vars_Func.GUI_ElementTyp.Dummy, 1f);
            dummy.MoveAlongX = false;
            dummy.MoveAlongY = true;
            dummy.Selectable = false;
            GUI_Element background = new GUI_Element(new Rectangle(0, 0, 1366, 768), "", Vars_Func.GUI_ElementTyp.StartBackgroundHUD);
            elements.Add(background);
            all.Add(background);
            
            textFrame = new GUI_Element(new Rectangle(1366 / 2 - 144, 513, 293, 72), "", Vars_Func.GUI_ElementTyp.TextFrame);
            elements.Add(textFrame);
            dummy.Children.Add(textFrame);
            all.Add(textFrame);

            startButton = new GUI_Element(new Rectangle(1366 / 2 - 144, 574, 293, 96), "\n  Press Enter To Start", Vars_Func.GUI_ElementTyp.TextBlance, Vars_Func.GUI_Typ.StartButton);
            startButton.Highlightable = true;
            dummy.Children.Add(startButton);
            all.Add(startButton);

            GUI_Element backgroundFrame = new GUI_Element(new Rectangle(0, 0, 1366, 768), "", Vars_Func.GUI_ElementTyp.BackgroundHUD);
            elements.Add(backgroundFrame);
            all.Add(backgroundFrame);

            burnedLogo = new GUI_Element(new Rectangle(1366 / 2 - 400, 0, 800, 525), "", Vars_Func.GUI_ElementTyp.UnderlordBurnedLogo);
            burnedLogo.SpriteColor = new Color(1, 1, 1, 1);
            dummy.Children.Add(burnedLogo);
            all.Add(burnedLogo);
            fireLogo = new GUI_Element(new Rectangle(1366 / 2 - 400, 0, 800, 525), "", Vars_Func.GUI_ElementTyp.UnderlordFireLogo);
            fireLogo.SpriteColor = new Color(1, 1, 1, 0);
            dummy.Children.Add(fireLogo);


        }
        #endregion

        #region Properties
        public static GUI_Element getGUI_Button()
        {
            return pressedButton;
        }

        public static void restGUI()
        {
            fadeOut = false;
            pressedButton = null;
            visible = false;
            visibleLerpCounter = 0;
            fireLerpCounter = 0;
            burnedLerpCounter = 0;
            cleanUp = false;
        }
        #endregion

        #region Update
        public static void update(GameTime time, MouseState mouseState, KeyboardState keyboard)
        {
            startButton.Update(time, null, mouseState);
            dummy.Update(time, null, mouseState);

            foreach (GUI_Element e in elements)
            {
                e.Update(time, null, mouseState);
            }

            FadeIn(time);

            if(visible)
            {
                if (!all.Contains(fireLogo))
                {
                    all.Add(fireLogo);
                }
                burnedLerpCounter -= (float)time.ElapsedGameTime.Milliseconds / 1500;
                if (burnedLerpCounter < 0)
                {
                    burnedLerpCounter = 0;
                }
                fireLerpCounter += (float)time.ElapsedGameTime.Milliseconds / 1500;
                if (fireLerpCounter > 1)
                {
                    fireLerpCounter = 1;
                }
                fireLogo.SpriteColor = new Color(fireLerpCounter, fireLerpCounter, fireLerpCounter, fireLerpCounter);
            }

            if ((startButton.Rectangle.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed) || 
                (keyboard.IsKeyDown(Keys.Enter)) && 
                !fadeOut)
            {
                dummy.Move = false;
                cleanUp = true;
                startButton.SpriteColor = Color.Gray;
            }

            if (cleanUp && fireLogo.CurrentRectangle.Y <= -500 && pressedButton == null)
            {
                pressedButton = startButton;
            }
        }

        public static void FadeIn(GameTime time)
        {
            if (!visible && !fadeOut)
            {
                visibleLerpCounter += (float)time.ElapsedGameTime.Milliseconds / 2000;
                if (visibleLerpCounter > 1)
                {
                    visibleLerpCounter = 1;
                    visible = true;
                }
                foreach (GUI_Element a in all)
                {
                    a.SpriteColor = new Color(visibleLerpCounter, visibleLerpCounter, visibleLerpCounter);
                }

                if (visibleLerpCounter > 0.3)
                {
                    dummy.Move = true;
                }
            }
        }

        public static void FadeOut(GameTime time)
        {
            if (visible)
            {
                visibleLerpCounter -= (float)time.ElapsedGameTime.Milliseconds / 1000;
                if (visibleLerpCounter < 0)
                {
                    visibleLerpCounter = 0;
                    visible = false;
                }
                foreach (GUI_Element a in all)
                {
                    a.SpriteColor = new Color(visibleLerpCounter, visibleLerpCounter, visibleLerpCounter);
                }
            }
        }
        #endregion

        #region Draw
        public static void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            dummy.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.Augusta));
            foreach (GUI_Element e in elements)
            {
                if (e.ElementTyp == Vars_Func.GUI_ElementTyp.BackgroundHUD)
                {
                    //draw the button
                    if (startButton.SpriteColor.A > 0)
                    {
                        startButton.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold2));
                    }
                    if (burnedLogo.SpriteColor.A > 0)
                    {
                        burnedLogo.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.Augusta));
                    }
                    // draw the fire logo
                    if (fireLogo.SpriteColor.A > 0)
                    {
                        fireLogo.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.Augusta));
                    }

                }
                e.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.Augusta));
            }
        }
        #endregion
    }
}
