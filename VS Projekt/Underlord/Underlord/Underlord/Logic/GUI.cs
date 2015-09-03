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
        static List<GUI_Element> tutorials = new List<GUI_Element>();
        static List<GUI_Element> tutorialButtons = new List<GUI_Element>();

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
            elements.Add(new GUI_Element(new Rectangle(1090, 13, 266, 265), "", Vars_Func.GUI_ElementTyp.MinimapHUD));
            elements.Add(new GUI_Element(new Rectangle(13, 13, 260, 265), "", Vars_Func.GUI_ElementTyp.RessoucesFrame));
            elements.Add(new GUI_Element(new Rectangle(13, 278, 110, 475), "", Vars_Func.GUI_ElementTyp.ButtonHUD));
            elements.Add(new GUI_Element(new Rectangle(13 + 110, 278, 110, 172), "", Vars_Func.GUI_ElementTyp.ButtonHUD));
            elements.Add(new GUI_Element(new Rectangle(1106, 590, 247, 160), "", Vars_Func.GUI_ElementTyp.InfoHUD));

            GUI_Element topLeftChain = new GUI_Element(new Rectangle(31 - 5, -30, 12, 64), "", Vars_Func.GUI_ElementTyp.LeftChain);
            GUI_Element topRightChain = new GUI_Element(new Rectangle(31 + 78 - 12 + 5, -30, 12, 64), "", Vars_Func.GUI_ElementTyp.RightChain);
            elements.Add(topLeftChain);
            elements.Add(topRightChain);
            GUI_Element leftChain = new GUI_Element(new Rectangle(31 - 5, 240, 12, 60), "", Vars_Func.GUI_ElementTyp.LeftChain);
            GUI_Element rightChain = new GUI_Element(new Rectangle(31 + 78 - 24 + 5, 240, 24, 60), "", Vars_Func.GUI_ElementTyp.SpecialChain);
            elements.Add(leftChain);
            elements.Add(rightChain);
            elements.Add(new GUI_Element(new Rectangle(13, 13, 260, 265), "", Vars_Func.GUI_ElementTyp.RessoucesPapier));
            elements.Add(new GUI_Element(new Rectangle(13, 13, 260, 265), "", Vars_Func.GUI_ElementTyp.RessoucesHUD));

            // Add mine button
            GUI_Element mine = new GUI_Element(new Rectangle(31, 285, 69, 60), "", Vars_Func.GUI_ElementTyp.Mine);
            mine.Highlightable = true;
            GUI_Element mineFrame = new GUI_Element(new Rectangle(mine.Rectangle.X - 11, mine.Rectangle.Y + 45, 96, 24), "  Mine(M)", Vars_Func.GUI_ElementTyp.FrameHUD);
            mine.Frame = mineFrame;
            GUI_Element mineLeftChain = new GUI_Element(new Rectangle(mine.Rectangle.X - 5, mine.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            mine.LeftChain = mineLeftChain;
            GUI_Element mineRightChain = new GUI_Element(new Rectangle(mine.Rectangle.X + 69 - 12 + 5, mine.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            mine.RightChain = mineRightChain;
            buttons.Add(mine);
            // Add room button
            GUI_Element room = new GUI_Element(new Rectangle(31, 363, 69, 60), "", Vars_Func.GUI_ElementTyp.Room);
            room.Highlightable = true;
            GUI_Element roomFrame = new GUI_Element(new Rectangle(room.Rectangle.X - 11, room.Rectangle.Y + 45, 96, 24), " Room(R)", Vars_Func.GUI_ElementTyp.FrameHUD);
            room.Frame = roomFrame;
            GUI_Element roomLeftChain = new GUI_Element(new Rectangle(room.Rectangle.X - 5, room.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            room.LeftChain = roomLeftChain;
            GUI_Element roomRightChain = new GUI_Element(new Rectangle(room.Rectangle.X + 69 - 12 + 5, room.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            room.RightChain = roomRightChain;
            buttons.Add(room);
            // Add merge button
            GUI_Element merge = new GUI_Element(new Rectangle(31, 441, 69, 60), "", Vars_Func.GUI_ElementTyp.MergeRoom);
            merge.Highlightable = true;
            GUI_Element mergeFrame = new GUI_Element(new Rectangle(merge.Rectangle.X - 11, merge.Rectangle.Y + 45, 96, 24), " Merge(T)", Vars_Func.GUI_ElementTyp.FrameHUD);
            merge.Frame = mergeFrame;
            GUI_Element mergeLeftChain = new GUI_Element(new Rectangle(merge.Rectangle.X - 5, merge.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            merge.LeftChain = mergeLeftChain;
            GUI_Element mergeRightChain = new GUI_Element(new Rectangle(merge.Rectangle.X + 69 - 12 + 5, merge.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            merge.RightChain = mergeRightChain;
            buttons.Add(merge);
            // Add delete button 
            GUI_Element delete = new GUI_Element(new Rectangle(31, 519, 69, 60), "", Vars_Func.GUI_ElementTyp.DeleteRoom);
            delete.Highlightable = true;
            GUI_Element deleteFrame = new GUI_Element(new Rectangle(delete.Rectangle.X - 11, delete.Rectangle.Y + 45, 96, 24), " Delete(Z)", Vars_Func.GUI_ElementTyp.FrameHUD);
            delete.Frame = deleteFrame;
            GUI_Element deleteLeftChain = new GUI_Element(new Rectangle(delete.Rectangle.X - 5, delete.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            delete.LeftChain = deleteLeftChain;
            GUI_Element deleteRightChain = new GUI_Element(new Rectangle(delete.Rectangle.X + 69 - 12 + 5, delete.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            delete.RightChain = deleteRightChain;
            buttons.Add(delete);
            // Add upgrade buttons
            GUI_Element upgrade = new GUI_Element(new Rectangle(31, 597, 69, 60), "", Vars_Func.GUI_ElementTyp.Upgrade);
            upgrade.Visable = true;
            upgrade.Highlightable = true;
            upgrade.Enabled = false;
            GUI_Element upgradeFrame = new GUI_Element(new Rectangle(upgrade.Rectangle.X - 11, upgrade.Rectangle.Y + 45, 96, 24), " Upgrade", Vars_Func.GUI_ElementTyp.FrameHUD);
            upgrade.Frame = upgradeFrame;
            GUI_Element upgradeLeftChain = new GUI_Element(new Rectangle(upgrade.Rectangle.X - 5, upgrade.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            upgrade.LeftChain = upgradeLeftChain;
            GUI_Element upgradeRightChain = new GUI_Element(new Rectangle(upgrade.Rectangle.X + 69 - 12 + 5, upgrade.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            upgrade.RightChain = upgradeRightChain;

            GUI_Element upgradeTopPole = new GUI_Element(new Rectangle(upgrade.Rectangle.X, upgrade.Rectangle.Y + 4, 390, 8), "", Vars_Func.GUI_ElementTyp.Pole);
            upgradeTopPole.Visable = false;
            upgrade.TopPole = upgradeTopPole;
            GUI_Element upgradeBottomPole = new GUI_Element(new Rectangle(upgrade.Rectangle.X, upgrade.Rectangle.Y + 85 - 4, 390, 8), "", Vars_Func.GUI_ElementTyp.Pole);
            upgradeBottomPole.Visable = false;
            upgrade.BottomPole = upgradeBottomPole;

            // Add live flag
            GUI_Element speed = new GUI_Element(new Rectangle(upgrade.Rectangle.X + 300, 597, 69, 60), "", Vars_Func.GUI_ElementTyp.SpeedUpgrade);
            speed.Visable = false;
            speed.Highlightable = true;
            GUI_Element speedFrame = new GUI_Element(new Rectangle(speed.Rectangle.X - 11, speed.Rectangle.Y + 45, 96, 24), "   Speed", Vars_Func.GUI_ElementTyp.FrameHUD);
            speedFrame.Visable = false;
            speed.Frame = speedFrame;
            GUI_Element speedLeftChain = new GUI_Element(new Rectangle(speed.Rectangle.X - 5, speed.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            speed.LeftChain = speedLeftChain;
            GUI_Element speedRightChain = new GUI_Element(new Rectangle(speed.Rectangle.X + 69 - 12 + 5, speed.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            speed.RightChain = speedRightChain;
            upgrade.Children.Add(speed);
            // Add live flag
            GUI_Element live = new GUI_Element(new Rectangle(upgrade.Rectangle.X + 200, 597, 69, 60), "", Vars_Func.GUI_ElementTyp.LifeUpgrade);
            live.Visable = false;
            live.Highlightable = true;
            GUI_Element liveFrame = new GUI_Element(new Rectangle(live.Rectangle.X - 11, live.Rectangle.Y + 45, 96, 24), "    Live", Vars_Func.GUI_ElementTyp.FrameHUD);
            liveFrame.Visable = false;
            live.Frame = liveFrame;
            GUI_Element liveLeftChain = new GUI_Element(new Rectangle(live.Rectangle.X - 5, live.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            live.LeftChain = liveLeftChain;
            GUI_Element liveRightChain = new GUI_Element(new Rectangle(live.Rectangle.X + 69 - 12 + 5, live.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            live.RightChain = liveRightChain;
            upgrade.Children.Add(live);
            // Add demage flag
            GUI_Element demage = new GUI_Element(new Rectangle(upgrade.Rectangle.X + 100, 597, 69, 60), "", Vars_Func.GUI_ElementTyp.DamageUpgrade);
            demage.Visable = false;
            demage.Highlightable = true;
            GUI_Element demageFrame = new GUI_Element(new Rectangle(demage.Rectangle.X - 11, demage.Rectangle.Y + 45, 96, 24), "  Demage", Vars_Func.GUI_ElementTyp.FrameHUD);
            demageFrame.Visable = false;
            demage.Frame = demageFrame;
            GUI_Element demageLeftChain = new GUI_Element(new Rectangle(demage.Rectangle.X - 5, demage.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            demage.LeftChain = demageLeftChain;
            GUI_Element demageRightChain = new GUI_Element(new Rectangle(demage.Rectangle.X + 69 - 12 + 5, demage.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            demage.RightChain = demageRightChain;
            upgrade.Children.Add(demage);
            upgradeButtons.Add(upgrade);

            // Add build-up buttons
            GUI_Element build = new GUI_Element(new Rectangle(31, 675, 69, 60), "", Vars_Func.GUI_ElementTyp.Build);
            build.Highlightable = true;
            GUI_Element buildFrame = new GUI_Element(new Rectangle(build.Rectangle.X - 11, build.Rectangle.Y + 45, 96, 24), " Build(N)", Vars_Func.GUI_ElementTyp.FrameHUD);
            build.Frame = buildFrame;

            GUI_Element buildLeftChain = new GUI_Element(new Rectangle(build.Rectangle.X - 5, build.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            build.LeftChain = buildLeftChain;
            GUI_Element buildRightChain = new GUI_Element(new Rectangle(build.Rectangle.X + 69 - 12 + 5, build.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            build.RightChain = buildRightChain;

            GUI_Element buildTopPole = new GUI_Element(new Rectangle(build.Rectangle.X, build.Rectangle.Y + 4, 580, 8), "", Vars_Func.GUI_ElementTyp.Pole);
            buildTopPole.Visable = false;
            build.TopPole = buildTopPole;
            GUI_Element buildBottomPole = new GUI_Element(new Rectangle(build.Rectangle.X, build.Rectangle.Y + 85 - 4, 580, 8), "", Vars_Func.GUI_ElementTyp.Pole);
            buildBottomPole.Visable = false;
            build.BottomPole = buildBottomPole;

            // Add entrance
            GUI_Element entrance = new GUI_Element(new Rectangle(build.Rectangle.X + 500, 675, 69, 60), "", Vars_Func.GUI_ElementTyp.Entrance);
            entrance.Visable = false;
            entrance.Highlightable = true;
            GUI_Element entranceFrame = new GUI_Element(new Rectangle(entrance.Rectangle.X - 11, entrance.Rectangle.Y + 45, 96, 24), "  Entrance", Vars_Func.GUI_ElementTyp.FrameHUD);
            entranceFrame.Visable = false;
            entrance.Frame = entranceFrame;
            GUI_Element entranceLeftChain = new GUI_Element(new Rectangle(entrance.Rectangle.X - 5, entrance.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            entrance.LeftChain = entranceLeftChain;
            GUI_Element entranceRightChain = new GUI_Element(new Rectangle(entrance.Rectangle.X + 69 - 12 + 5, entrance.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            entrance.RightChain = entranceRightChain;
            build.Children.Add(entrance);
            // Add tempel building
            GUI_Element tempel = new GUI_Element(new Rectangle(build.Rectangle.X + 400, 675, 69, 60), "", Vars_Func.GUI_ElementTyp.Temple);
            tempel.Visable = false;
            tempel.Highlightable = true;
            GUI_Element tempelFrame = new GUI_Element(new Rectangle(tempel.Rectangle.X - 11, tempel.Rectangle.Y + 45, 96, 24), "  Tempel", Vars_Func.GUI_ElementTyp.FrameHUD);
            tempelFrame.Visable = false;
            tempel.Frame = tempelFrame;
            GUI_Element tempelLeftChain = new GUI_Element(new Rectangle(tempel.Rectangle.X - 5, tempel.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            tempel.LeftChain = tempelLeftChain;
            GUI_Element tempelRightChain = new GUI_Element(new Rectangle(tempel.Rectangle.X + 69 - 12 + 5, tempel.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            tempel.RightChain = tempelRightChain;
            build.Children.Add(tempel);
            // Add farm building
            GUI_Element farm = new GUI_Element(new Rectangle(build.Rectangle.X + 300, 675, 69, 60), "", Vars_Func.GUI_ElementTyp.Farm);
            farm.Visable = false;
            farm.Highlightable = true;
            GUI_Element farmFrame = new GUI_Element(new Rectangle(farm.Rectangle.X - 11, farm.Rectangle.Y + 45, 96, 24), "  Farm", Vars_Func.GUI_ElementTyp.FrameHUD);
            farmFrame.Visable = false;
            farm.Frame = farmFrame;
            GUI_Element farmLeftChain = new GUI_Element(new Rectangle(farm.Rectangle.X - 5, farm.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            farm.LeftChain = farmLeftChain;
            GUI_Element farmRightChain = new GUI_Element(new Rectangle(farm.Rectangle.X + 69 - 12 + 5, farm.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            farm.RightChain = farmRightChain;
            build.Children.Add(farm);
            // Add graveyard building
            GUI_Element graveyard = new GUI_Element(new Rectangle(build.Rectangle.X + 200, 675, 69, 60), "", Vars_Func.GUI_ElementTyp.Graveyard);
            graveyard.Visable = false;
            graveyard.Highlightable = true;
            GUI_Element graveyardFrame = new GUI_Element(new Rectangle(graveyard.Rectangle.X - 11, graveyard.Rectangle.Y + 45, 96, 24), " Skeleton", Vars_Func.GUI_ElementTyp.FrameHUD);
            graveyardFrame.Visable = false;
            graveyard.Frame = graveyardFrame;
            GUI_Element graveyardLeftChain = new GUI_Element(new Rectangle(graveyard.Rectangle.X - 5, graveyard.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            graveyard.LeftChain = graveyardLeftChain;
            GUI_Element graveyardRightChain = new GUI_Element(new Rectangle(graveyard.Rectangle.X + 69 - 12 + 5, graveyard.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            graveyard.RightChain = graveyardRightChain;
            build.Children.Add(graveyard);
            // Add nest building
            GUI_Element nest = new GUI_Element(new Rectangle(build.Rectangle.X + 100, 675, 69, 60), "", Vars_Func.GUI_ElementTyp.Nest);
            nest.Visable = false;
            nest.Highlightable = true;
            GUI_Element nestFrame = new GUI_Element(new Rectangle(nest.Rectangle.X - 11, nest.Rectangle.Y + 45, 96, 24), "    Ant", Vars_Func.GUI_ElementTyp.FrameHUD);
            nestFrame.Visable = false;
            nest.Frame = nestFrame;
            GUI_Element nestLeftChain = new GUI_Element(new Rectangle(nest.Rectangle.X - 5, nest.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            nest.LeftChain = nestLeftChain;
            GUI_Element nestRightChain = new GUI_Element(new Rectangle(nest.Rectangle.X + 69 - 12 + 5, nest.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            nest.RightChain = nestRightChain;
            build.Children.Add(nest);

            // Finaliy add the build-button 
            buttons.Add(build);

            //buildButtons.Add(tempel);
            //buildButtons.Add(farm);
            //buildButtons.Add(graveyard);
            //buildButtons.Add(nest);
            //buildButtons.Add(new GUI_Element(new Rectangle(1000, 680, 88, 76), "Entrance", Vars_Func.GUI_ElementTyp.Entrance));


            GUI_Element fireball = new GUI_Element(new Rectangle(31 + 110, 285, 69, 60), "", Vars_Func.GUI_ElementTyp.Fireball);
            fireball.Highlightable = true;
            GUI_Element fireballFrame = new GUI_Element(new Rectangle(fireball.Rectangle.X - 11, fireball.Rectangle.Y + 45, 96, 24), "  Fireball", Vars_Func.GUI_ElementTyp.FrameHUD);
            fireball.Frame = fireballFrame;
            GUI_Element fireballLeftChain = new GUI_Element(new Rectangle(fireball.Rectangle.X - 5, fireball.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            fireball.LeftChain = fireballLeftChain;
            GUI_Element fireballRightChain = new GUI_Element(new Rectangle(fireball.Rectangle.X + 69 - 12 + 5, fireball.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            fireball.RightChain = fireballRightChain;
            buttons.Add(fireball);
            // Add room button
            GUI_Element imp = new GUI_Element(new Rectangle(31 + 110, 363, 69, 60), "", Vars_Func.GUI_ElementTyp.SummonImp);
            imp.Highlightable = true;
            GUI_Element impFrame = new GUI_Element(new Rectangle(imp.Rectangle.X - 11, imp.Rectangle.Y + 45, 96, 24), "   Imp", Vars_Func.GUI_ElementTyp.FrameHUD);
            imp.Frame = impFrame;
            GUI_Element impLeftChain = new GUI_Element(new Rectangle(imp.Rectangle.X - 5, imp.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            imp.LeftChain = impLeftChain;
            GUI_Element impRightChain = new GUI_Element(new Rectangle(imp.Rectangle.X + 69 - 12 + 5, imp.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            imp.RightChain = impRightChain;
            buttons.Add(imp);



            //buttons.Add(new GUI_Element(new Rectangle(80, 680, 88, 76), "  Mine(M)", Vars_Func.GUI_ElementTyp.Mine));
            //buttons.Add(new GUI_Element(new Rectangle(180, 680, 88, 76), "  Room(R)", Vars_Func.GUI_ElementTyp.Room));
            //buttons.Add(new GUI_Element(new Rectangle(280, 680, 88, 76), "  Merge(T)", Vars_Func.GUI_ElementTyp.MergeRoom));
            //buttons.Add(new GUI_Element(new Rectangle(380, 680, 88, 76), "  Delete(Z)", Vars_Func.GUI_ElementTyp.DeleteRoom));
            //buttons.Add(new GUI_Element(new Rectangle(480, 680, 88, 76), "  Nest(N)", Vars_Func.GUI_ElementTyp.Build));
            //buttons.Add(new GUI_Element(new Rectangle(10, 110, 88, 76), "  Fireball", Vars_Func.GUI_ElementTyp.Fireball));
            //buttons.Add(new GUI_Element(new Rectangle(110, 110, 88, 76), "  Imp", Vars_Func.GUI_ElementTyp.SummonImp));

            //upgradeButtons.Add(new GUI_Element(new Rectangle(600, 680, 88, 76), "Dmg", Vars_Func.GUI_ElementTyp.DamageUpgrade));
            //upgradeButtons.Add(new GUI_Element(new Rectangle(700, 680, 88, 76), "Live", Vars_Func.GUI_ElementTyp.LifeUpgrade));
            //upgradeButtons.Add(new GUI_Element(new Rectangle(800, 680, 88, 76), "Speed", Vars_Func.GUI_ElementTyp.SpeedUpgrade));

            //buildButtons.Add(new GUI_Element(new Rectangle(600, 680, 88, 76), "Ants", Vars_Func.GUI_ElementTyp.PlaceAnts));
            //buildButtons.Add(new GUI_Element(new Rectangle(700, 680, 88, 76), "Skeletons", Vars_Func.GUI_ElementTyp.PlaceSkeletons));
            //buildButtons.Add(new GUI_Element(new Rectangle(800, 680, 88, 76), "Farm", Vars_Func.GUI_ElementTyp.PlaceFarm));
            //buildButtons.Add(new GUI_Element(new Rectangle(900, 680, 88, 76), "Temple", Vars_Func.GUI_ElementTyp.PlaceTemple));
            //buildButtons.Add(new GUI_Element(new Rectangle(1000, 680, 88, 76), "Entrance", Vars_Func.GUI_ElementTyp.PlaceEntrance));

            tutorials.Add(new GUI_Element(new Rectangle(500, 300, 400, 400), "", Vars_Func.GUI_ElementTyp.GUI_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(500, 300, 400, 400), "", Vars_Func.GUI_ElementTyp.HQCreature_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(500, 300, 400, 400), "", Vars_Func.GUI_ElementTyp.Creature_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(500, 300, 400, 400), "", Vars_Func.GUI_ElementTyp.Minimap_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(500, 300, 400, 400), "", Vars_Func.GUI_ElementTyp.Nest_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(500, 300, 400, 400), "", Vars_Func.GUI_ElementTyp.PlaceNest_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(500, 300, 400, 400), "", Vars_Func.GUI_ElementTyp.Resources_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(500, 300, 400, 400), "", Vars_Func.GUI_ElementTyp.Upgrades_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(500, 300, 400, 400), "", Vars_Func.GUI_ElementTyp.Wavetimer_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(500, 300, 400, 400), "", Vars_Func.GUI_ElementTyp.Spells_Tutorial));

            tutorialButtons.Add(new GUI_Element(new Rectangle(570, 650, 20, 20), "", Vars_Func.GUI_ElementTyp.GUI_TutorialButton));
            tutorialButtons.Add(new GUI_Element(new Rectangle(1320, 600, 20, 20), "", Vars_Func.GUI_ElementTyp.HQCreature_TutorialButton));
            tutorialButtons.Add(new GUI_Element(new Rectangle(1320, 600, 20, 20), "", Vars_Func.GUI_ElementTyp.Creature_TutorialButton));
            tutorialButtons.Add(new GUI_Element(new Rectangle(1320, 280, 20, 20), "", Vars_Func.GUI_ElementTyp.Minimap_TutorialButton));
            tutorialButtons.Add(new GUI_Element(new Rectangle(1320, 600, 20, 20), "", Vars_Func.GUI_ElementTyp.Nest_TutorialButton));
            tutorialButtons.Add(new GUI_Element(new Rectangle(1090, 650, 20, 20), "", Vars_Func.GUI_ElementTyp.PlaceNest_TutorialButton));
            tutorialButtons.Add(new GUI_Element(new Rectangle(310, 10, 20, 20), "", Vars_Func.GUI_ElementTyp.Resources_TutorialButton));
            tutorialButtons.Add(new GUI_Element(new Rectangle(1090, 650, 20, 20), "", Vars_Func.GUI_ElementTyp.Upgrades_TutorialButton));
            tutorialButtons.Add(new GUI_Element(new Rectangle(1070, 80, 20, 20), "", Vars_Func.GUI_ElementTyp.Wavetimer_TutorialButton));
            tutorialButtons.Add(new GUI_Element(new Rectangle(210, 110, 20, 20), "", Vars_Func.GUI_ElementTyp.Spells_TutorialButton));
        }


        #region Properties
        public static GUI_Element getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp typ)
        {
            foreach (GUI_Element b in tutorialButtons)
            {
                if (b.ElementTyp == typ) return b;
            }
            return null;
        }
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
                foreach (GUI_Element c in b.Children)
                {
                    if (c.ElementTyp == typ) return c;
                }
            }
            return null;
        }
        public static GUI_Element getGUI_Button(Vars_Func.GUI_ElementTyp typ)
        {
            foreach (GUI_Element b in buttons)
            {
                if (b.ElementTyp == typ) return b;
                foreach (GUI_Element c in b.Children)
                {
                    if (c.ElementTyp == typ) return c;
                }
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

        #region Update
        public static void update(GameTime time, Environment.Map map, MouseState mouseState)
        {
            foreach (GUI_Element u in upgradeButtons)
            {
                u.Update(time, map, mouseState);
            }

            foreach (GUI_Element b in buttons)
            {
                b.Update(time, map, mouseState);
            }
        }
        #endregion

        //    public static void Draw(SpriteBatch spriteBatch, SpriteFont font, MouseState mouseState, Renderer.Camera camera, Environment.Map map)
        //    {
        //        if (Interaction.GameState == Vars_Func.GameState.MainMenu)
        //        {
        //            //draw the meunElements
        //            foreach (GUI_Element e in menuElements)
        //            {
        //                e.Draw(spriteBatch, font, Color.White);
        //            }
        //            //draw the menuButtons
        //            foreach (GUI_Element b in menuButtons)
        //            {
        //                if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
        //                {
        //                    b.Draw(spriteBatch, font, Color.Gray);
        //                }
        //                else
        //                {
        //                    b.Draw(spriteBatch, font, Color.White);
        //                }
        //            }
        //        }
        //        else if(Interaction.GameState == Vars_Func.GameState.Highscore)
        //        {

        //        }
        //        else if (Interaction.GameState == Vars_Func.GameState.Tutorial)
        //        {

        //        }
        //        else
        //        {
        //            if (Interaction.GameState == Vars_Func.GameState.ReturnToMainMenu)
        //            {
        //                //draw the returnScreenElements
        //                foreach (GUI_Element e in returnScreenElements)
        //                {
        //                    e.Draw(spriteBatch, font, Color.White);
        //                }
        //                //draw the returnScreenButtons
        //                foreach (GUI_Element b in returnScreenButtons)
        //                {
        //                    if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
        //                    {
        //                        b.Draw(spriteBatch, font, Color.Gray);
        //                    }
        //                    else
        //                    {
        //                        b.Draw(spriteBatch, font, Color.White);
        //                    }
        //                }
        //            }
        //            //draw the TutorialElements
        //            switch (Interaction.GameState)
        //            {
        //                case Vars_Func.GameState.GUI_Tutorial:
        //                    foreach (GUI_Element e in tutorials)
        //                    {
        //                        if(e.ElementTyp == Vars_Func.GUI_ElementTyp.GUI_Tutorial) e.Draw(spriteBatch, font, Color.White);
        //                    }
        //                    break;
        //                case Vars_Func.GameState.HQCreature_Tutorial:
        //                    foreach (GUI_Element e in tutorials)
        //                    {
        //                        if (e.ElementTyp == Vars_Func.GUI_ElementTyp.HQCreature_Tutorial) e.Draw(spriteBatch, font, Color.White);
        //                    }
        //                    break;
        //                case Vars_Func.GameState.Creature_Tutorial:
        //                    foreach (GUI_Element e in tutorials)
        //                    {
        //                        if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Creature_Tutorial) e.Draw(spriteBatch, font, Color.White);
        //                    }
        //                    break;
        //                case Vars_Func.GameState.Minimap_Tutorial:
        //                    foreach (GUI_Element e in tutorials)
        //                    {
        //                        if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Minimap_Tutorial) e.Draw(spriteBatch, font, Color.White);
        //                    }
        //                    break;
        //                case Vars_Func.GameState.Nest_Tutorial:
        //                    foreach (GUI_Element e in tutorials)
        //                    {
        //                        if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Nest_Tutorial) e.Draw(spriteBatch, font, Color.White);
        //                    }
        //                    break;
        //                case Vars_Func.GameState.PlaceNest_Tutorial:
        //                    foreach (GUI_Element e in tutorials)
        //                    {
        //                        if (e.ElementTyp == Vars_Func.GUI_ElementTyp.PlaceNest_Tutorial) e.Draw(spriteBatch, font, Color.White);
        //                    }
        //                    break;
        //                case Vars_Func.GameState.Resources_Tutorial:
        //                    foreach (GUI_Element e in tutorials)
        //                    {
        //                        if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Resources_Tutorial) e.Draw(spriteBatch, font, Color.White);
        //                    }
        //                    break;
        //                case Vars_Func.GameState.Upgrades_Tutorial:
        //                    foreach (GUI_Element e in tutorials)
        //                    {
        //                        if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Upgrades_Tutorial) e.Draw(spriteBatch, font, Color.White);
        //                    }
        //                    break;
        //                case Vars_Func.GameState.Wavetimer_Tutorial:
        //                    foreach (GUI_Element e in tutorials)
        //                    {
        //                        if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Wavetimer_Tutorial) e.Draw(spriteBatch, font, Color.White);
        //                    }
        //                    break;
        //                case Vars_Func.GameState.Spells_Tutorial:
        //                    foreach (GUI_Element e in tutorials)
        //                    {
        //                        if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Spells_Tutorial) e.Draw(spriteBatch, font, Color.White);
        //                    }
        //                    break;
        //            }
        //            //draw the GUI_elements
        //            foreach (GUI_Element e in elements)
        //            {
        //                e.Draw(spriteBatch, font, Color.White);
        //            }
        //            //draw the buttons
        //            foreach (GUI_Element b in buttons)
        //            {
        //                if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
        //                {
        //                    b.Draw(spriteBatch, font, Color.Gray);
        //                }
        //                else
        //                {
        //                    b.Draw(spriteBatch, font, Color.White);
        //                }
        //            }
        //            //draw the tutorialButtons that are visible everytime
        //            foreach (GUI_Element b in tutorialButtons)
        //            {
        //                if (b.ElementTyp == Vars_Func.GUI_ElementTyp.GUI_TutorialButton ||
        //                    b.ElementTyp == Vars_Func.GUI_ElementTyp.Minimap_TutorialButton ||
        //                    b.ElementTyp == Vars_Func.GUI_ElementTyp.Resources_TutorialButton ||
        //                    b.ElementTyp == Vars_Func.GUI_ElementTyp.Spells_TutorialButton ||
        //                    b.ElementTyp == Vars_Func.GUI_ElementTyp.Wavetimer_TutorialButton)
        //                {
        //                    if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
        //                    {
        //                        b.Draw(spriteBatch, font, Color.Gray);
        //                    }
        //                    else
        //                    {
        //                        b.Draw(spriteBatch, font, Color.White);
        //                    }
        //                }
        //            }
        //            if (Interaction.GameState == Vars_Func.GameState.Build ||
        //                Interaction.GameState == Vars_Func.GameState.PlaceAnts ||
        //                Interaction.GameState == Vars_Func.GameState.PlaceSkeletons ||
        //                Interaction.GameState == Vars_Func.GameState.PlaceFarm ||
        //                Interaction.GameState == Vars_Func.GameState.PlaceTemple ||
        //                Interaction.GameState == Vars_Func.GameState.PlaceEntrance)
        //            {
        //                //draw the tutorialButton for buildButtons
        //                foreach (GUI_Element b in tutorialButtons)
        //                {
        //                    if (b.ElementTyp == Vars_Func.GUI_ElementTyp.PlaceNest_TutorialButton)
        //                    {
        //                        if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
        //                        {
        //                            b.Draw(spriteBatch, font, Color.Gray);
        //                        }
        //                        else
        //                        {
        //                            b.Draw(spriteBatch, font, Color.White);
        //                        }
        //                    }
        //                }
        //                //draw the buildButtons
        //                foreach (GUI_Element b in buildButtons)
        //                {
        //                    if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
        //                    {
        //                        b.Draw(spriteBatch, font, Color.Gray);
        //                    }
        //                    else
        //                    {
        //                        b.Draw(spriteBatch, font, Color.White);
        //                    }
        //                }
        //                //draw the costs for the buildings
        //                spriteBatch.DrawString(font, Interaction.NestCost.ToString(), new Vector2(620, 740), Color.Black);
        //                spriteBatch.DrawString(font, Interaction.NestCost.ToString(), new Vector2(720, 740), Color.Black);
        //                spriteBatch.DrawString(font, Interaction.FarmCost.ToString(), new Vector2(820, 740), Color.Black);
        //                spriteBatch.DrawString(font, Interaction.TempleCost.ToString(), new Vector2(920, 740), Color.Black);
        //                spriteBatch.DrawString(font, Interaction.EntranceCost.ToString(), new Vector2(1020, 740), Color.Black);
        //            }
        //            //draw the player ressources
        //            spriteBatch.DrawString(font, Player.Gold.ToString(), new Vector2(20, 70), Color.Black);
        //            spriteBatch.DrawString(font, Player.Mana.ToString(), new Vector2(120, 70), Color.Black);
        //            spriteBatch.DrawString(font, Player.Food.ToString(), new Vector2(220, 70), Color.Black);
        //            //draw the spellcosts
        //            spriteBatch.DrawString(font, Spells.FireballCost.ToString(), new Vector2(30, 170), Color.Black);
        //            spriteBatch.DrawString(font, Spells.SummonImpCost.ToString(), new Vector2(130, 170), Color.Black);
        //            //draw different values for other types of selected objects
        //            #region Selected Object
        //            switch (selectedThingTyp)
        //            {
        //                case Vars_Func.ThingTyp.Wall:
        //                    if (wall != null)
        //                    {
        //                        spriteBatch.DrawString(font, "Typ: " + wall.Typ.ToString(), new Vector2(1121, 600), Color.Black);
        //                        spriteBatch.DrawString(font, "HP: " + wall.HP.ToString(), new Vector2(1121, 620), Color.Black);
        //                        spriteBatch.DrawString(font, "Gold: " + wall.Gold.ToString(), new Vector2(1121, 640), Color.Black);
        //                    }
        //                    break;
        //                case Vars_Func.ThingTyp.Nest:
        //                    if (nest.Typ != Vars_Func.NestTyp.Entrance)
        //                    {
        //                        //draw the tutorialButton for upgradeButtons and nests
        //                        foreach (GUI_Element b in tutorialButtons)
        //                        {
        //                            if (b.ElementTyp == Vars_Func.GUI_ElementTyp.Upgrades_TutorialButton ||
        //                                b.ElementTyp == Vars_Func.GUI_ElementTyp.Nest_TutorialButton)
        //                            {
        //                                if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
        //                                {
        //                                    b.Draw(spriteBatch, font, Color.Gray);
        //                                }
        //                                else
        //                                {
        //                                    b.Draw(spriteBatch, font, Color.White);
        //                                }
        //                            }
        //                        }
        //                        //draw the upgradeButtons
        //                        foreach (GUI_Element b in upgradeButtons)
        //                        {
        //                            if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
        //                            {
        //                                b.Draw(spriteBatch, font, Color.Gray);
        //                            }
        //                            else
        //                            {
        //                                b.Draw(spriteBatch, font, Color.White);
        //                            }
        //                        }
        //                        spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(1121, 600), Color.Black);
        //                        spriteBatch.DrawString(font, "Nutrition: " + nest.Nutrition.ToString() + "/" + nest.MaxNutrition.ToString(), new Vector2(1121, 620), Color.Black);
        //                        spriteBatch.DrawString(font, "Upgrades:", new Vector2(1121, 640), Color.Black);
        //                        spriteBatch.DrawString(font, "Dmg: " + nest.UpgradeCount[0], new Vector2(1121, 660), Color.Black);
        //                        spriteBatch.DrawString(font, "Live: " + nest.UpgradeCount[1], new Vector2(1121, 680), Color.Black);
        //                        spriteBatch.DrawString(font, "Speed: " + nest.UpgradeCount[2], new Vector2(1121, 700), Color.Black);
        //                        //draw the upgradecosts
        //                        spriteBatch.DrawString(font, nest.NextUpgradeCost.ToString(), new Vector2(620, 740), Color.Black);
        //                        spriteBatch.DrawString(font, nest.NextUpgradeCost.ToString(), new Vector2(720, 740), Color.Black);
        //                        spriteBatch.DrawString(font, nest.NextUpgradeCost.ToString(), new Vector2(820, 740), Color.Black);
        //                    }
        //                    else
        //                    {
        //                        spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(1121, 600), Color.Black);
        //                        spriteBatch.DrawString(font, "Start Age: ", new Vector2(1121, 620), Color.Black);
        //                    }
        //                    break;
        //                case Vars_Func.ThingTyp.DungeonCreature:
        //                    if (creature != null)
        //                    {
        //                        //draw the tutorialButton for creatures
        //                        foreach (GUI_Element b in tutorialButtons)
        //                        {
        //                            if (b.ElementTyp == Vars_Func.GUI_ElementTyp.Creature_TutorialButton)
        //                            {
        //                                if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
        //                                {
        //                                    b.Draw(spriteBatch, font, Color.Gray);
        //                                }
        //                                else
        //                                {
        //                                    b.Draw(spriteBatch, font, Color.White);
        //                                }
        //                            }
        //                        }
        //                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1121, 600), Color.Black);
        //                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1121, 620), Color.Black);
        //                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(1121, 640), Color.Black);
        //                        spriteBatch.DrawString(font, "Age: " + ((int)creature.Age).ToString(), new Vector2(1121, 660), Color.Black);
        //                    }
        //                    break;
        //                case Vars_Func.ThingTyp.HeroCreature:
        //                    if (creature != null)
        //                    {
        //                        //draw the tutorialButton for creatures
        //                        foreach (GUI_Element b in tutorialButtons)
        //                        {
        //                            if (b.ElementTyp == Vars_Func.GUI_ElementTyp.Creature_TutorialButton)
        //                            {
        //                                if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
        //                                {
        //                                    b.Draw(spriteBatch, font, Color.Gray);
        //                                }
        //                                else
        //                                {
        //                                    b.Draw(spriteBatch, font, Color.White);
        //                                }
        //                            }
        //                        }
        //                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1121, 600), Color.Black);
        //                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1121, 620), Color.Black);
        //                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(1121, 640), Color.Black);
        //                        spriteBatch.DrawString(font, "Age: " + ((int)creature.Age).ToString(), new Vector2(1121, 660), Color.Black);
        //                    }
        //                    break;
        //                case Vars_Func.ThingTyp.NeutralCreature:
        //                    if (creature != null)
        //                    {
        //                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1121, 600), Color.Black);
        //                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1121, 620), Color.Black);
        //                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(1121, 640), Color.Black);
        //                    }
        //                    break;
        //                case Vars_Func.ThingTyp.HQCreature:
        //                    if (creature != null)
        //                    {
        //                        //draw the tutorialButton for the HQCreature
        //                        foreach (GUI_Element b in tutorialButtons)
        //                        {
        //                            if (b.ElementTyp == Vars_Func.GUI_ElementTyp.HQCreature_TutorialButton)
        //                            {
        //                                if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
        //                                {
        //                                    b.Draw(spriteBatch, font, Color.Gray);
        //                                }
        //                                else
        //                                {
        //                                    b.Draw(spriteBatch, font, Color.White);
        //                                }
        //                            }
        //                        }
        //                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1121, 600), Color.Black);
        //                        spriteBatch.DrawString(font, "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1121, 620), Color.Black);
        //                        spriteBatch.DrawString(font, "Damage: " + creature.Damage.ToString(), new Vector2(1121, 640), Color.Black);
        //                    }
        //                    break;
        //                case Vars_Func.ThingTyp.length:
        //                    break;
        //            }
        //            #endregion
        //        }
        //    }
        //}
        
        #region Draw
        public static void Draw(SpriteBatch spriteBatch, SpriteFont font, Minimap minimap, Vector2 indexOfMiddleHexagon)
        {
            //draw the GUI_elements
            foreach (GUI_Element e in elements)
            {
                e.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold));
                if (e.ElementTyp == Vars_Func.GUI_ElementTyp.BackgroundHUD)
                {
                    minimap.drawMinimap(spriteBatch, indexOfMiddleHexagon);
                }
            }
            //draw the buttons
            foreach (GUI_Element b in buttons)
            {
                b.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.Augusta));
                if (b.ElementTyp == Vars_Func.GUI_ElementTyp.DeleteRoom)
                {
                    //draw the upgradeButtons
                    foreach (GUI_Element u in upgradeButtons)
                    {
                        u.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.Augusta));
                    }
                }
            }

            foreach (GUI_Element b in tutorialButtons)
            {
                if (b.ElementTyp == Vars_Func.GUI_ElementTyp.GUI_TutorialButton ||
                    b.ElementTyp == Vars_Func.GUI_ElementTyp.Minimap_TutorialButton ||
                    b.ElementTyp == Vars_Func.GUI_ElementTyp.Resources_TutorialButton ||
                    b.ElementTyp == Vars_Func.GUI_ElementTyp.Spells_TutorialButton ||
                    b.ElementTyp == Vars_Func.GUI_ElementTyp.Wavetimer_TutorialButton)
                {
                    //if (b.Rectangle.Contains(mouseState.X, mouseState.Y))
                    //{
                        b.Draw(spriteBatch, font);
                    //}
                    //else
                    //{
                    //    b.Draw(spriteBatch, font);
                    //}
                }
            }

            //draw the player ressources
            spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold), "Gold: " + Player.Gold, new Vector2(120, 50), Color.Black);
            spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold), "Mana: " + Player.Mana, new Vector2(120, 125), Color.Black);
            spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold), "Food: " + Player.Food, new Vector2(110, 200), Color.Black);

            bool enableUpgrades = false;
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
                        enableUpgrades = true;
                        spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(1121, 600), Color.Black);
                        spriteBatch.DrawString(font, "Nutrition: " + nest.Nutrition.ToString() + "/" + nest.MaxNutrition.ToString(), new Vector2(1121, 620), Color.Black);
                        spriteBatch.DrawString(font, "Upgrades:", new Vector2(1121, 640), Color.Black);
                        spriteBatch.DrawString(font, "Damage: " + nest.UpgradeCount[0], new Vector2(1121, 660), Color.Black);
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

            if (enableUpgrades)
            {
                // Enable the upgrade buttons
                foreach (GUI_Element b in upgradeButtons)
                {
                    if (!b.Enabled)
                    {
                        b.Enabled = true;
                    }
                }
            }
            else
            {
                // Disable the upgrade buttons
                foreach (GUI_Element b in upgradeButtons)
                {
                    if (b.Enabled)
                    {
                        b.Enabled = false;
                    }
                }
            }
        }
        #endregion
    }
}
