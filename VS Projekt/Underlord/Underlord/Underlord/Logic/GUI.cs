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
        static List<GUI_Element> buildButtons = new List<GUI_Element>();
        static List<GUI_Element> returnScreenElements = new List<GUI_Element>();
        static List<GUI_Element> returnScreenButtons = new List<GUI_Element>();

        public static void createGUI()
        {
            menuElements.Add(new GUI_Element(new Rectangle(0, 0, 400, 400), "", Vars_Func.GUI_ElementTyp.Menu));
            menuElements.Add(new GUI_Element(new Rectangle(1200, 0, 400, 400), "", Vars_Func.GUI_ElementTyp.Menu));
            menuButtons.Add(new GUI_Element(new Rectangle(20, 20, 48, 48), "Start Game", Vars_Func.GUI_ElementTyp.StartGame));
            menuButtons.Add(new GUI_Element(new Rectangle(1220, 20, 48, 48), "Highscore", Vars_Func.GUI_ElementTyp.Highscore));
            menuButtons.Add(new GUI_Element(new Rectangle(20, 70, 48, 48), "Tutorial", Vars_Func.GUI_ElementTyp.Tutorial));
            menuButtons.Add(new GUI_Element(new Rectangle(1220, 70, 48, 48), "Quit", Vars_Func.GUI_ElementTyp.QuitGame));

            returnScreenElements.Add(new GUI_Element(new Rectangle(500, 300, 400, 400), "", Vars_Func.GUI_ElementTyp.Menu));
            returnScreenButtons.Add(new GUI_Element(new Rectangle(600, 400, 48, 48), "Yes", Vars_Func.GUI_ElementTyp.ReturnAccept));
            returnScreenButtons.Add(new GUI_Element(new Rectangle(700, 400, 48, 48), "No", Vars_Func.GUI_ElementTyp.ReturnDecline));

            elements.Add(new GUI_Element(new Rectangle(0, 0, 1366, 768), "", Vars_Func.GUI_ElementTyp.BackgroundHUD));
            elements.Add(new GUI_Element(new Rectangle(1093, 13, 260, 265), "", Vars_Func.GUI_ElementTyp.MinimapHUD));


            //elements.Add(new GUI_Element(new Rectangle(0, 200, 480, 10), "", Vars_Func.GUI_ElementTyp.LeftHUD));
            //elements.Add(new GUI_Element(new Rectangle(1356, 250, 430, 10), "", Vars_Func.GUI_ElementTyp.RightHUD));
            //elements.Add(new GUI_Element(new Rectangle(0, 680, 1116, 88), "", Vars_Func.GUI_ElementTyp.BottomHUD));
            //elements.Add(new GUI_Element(new Rectangle(200, 0, 10, 916), "", Vars_Func.GUI_ElementTyp.TopHUD));
            elements.Add(new GUI_Element(new Rectangle(13, 13, 400, 200), "", Vars_Func.GUI_ElementTyp.RessoucesHUD));
            elements.Add(new GUI_Element(new Rectangle(10, 10, 88, 88), "", Vars_Func.GUI_ElementTyp.GoldHUD));
            elements.Add(new GUI_Element(new Rectangle(110, 10, 88, 88), "", Vars_Func.GUI_ElementTyp.ManaHUD));
            elements.Add(new GUI_Element(new Rectangle(210, 10, 88, 88), "", Vars_Func.GUI_ElementTyp.FoodHUD));
            elements.Add(new GUI_Element(new Rectangle(1116, 600, 250, 168), "", Vars_Func.GUI_ElementTyp.InfoHUD));

            buttons.Add(new GUI_Element(new Rectangle(80, 680, 88, 76), "  Mine(M)", Vars_Func.GUI_ElementTyp.Mine));
            buttons.Add(new GUI_Element(new Rectangle(180, 680, 88, 76), "  Room(R)", Vars_Func.GUI_ElementTyp.Room));
            buttons.Add(new GUI_Element(new Rectangle(280, 680, 88, 76), "  Merge(T)", Vars_Func.GUI_ElementTyp.MergeRoom));
            buttons.Add(new GUI_Element(new Rectangle(380, 680, 88, 76), "  Delete(Z)", Vars_Func.GUI_ElementTyp.DeleteRoom));
            buttons.Add(new GUI_Element(new Rectangle(480, 680, 88, 76), "  Nest(N)", Vars_Func.GUI_ElementTyp.Build));
            buttons.Add(new GUI_Element(new Rectangle(10, 110, 88, 76), "  Fireball", Vars_Func.GUI_ElementTyp.Fireball));
            buttons.Add(new GUI_Element(new Rectangle(110, 110, 88, 76), "  Imp", Vars_Func.GUI_ElementTyp.SummonImp));

            upgradeButtons.Add(new GUI_Element(new Rectangle(600, 680, 88, 76), "Dmg", Vars_Func.GUI_ElementTyp.DamageUpgrade));
            upgradeButtons.Add(new GUI_Element(new Rectangle(700, 680, 88, 76), "Live", Vars_Func.GUI_ElementTyp.LifeUpgrade));
            upgradeButtons.Add(new GUI_Element(new Rectangle(800, 680, 88, 76), "Speed", Vars_Func.GUI_ElementTyp.SpeedUpgrade));

            buildButtons.Add(new GUI_Element(new Rectangle(600, 680, 88, 76), "Ants", Vars_Func.GUI_ElementTyp.PlaceAnts));
            buildButtons.Add(new GUI_Element(new Rectangle(700, 680, 88, 76), "Skeletons", Vars_Func.GUI_ElementTyp.PlaceSkeletons));
            buildButtons.Add(new GUI_Element(new Rectangle(800, 680, 88, 76), "Farm", Vars_Func.GUI_ElementTyp.PlaceFarm));
            buildButtons.Add(new GUI_Element(new Rectangle(900, 680, 88, 76), "Temple", Vars_Func.GUI_ElementTyp.PlaceTemple));
            buildButtons.Add(new GUI_Element(new Rectangle(1000, 680, 88, 76), "Entrance", Vars_Func.GUI_ElementTyp.PlaceEntrance));
        }
        

        #region Properties
        public static GUI_Element getGUI_ReturnScreenButton(Vars_Func.GUI_ElementTyp typ)
        {
            foreach (GUI_Element b in returnScreenButtons)
            {
                if (b.ElementTyp == typ) return b;
            }
            return null;
        }
        public static GUI_Element getGUI_MenuButton(Vars_Func.GUI_ElementTyp typ)
        {
            foreach (GUI_Element b in menuButtons)
            {
                if (b.ElementTyp == typ) return b;
            }
            return null;
        }
        public static GUI_Element getGUI_BuildButton(Vars_Func.GUI_ElementTyp typ)
        {
            foreach (GUI_Element b in buildButtons)
            {
                if (b.ElementTyp == typ) return b;
            }
            return null;
        }
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

        public static void Draw(SpriteBatch spriteBatch, SpriteFont font, MouseState mouseState, Renderer.Camera camera, Environment.Map map)
        {
            if (Interaction.GameState == Vars_Func.GameState.MainMenu)
            {
                //draw the meunElements
                foreach (GUI_Element e in menuElements)
                {
                    e.Draw(spriteBatch, font, Color.White);
                }
                //draw the menuButtons
                foreach (GUI_Element b in menuButtons)
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
            }
            else if(Interaction.GameState == Vars_Func.GameState.Highscore)
            {

            }
            else if (Interaction.GameState == Vars_Func.GameState.Tutorial)
            {

            }
            else
            {
                if (Interaction.GameState == Vars_Func.GameState.ReturnToMainMenu)
                {
                    //draw the returnScreenElements
                    foreach (GUI_Element e in returnScreenElements)
                    {
                        e.Draw(spriteBatch, font, Color.White);
                    }
                    //draw the returnScreenButtons
                    foreach (GUI_Element b in returnScreenButtons)
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
                }
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
                if (Interaction.GameState == Vars_Func.GameState.Build ||
                    Interaction.GameState == Vars_Func.GameState.PlaceAnts ||
                    Interaction.GameState == Vars_Func.GameState.PlaceSkeletons ||
                    Interaction.GameState == Vars_Func.GameState.PlaceFarm ||
                    Interaction.GameState == Vars_Func.GameState.PlaceTemple ||
                    Interaction.GameState == Vars_Func.GameState.PlaceEntrance)
                {
                    foreach (GUI_Element b in buildButtons)
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
                }
                //draw the player ressources
                spriteBatch.DrawString(font, Player.Gold.ToString(), new Vector2(20, 70), Color.Black);
                spriteBatch.DrawString(font, Player.Mana.ToString(), new Vector2(140, 70), Color.Black);
                spriteBatch.DrawString(font, Player.Food.ToString(), new Vector2(260, 70), Color.Black);
                //draw the spellcosts
                spriteBatch.DrawString(font, Spells.FireballCost.ToString(), new Vector2(20, 170), Color.Black);
                spriteBatch.DrawString(font, Spells.SummonImpCost.ToString(), new Vector2(120, 170), Color.Black);
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
                                if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
                                {
                                    b.Draw(spriteBatch, font, Color.Gray);
                                }
                                else
                                {
                                    b.Draw(spriteBatch, font, Color.White);
                                }
                            }
                            nest.DrawTargetFlag(camera, map.getHexagonAt(nest.TargetPosition).get3DPosition(), Color.White, false, 0.0f);
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
}
