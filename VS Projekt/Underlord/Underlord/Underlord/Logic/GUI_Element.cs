using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Underlord.Logic
{
    class GUI_Element
    {
        Rectangle rectangle, currentRectangle, selectionRectangle;
        String text;
        Vars_Func.GUI_ElementTyp elementTyp;
        Color spriteColor;

        Vars_Func.GUI_Typ typ;
        GUI_Element frameElement, leftChain, rightChain, topPole, bottomPole;
        List<GUI_Element> childElements = new List<GUI_Element>();

        bool enabled = true, visable = true, highlightable = false;
        bool move = false, moveAlongX = true, moveAlongY = false;
        bool selectable = true, pressed = false, mousePressed = false;
        float positionLerpCounter = 0, speed;
        float textYBonus = 0; 

        #region Properties
        public Vars_Func.GUI_ElementTyp ElementTyp
        {
            get { return elementTyp; }
        }
        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }
        public Vars_Func.GUI_Typ Typ
        {
            get { return typ; }
        }
        public Rectangle CurrentRectangle
        {
            get { return currentRectangle; }
            set { currentRectangle = value; }
        }
        public Rectangle SelectionRectangle
        {
            get { return selectionRectangle; }
        }
        public Color SpriteColor
        {
            get { return spriteColor; }
            set { spriteColor = value; }
        }
        public String Text
        {
            set { text = value; }
        }
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        public bool Visable
        {
            get { return visable; }
            set { visable = value; }
        }
        public bool Selectable
        {
            get { return selectable; }
            set { selectable = value; }
        }
        public bool Highlightable
        {
            set { highlightable = value; }
        }
        public bool Pressed
        {
            get { return pressed; }
        }
        public bool Move
        {
            get { return move; }
            set { move = value; }
        }
        public bool MoveAlongX
        {
            set { moveAlongX = value; }
        }
        public bool MoveAlongY
        {
            set { moveAlongY = value; }
        }
        public GUI_Element Frame
        {
            get { return frameElement; }
            set { frameElement = value; }
        }
        public GUI_Element LeftChain
        {
            get { return leftChain; }
            set { leftChain = value; }
        }
        public GUI_Element RightChain
        {
            get { return rightChain; }
            set { rightChain = value; }
        }
        public GUI_Element TopPole
        {
            get { return topPole; }
            set { topPole = value; }
        }
        public GUI_Element BottomPole
        {
            get { return bottomPole; }
            set { bottomPole = value; }
        }
        public List<GUI_Element> Children
        {
            get { return childElements; }
        }
        public float YBonus
        {
            set { textYBonus = value; }
        } 
        #endregion
        
        #region Constructor
        public GUI_Element(Rectangle rectangle, String text, Vars_Func.GUI_ElementTyp elementTyp)
            : this(rectangle, text, elementTyp, Vars_Func.GUI_Typ.None, 1)
        {

        }
        public GUI_Element(Rectangle rectangle, String text, Vars_Func.GUI_ElementTyp elementTyp, Vars_Func.GUI_Typ typ)
            : this(rectangle, text, elementTyp, typ, 1)
        {

        }
        public GUI_Element(Rectangle rectangle, String text, Vars_Func.GUI_ElementTyp elementTyp, float speed)
            : this(rectangle, text, elementTyp, Vars_Func.GUI_Typ.None, speed)
        {

        }
        public GUI_Element(Rectangle rectangle, String text, Vars_Func.GUI_ElementTyp elementTyp, Vars_Func.GUI_Typ typ, float speed)
        {
            this.rectangle = rectangle;
            this.currentRectangle = rectangle;
            this.text = text;
            this.elementTyp = elementTyp;
            this.typ = typ;
            this.spriteColor = Color.White;
            this.speed = speed;
        }
        #endregion

        #region Update
        public void Update(GameTime time, Environment.Map map, MouseState mouseState)
        {
            // Set color when element is disabled
            if (!enabled)
            {
                spriteColor = Color.SteelBlue;
                if (frameElement != null)
                {
                    frameElement.SpriteColor = Color.SteelBlue;
                }
            }
            else
            {
                if (frameElement != null)
                {
                    frameElement.SpriteColor = Color.White;
                }
                // Set the right color
                this.SetColor(mouseState);
                // Set selection
                this.SetSelection(mouseState);
            }
                // Move children
                if (move)
                {
                    this.MoveForward(time);
                }
                else
                {
                    this.MoveBackward(time);
                }

                if (positionLerpCounter > 0)
                {
                    foreach (GUI_Element c in childElements)
                    {
                        this.UpdatePosition(c);
                    }
                }
            
            // Update children
            foreach (GUI_Element c in childElements)
            {
                c.Update(time, map, mouseState);
            }
        }

        private void UpdatePosition(GUI_Element c)
        {
            if (topPole != null)
            {
                float tempWidth = MathHelper.Lerp(0, topPole.Rectangle.Width, positionLerpCounter);
                topPole.CurrentRectangle = new Rectangle(topPole.Rectangle.X, topPole.Rectangle.Y, (int)tempWidth, topPole.Rectangle.Height);
            }
            if (bottomPole != null)
            {
                float tempWidth = MathHelper.Lerp(0, bottomPole.Rectangle.Width, positionLerpCounter);
                bottomPole.CurrentRectangle = new Rectangle(bottomPole.Rectangle.X, bottomPole.Rectangle.Y, (int)tempWidth, bottomPole.Rectangle.Height);
            }

            c.CurrentRectangle = this.getNewRectangel(c);
            if (c.Frame != null)
            {
                float tempFrameX = MathHelper.Lerp(rectangle.X, c.Frame.Rectangle.X, positionLerpCounter);
                c.Frame.CurrentRectangle = new Rectangle((int)tempFrameX, c.Frame.Rectangle.Y, c.Frame.Rectangle.Width, c.Frame.Rectangle.Height);
            }
            if (c.LeftChain != null)
            {
                c.LeftChain.CurrentRectangle = this.getNewRectangel(c.LeftChain);
            }
            if (c.RightChain != null)
            {
                c.RightChain.CurrentRectangle = this.getNewRectangel(c.RightChain);
            }
        }

        private Rectangle getNewRectangel(GUI_Element c)
        {
            float tempX = rectangle.X;
            if (moveAlongX)
            {
                tempX = MathHelper.Lerp(rectangle.X, c.Rectangle.X, positionLerpCounter);
            }
            float tempY = c.Rectangle.Y;
            if (moveAlongY)
            {
                tempX = c.Rectangle.X;
                tempY = MathHelper.Lerp(rectangle.Y, c.Rectangle.Y, positionLerpCounter);
            }
            return new Rectangle((int)tempX, (int)tempY, c.Rectangle.Width, c.Rectangle.Height);
        }

        private void SetColor(MouseState mouseState)
        {
            if (highlightable)
            {
                if (rectangle.Contains(mouseState.X, mouseState.Y))
                {
                    spriteColor = Color.Gray;
                    if (frameElement != null)
                    {
                        frameElement.SpriteColor = Color.Gray;
                    }
                }
                else
                {
                    spriteColor = Color.White;
                    if (frameElement != null)
                    {
                        frameElement.SpriteColor = Color.White;
                    }
                }
            }
        }

        private void SetSelection(MouseState mouseState)
        {
            //if (rectangle.Contains(mouseState.X, mouseState.Y) && !useSelectionRect && selectable)
            //{
            //    useSelectionRect = true;
            //    move = true;
            //    int maxX = 0, maxY = 0;
            //    foreach (GUI_Element c in childElements)
            //    {
            //        if (maxX < (c.Rectangle.X + c.Rectangle.Width))
            //        {
            //            maxX = (c.Rectangle.X + c.Rectangle.Width);
            //        }
            //        if (maxY < (c.Rectangle.Y + c.Rectangle.Height))
            //        {
            //            maxY = (c.Rectangle.Y + c.Rectangle.Height);
            //        }
            //    }
            //    selectionRectangle = new Rectangle(rectangle.X, rectangle.Y, maxX, maxY);
            //}

            //if (useSelectionRect && !selectionRectangle.Contains(mouseState.X, mouseState.Y) && selectable)
            //{
            //    useSelectionRect = false;
            //    move = false;
            //}
            //if (rectangle.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed && !mousePressed)
            //{
            //    mousePressed = true;
            //    move = true;
            //    pressed = true;
            //}
            
            //if (
            //     (mouseState.RightButton == ButtonState.Pressed) )
            //{
            //    move = false;
            //    pressed = false;
            //}
        }

        private void MoveForward(GameTime time)
        {
            if (topPole != null)
            {
                topPole.Visable = true;
            }
            if (bottomPole != null)
            {
                bottomPole.Visable = true;
            }
            positionLerpCounter += (float)time.ElapsedGameTime.Milliseconds / 1000 * speed;
            if (positionLerpCounter > 1)
            {
                positionLerpCounter = 1;
            }
            foreach (GUI_Element c in childElements)
            {
                if (!c.Visable)
                {
                    c.Visable = true;
                    if (c.Frame != null)
                    {
                        c.Frame.Visable = true;
                    }
                }
            }
        }

        private void MoveBackward(GameTime time)
        {
            if (topPole != null)
            {
                topPole.Visable = false;
            }
            if (bottomPole != null)
            {
                bottomPole.Visable = false;
            }
            positionLerpCounter -= (float)time.ElapsedGameTime.Milliseconds / 1000 * speed;
            if (positionLerpCounter < 0)
            {
                positionLerpCounter = 0;
                foreach (GUI_Element c in childElements)
                {
                    if (c.Visable)
                    {
                        c.Visable = false;
                        if (c.Frame != null)
                        {
                            c.Frame.Visable = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region Drawing
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (visable)
            {
                if (topPole != null)
                {
                    topPole.Draw(spriteBatch, font);
                }
                if (bottomPole != null)
                {
                    bottomPole.Draw(spriteBatch, font);
                }
 
                if (leftChain != null)
                {
                    leftChain.Draw(spriteBatch, font);
                }
                if (rightChain != null)
                {
                    rightChain.Draw(spriteBatch, font);
                }
                foreach (GUI_Element c in childElements)
                {
                    c.Draw(spriteBatch, font);
                }
                if (elementTyp == Vars_Func.GUI_ElementTyp.BottomHUD) spriteBatch.Draw(Vars_Func.getGUI_ElementTextures(elementTyp), new Vector2(currentRectangle.X, currentRectangle.Y), Color.Red);
                //else spriteBatch.Draw(Vars_Func.getGUI_ElementTextures(elementTyp), new Vector2(rectangle.X, rectangle.Y), spriteColor);

                else spriteBatch.Draw(Vars_Func.getGUI_ElementTextures(elementTyp), currentRectangle, spriteColor);
                spriteBatch.DrawString(font, text, new Vector2(currentRectangle.X, currentRectangle.Y + 2 + textYBonus), Color.Black);

                if (frameElement != null)
                {
                    frameElement.Draw(spriteBatch, font);
                }
            }
        }
        #endregion
    }
}
