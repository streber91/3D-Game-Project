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
        static List<GUI_Element> waveElements = new List<GUI_Element>();

        static GUI_Element tempTutorial = null, selectedTutorial;
        static bool isButtonPressed = false, alreadyDrawing = false, showHelp;
        static float tutorialLerpCounter = 0;

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
            elements.Add(new GUI_Element(new Rectangle(1090, 768 - 13 - 265, 260, 265), "", Vars_Func.GUI_ElementTyp.InfoHUD));
            elements.Add(new GUI_Element(new Rectangle(1090 - 260, 768 - 13 - 160, 260, 160), "", Vars_Func.GUI_ElementTyp.SecondInfoHUD));

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

            GUI_Element timer = new GUI_Element(new Rectangle(1090 - 144, 13, 144, 36), "", Vars_Func.GUI_ElementTyp.TextFieldSmall);
            timer.YBonus = 10;
            waveElements.Add(timer);

            GUI_Element wave = new GUI_Element(new Rectangle(1090 - 144 - 36, 13, 36, 36), "", Vars_Func.GUI_ElementTyp.InfoHUD);
            wave.YBonus = 10;
            waveElements.Add(wave);

            #region Controll Buttons
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

            #region Upgrades
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
            GUI_Element demageFrame = new GUI_Element(new Rectangle(demage.Rectangle.X - 11, demage.Rectangle.Y + 45, 96, 24), "  Damage", Vars_Func.GUI_ElementTyp.FrameHUD);
            demageFrame.Visable = false;
            demage.Frame = demageFrame;
            GUI_Element demageLeftChain = new GUI_Element(new Rectangle(demage.Rectangle.X - 5, demage.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            demage.LeftChain = demageLeftChain;
            GUI_Element demageRightChain = new GUI_Element(new Rectangle(demage.Rectangle.X + 69 - 12 + 5, demage.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            demage.RightChain = demageRightChain;
            upgrade.Children.Add(demage);
            buttons.Add(upgrade);

            upgradeButtons.Add(demage);
            upgradeButtons.Add(live);
            upgradeButtons.Add(speed);
            #endregion

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
            buildButtons.Add(tempel);
            buildButtons.Add(farm);
            buildButtons.Add(graveyard);
            buildButtons.Add(nest);
            buildButtons.Add(entrance);
            #endregion

            #region Spells
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
            GUI_Element impFrame = new GUI_Element(new Rectangle(imp.Rectangle.X - 11, imp.Rectangle.Y + 45, 96, 24), "       Imp", Vars_Func.GUI_ElementTyp.FrameHUD);
            imp.Frame = impFrame;
            GUI_Element impLeftChain = new GUI_Element(new Rectangle(imp.Rectangle.X - 5, imp.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.LeftChain);
            imp.LeftChain = impLeftChain;
            GUI_Element impRightChain = new GUI_Element(new Rectangle(imp.Rectangle.X + 69 - 12 + 5, imp.Rectangle.Y, 12, 90), "", Vars_Func.GUI_ElementTyp.RightChain);
            imp.RightChain = impRightChain;
            buttons.Add(imp);
            #endregion

            #region Tutorial-Text-Fields
            tutorials.Add(new GUI_Element(new Rectangle(13 + 260, 25, 637, 163), Player.loadString("Content/Tutorials/Main-GUI.txt"), Vars_Func.GUI_ElementTyp.GUI_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(13 + 260, 25, 637, 163), Player.loadString("Content/Tutorials/Head.txt"), Vars_Func.GUI_ElementTyp.HQCreature_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(13 + 260, 25, 637, 163), Player.loadString("Content/Tutorials/Creature.txt"), Vars_Func.GUI_ElementTyp.Creature_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(13 + 260, 25, 637, 163), Player.loadString("Content/Tutorials/Minimap.txt"), Vars_Func.GUI_ElementTyp.Minimap_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(13 + 260, 25, 637, 163), Player.loadString("Content/Tutorials/Nest.txt"), Vars_Func.GUI_ElementTyp.Nest_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(13 + 260, 25, 637, 163), Player.loadString("Content/Tutorials/Build.txt"), Vars_Func.GUI_ElementTyp.PlaceNest_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(13 + 260, 25, 637, 163), Player.loadString("Content/Tutorials/Reccources.txt"), Vars_Func.GUI_ElementTyp.Resources_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(13 + 260, 25, 637, 163), Player.loadString("Content/Tutorials/Upgrades.txt"), Vars_Func.GUI_ElementTyp.Upgrades_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(13 + 260, 25, 637, 163), Player.loadString("Content/Tutorials/Wavetimer.txt"), Vars_Func.GUI_ElementTyp.Wavetimer_Tutorial));
            tutorials.Add(new GUI_Element(new Rectangle(13 + 260, 25, 637, 163), Player.loadString("Content/Tutorials/Spells.txt"), Vars_Func.GUI_ElementTyp.Spells_Tutorial));
            #endregion
            
            #region Tutorials Buttons
            //Set the commen gui tutorial button
            GUI_Element GUI_t = new GUI_Element(new Rectangle(13, 278, 20, 20), " ?", Vars_Func.GUI_ElementTyp.GUI_TutorialButton);
            GUI_t.Highlightable = true;
            tutorialButtons.Add(GUI_t);

            //Set the hq creature tutorial button
            GUI_Element hq_t = new GUI_Element(new Rectangle(1310, 763 - 265 + 13, 20, 20), " ?", Vars_Func.GUI_ElementTyp.HQCreature_TutorialButton);
            hq_t.Highlightable = true;
            tutorialButtons.Add(hq_t);

            //Set the creature tutorial button
            GUI_Element cre_t = new GUI_Element(new Rectangle(1310, 763 - 265 + 13, 20, 20), " ?", Vars_Func.GUI_ElementTyp.Creature_TutorialButton);
            cre_t.Highlightable = true;
            tutorialButtons.Add(cre_t);

            //Set the minimap tutorial button
            GUI_Element map_t = new GUI_Element(new Rectangle(1366 - 13 - 17, 13 + 265 - 20 - 1, 20, 20), " ?", Vars_Func.GUI_ElementTyp.Minimap_TutorialButton);
            map_t.Highlightable = true;
            tutorialButtons.Add(map_t);

            //Set the nest tutorial button
            GUI_Element nest_t = new GUI_Element(new Rectangle(1310, 763 - 265 + 13, 20, 20), " ?", Vars_Func.GUI_ElementTyp.Nest_TutorialButton);
            nest_t.Highlightable = true;
            tutorialButtons.Add(nest_t);

            //Set the place-nest tutorial button
            GUI_Element pla_t = new GUI_Element(new Rectangle(0, 675, 20, 20), " ?", Vars_Func.GUI_ElementTyp.PlaceNest_TutorialButton);
            pla_t.Highlightable = true;
            tutorialButtons.Add(pla_t);

            //Set the ressource tutorial button
            GUI_Element res_t = new GUI_Element(new Rectangle(260 - 7, 13 + 265 - 20, 20, 20), " ?", Vars_Func.GUI_ElementTyp.Resources_TutorialButton);
            res_t.Highlightable = true;
            tutorialButtons.Add(res_t);

            //Set the upgrade tutorial button
            GUI_Element upg_t = new GUI_Element(new Rectangle(0, 597, 20, 20), " ?", Vars_Func.GUI_ElementTyp.Upgrades_TutorialButton);
            upg_t.Highlightable = true;
            tutorialButtons.Add(upg_t);

            //Set the wave tutorial button
            GUI_Element wave_t = new GUI_Element(new Rectangle(1090 - 20, 13 + 36, 20, 20), " ?", Vars_Func.GUI_ElementTyp.Wavetimer_TutorialButton);
            wave_t.Highlightable = true;
            tutorialButtons.Add(wave_t);

            //Set the spell tutorial button
            GUI_Element spell_t = new GUI_Element(new Rectangle(110 + 13, 278, 20, 20), " ?", Vars_Func.GUI_ElementTyp.Spells_TutorialButton);
            spell_t.Highlightable = true;
            tutorialButtons.Add(spell_t);
            #endregion
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
        public static bool IsPressed
        {
            get { return isButtonPressed; }
        }
        #endregion

        #region Update
        public static void update(GameTime time, Environment.Map map, MouseState mouseState)
        {
            showHelp = Setting_GUI.ShowHelp;
            if (showHelp)
            {
                foreach (GUI_Element t in tutorialButtons)
                {
                    t.Update(time, map, mouseState);
                }
            }

            foreach (GUI_Element u in upgradeButtons)
            {
                u.Update(time, map, mouseState);
            }

            bool pressed = false;
            foreach (GUI_Element b in buttons)
            {
                b.Update(time, map, mouseState);
                if (b.Pressed)
                {
                    pressed = true;
                }
            }

            if (pressed && !isButtonPressed)
            {
                isButtonPressed = true;
            }
            if (!pressed && isButtonPressed)
            {
                isButtonPressed = false; 
            }

             #region Select the right Tutorial-GUI-Element when chosen and store it in tempTutorial
            if (showHelp)
            {
                switch (Interaction.GameState)
                {
                    case Vars_Func.GameState.GUI_Tutorial:
                        foreach (GUI_Element e in tutorials)
                        {
                            if (e.ElementTyp == Vars_Func.GUI_ElementTyp.GUI_Tutorial) tempTutorial = e;
                        }
                        break;
                    case Vars_Func.GameState.HQCreature_Tutorial:
                        foreach (GUI_Element e in tutorials)
                        {
                            if (e.ElementTyp == Vars_Func.GUI_ElementTyp.HQCreature_Tutorial) tempTutorial = e;
                        }
                        break;
                    case Vars_Func.GameState.Creature_Tutorial:
                        foreach (GUI_Element e in tutorials)
                        {
                            if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Creature_Tutorial) tempTutorial = e;
                        }
                        break;
                    case Vars_Func.GameState.Minimap_Tutorial:
                        foreach (GUI_Element e in tutorials)
                        {
                            if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Minimap_Tutorial) tempTutorial = e;
                        }
                        break;
                    case Vars_Func.GameState.Nest_Tutorial:
                        foreach (GUI_Element e in tutorials)
                        {
                            if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Nest_Tutorial) tempTutorial = e;
                        }
                        break;
                    case Vars_Func.GameState.PlaceNest_Tutorial:
                        foreach (GUI_Element e in tutorials)
                        {
                            if (e.ElementTyp == Vars_Func.GUI_ElementTyp.PlaceNest_Tutorial) tempTutorial = e;
                        }
                        break;
                    case Vars_Func.GameState.Resources_Tutorial:
                        foreach (GUI_Element e in tutorials)
                        {
                            if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Resources_Tutorial) tempTutorial = e;
                        }
                        break;
                    case Vars_Func.GameState.Upgrades_Tutorial:
                        foreach (GUI_Element e in tutorials)
                        {
                            if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Upgrades_Tutorial) tempTutorial = e;
                        }
                        break;
                    case Vars_Func.GameState.Wavetimer_Tutorial:
                        foreach (GUI_Element e in tutorials)
                        {
                            if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Wavetimer_Tutorial) tempTutorial = e;
                        }
                        break;
                    case Vars_Func.GameState.Spells_Tutorial:
                        foreach (GUI_Element e in tutorials)
                        {
                            if (e.ElementTyp == Vars_Func.GUI_ElementTyp.Spells_Tutorial) tempTutorial = e;
                        }
                        break;
                    default:
                        tempTutorial = null;
                        break;
                }
            #endregion

                if (tempTutorial != null)
                {
                    // Move the old, selected tutorial text back 
                    if (selectedTutorial != null && selectedTutorial != tempTutorial)
                    {
                        tutorialLerpCounter -= (float)time.ElapsedGameTime.Milliseconds / 500;
                        if (tutorialLerpCounter < 0)
                        {
                            selectedTutorial = tempTutorial;
                            tutorialLerpCounter = 0;
                        }
                        float yValue = MathHelper.Lerp(-300, selectedTutorial.Rectangle.Y, tutorialLerpCounter);
                        selectedTutorial.CurrentRectangle = new Rectangle(selectedTutorial.Rectangle.X, (int)yValue, selectedTutorial.CurrentRectangle.Width, selectedTutorial.CurrentRectangle.Height);
                    }
                    //Some start conditions
                    if (selectedTutorial == null)
                    {
                        selectedTutorial = tempTutorial;
                    }
                    //Move the selected tutorial text the right position
                    if (selectedTutorial == tempTutorial && tutorialLerpCounter != 1)
                    {
                        tutorialLerpCounter += (float)time.ElapsedGameTime.Milliseconds / 500;
                        if (tutorialLerpCounter > 1)
                        {
                            tutorialLerpCounter = 1;
                        }
                        float yValue = MathHelper.Lerp(-300, selectedTutorial.Rectangle.Y, tutorialLerpCounter);
                        selectedTutorial.CurrentRectangle = new Rectangle(selectedTutorial.Rectangle.X, (int)yValue, selectedTutorial.CurrentRectangle.Width, selectedTutorial.CurrentRectangle.Height);
                    }
                }
                else
                {
                    if (selectedTutorial != null)
                    {
                        tutorialLerpCounter -= (float)time.ElapsedGameTime.Milliseconds / 500;
                        if (tutorialLerpCounter < 0)
                        {
                            selectedTutorial = null;
                            tutorialLerpCounter = 0;
                        }
                        if (selectedTutorial != null)
                        {
                            float yValue = MathHelper.Lerp(-300, selectedTutorial.Rectangle.Y, tutorialLerpCounter);
                            selectedTutorial.CurrentRectangle = new Rectangle(selectedTutorial.Rectangle.X, (int)yValue, selectedTutorial.CurrentRectangle.Width, selectedTutorial.CurrentRectangle.Height);
                        }
                    }
                }
            }
        }
        #endregion
            
        #region Draw
        public static void Draw(SpriteBatch spriteBatch, SpriteFont font, Minimap minimap, Vector2 indexOfMiddleHexagon)
        {
            #region Draw Elements
            //draw the GUI_elements
            foreach (GUI_Element e in elements)
            {
                e.Draw(spriteBatch, Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold));
                if (e.ElementTyp == Vars_Func.GUI_ElementTyp.BackgroundHUD)
                {
                    minimap.drawMinimap(spriteBatch, indexOfMiddleHexagon);
                }
            }
            #endregion

            #region Draw Buttons
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
            #endregion

            #region Draw Tutorial-Elements
            //draw the TutorialElements
            if (showHelp)
            {
                if (selectedTutorial != null)
                {
                    selectedTutorial.Draw(spriteBatch, font);
                }
            }
            #endregion

            #region Draw Standart-Tutorial-Buttons
            // draw the standard tutorialbuttons
            if (showHelp)
            {
                foreach (GUI_Element b in tutorialButtons)
                {
                    if (b.ElementTyp == Vars_Func.GUI_ElementTyp.GUI_TutorialButton ||
                        b.ElementTyp == Vars_Func.GUI_ElementTyp.Minimap_TutorialButton ||
                        b.ElementTyp == Vars_Func.GUI_ElementTyp.Resources_TutorialButton ||
                        b.ElementTyp == Vars_Func.GUI_ElementTyp.Spells_TutorialButton ||
                        b.ElementTyp == Vars_Func.GUI_ElementTyp.Wavetimer_TutorialButton)
                    {
                        b.Draw(spriteBatch, font);
                    }
                }
            }
            #endregion

            #region Draw Building-Elements
            if (Interaction.GameState == Vars_Func.GameState.Build ||
                Interaction.GameState == Vars_Func.GameState.PlaceAnts ||
                Interaction.GameState == Vars_Func.GameState.PlaceSkeletons ||
                Interaction.GameState == Vars_Func.GameState.PlaceFarm ||
                Interaction.GameState == Vars_Func.GameState.PlaceTemple ||
                Interaction.GameState == Vars_Func.GameState.PlaceEntrance)
            {
                if (showHelp)
                {
                    //draw the tutorialButton for buildButtons
                    foreach (GUI_Element b in tutorialButtons)
                    {
                        if (b.ElementTyp == Vars_Func.GUI_ElementTyp.PlaceNest_TutorialButton)
                        {
                            b.Draw(spriteBatch, font);
                        }
                    }
                }
                alreadyDrawing = true;
                spriteBatch.DrawString(font, "Costs: ", new Vector2(1090 - 260 + 20, 610), Color.Black);
                spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Nest: " + Interaction.NestCost.ToString() + " Gold", new Vector2(1090 - 260 + 20, 630), Color.Black);
                spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Graveyard: " + Interaction.NestCost.ToString() + " Gold", new Vector2(1090 - 260 + 20, 650), Color.Black);
                spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Farm: " + Interaction.FarmCost.ToString() + " Gold", new Vector2(1090 - 260 + 20, 670), Color.Black);
                spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Tempel: " + Interaction.TempleCost.ToString() + " Gold", new Vector2(1090 - 260 + 20, 690), Color.Black);
                spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Entrance: " + Interaction.EntranceCost.ToString() + " Gold", new Vector2(1090 - 260 + 20, 710), Color.Black);
            }
            #endregion

            #region Draw Wave-Elements
            foreach (GUI_Element w in waveElements)
            {
                w.Draw(spriteBatch, font);
            }
            #endregion

            #region Draw Ressources
            //draw the player ressources
            spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold), "Gold: ", new Vector2(120, 50), Color.Black);
            spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold), Player.Gold.ToString(), new Vector2(120, 70), Color.Black);
            spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold), "Mana: ", new Vector2(120, 125), Color.Black);
            spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold), Player.Mana.ToString(), new Vector2(120, 145), Color.Black);
            spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold), "Food: ", new Vector2(120, 200), Color.Black);
            spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold), Player.Food.ToString(), new Vector2(120, 220), Color.Black);
            #endregion

            #region Draw Wavetimer
            //draw the Wavetimer and Counter
            spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold), ((int)WaveController.TimeToNextWave / 60) + ":" + ((int)WaveController.TimeToNextWave % 60), new Vector2(1090 - 104, 17), Color.Black);
            spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaBold), WaveController.WaveCounter.ToString(), new Vector2(1090 - 144 - 34, 17), Color.Black);
            #endregion

            bool enableUpgrades = false;
            //draw different values for other types of selected objects
            #region Selected Object
            switch (selectedThingTyp)
            {
                case Vars_Func.ThingTyp.Wall:
                    if (wall != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + wall.Typ.ToString(), new Vector2(1110, 520), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "HP: " + wall.HP.ToString(), new Vector2(1110, 540), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Gold: " + wall.Gold.ToString(), new Vector2(1110, 560), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.Nest:
                    if (nest.Typ == Vars_Func.NestTyp.Beetle ||
                        nest.Typ == Vars_Func.NestTyp.Skeleton)
                    {
                        //draw the tutorialButton for upgradeButtons and nests
                        if (showHelp)
                        {
                            foreach (GUI_Element b in tutorialButtons)
                            {
                                if (b.ElementTyp == Vars_Func.GUI_ElementTyp.Upgrades_TutorialButton ||
                                    b.ElementTyp == Vars_Func.GUI_ElementTyp.Nest_TutorialButton)
                                {
                                    b.Draw(spriteBatch, font);
                                }
                            }
                        }
                        //draw the upgradeButtons
                        foreach (GUI_Element b in upgradeButtons)
                        {
                            b.Draw(spriteBatch, font);
                        }
                        enableUpgrades = true;

                        spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(1110, 520), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Nutrition: " + nest.Nutrition.ToString() + "/" + nest.MaxNutrition.ToString(), new Vector2(1110, 540), Color.Black);
                        spriteBatch.DrawString(font, "Upgrades:", new Vector2(1110, 560), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Damage: " + nest.UpgradeCount[0], new Vector2(1110, 580), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Live: " + nest.UpgradeCount[1], new Vector2(1110, 600), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Speed: " + nest.UpgradeCount[2], new Vector2(1110, 620), Color.Black);

                        //draw the upgradecosts
                        spriteBatch.DrawString(font, "Upgrade-Costs:", new Vector2(1110, 640), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Damage: " + nest.NextUpgradeCost.ToString() + " Gold", new Vector2(1110, 660), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Live: " + nest.NextUpgradeCost.ToString() + " Gold", new Vector2(1110, 680), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Speed: " + nest.NextUpgradeCost.ToString() + " Gold", new Vector2(1110, 700), Color.Black);
                    }
                    else if (nest.Typ == Vars_Func.NestTyp.Entrance)
                    {
                        alreadyDrawing = true;
                        spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(1110, 520), Color.Black);
                    }
                    else if (nest.Typ == Vars_Func.NestTyp.Farm)
                    {
                        spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(1110, 520), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Food per second: " + (int)(nest.NestHexagons.Count / 10) + "." + (nest.NestHexagons.Count % 10), new Vector2(1110, 540), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Food: " + nest.Food, new Vector2(1110, 560), Color.Black);
                    }
                    else if (nest.Typ == Vars_Func.NestTyp.Temple)
                    {
                        spriteBatch.DrawString(font, "Typ: " + nest.Typ.ToString(), new Vector2(1110, 520), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Mana per second: " + (int)(nest.NestHexagons.Count / 10) + "." + (nest.NestHexagons.Count%10), new Vector2(1110, 540), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.DungeonCreature:
                    if (creature != null)
                    {
                        //draw the tutorialButton for creatures
                        if (showHelp)
                        {
                            foreach (GUI_Element b in tutorialButtons)
                            {
                                if (b.ElementTyp == Vars_Func.GUI_ElementTyp.Creature_TutorialButton)
                                {
                                    b.Draw(spriteBatch, font);
                                }
                            }
                        }
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1110, 520), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1110, 540), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Damage: " + creature.Damage.ToString(), new Vector2(1110, 560), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Age: " + ((int)creature.Age).ToString(), new Vector2(1110, 580), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.HeroCreature:
                    if (creature != null)
                    {
                        if (showHelp)
                        {
                            foreach (GUI_Element b in tutorialButtons)
                            {
                                if (b.ElementTyp == Vars_Func.GUI_ElementTyp.Creature_TutorialButton)
                                {
                                    b.Draw(spriteBatch, font);
                                }
                            }
                        }
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1110, 520), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1110, 540), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Damage: " + creature.Damage.ToString(), new Vector2(1110, 560), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Age: " + ((int)creature.Age).ToString(), new Vector2(1110, 580), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.NeutralCreature:
                    if (creature != null)
                    {
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1110, 520), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1110, 540), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Damage: " + creature.Damage.ToString(), new Vector2(1110, 560), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.HQCreature:
                    if (creature != null)
                    {
                        //draw the tutorialButton for the HQCreature
                        if (showHelp)
                        {
                            foreach (GUI_Element b in tutorialButtons)
                            {
                                if (b.ElementTyp == Vars_Func.GUI_ElementTyp.HQCreature_TutorialButton)
                                {
                                    b.Draw(spriteBatch, font);
                                }
                            }
                        }
                        spriteBatch.DrawString(font, "Typ: " + creature.Typ.ToString(), new Vector2(1110, 520), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "HP: " + (creature.HP - creature.DamageTaken).ToString(), new Vector2(1110, 540), Color.Black);
                        spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Damage: " + creature.Damage.ToString(), new Vector2(1110, 560), Color.Black);
                    }
                    break;
                case Vars_Func.ThingTyp.length:
                    //Reset the info element and let them draw the spell costs
                    if (alreadyDrawing && Interaction.GameState == Vars_Func.GameState.Ingame)
                    {
                        alreadyDrawing = false;
                    }
                    break;
            }
            #endregion

            if (!alreadyDrawing)
            {
                //draw the spellcosts
                spriteBatch.DrawString(font, "Spell-Costs: ", new Vector2(1090 - 260 + 20, 610), Color.Black);
                spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Fireball: " + Spells.FireballCost.ToString() + " Mana", new Vector2(1090 - 260 + 20, 630), Color.Black);
                spriteBatch.DrawString(Vars_Func.getGUI_Font(Vars_Func.GUI_Font.AugustaSmall), "Summon Imp: " + Spells.SummonImpCost.ToString() + " Mana", new Vector2(1090 - 260 + 20, 650), Color.Black);
            }

            if (enableUpgrades)
            {
                // Enable the upgrade buttons
                foreach (GUI_Element b in buttons)
                {
                    if(b.ElementTyp == Vars_Func.GUI_ElementTyp.Upgrade && !b.Enabled){
                        b.Enabled = true;
                    }
                }
            }
            else
            {
                // Disable the upgrade buttons
                foreach (GUI_Element b in buttons)
                {
                    if (b.ElementTyp == Vars_Func.GUI_ElementTyp.Upgrade && b.Enabled)
                    {
                        b.Enabled = false;
                    }
                }
            }
        }
        #endregion
    }
}
