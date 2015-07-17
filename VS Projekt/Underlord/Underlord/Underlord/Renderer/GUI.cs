using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Underlord.Renderer
{
    class GUI
    {
        Logic.Vars_Func.ThingTyp selectedThingTyp;
        Entity.Nest nest;
        Entity.Wall wall;
        Entity.Creature creature;

        #region Properties
        public Logic.Vars_Func.ThingTyp SelectedThingTyp
        {
            get { return selectedThingTyp; }
            set { selectedThingTyp = value; }
        }
        public Entity.Wall Wall
        {
            get { return wall; }
            set { wall = value; }
        }
        public Entity.Nest Nest
        {
            get { return nest; }
            set { nest = value; }
        }
        public Entity.Creature Creature
        {
            get { return creature; }
            set { creature = value; }
        }
        #endregion

        #region Constructor
        public GUI()
        {
            selectedThingTyp = Logic.Vars_Func.ThingTyp.length;
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            //draw different values for other types of selected objects
            switch (selectedThingTyp)
            {
                case Logic.Vars_Func.ThingTyp.Wall:
                    spriteBatch.DrawString(font, "Typ: " + wall.Typ.ToString(), new Vector2(10, 80), Color.White);
                    spriteBatch.DrawString(font, "HP: " + wall.HP.ToString(), new Vector2(10, 95), Color.White);
                    spriteBatch.DrawString(font, "Gold: " + wall.Gold.ToString(), new Vector2(10, 110), Color.White);
                    break;
                case Logic.Vars_Func.ThingTyp.Nest:
                    spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(10, 80), Color.White);
                    spriteBatch.DrawString(font, "Size: " + nest.Size.ToString(), new Vector2(10, 95), Color.White);
                    spriteBatch.DrawString(font, "Nutrition: " + nest.Nutrition.ToString() + "/" + nest.MaxNutrition.ToString(), new Vector2(10, 110), Color.White);
                    break;
                case Logic.Vars_Func.ThingTyp.DungeonCreature:
                    break;
                case Logic.Vars_Func.ThingTyp.HeroCreature:
                    break;
                case Logic.Vars_Func.ThingTyp.NeutralCreature:
                    break;
                case Logic.Vars_Func.ThingTyp.HQCreature:
                    break;
                case Logic.Vars_Func.ThingTyp.length:
                    break;
            }
        }
    }
}
