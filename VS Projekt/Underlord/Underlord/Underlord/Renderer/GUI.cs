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
    class GUI
    {
        Vars_Func.ThingTyp selectedThingTyp;
        Nest nest;
        Wall wall;
        Creature creature;

        #region Properties
        public Vars_Func.ThingTyp SelectedThingTyp
        {
            get { return selectedThingTyp; }
            set { selectedThingTyp = value; }
        }
        public Wall Wall
        {
            get { return wall; }
            set { wall = value; }
        }
        public Nest Nest
        {
            get { return nest; }
            set { nest = value; }
        }
        public Creature Creature
        {
            get { return creature; }
            set { creature = value; }
        }
        #endregion

        #region Constructor
        public GUI()
        {
            selectedThingTyp = Vars_Func.ThingTyp.length;
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            //draw different values for other types of selected objects
            switch (selectedThingTyp)
            {
                case Vars_Func.ThingTyp.Wall:
                    spriteBatch.DrawString(font, "Typ: " + wall.Typ.ToString(), new Vector2(10, 80), Color.White);
                    spriteBatch.DrawString(font, "HP: " + wall.HP.ToString(), new Vector2(10, 95), Color.White);
                    spriteBatch.DrawString(font, "Gold: " + wall.Gold.ToString(), new Vector2(10, 110), Color.White);
                    break;
                case Vars_Func.ThingTyp.Nest:
                    spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(10, 80), Color.White);
                    spriteBatch.DrawString(font, "Size: " + nest.Size.ToString(), new Vector2(10, 95), Color.White);
                    spriteBatch.DrawString(font, "Nutrition: " + nest.Nutrition.ToString() + "/" + nest.MaxNutrition.ToString(), new Vector2(10, 110), Color.White);
                    break;
                case Vars_Func.ThingTyp.DungeonCreature:
                    break;
                case Vars_Func.ThingTyp.HeroCreature:
                    break;
                case Vars_Func.ThingTyp.NeutralCreature:
                    break;
                case Vars_Func.ThingTyp.HQCreature:
                    break;
                case Vars_Func.ThingTyp.length:
                    break;
            }
        }
    }
}
