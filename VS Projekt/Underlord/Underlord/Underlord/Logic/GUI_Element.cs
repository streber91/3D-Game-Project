using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Underlord.Logic
{
    class GUI_Element
    {
        Rectangle rectangle;
        String text;
        Vars_Func.GUI_ElementTyp elementTyp;

        #region Properties
        public Vars_Func.GUI_ElementTyp ElementTyp
        {
            get { return elementTyp; }
        }
        public Rectangle Rectangle
        {
            get { return rectangle; }
        }
        #endregion

        public GUI_Element(Rectangle rectangle, String text, Vars_Func.GUI_ElementTyp elementTyp)
        {
            this.rectangle = rectangle;
            this.text = text;
            this.elementTyp = elementTyp;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Color spriteColor)
        {
            if (elementTyp == Vars_Func.GUI_ElementTyp.BottomHUD) spriteBatch.Draw(Vars_Func.getGUI_ElementTextures(elementTyp), new Vector2(rectangle.X, rectangle.Y), Color.Red);
            else spriteBatch.Draw(Vars_Func.getGUI_ElementTextures(elementTyp), new Vector2(rectangle.X, rectangle.Y), spriteColor);
            spriteBatch.DrawString(font, text, new Vector2(rectangle.X, rectangle.Y+30), Color.Black);
        }
    }
}
