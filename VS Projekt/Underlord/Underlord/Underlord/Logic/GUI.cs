using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Underlord.Entity;

namespace Underlord.Logic
{
    static class GUI
    {
        static Vars_Func.ThingTyp selectedThingTyp = Vars_Func.ThingTyp.length;
        static Nest nest;
        static Wall wall;
        static Creature creature;
        static List<GUI_Element> menuElements = new List<GUI_Element>();
        static List<GUI_Element> menuButtons = new List<GUI_Element>();
        static List<GUI_Element> elements = new List<GUI_Element>();
        static List<GUI_Element> buttons = new List<GUI_Element>();
        static List<GUI_Element> upgradeButtons = new List<GUI_Element>();

        public static void createGUI()
        {
            menuElements.Add(new GUI_Element(new Rectangle(100, 700, 48, 48), "", Vars_Func.GUI_ElementTyp.Menu));

            menuButtons.Add(new GUI_Element(new Rectangle(100, 700, 48, 48), "Start Game", Vars_Func.GUI_ElementTyp.StartGame));
            menuButtons.Add(new GUI_Element(new Rectangle(100, 700, 48, 48), "Highscore", Vars_Func.GUI_ElementTyp.Highscore));
            menuButtons.Add(new GUI_Element(new Rectangle(100, 700, 48, 48), "Quit", Vars_Func.GUI_ElementTyp.QuitGame));


            elements.Add(new GUI_Element(new Rectangle(0, 0, 1366, 768), "", Vars_Func.GUI_ElementTyp.BackgroundHUD));
            elements.Add(new GUI_Element(new Rectangle(1093, 13, 260, 265), "", Vars_Func.GUI_ElementTyp.MinimapHUD));


            //elements.Add(new GUI_Element(new Rectangle(0, 200, 480, 10), "", Vars_Func.GUI_ElementTyp.LeftHUD));
            //elements.Add(new GUI_Element(new Rectangle(1356, 250, 430, 10), "", Vars_Func.GUI_ElementTyp.RightHUD));
            //elements.Add(new GUI_Element(new Rectangle(0, 680, 1116, 88), "", Vars_Func.GUI_ElementTyp.BottomHUD));
            //elements.Add(new GUI_Element(new Rectangle(200, 0, 10, 916), "", Vars_Func.GUI_ElementTyp.TopHUD));
            elements.Add(new GUI_Element(new Rectangle(13, 13, 400, 200), "", Vars_Func.GUI_ElementTyp.RessoucesHUD));
            elements.Add(new GUI_Element(new Rectangle(10, 10, 88, 88), "", Vars_Func.GUI_ElementTyp.GoldHUD));
            elements.Add(new GUI_Element(new Rectangle(130, 10, 88, 88), "", Vars_Func.GUI_ElementTyp.ManaHUD));
            elements.Add(new GUI_Element(new Rectangle(250, 10, 88, 88), "", Vars_Func.GUI_ElementTyp.FoodHUD));
            elements.Add(new GUI_Element(new Rectangle(1116, 600, 250, 168), "", Vars_Func.GUI_ElementTyp.InfoHUD));

            buttons.Add(new GUI_Element(new Rectangle(100, 680, 88, 76), "  Mine(M)", Vars_Func.GUI_ElementTyp.Mine));
            buttons.Add(new GUI_Element(new Rectangle(200, 680, 88, 76), "  Room(R)", Vars_Func.GUI_ElementTyp.Room));
            buttons.Add(new GUI_Element(new Rectangle(300, 680, 88, 76), "  Merge(T)", Vars_Func.GUI_ElementTyp.MergeRoom));
            buttons.Add(new GUI_Element(new Rectangle(400, 680, 88, 76), "  Delete(Z)", Vars_Func.GUI_ElementTyp.DeleteRoom));
            buttons.Add(new GUI_Element(new Rectangle(500, 680, 88, 76), "  Nest(N)", Vars_Func.GUI_ElementTyp.Build));

            upgradeButtons.Add(new GUI_Element(new Rectangle(660, 700, 48, 48), "Dmg", Vars_Func.GUI_ElementTyp.DamageUpgrade));
            upgradeButtons.Add(new GUI_Element(new Rectangle(730, 700, 48, 48), "Live", Vars_Func.GUI_ElementTyp.LifeUpgrade));
            upgradeButtons.Add(new GUI_Element(new Rectangle(800, 700, 48, 48), "Speed", Vars_Func.GUI_ElementTyp.SpeedUpgrade));
        }
        

        #region Properties
        public static GUI_Element getGUI_UpgradeButton(Vars_Func.GUI_ElementTyp typ)
        {
            foreach (GUI_Element b in upgradeButtons)
            {
                if (b.ElementTyp == typ) return b;
            }
            return null;
        }
        public static GUI_Element getGUI_Button(Vars_Func.GUI_ElementTyp typ)
        {
            foreach (GUI_Element b in buttons)
            {
                if (b.ElementTyp == typ) return b;
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

        public static void update(GameTime time, Environment.Map map)
        {

        }

        public static void Draw(SpriteBatch spriteBatch, SpriteFont font, MouseState mouseState)
        {
            //draw the GUI_elements
            foreach (GUI_Element e in elements)
            {
                e.Draw(spriteBatch, font, Color.White);
            }
            //draw the buttons
            foreach (GUI_Element b in buttons)
            {
                if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
                {
                    b.Draw(spriteBatch, font, Color.Gray);
                }
                else
                {
                    b.Draw(spriteBatch, font, Color.White);
                }
                
            }
            //draw the player ressources
            spriteBatch.DrawString(font, "Gold: " + Player.Gold, new Vector2(10, 85), Color.Black);
            spriteBatch.DrawString(font, "Mana: " + Player.Mana, new Vector2(130, 85), Color.Black);
            spriteBatch.DrawString(font, "Food: " + Player.Food, new Vector2(250, 85), Color.Black);
            //draw different values for other types of selected objects
            #region Selected Object
            switch (selectedThingTyp)
            {
                case Vars_Func.ThingTyp.Wall:
                    if (wall != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + wall.Typ.ToString(), new Vector2(1121, 600), Color.Black);
                        spriteBatch.DrawString(font, "HP: " + wall.HP.ToString(), new Vector2(1121, 620), Color.Black);
                        spriteBatch.DrawString(font, "Gold: " + wall.Gold.ToString(), new Vector2(1121, 640), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.Nest:
                    if (nest.Typ != Vars_Func.NestTyp.Entrance)
                    {
                        //draw the upgradeButtons
                        foreach (GUI_Element b in upgradeButtons)
                        {
                            b.Draw(spriteBatch, font, Color.White);
                        }
                        spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(1121, 600), Color.Black);
                        spriteBatch.DrawString(font, "Nutrition: " + nest.Nutrition.ToString() + "/" + nest.MaxNutrition.ToString(), new Vector2(1121, 620), Color.Black);
                        spriteBatch.DrawString(font, "Upgrades:", new Vector2(1121, 640), Color.Black);
                        spriteBatch.DrawString(font, "Dmg: " + nest.UpgradeCount[0], new Vector2(1121, 660), Color.Black);
                        spriteBatch.DrawString(font, "Live: " + nest.UpgradeCount[1], new Vector2(1121, 680), Color.Black);
                        spriteBatch.DrawString(font, "Speed: " + nest.UpgradeCount[2], new Vector2(1121, 700), Color.Black);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(1121, 600), Color.Black);
                        spriteBatch.DrawString(font, "Start Age: ", new Vector2(1121, 620), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.DungeonCreature:
                    if (creature != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1121, 600), Color.Black);
                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1121, 620), Color.Black);
                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(1121, 640), Color.Black);
                        spriteBatch.DrawString(font, "Age: " + ((int)creature.Age).ToString(), new Vector2(1121, 660), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.HeroCreature:
                    if (creature != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1121, 600), Color.Black);
                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1121, 620), Color.Black);
                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(1121, 640), Color.Black);
                        spriteBatch.DrawString(font, "Age: " + ((int)creature.Age).ToString(), new Vector2(1121, 660), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.NeutralCreature:
                    if (creature != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1121, 600), Color.Black);
                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1121, 620), Color.Black);
                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(1121, 640), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.HQCreature:
                    if (creature != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1121, 600), Color.Black);
                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1121, 620), Color.Black);
                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(1121, 640), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.length:
                    break;
            }
            #endregion
        }
    }
}
