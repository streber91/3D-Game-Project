using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Underlord.Entity;
using Underlord.Renderer;

namespace Underlord.Logic
{
    static class Interaction
    {
        static int nestCost = 100;
        static int farmCost = 100;
        static int templeCost = 100;
        static int entranceCost = 100;
        static Game1 game;
        static Vars_Func.GameState gameState = Vars_Func.GameState.MainMenu;
        static Vector2 indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
        static int radius = 0;
        static int counter = 0;
        static float timeCounter = 0.0f;
        static Vector2 oldMousePosition;
        static List<Vector2> toBeRoomPositions = new List<Vector2>();
        static Vars_Func.UpgradeTyp upgradeTyp = Vars_Func.UpgradeTyp.length;
        static Environment.Hexagon lastSelectedHexagon;

        #region Properties
        public static int NestCost
        {
            get { return nestCost; }
            set { nestCost = value; }
        }
        public static int FarmCost
        {
            get { return farmCost; }
            set { farmCost = value; }
        }
        public static int TempleCost
        {
            get { return templeCost; }
            set { templeCost = value; }
        }
        public static int EntranceCost
        {
            get { return entranceCost; }
            set { entranceCost = value; }
        }
        public static Game1 Game
        {
            get { return game; }
            set { game = value; }
        }
        public static Vars_Func.GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }
        #endregion

        public static void Update(GameTime gameTime, Environment.Map map, Vector2 mouseover, MouseState mouseState, MouseState lastMouseState, KeyboardState keyboard)
        {
            timeCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (gameState != Vars_Func.GameState.MainMenu &&
                gameState != Vars_Func.GameState.Highscore &&
                gameState != Vars_Func.GameState.Tutorial)
            {
                if (timeCounter > 100)
                {
                    //switch to CreateRoom Mode in "R" or the "CreateRoom-Button" is pressed
                    if (keyboard.IsKeyDown(Keys.R) ||
                        (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_Button(Vars_Func.GUI_ElementTyp.Room).Rectangle.Contains(mouseState.X, mouseState.Y)))
                    {
                        gameState = Vars_Func.GameState.CreateRoom;
                        timeCounter = 0;
                    }
                    //switch to Build Mode if "N" or the "Build-Button" is pressed
                    if (keyboard.IsKeyDown(Keys.N) ||
                        (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_Button(Vars_Func.GUI_ElementTyp.Build).Rectangle.Contains(mouseState.X, mouseState.Y)))
                    {
                        if (GUI.SelectedThingTyp == Vars_Func.ThingTyp.Nest) GUI.SelectedThingTyp = Vars_Func.ThingTyp.length;
                        indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                        radius = 0;
                        counter = 0;
                        gameState = Vars_Func.GameState.Build;
                        timeCounter = 0;
                    }
                    //switch to Mine Mode if "M" or the "Mine-Button" is pressed
                    if (keyboard.IsKeyDown(Keys.M) ||
                        (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_Button(Vars_Func.GUI_ElementTyp.Mine).Rectangle.Contains(mouseState.X, mouseState.Y)))
                    {
                        indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                        radius = 0;
                        counter = 0;
                        gameState = Vars_Func.GameState.Mine;
                        timeCounter = 0;
                    }
                    //switch to MergeRooms Mode if "T" or the "MergeRooms-Button" is pressed
                    if (keyboard.IsKeyDown(Keys.T) ||
                        (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_Button(Vars_Func.GUI_ElementTyp.MergeRoom).Rectangle.Contains(mouseState.X, mouseState.Y)))
                    {
                        indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                        radius = 0;
                        counter = 0;
                        gameState = Vars_Func.GameState.MergeRooms;
                        timeCounter = 0;
                    }
                    //switch to DeleteRoom Mode if "Z" or the "DeleteRoom-Button" is pressed
                    if (keyboard.IsKeyDown(Keys.Z) ||
                        (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_Button(Vars_Func.GUI_ElementTyp.DeleteRoom).Rectangle.Contains(mouseState.X, mouseState.Y)))
                    {
                        indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                        radius = 0;
                        counter = 0;
                        gameState = Vars_Func.GameState.DeleteRoom;
                        timeCounter = 0;
                    }
                    //switch to Fireball Mode if the "Fireball-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_Button(Vars_Func.GUI_ElementTyp.Fireball).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                        radius = 0;
                        counter = 0;
                        gameState = Vars_Func.GameState.Fireball;
                        timeCounter = 0;
                    }
                    //switch to SummonImp Mode if the "SummonImp-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_Button(Vars_Func.GUI_ElementTyp.SummonImp).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                        radius = 0;
                        counter = 0;
                        gameState = Vars_Func.GameState.SummonImp;
                        timeCounter = 0;
                    }
                    //switch to BuildUpgrade Mode if the "DamageUpgrade-Button" is pressed and a nest is selected
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.SelectedThingTyp == Vars_Func.ThingTyp.Nest &&
                        GUI.getGUI_UpgradeButton(Vars_Func.GUI_ElementTyp.DamageUpgrade).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        upgradeTyp = Vars_Func.UpgradeTyp.Damage;
                        indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                        radius = 0;
                        counter = 0;
                        gameState = Vars_Func.GameState.BuildUpgrade;
                        timeCounter = 0;
                    }
                    //switch to BuildUpgrade Mode if the "LifeUpgrade-Button" is pressed and a nest is selected
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.SelectedThingTyp == Vars_Func.ThingTyp.Nest &&
                        GUI.getGUI_UpgradeButton(Vars_Func.GUI_ElementTyp.LifeUpgrade).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        upgradeTyp = Vars_Func.UpgradeTyp.Life;
                        indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                        radius = 0;
                        counter = 0;
                        gameState = Vars_Func.GameState.BuildUpgrade;
                        timeCounter = 0;
                    }
                    //switch to BuildUpgrade Mode if the "SpeedUpgrade-Button" is pressed and a nest is selected
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.SelectedThingTyp == Vars_Func.ThingTyp.Nest &&
                        GUI.getGUI_UpgradeButton(Vars_Func.GUI_ElementTyp.SpeedUpgrade).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        upgradeTyp = Vars_Func.UpgradeTyp.Speed;
                        indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                        radius = 0;
                        counter = 0;
                        gameState = Vars_Func.GameState.BuildUpgrade;
                        timeCounter = 0;
                    }
                    //switch to GUI_Tutorial Mode if the "GUI_TutorialButton-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.GUI_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.GUI_Tutorial;
                        timeCounter = 0;
                    }
                    //switch to Minimap_Tutorial Mode if the "Minimap_TutorialButton-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.Minimap_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.Minimap_Tutorial;
                        timeCounter = 0;
                    }
                    //switch to Resources_Tutorial Mode if the "Resources_TutorialButton-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.Resources_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.Resources_Tutorial;
                        timeCounter = 0;
                    }
                    //switch to Spells_Tutorial Mode if the "Spells_TutorialButton-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.Spells_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.Spells_Tutorial;
                        timeCounter = 0;
                    }
                    //switch to Wavetimer_Tutorial Mode if the "Wavetimer_TutorialButton-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.Wavetimer_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.Wavetimer_Tutorial;
                        timeCounter = 0;
                    }
                    //switch to Nest_Tutorial Mode if the "Nest_TutorialButton-Button" is pressed and a nest is selected
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.SelectedThingTyp == Vars_Func.ThingTyp.Nest &&
                        GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.Nest_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.Nest_Tutorial;
                        timeCounter = 0;
                    }
                    //switch to Upgrades_Tutorial Mode if the "Upgrades_TutorialButton-Button" is pressed and a nest is selected
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.SelectedThingTyp == Vars_Func.ThingTyp.Nest &&
                        GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.Upgrades_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.Upgrades_Tutorial;
                        timeCounter = 0;
                    }
                    //switch to Creature_Tutorial Mode if the "Creature_TutorialButton-Button" is pressed and a nest is selected
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        (GUI.SelectedThingTyp == Vars_Func.ThingTyp.DungeonCreature || GUI.SelectedThingTyp == Vars_Func.ThingTyp.HeroCreature) &&
                        GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.Creature_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.Creature_Tutorial;
                        timeCounter = 0;
                    }
                    //switch to HQCreature_Tutorial Mode if the "HQCreature_TutorialButton-Button" is pressed and a nest is selected
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.SelectedThingTyp == Vars_Func.ThingTyp.HQCreature &&
                        GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.HQCreature_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.HQCreature_Tutorial;
                        timeCounter = 0;
                    }
                    //change the target for the nest to the clicked mouseposition
                    if(lastMouseState.RightButton == ButtonState.Released &&
                        mouseState.RightButton == ButtonState.Pressed &&
                        GUI.SelectedThingTyp == Vars_Func.ThingTyp.Nest &&
                        GUI.Nest.Typ != Vars_Func.NestTyp.Entrance &&
                        (map.getHexagonAt(mouseover).Obj == null ||
                        (map.getHexagonAt(mouseover).Obj.getThingTyp() != Vars_Func.ThingTyp.Wall &&
                        map.getHexagonAt(mouseover).Obj.getThingTyp() != Vars_Func.ThingTyp.Nest &&
                        map.getHexagonAt(mouseover).Obj.getThingTyp() != Vars_Func.ThingTyp.Farm &&
                        map.getHexagonAt(mouseover).Obj.getThingTyp() != Vars_Func.ThingTyp.Temple &&
                        map.getHexagonAt(mouseover).Obj.getThingTyp() != Vars_Func.ThingTyp.HQCreature &&
                        map.getHexagonAt(mouseover).Obj.getThingTyp() != Vars_Func.ThingTyp.Upgrade)))
                    {
                        map.getHexagonAt(GUI.Nest.TargetPosition).TargetFlag = false;
                        GUI.Nest.TargetPosition = mouseover;
                        map.getHexagonAt(GUI.Nest.TargetPosition).TargetFlag = true;
                        lastSelectedHexagon = map.getHexagonAt(GUI.Nest.TargetPosition);
                    }
                }
            }
            switch (gameState)
            {
                #region MainMenu
                case Vars_Func.GameState.MainMenu:
                    //start the game if the "StartGame-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_MenuButton(Vars_Func.GUI_ElementTyp.StartGame).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.Ingame;
                        game.reinitialize();
                    }
                    //show the highscore if the "Highscore-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_MenuButton(Vars_Func.GUI_ElementTyp.Highscore).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.Highscore;
                    }
                    //show the tutorial if the "Tutorial-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_MenuButton(Vars_Func.GUI_ElementTyp.Tutorial).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.Tutorial;
                    }
                    //quit the game if the "QuitGame-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_MenuButton(Vars_Func.GUI_ElementTyp.QuitGame).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        game.Exit();
                    }
                    break;
                #endregion
                #region Ingame
                case Vars_Func.GameState.Ingame:
                    //colors the hexagon at mouseposition in Ingame mode yellow
                    map.getHexagonAt(mouseover).Color = Color.Yellow;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //switch to the return screen to exit the game and save the highscore
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.ReturnToMainMenu;
                            timeCounter = 0;
                        }
                        //try to select object that is currently at mouseposition
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if(lastSelectedHexagon != null) lastSelectedHexagon.TargetFlag = false;
                            if (map.getHexagonAt(mouseover).Obj != null)
                            {
                                switch ((map.getHexagonAt(mouseover).Obj.getThingTyp()))
                                {
                                    case Vars_Func.ThingTyp.Wall:
                                        GUI.SelectedThingTyp = Vars_Func.ThingTyp.Wall;
                                        GUI.Wall = (Wall)map.getHexagonAt(mouseover).Obj;
                                        break;
                                    case Vars_Func.ThingTyp.Nest:
                                        GUI.SelectedThingTyp = Vars_Func.ThingTyp.Nest;
                                        GUI.Nest = (Nest)map.getHexagonAt(mouseover).Obj;
                                        if (GUI.Nest.Typ != Vars_Func.NestTyp.Entrance)
                                        {
                                            GUI.Nest = (Nest)map.Rooms[map.getHexagonAt(mouseover).RoomNumber - 1].RoomObject;
                                            map.getHexagonAt(GUI.Nest.TargetPosition).TargetFlag = true;
                                            lastSelectedHexagon = map.getHexagonAt(GUI.Nest.TargetPosition);
                                        }
                                        break;
                                    case Vars_Func.ThingTyp.DungeonCreature:
                                        GUI.SelectedThingTyp = Vars_Func.ThingTyp.DungeonCreature;
                                        GUI.Creature = (Creature)map.getHexagonAt(mouseover).Obj;
                                        break;
                                    case Vars_Func.ThingTyp.HeroCreature:
                                        GUI.SelectedThingTyp = Vars_Func.ThingTyp.HeroCreature;
                                        GUI.Creature = (Creature)map.getHexagonAt(mouseover).Obj;
                                        break;
                                    case Vars_Func.ThingTyp.NeutralCreature:
                                        GUI.SelectedThingTyp = Vars_Func.ThingTyp.NeutralCreature;
                                        GUI.Creature = (Creature)map.getHexagonAt(mouseover).Obj;
                                        break;
                                    case Vars_Func.ThingTyp.HQCreature:
                                        GUI.SelectedThingTyp = Vars_Func.ThingTyp.HQCreature;
                                        GUI.Creature = (Creature)map.getHexagonAt(mouseover).Obj;
                                        break;
                                }
                            }
                            else
                            {
                                GUI.SelectedThingTyp = Vars_Func.ThingTyp.length;
                            }
                        }
                    }
                    break;
                #endregion
                #region Highscore
                case Vars_Func.GameState.Highscore:
                    if (timeCounter > 100)
                    {
                        //back to MainMenu
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.MainMenu;
                            timeCounter = 0;
                        }
                    }
                    break;
                #endregion
                #region Tutorial
                case Vars_Func.GameState.Tutorial:
                    if (timeCounter > 100)
                    {
                        //back to MainMenu
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.MainMenu;
                            timeCounter = 0;
                        }
                    }
                    break;
                #endregion
                #region CreateRoom
                case Vars_Func.GameState.CreateRoom:
                    //color the hexagons around the coursor too
                    /*Vector2[] neigbors = map.getHexagonAt(mouseover.X, mouseover.Y).getNeighbors();
                    foreach (Vector2 hex in neigbors)
                    {
                        map.getHexagonAt(hex.X, hex.Y).Color = Color.Blue;
                    }*/
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame mode and reset of values
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                            System.Diagnostics.Debug.WriteLine("Escape");
                        }
                        //first click determines the middle of the new room
                        oldMousePosition = indexOfMiddleHexagonForRoomCreation;
                        if (counter == 0)
                        {
                            //colors the hexagon at mouseposition in CreateRoom mode blue
                            map.getHexagonAt(mouseover).Color = Color.Blue;
                            if(lastMouseState.LeftButton == ButtonState.Released &&
                                mouseState.LeftButton == ButtonState.Pressed &&
                                (map.getHexagonAt(mouseover).Obj == null || map.getHexagonAt(mouseover).Obj.getThingTyp() != Vars_Func.ThingTyp.Wall) && 
                                map.getHexagonAt(mouseover).RoomNumber == 0)
                            {
                                indexOfMiddleHexagonForRoomCreation = mouseover;
                                ++counter;
                                timeCounter = 0;
                            }
                        }
                        //determines the hexagons that the new room would have
                        else if (counter == 1)
                        {
                            Vector2 newMousePosition = mouseover;
                            if (oldMousePosition != newMousePosition)
                            {
                                oldMousePosition = newMousePosition;
                                radius = Vars_Func.computeDistance(indexOfMiddleHexagonForRoomCreation, mouseover, map);
                                toBeRoomPositions = determineToBeRoomPositions(indexOfMiddleHexagonForRoomCreation, radius, map);
                                foreach (Vector2 pos in toBeRoomPositions)
                                {
                                    map.getHexagonAt(pos).Color = Color.Blue;
                                }
                            }
                            //second click determines the radius for the new room and creates the room
                            if (lastMouseState.LeftButton == ButtonState.Released && 
                                mouseState.LeftButton == ButtonState.Pressed)
                            {
                                radius = Vars_Func.computeDistance(indexOfMiddleHexagonForRoomCreation, mouseover, map);
                                map.Rooms.Add(new Environment.Room(indexOfMiddleHexagonForRoomCreation, radius, map));
                                indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                                radius = 0;
                                counter = 0;
                                timeCounter = 0;
                            }
                        }
                    }
                    break;
                #endregion
                #region Build
                case Vars_Func.GameState.Build:
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //switch to PlaceAnts Mode if the "PlaceAnts-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceAnts).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceAnts;
                            timeCounter = 0;
                        }
                        //switch to PlaceSkeletons Mode if the "PlaceSkeletons-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceSkeletons).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceSkeletons;
                            timeCounter = 0;
                        }
                        //switch to PlaceFarm Mode if the "PlaceFarm-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceFarm).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceFarm;
                            timeCounter = 0;
                        }
                        //switch to PlaceTemple Mode if the "PlaceTemple-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceTemple).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceTemple;
                            timeCounter = 0;
                        }
                        //switch to PlaceEntrance Mode if the "PlaceEntrance-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceEntrance).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceEntrance;
                            timeCounter = 0;
                        }
                        //switch to PlaceNest_Tutorial Mode if the "PlaceNest_TutorialButton-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.PlaceNest_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            gameState = Vars_Func.GameState.PlaceNest_Tutorial;
                            timeCounter = 0;
                        }
                    }
                    break;
                #endregion
                #region Mine
                case Vars_Func.GameState.Mine:
                    //colors the hexagon at mouseposition in Mine mode red
                    map.getHexagonAt(mouseover).Color = Color.Red;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //try to mine a wall at mouseposition
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            //mine only if there is a wall
                            if (map.getHexagonAt(mouseover).Obj != null && map.getHexagonAt(mouseover).Obj.getThingTyp() == Vars_Func.ThingTyp.Wall &&
                                !map.MineJobs.Contains(mouseover))
                            {
                                if (((Wall)map.getHexagonAt(mouseover).Obj).Typ == Vars_Func.WallTyp.Stone)
                                {
                                    map.MineJobs.Add(mouseover);
                                    map.JobsWaiting.Enqueue(new Job(Vars_Func.ImpJob.Mine, mouseover));
                                }
                                else if (((Wall)map.getHexagonAt(mouseover).Obj).Typ == Vars_Func.WallTyp.Gold)
                                {
                                    map.MineJobs.Add(mouseover);
                                    map.JobsWaiting.Enqueue(new Job(Vars_Func.ImpJob.MineGold, mouseover));
                                }
                                else if (((Wall)map.getHexagonAt(mouseover).Obj).Typ == Vars_Func.WallTyp.Diamond)
                                {
                                    map.MineJobs.Add(mouseover);
                                    map.JobsWaiting.Enqueue(new Job(Vars_Func.ImpJob.MineDiamonds, mouseover));
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region MergeRooms
                case Vars_Func.GameState.MergeRooms:
                    //colors the hexagon at mouseposition in MergeRooms mode blue
                    map.getHexagonAt(mouseover).Color = Color.Blue;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //try to merge rooms
                        if (lastMouseState.LeftButton == ButtonState.Released && 
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (map.getHexagonAt(mouseover).RoomNumber != 0)
                            {
                                for (int i = 0; i < 6; ++i)
                                {
                                    Environment.Room room;
                                    //if the selected hexagon and the neighbor hexagon have different roomnumbers
                                    //and the neighbor hexagon is a room
                                    //and there isn't a nest in the room of the neighbor hexagon
                                    if (map.getHexagonAt(mouseover).RoomNumber != map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber &&
                                        map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber != 0 &&
                                        map.Rooms.ElementAt(map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber - 1).NestType == Vars_Func.NestTyp.length)
                                    {
                                        //than get the room of the neighbor hexagon
                                        //and merge it in the room of the selected hexagon
                                        room = map.Rooms.ElementAt(map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber - 1);
                                        map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).mergeRoom(room, map);
                                    }
                                }
                            }
                            timeCounter = 0;
                        }
                    }
                    break;
                #endregion
                #region DeleteRoom
                case Vars_Func.GameState.DeleteRoom:
                    //colors the hexagon at mouseposition in DeleteRoom mode blue
                    map.getHexagonAt(mouseover).Color = Color.Blue;
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //try to delete a room
                        if (lastMouseState.LeftButton == ButtonState.Released && 
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            //if there is a room
                            //and there isn't a nest in the room
                            if (map.getHexagonAt(mouseover).RoomNumber != 0 &&
                                map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).NestType == Vars_Func.NestTyp.length)
                            {
                                //then delete that room
                                map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).deleteRoom(map);
                            }
                            timeCounter = 0;
                        }
                    }
                    break;
                #endregion
                #region BuildUpgrade
                case Vars_Func.GameState.BuildUpgrade:
                    //colors the hexagon at mouseposition in BuildUpgrade mode purple
                    map.getHexagonAt(mouseover).Color = Color.Purple;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //try to place an upgrade at mouseposition
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (Player.enoughGold(GUI.Nest.NextUpgradeCost))
                            {
                                //place upgrade only when there is a room at mouseposition
                                //and there isn't an object already
                                //and that room is the same room which the selected nest belong to
                                //and the nest has grown to that position
                                if (map.getHexagonAt(mouseover).RoomNumber != 0 &&
                                    map.getHexagonAt(mouseover).Obj == null &&
                                    map.Rooms[map.getHexagonAt(mouseover).RoomNumber - 1].RoomObject == GUI.Nest &&
                                    map.getHexagonAt(mouseover).Nest == true)
                                {
                                    //the neighbors of the hexagon at mouseposition must be in the same room
                                    //and must be free for buildings
                                    //and the nest must have grown to that positions
                                    bool placeable = true;
                                    for (int i = 0; i < 6; ++i)
                                    {
                                        if (map.getHexagonAt(mouseover).RoomNumber != map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber ||
                                            map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).Nest == false ||
                                            map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).Building == true)
                                            placeable = false;
                                    }
                                    if (placeable)
                                    {
                                        GUI.Nest.addUpgrade(upgradeTyp, mouseover, map.getHexagonAt(mouseover), map);
                                        Player.Gold -= GUI.Nest.NextUpgradeCost;
                                        GUI.Nest.NextUpgradeCost += 50;
                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region Fireball
                case Vars_Func.GameState.Fireball:
                    //colors the hexagon at mouseposition in Fireball mode red
                    map.getHexagonAt(mouseover).Color = Color.Red;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //shot a fireball at mouseposition
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            Spells.castSpell(Vars_Func.SpellType.Fireball, mouseover, map);
                        }
                    }
                    break;
                #endregion
                #region SummonImp
                case Vars_Func.GameState.SummonImp:
                    //colors the hexagon at mouseposition in Fireball mode red
                    map.getHexagonAt(mouseover).Color = Color.Red;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //shot a fireball at mouseposition
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            Spells.castSpell(Vars_Func.SpellType.SummonImp, mouseover, map);
                        }
                    }
                    break;
                #endregion
                #region PlaceAnts
                case Vars_Func.GameState.PlaceAnts:
                    //colors the hexagon at mouseposition in Build mode purple
                    map.getHexagonAt(mouseover).Color = Color.Purple;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //switch to PlaceSkeletons Mode if the "PlaceSkeletons-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceSkeletons).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceSkeletons;
                            timeCounter = 0;
                        }
                        //switch to PlaceFarm Mode if the "PlaceFarm-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceFarm).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceFarm;
                            timeCounter = 0;
                        }
                        //switch to PlaceTemple Mode if the "PlaceTemple-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceTemple).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceTemple;
                            timeCounter = 0;
                        }
                        //switch to PlaceEntrance Mode if the "PlaceEntrance-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceEntrance).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceEntrance;
                            timeCounter = 0;
                        }
                        //switch to PlaceNest_Tutorial Mode if the "PlaceNest_TutorialButton-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.PlaceNest_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            gameState = Vars_Func.GameState.PlaceNest_Tutorial;
                            timeCounter = 0;
                        }
                        //try to place a nest at mouseposition
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (Player.enoughGold(nestCost))
                            {
                                //place nest only when there is a room at mouseposition and the room doesn't have a nest already
                                if (map.getHexagonAt(mouseover).RoomNumber != 0 && map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).NestType == Vars_Func.NestTyp.length)
                                {
                                    //the neighbors of the hexagon at mouseposition must be in the same room
                                    bool placeable = true;
                                    for (int i = 0; i < 6; ++i)
                                    {
                                        if (map.getHexagonAt(mouseover).RoomNumber != map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber) placeable = false;
                                    }
                                    if (placeable)
                                    {
                                        new Nest(Vars_Func.NestTyp.Beetle, mouseover, map, map.getHexagonAt(mouseover).Neighbors[3]);
                                        Player.Gold -= nestCost;
                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region PlaceSkeletons
                case Vars_Func.GameState.PlaceSkeletons:
                    //colors the hexagon at mouseposition in Build mode purple
                    map.getHexagonAt(mouseover).Color = Color.Purple;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //switch to PlaceAnts Mode if the "PlaceAnts-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceAnts).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceAnts;
                            timeCounter = 0;
                        }
                        //switch to PlaceFarm Mode if the "PlaceFarm-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceFarm).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceFarm;
                            timeCounter = 0;
                        }
                        //switch to PlaceTemple Mode if the "PlaceTemple-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceTemple).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceTemple;
                            timeCounter = 0;
                        }
                        //switch to PlaceEntrance Mode if the "PlaceEntrance-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceEntrance).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceEntrance;
                            timeCounter = 0;
                        }
                        //switch to PlaceNest_Tutorial Mode if the "PlaceNest_TutorialButton-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.PlaceNest_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            gameState = Vars_Func.GameState.PlaceNest_Tutorial;
                            timeCounter = 0;
                        }
                        //try to place a nest at mouseposition
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (Player.enoughGold(nestCost))
                            {
                                //place nest only when there is a room at mouseposition and the room doesn't have a nest already
                                if (map.getHexagonAt(mouseover).RoomNumber != 0 && map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).NestType == Vars_Func.NestTyp.length)
                                {
                                    //the neighbors of the hexagon at mouseposition must be in the same room
                                    bool placeable = true;
                                    for (int i = 0; i < 6; ++i)
                                    {
                                        if (map.getHexagonAt(mouseover).RoomNumber != map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber) placeable = false;
                                    }
                                    if (placeable)
                                    {
                                        new Nest(Vars_Func.NestTyp.Skeleton, mouseover, map, map.getHexagonAt(mouseover).Neighbors[3]);
                                        Player.Gold -= nestCost;
                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region PlaceFarm
                case Vars_Func.GameState.PlaceFarm:
                    //colors the hexagon at mouseposition in Build mode purple
                    map.getHexagonAt(mouseover).Color = Color.Purple;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //switch to PlaceAnts Mode if the "PlaceAnts-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceAnts).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceAnts;
                            timeCounter = 0;
                        }
                        //switch to PlaceSkeletons Mode if the "PlaceSkeletons-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceSkeletons).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceSkeletons;
                            timeCounter = 0;
                        }
                        //switch to PlaceTemple Mode if the "PlaceTemple-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceTemple).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceTemple;
                            timeCounter = 0;
                        }
                        //switch to PlaceEntrance Mode if the "PlaceEntrance-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceEntrance).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceEntrance;
                            timeCounter = 0;
                        }
                        //switch to PlaceNest_Tutorial Mode if the "PlaceNest_TutorialButton-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.PlaceNest_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            gameState = Vars_Func.GameState.PlaceNest_Tutorial;
                            timeCounter = 0;
                        }
                        //try to place a nest at mouseposition
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (Player.enoughGold(farmCost))
                            {
                                //place nest only when there is a room at mouseposition and the room doesn't have a nest already
                                if (map.getHexagonAt(mouseover).RoomNumber != 0 && map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).NestType == Vars_Func.NestTyp.length)
                                {
                                    //the neighbors of the hexagon at mouseposition must be in the same room
                                    bool placeable = true;
                                    for (int i = 0; i < 6; ++i)
                                    {
                                        if (map.getHexagonAt(mouseover).RoomNumber != map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber) placeable = false;
                                    }
                                    if (placeable)
                                    {
                                        new Nest(Vars_Func.NestTyp.Farm, mouseover, map, map.getHexagonAt(mouseover).Neighbors[3]);
                                        Player.Gold -= farmCost;
                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region PlaceTemple
                case Vars_Func.GameState.PlaceTemple:
                    //colors the hexagon at mouseposition in Build mode purple
                    map.getHexagonAt(mouseover).Color = Color.Purple;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //switch to PlaceAnts Mode if the "PlaceAnts-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceAnts).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceAnts;
                            timeCounter = 0;
                        }
                        //switch to PlaceSkeletons Mode if the "PlaceSkeletons-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceSkeletons).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceSkeletons;
                            timeCounter = 0;
                        }
                        //switch to PlaceFarm Mode if the "PlaceFarm-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceFarm).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceFarm;
                            timeCounter = 0;
                        }
                        //switch to PlaceTemple Mode if the "PlaceTemple-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceTemple).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceTemple;
                            timeCounter = 0;
                        }
                        //switch to PlaceEntrance Mode if the "PlaceEntrance-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceEntrance).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceEntrance;
                            timeCounter = 0;
                        }
                        //switch to PlaceNest_Tutorial Mode if the "PlaceNest_TutorialButton-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.PlaceNest_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            gameState = Vars_Func.GameState.PlaceNest_Tutorial;
                            timeCounter = 0;
                        }
                        //try to place a nest at mouseposition
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (Player.enoughGold(templeCost))
                            {
                                //place nest only when there is a room at mouseposition and the room doesn't have a nest already
                                if (map.getHexagonAt(mouseover).RoomNumber != 0 && map.Rooms.ElementAt(map.getHexagonAt(mouseover).RoomNumber - 1).NestType == Vars_Func.NestTyp.length)
                                {
                                    //the neighbors of the hexagon at mouseposition must be in the same room
                                    bool placeable = true;
                                    for (int i = 0; i < 6; ++i)
                                    {
                                        if (map.getHexagonAt(mouseover).RoomNumber != map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[i]).RoomNumber) placeable = false;
                                    }
                                    if (placeable)
                                    {
                                        new Nest(Vars_Func.NestTyp.Temple, mouseover, map, map.getHexagonAt(mouseover).Neighbors[3]);
                                        Player.Gold -= templeCost;
                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region PlaceEntrance
                case Vars_Func.GameState.PlaceEntrance:
                    //colors the hexagon at mouseposition in Build mode purple
                    map.getHexagonAt(mouseover).Color = Color.Purple;
                    //only ten clicks per second
                    if (timeCounter > 100)
                    {
                        //back to Ingame Mode
                        if (keyboard.IsKeyDown(Keys.Escape))
                        {
                            gameState = Vars_Func.GameState.Ingame;
                            timeCounter = 0;
                        }
                        //switch to PlaceAnts Mode if the "PlaceAnts-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceAnts).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceAnts;
                            timeCounter = 0;
                        }
                        //switch to PlaceSkeletons Mode if the "PlaceSkeletons-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceSkeletons).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceSkeletons;
                            timeCounter = 0;
                        }
                        //switch to PlaceFarm Mode if the "PlaceFarm-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceFarm).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceFarm;
                            timeCounter = 0;
                        }
                        //switch to PlaceTemple Mode if the "PlaceTemple-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceTemple).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceTemple;
                            timeCounter = 0;
                        }
                        //switch to PlaceEntrance Mode if the "PlaceEntrance-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_BuildButton(Vars_Func.GUI_ElementTyp.PlaceEntrance).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            indexOfMiddleHexagonForRoomCreation = new Vector2(0, 0);
                            radius = 0;
                            counter = 0;
                            gameState = Vars_Func.GameState.PlaceEntrance;
                            timeCounter = 0;
                        }
                        //switch to PlaceNest_Tutorial Mode if the "PlaceNest_TutorialButton-Button" is pressed
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed &&
                            GUI.getGUI_TutorialButtons(Vars_Func.GUI_ElementTyp.PlaceNest_TutorialButton).Rectangle.Contains(mouseState.X, mouseState.Y))
                        {
                            gameState = Vars_Func.GameState.PlaceNest_Tutorial;
                            timeCounter = 0;
                        }
                        //try to place a nest at mouseposition
                        if (lastMouseState.LeftButton == ButtonState.Released &&
                            mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (Player.enoughGold(entranceCost))
                            {
                                //place entrance only when there isn't a room at mouseposition and there isn't an object at the position
                                if (map.getHexagonAt(mouseover).RoomNumber == 0 &&
                                    map.getHexagonAt(mouseover).Obj == null &&
                                    (map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[3]).Obj == null ||
                                    map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[3]).Obj.getThingTyp() != Vars_Func.ThingTyp.Wall ||
                                    map.getHexagonAt(map.getHexagonAt(mouseover).Neighbors[3]).Obj.getThingTyp() != Vars_Func.ThingTyp.HQCreature))
                                {
                                    new Nest(Vars_Func.NestTyp.Entrance, mouseover, map, map.HQPosition);
                                    Player.Gold -= entranceCost;
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region ReturnToMainMenu
                    case Vars_Func.GameState.ReturnToMainMenu:
                    //return to the mainmenu and save the highscore if the "ReturnAccept-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_ReturnScreenButton(Vars_Func.GUI_ElementTyp.ReturnAccept).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.MainMenu;
                    }
                    //return to the game if the "ReturnDecline-Button" is pressed
                    if (lastMouseState.LeftButton == ButtonState.Released &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        GUI.getGUI_ReturnScreenButton(Vars_Func.GUI_ElementTyp.ReturnDecline).Rectangle.Contains(mouseState.X, mouseState.Y))
                    {
                        gameState = Vars_Func.GameState.Ingame;
                    }
                    break;
                #endregion
                #region default
                default:
                //only ten clicks per second
                if (timeCounter > 100)
                {
                    //back to Ingame Mode
                    if (keyboard.IsKeyDown(Keys.Escape))
                    {
                        gameState = Vars_Func.GameState.Ingame;
                        timeCounter = 0;
                    }
                }
                break;
                #endregion
            }
        }

        public static List<Vector2> determineToBeRoomPositions(Vector2 middleHexagonIndexNumber, int radius, Environment.Map map)
        {
            List<Vector2> list = new List<Vector2>();
            //create a list with only the middle hexagon
            if (radius == 0)
            {
                list.Add(middleHexagonIndexNumber);
            }
            //create a list through a broad-first-search
            else
            {
                list.Add(middleHexagonIndexNumber); //add middle hexagon
                Queue<Vector2> queue = new Queue<Vector2>(); //create a queue
                queue.Enqueue(middleHexagonIndexNumber); //add middle hexagon to queue
                queue.Enqueue(new Vector2(radius + map.getPlanelength(), 0)); //add dummy-element to queue (x-value is a value that can't be reached with coordinated of the map)
                map.getHexagonAt(middleHexagonIndexNumber).Visited = true; //set visited for the middle element at true
                while (queue.Count != 1)
                {
                    Vector2 tmp = queue.Dequeue(); //get the first element of the queue
                    if (tmp.X == map.getPlanelength() + 1) break; //stop if the element is the dummy-element with an x-value that sasy that the radius is reached
                    //if element is a map-element
                    else if (tmp.X < map.getPlanelength())
                    {
                        //for all neighbors
                        for (int i = 0; i < 6; ++i)
                        {
                            Vector2 neighbor = map.getHexagonAt(tmp).Neighbors[i];
                            //which weren't visited already
                            if (map.getHexagonAt(neighbor).Visited == false)
                            {
                                map.getHexagonAt(neighbor).Visited = true; //set visited at true
                                //when there isn't an object on the hexagon or the object isn't a wall, a HQCreature or a Nest(Entrance)
                                //and the hexagon isn't already a room
                                if ((map.getHexagonAt(neighbor).Obj == null ||
                                    (map.getHexagonAt(neighbor).Obj.getThingTyp() != Logic.Vars_Func.ThingTyp.Wall &&
                                    map.getHexagonAt(neighbor).Obj.getThingTyp() != Logic.Vars_Func.ThingTyp.HQCreature &&
                                    map.getHexagonAt(neighbor).Obj.getThingTyp() != Logic.Vars_Func.ThingTyp.Nest)) &&
                                    map.getHexagonAt(neighbor).RoomNumber == 0)
                                {
                                    queue.Enqueue(neighbor); //add the neighbor to the queue
                                    list.Add(neighbor); //and add the neighbor to the room
                                }
                            }
                        }
                    }
                    //if element is the dummy-element with radius isn't reached
                    else queue.Enqueue(new Vector2(tmp.X - 1, 0));
                }
                //set visited for all hexagon at false (for the next use of searching)
                foreach (Environment.Hexagon hex in map.getMapHexagons())
                {
                    if (hex.Visited == true) hex.Visited = false;
                }
            }
            return list;
        }
    }
}
