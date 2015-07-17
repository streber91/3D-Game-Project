using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Underlord.Entity;

namespace Underlord.Logic
{
    static class GUI
    {
        static Vars_Func.ThingTyp selectedThingTyp = Vars_Func.ThingTyp.length;
        static Nest nest;
        static Wall wall;
        static Creature creature;
        static List<GUI_Element> elements = new List<GUI_Element>();

        public static void createGUI()
        {
            elements.Add(new GUI_Element(new Rectangle(100, 700, 48, 48), "M", Vars_Func.GUI_ElementTyp.Mine));
            elements.Add(new GUI_Element(new Rectangle(170, 700, 48, 48), "R", Vars_Func.GUI_ElementTyp.Room));
            elements.Add(new GUI_Element(new Rectangle(240, 700, 48, 48), "T", Vars_Func.GUI_ElementTyp.MergeRoom));
            elements.Add(new GUI_Element(new Rectangle(310, 700, 48, 48), "Z", Vars_Func.GUI_ElementTyp.DeleteRoom));
            elements.Add(new GUI_Element(new Rectangle(380, 700, 48, 48), "N", Vars_Func.GUI_ElementTyp.Build));
        }
        

        #region Properties
        public static GUI_Element getGUI_Element(Vars_Func.GUI_ElementTyp typ)
        {
            foreach (GUI_Element e in elements)
            {
                if (e.ElementTyp == typ) return e;
            }
            return null;
        }
        public static Vars_Func.ThingTyp SelectedThingTyp
        {
            set { selectedThingTyp = value; }
        }
        public static Wall Wall
        {
            set { wall = value; }
        }
        public static Nest Nest
        {
            set { nest = value; }
        }
        public static Creature Creature
        {
            get { return creature; }
            set { creature = value; }
        }
        #endregion

        public static void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            //draw the GUI_elements
            foreach (GUI_Element e in elements)
            {
                e.Draw(spriteBatch, font);
            }
            //draw different values for other types of selected objects
            #region Selected Object
            switch (selectedThingTyp)
            {
                case Vars_Func.ThingTyp.Wall:
                    if (wall != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + wall.Typ.ToString(), new Vector2(10, 80), Color.White);
                        spriteBatch.DrawString(font, "HP: " + wall.HP.ToString(), new Vector2(10, 95), Color.White);
                        spriteBatch.DrawString(font, "Gold: " + wall.Gold.ToString(), new Vector2(10, 110), Color.White);
                    }
                    break;
                case Vars_Func.ThingTyp.Nest:
                    spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(10, 80), Color.White);
                    spriteBatch.DrawString(font, "Nutrition: " + nest.Nutrition.ToString() + "/" + nest.MaxNutrition.ToString(), new Vector2(10, 95), Color.White);
                    break;
                case Vars_Func.ThingTyp.DungeonCreature:
                    if (creature != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(10, 80), Color.White);
                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(10, 95), Color.White);
                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(10, 110), Color.White);
                        spriteBatch.DrawString(font, "Age: " + ((int)creature.Age).ToString(), new Vector2(10, 125), Color.White);
                    }
                    break;
                case Vars_Func.ThingTyp.HeroCreature:
                    if (creature != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(10, 80), Color.White);
                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(10, 95), Color.White);
                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(10, 110), Color.White);
                        spriteBatch.DrawString(font, "Age: " + ((int)creature.Age).ToString(), new Vector2(10, 125), Color.White);
                    }
                    break;
                case Vars_Func.ThingTyp.NeutralCreature:
                    if (creature != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(10, 80), Color.White);
                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(10, 95), Color.White);
                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(10, 110), Color.White);
                    }
                    break;
                case Vars_Func.ThingTyp.HQCreature:
                    if (creature != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(10, 80), Color.White);
                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(10, 95), Color.White);
                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(10, 110), Color.White);
                    }
                    break;
                case Vars_Func.ThingTyp.length:
                    break;
            }
            #endregion
        }
    }
}
