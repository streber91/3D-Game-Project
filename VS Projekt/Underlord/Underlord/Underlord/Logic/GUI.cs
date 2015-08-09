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
            elements.Add(new GUI_Element(new Rectangle(100, 700, 48, 48), "Mine(M)", Vars_Func.GUI_ElementTyp.Mine));
            elements.Add(new GUI_Element(new Rectangle(200, 700, 48, 48), "Room(R)", Vars_Func.GUI_ElementTyp.Room));
            elements.Add(new GUI_Element(new Rectangle(300, 700, 48, 48), "Merge(T)", Vars_Func.GUI_ElementTyp.MergeRoom));
            elements.Add(new GUI_Element(new Rectangle(400, 700, 48, 48), "Delete(Z)", Vars_Func.GUI_ElementTyp.DeleteRoom));
            elements.Add(new GUI_Element(new Rectangle(500, 700, 48, 48), "Nest(N)", Vars_Func.GUI_ElementTyp.Build));

            elements.Add(new GUI_Element(new Rectangle(660, 700, 48, 48), "Dmg", Vars_Func.GUI_ElementTyp.DamageUpgrade));
            elements.Add(new GUI_Element(new Rectangle(730, 700, 48, 48), "Live", Vars_Func.GUI_ElementTyp.LifeUpgrade));
            elements.Add(new GUI_Element(new Rectangle(800, 700, 48, 48), "Speed", Vars_Func.GUI_ElementTyp.SpeedUpgrade));
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
            get { return selectedThingTyp; }
            set { selectedThingTyp = value; }
        }
        public static Wall Wall
        {
            set { wall = value; }
        }
        public static Nest Nest
        {
            get { return nest; }
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
                    spriteBatch.DrawString(font, "Upgrades:", new Vector2(10, 110), Color.White);
                    spriteBatch.DrawString(font, "Dmg: " + nest.UpgradeCount[0], new Vector2(10, 125), Color.White);
                    spriteBatch.DrawString(font, "Live: " + nest.UpgradeCount[1], new Vector2(10, 140), Color.White);
                    spriteBatch.DrawString(font, "Speed: " + nest.UpgradeCount[2], new Vector2(10, 155), Color.White);
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
