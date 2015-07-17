using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Underlord.Entity;
using Underlord.Logic;

namespace Underlord.Renderer
{
    static class GUI
    {
        static Vars_Func.ThingTyp selectedThingTyp = Vars_Func.ThingTyp.length;
        static Nest nest;
        static Wall wall;
        static Creature creature;

        #region Properties
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
            //draw different values for other types of selected objects
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
                        spriteBatch.DrawString(font, "Age: " + creature.Age.ToString(), new Vector2(10, 125), Color.White);
                    }
                    break;
                case Vars_Func.ThingTyp.HeroCreature:
                    if (creature != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(10, 80), Color.White);
                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(10, 95), Color.White);
                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(10, 110), Color.White);
                        spriteBatch.DrawString(font, "Age: " + creature.Age.ToString(), new Vector2(10, 125), Color.White);
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
        }
    }
}
