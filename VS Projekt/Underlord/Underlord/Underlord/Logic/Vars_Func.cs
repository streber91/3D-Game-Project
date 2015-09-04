using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Underlord.Animation;
using Underlord.Basic;

namespace Underlord.Logic
{
   static class Vars_Func
    {
       public enum ThingTyp { Wall, Upgrade, Nest, DungeonCreature, HeroCreature, NeutralCreature, HQCreature, Imp, Farm, Temple, length };
       public enum CreatureTyp { Beetle, Knight, HQCreatur, Skeleton, length };
       public enum NestTyp { Beetle, Entrance, Temple, Farm, Skeleton, length };
       public enum UpgradeTyp {Damage, Life, Speed, length };
       public enum WallTyp { Stone, Gold, Diamond, HQ, EN, length };
       public enum HexTyp { Sand, Stone, BeetleNest, Farm, Temple, Graveyard, length };
       public enum SpellType { SummonImp, Fireball, length  }

       public enum GameState { StartMenu, MainMenu, Ingame, Settings, Highscore, Tutorial, 
                               CreateRoom, Build, Mine, MergeRooms, DeleteRoom, Upgrade, BuildUpgrade, Fireball, SummonImp, PlaceAnts, PlaceSkeletons, PlaceFarm, PlaceTemple, PlaceEntrance, ReturnToMainMenu,
                               GUI_Tutorial, HQCreature_Tutorial, Creature_Tutorial, Minimap_Tutorial, Nest_Tutorial, PlaceNest_Tutorial, Resources_Tutorial, Upgrades_Tutorial, Wavetimer_Tutorial, Spells_Tutorial, length };

       public enum ImpJob { Idle, Harvest, Feed, Mine, MineDiamonds, MineGold, length };

       public enum GUI_Font { Augusta, AugustaBold, AugustaBold2, AugustaHeadline, AugustaTextField, AusgustaText, length };

       public enum GUI_Typ { StartButton, NewGameButton, SettingsButton, HighScoreButton, QuitButton, None, length }; 

       public enum GUI_ElementTyp { Dummy, 
                                    Nest, Graveyard, Farm, Temple, Entrance,
                                    FrameHUD, ButtonHUD, BackgroundHUD, MinimapHUD, RessoucesFrame, RessoucesHUD, RessoucesPapier, InfoHUD,
                                    Food, Gold, Mana,
                                    Mine, Room, MergeRoom, DeleteRoom, Build, Upgrade, 
									DamageUpgrade, LifeUpgrade, SpeedUpgrade,
                                    LeftChain, RightChain, SpecialChain, Pole,
                                    LeftHUD, RightHUD, BottomHUD, TopHUD,
                                    StartBackgroundHUD, BlackBackgoundHUD, TextFrame, TextBlance, UnderlordHUD, UnderlordFireLogo, UnderlordBurnedLogo,
                                    TextFieldSmall, TextFieldMiddle, TextFieldBig, BookField, TextArrow, ChainSmall, ChainMiddle, ChainBig, ChainLarge,
                                    FullScreenButton, 
                                    Menu, StartGame, QuitGame, Highscore, Tutorial, ReturnAccept, ReturnDecline,
                                    Fireball, SummonImp,
                                    GUI_Tutorial, HQCreature_Tutorial, Creature_Tutorial, Minimap_Tutorial, Nest_Tutorial, PlaceNest_Tutorial, Resources_Tutorial, Upgrades_Tutorial, Wavetimer_Tutorial, Spells_Tutorial,
                                    GUI_TutorialButton, HQCreature_TutorialButton, Creature_TutorialButton, Minimap_TutorialButton, Nest_TutorialButton, PlaceNest_TutorialButton, Resources_TutorialButton, Upgrades_TutorialButton, Wavetimer_TutorialButton, Spells_TutorialButton, length
       };

       public enum ImpState { Walking, Digging, Praying, Harvesting, Nothing, length };
       public enum CreatureState { Walking, Fighting, Nothing, Starting, OpenMouth, CloseMouth, PingPong, length };

       public enum GrowObject { Farm, Temple, Graveyard, length };

       #region Fields
       static List<AnimationModel> CreatureModels;
       static List<BasicModel> CreatureShadows;
       static List<BasicModel> NestModels;
       static List<BasicModel> UpgradeModels;
       static List<BasicModel> WallModels;
       static List<BasicModel> HexagonModels;
       static ImpModel ImpModel;
       static BasicModel ImpShadow;
       static BasicModel TorchModel;
       static Model TorchFireModel;
       static BasicModel HQMouthModel;
       static LightModel EntranceRayModel;
       static List<BasicModel> GrowModels;
       static BasicModel TargetFlag;

       static Texture2D pixel;
       static List<SpriteFont> GUI_Fonts;
       static List<Texture2D> GUI_Elements;

       static List<Texture2D> Rock_Texture;
       static List<Texture2D> Gold_Texture;
       static List<Texture2D> Diamond_Texture;

       static Vector3[] CreaturParamters = { new Vector3(0, 0.07f, MathHelper.PiOver2), new Vector3(0f, 0.04f, MathHelper.PiOver2), new Vector3(0, 1.5f, MathHelper.PiOver2), new Vector3(0f, 0.04f, MathHelper.PiOver2) };
       static Vector3[] NestParamters = { new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0.01f, 1, 0), new Vector3(0.01f, 1, 0) };
       static Vector3[] GrowObjectParameters = { new Vector3(0.01f, 1, 0), new Vector3(0.01f, 1, 0), new Vector3(0.01f, 1, 0) };

       public static AnimationModel getCreatureModell(CreatureTyp typ) { return CreatureModels[(int)typ]; }
       public static BasicModel getCreatureShadow(CreatureTyp typ) { return CreatureShadows[(int)typ]; }
       public static BasicModel getNestModell(NestTyp typ) { return NestModels[(int)typ]; }
       public static BasicModel getUpgradeModell(UpgradeTyp typ) { return UpgradeModels[(int)typ]; }
       public static BasicModel getWallModell(WallTyp typ) { return WallModels[(int)typ]; }
       public static BasicModel getHexagonModell(HexTyp typ) { return HexagonModels[(int)typ]; }
       public static ImpModel getImpModell() { return ImpModel; }
       public static BasicModel getImpShadow() { return ImpShadow; }
       public static BasicModel getTorchModel() { return TorchModel; }
       public static Model getTorchFireModel() { return TorchFireModel; }
       public static BasicModel getHQMouthModel() { return HQMouthModel; }
       public static LightModel getEntranceRayModel() { return EntranceRayModel; }
       public static BasicModel getGrowModel(GrowObject typ) { return GrowModels[(int)typ]; }
       public static BasicModel getTargetFlag() { return TargetFlag; }

       public static Texture2D getPixel() { return pixel; }
       public static SpriteFont getGUI_Font(GUI_Font typ) { return GUI_Fonts[(int)typ]; }
       public static Texture2D getGUI_ElementTextures(GUI_ElementTyp typ) { return GUI_Elements[(int)typ]; }

       public static Texture2D getWall_RockTexture(int index) { return Rock_Texture[index]; }
       public static Texture2D getWall_GoldTexture(int index) { return Gold_Texture[index]; }
       public static Texture2D getWall_DiamondTexture(int index) { return Diamond_Texture[index]; }

       public static Vector3 getCreatureParams(CreatureTyp typ) { return CreaturParamters[(int)typ]; }
       public static Vector3 getNestParams(NestTyp typ) { return NestParamters[(int)typ]; }
       public static Vector3 getGrowObjectParams(GrowObject typ) { return GrowObjectParameters[(int)typ]; }
       #endregion

       public static void loadContent(ContentManager Content)
       {
           CreatureModels = new List<AnimationModel>();
           CreatureShadows = new List<BasicModel>();
           NestModels = new List<BasicModel>();
           UpgradeModels = new List<BasicModel>();
           WallModels = new List<BasicModel>();
           HexagonModels = new List<BasicModel>();
           GUI_Elements = new List<Texture2D>();
           Rock_Texture = new List<Texture2D>();
           Gold_Texture = new List<Texture2D>();
           Diamond_Texture = new List<Texture2D>();
           GrowModels = new List<BasicModel>();

           pixel = Content.Load<Texture2D>("TEST");
           GUI_Fonts = new List<SpriteFont>();

           //Augusta, AugustaBold, AugustaBold2, AugustaHeadline, AugustaTextField, AusgustaText
           #region Fonts
           GUI_Fonts.Add(Content.Load<SpriteFont>("Fonts//Augusta"));
           GUI_Fonts.Add(Content.Load<SpriteFont>("Fonts//AugustaBold"));
           GUI_Fonts.Add(Content.Load<SpriteFont>("Fonts//AugustaBold_2"));
           GUI_Fonts.Add(Content.Load<SpriteFont>("Fonts//Augusta_Headline"));
           GUI_Fonts.Add(Content.Load<SpriteFont>("Fonts//Augusta_TextField"));
           GUI_Fonts.Add(Content.Load<SpriteFont>("Fonts//Augusta_Text"));
           #endregion

           //Dummy
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Dummy//dummy_texture_1366x768"));

           //Nest, Graveyard, Farm, Temple, Entrance,
           #region Building
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Buildings//ant"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Buildings//skeleton"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Buildings//farm"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Buildings//tempel"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Buildings//entrance"));
           #endregion

           //FrameHUD, ButtonHUD, BackgroundHUD, MinimapHUD, RessoucesFrame, RessoucesHUD, RessoucesPapier, InfoHUD
           #region HUD
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Screen//screen_frame_iron_96x24"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Screen//screen_frame_iron_110x475_01"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Screen//screen_background_iron_01"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Minimap//minimap_frame_iron_260x265"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Ressources//res_frame_singel_iron_260x265"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Ressources//res_singel_iron_260x265"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Ressources//res_papier_singel_260x265"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Screen//info_frame_iron_247x160"));
           #endregion

           //Food, Gold, Mana
           #region Ressources
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Ressources//res_foot"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Ressources//res_gold"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Ressources//res_mana"));
           #endregion

           ////BackgroundHUD, MinimapHUD, FoodHUD, GoldHUD, ManaHUD
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Screen//screen_background_01"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Minimap//minimap_background_02"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Ressources//scroll_foot"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Ressources//scroll_gold"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Ressources//scroll_mana"));

           //Mine, Room, MergeRoom, DeleteRoom, Build, Upgrade
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Functions//mine"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Functions//room"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Functions//merge"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Functions//delete"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Functions//build"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Upgrades//flag"));

           ////Mine, Room, MergeRoom, DeleteRoom, Build, PlaceAnts, PlaceSkeletons, PlaceFarm, PlaceTemple, PlaceEntrance, DamageUpgrade, LifeUpgrade, SpeedUpgrade
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_mine"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_room"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_merge"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_delete"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_88"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_88"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_88"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_88"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_88"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_88"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_88"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_88"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Scrolls//scroll_88"));

           //DamageUpgrade, LifeUpgrade, SpeedUpgrade
           #region Upgrades
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Upgrades//upgrade_deg"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Upgrades//upgrade_lev"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Upgrades//upgrade_spd"));
           #endregion

           //LeftChain, RightChain, SpecialChain, Pole
           #region Special-GUI Elements
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Elements//chain_short_left_48x256"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Elements//chain_short_right_48x256"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Elements//chain_special_24x60"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Elements//pole_iron_500x8"));
           #endregion
                 
           //LeftHUD, RightHUD, BottomHUD, TopHUD, RessoucesHUD, InfoHUD
           GUI_Elements.Add(Content.Load<Texture2D>("Left_HUD_Test"));
           GUI_Elements.Add(Content.Load<Texture2D>("Right_HUD_Test"));
           GUI_Elements.Add(Content.Load<Texture2D>("Bottom_HUD_Test"));
           GUI_Elements.Add(Content.Load<Texture2D>("Top_HUD_Test"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Ressouces_HUD_Test"));
           //GUI_Elements.Add(Content.Load<Texture2D>("Info_HUD_Test"));

           //StartBackgroundHUD, BlackBackgoundHUD, TextFrame, TextBlance, UnderlordHUD, UnderlordFireLogo, UnderlordBurnedLogo
           #region Start-GUI Elements
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Start//start_background_1366x768"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Start//blackground_1366x768"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Start//start_text_chains_293x72"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Start//start_text_blance_293x96"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Start//start_underlord_frame_800x350"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Start//underlord_fire_800x525"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Start//underlord_burned_800x525"));
           #endregion

           //TextFieldSmall, TextFieldMiddle, TextFieldBig, BookField, TextArrow, ChainSmall, ChainMiddle, ChainBig, ChainLarge,
           #region Main-Menu-GUI Elements
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/TextFields//text_small_288x72"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/TextFields//text_middle_384x96"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/TextFields//text_big_512x128"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/TextFields//book_911x512"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/TextFields//text_back_384x96"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Chains//chain_small_288x64"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Chains//chain_middle_384x84"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Chains//chain_big_512x100"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/Chains//chain_large_911x237"));
           #endregion

           //FullScreenButton
           #region Setting-GUI Elements
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/GUI/TextFields//scroll_76x88"));
           #endregion

           //Menu, StartGame, QuitGame, Highscore, Tutorial, ReturnAccept, ReturnDecline
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));

           //Spells (Fireball, SummonImp)
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Spells//fireball"));
           GUI_Elements.Add(Content.Load<Texture2D>("Textures/HUD/Spells//imp"));

           //Tutorials
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST"));

           //TutorialButtons
           GUI_Elements.Add(Content.Load<Texture2D>("TEST2"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST2"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST2"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST2"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST2"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST2"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST2"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST2"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST2"));
           GUI_Elements.Add(Content.Load<Texture2D>("TEST2"));

           Rock_Texture.Add(Content.Load<Texture2D>("Textures/Rock//wall_rock_broken_01_TEXT"));
           Rock_Texture.Add(Content.Load<Texture2D>("Textures/Rock//wall_rock_broken_02_TEXT"));
           Rock_Texture.Add(Content.Load<Texture2D>("Textures/Rock//wall_rock_broken_03_TEXT"));

           Gold_Texture.Add(Content.Load<Texture2D>("Textures/Gold//wall_gold_broken_01_TEXT"));
           Gold_Texture.Add(Content.Load<Texture2D>("Textures/Gold//wall_gold_broken_02_TEXT"));
           Gold_Texture.Add(Content.Load<Texture2D>("Textures/Gold//wall_gold_broken_03_TEXT"));

           Diamond_Texture.Add(Content.Load<Texture2D>("Textures/Diamond//wall_diamond_broken_01_TEXT"));
           Diamond_Texture.Add(Content.Load<Texture2D>("Textures/Diamond//wall_diamond_broken_02_TEXT"));
           Diamond_Texture.Add(Content.Load<Texture2D>("Textures/Diamond//wall_diamond_broken_03_TEXT"));

           WallModels.Add(new BasicModel(Content.Load<Model>("Models/Wall/sand_Wall_HEX_01")));
           WallModels.Add(new BasicModel(Content.Load<Model>("Models/Wall/sand_Wall_HEX_01")));
           WallModels.Add(new BasicModel(Content.Load<Model>("Models/Wall/sand_Wall_HEX_01")));
           WallModels.Add(new BasicModel(Content.Load<Model>("Models/Wall/sand_Wall_HEX_01")));

           WallModels[(int)WallTyp.Stone].Texture = Content.Load<Texture2D>("Textures/Rock//wall_rock_TEXT");
           WallModels[(int)WallTyp.Gold].Texture = Content.Load<Texture2D>("Textures/Gold//wall_gold_TEXT");
           WallModels[(int)WallTyp.Diamond].Texture = Content.Load<Texture2D>("Textures/Diamond//wall_diamond_TEXT");

           HexagonModels.Add(new BasicModel(Content.Load<Model>("Models//floorSand_HEX_03")));
           HexagonModels.Add(new BasicModel(Content.Load<Model>("Models//floorSand_HEX_03")));
           HexagonModels.Add(new BasicModel(Content.Load<Model>("Models//floorSand_HEX_03")));
           HexagonModels.Add(new BasicModel(Content.Load<Model>("Models//floorSand_HEX_03")));
           HexagonModels.Add(new BasicModel(Content.Load<Model>("Models//floorSand_HEX_03")));
           HexagonModels.Add(new BasicModel(Content.Load<Model>("Models//floorSand_HEX_03")));

           HexagonModels[(int)HexTyp.Sand].Texture = Content.Load<Texture2D>("Textures//floor_stone_TEXT");
           HexagonModels[(int)HexTyp.Stone].Texture = Content.Load<Texture2D>("Textures//floor_stone_TEXT");
           HexagonModels[(int)HexTyp.BeetleNest].Texture = Content.Load<Texture2D>("Textures/Nest//nest_orange_TEXT");
           HexagonModels[(int)HexTyp.Graveyard].Texture = Content.Load<Texture2D>("Textures/Farm//farm_dirt_TEXT");
           HexagonModels[(int)HexTyp.Temple].Texture = Content.Load<Texture2D>("Textures/Tempel//tempel_white_TEXT");
           HexagonModels[(int)HexTyp.Farm].Texture = Content.Load<Texture2D>("Textures/Farm//farm_dirt_TEXT");

           NestModels.Add(new BasicModel(Content.Load<Model>("Models/Nest//nest_GEO_01")));    
           NestModels.Add(new BasicModel(Content.Load<Model>("Models/Entrance//entrance_GEO_01")));
           NestModels.Add(new BasicModel(Content.Load<Model>("Models/Tempel//tempel_house_GEO_01")));
           NestModels.Add(new BasicModel(Content.Load<Model>("Models/Farm//farm_house_GEO_01")));
           NestModels.Add(new BasicModel(Content.Load<Model>("Models/Graveyard//graveyard_house_GEO_01")));

           NestModels[(int)NestTyp.Beetle].Texture = Content.Load<Texture2D>("Textures/Nest//nest_orange_TEXT");
           //NestModels[(int)NestTyp.Entrance].Texture = Content.Load<Texture2D>("Textures//nest_orange_TEXT");
           //NestModels[(int)NestTyp.Skeleton].Texture = Content.Load<Texture2D>("Textures//nest_orange_TEXT");
           //NestModels[(int)NestTyp.Temple].Texture = Content.Load<Texture2D>("Textures/Tempel//tempel_white_TEXT");
           //NestModels[(int)NestTyp.Farm].Texture = Content.Load<Texture2D>("Textures//nest_orange_TEXT");

           UpgradeModels.Add(new BasicModel(Content.Load<Model>("Models/Flags//flag_Deg_GEO_01")));
           UpgradeModels.Add(new BasicModel(Content.Load<Model>("Models/Flags//flag_Lve_GEO_01")));
           UpgradeModels.Add(new BasicModel(Content.Load<Model>("Models/Flags//flag_Spd_GEO_01")));
           
           GrowModels.Add(new BasicModel(Content.Load<Model>("Models/Farm//farm_floor_GEO_01")));
           GrowModels.Add(new BasicModel(Content.Load<Model>("Models/Tempel//tempel_floor_GEO_01")));
           GrowModels.Add(new BasicModel(Content.Load<Model>("Models/Graveyard//graveyard_floor_GEO_01")));

           TargetFlag = new BasicModel(Content.Load<Model>("Models/Flags//flag_Deg_GEO_01"));

           // Add ant character
           AnimationModel ant = new AnimationModel(Content.Load<Model>("AnimationModels/Ant//ant_simple_walk_ANI_01"));
           ant.AddClip(new AnimationModel(Content.Load<Model>("AnimationModels/Ant//ant_simple_fight_ANI_01")));
           CreatureModels.Add(ant);
           CreatureShadows.Add(new BasicModel(Content.Load<Model>("AnimationModels/Ant//ant_shadow_GEO_01")));

           // Add knight character
           AnimationModel knight = new AnimationModel(Content.Load<Model>("AnimationModels/Knight//knight_simple_walk_ANI_01"));
           knight.AddClip(new AnimationModel(Content.Load<Model>("AnimationModels/Knight//knight_simple_fight_ANI_01")));
           knight.AddClip(new AnimationModel(Content.Load<Model>("AnimationModels/Knight//knight_simple_flying_ANI_01")));
           CreatureModels.Add(knight);
           CreatureShadows.Add(new BasicModel(Content.Load<Model>("AnimationModels/Knight//knight_shadow_GEO_01")));

           // Add HQ character
           CreatureModels.Add(new AnimationModel(Content.Load<Model>("AnimationModels/HQ//devil_head_GEO_01")));
           HQMouthModel = new BasicModel(Content.Load<Model>("AnimationModels/HQ//devil_mouth_GEO_01"));
           CreatureShadows.Add(new BasicModel(Content.Load<Model>("AnimationModels/Skeleton//skeleton_shadow_GEO_01")));

           // Add skeleton character
           AnimationModel skeleton = new AnimationModel(Content.Load<Model>("AnimationModels/Skeleton//skeleton_simple_walk_ANI_01"));
           skeleton.AddClip(new AnimationModel(Content.Load<Model>("AnimationModels/Skeleton//skeleton_simple_fight_ANI_01")));
           CreatureModels.Add(skeleton);
           CreatureShadows.Add(new BasicModel(Content.Load<Model>("AnimationModels/Skeleton//skeleton_shadow_GEO_01")));

           // Add imp character
           ImpModel = new ImpModel(Content.Load<Model>("AnimationModels/Imp//imp_simple_walk_ANI_01"));
           ImpModel.AddClip(new AnimationModel(Content.Load<Model>("AnimationModels/Imp//imp_simple_grab_ANI_01")));
           ImpModel.AddClip(new AnimationModel(Content.Load<Model>("AnimationModels/Imp//imp_simple_praying_ANI_01")));
           ImpModel.AddClip(new AnimationModel(Content.Load<Model>("AnimationModels/Imp//imp_simple_harvesting_ANI_01")));
           ImpShadow = new BasicModel(Content.Load<Model>("AnimationModels/Imp//imp_shadow_GEO_01"));

           // Add torch model
           TorchModel = new BasicModel(Content.Load<Model>("Models/Torch//torch_pillar_HEX_01"));
           // Add torch fire model
           TorchFireModel = Content.Load<Model>("Models/Torch//torch_fire_HEX_01");
           // Add god' ray model
           EntranceRayModel = new LightModel(Content.Load<Model>("Models/Entrance/entrance_rays_GEO_01"));
       }

       public static Vector3 mousepos(GraphicsDevice graphics, MouseState mousestate, Matrix projection, Matrix view)
       {
           // calculate 3D mouspose out of an 2D Mosepos with Z = 0
           // represents the mousover in 3D space
           Vector3 vec1 = graphics.Viewport.Unproject(new Vector3(mousestate.X, mousestate.Y, 1), projection, view, Matrix.Identity);
           Vector3 vec2 = graphics.Viewport.Unproject(new Vector3(mousestate.X, mousestate.Y, 0), projection, view, Matrix.Identity);
           float a = -vec1.Z / (vec2.Z - vec1.Z);
           Vector3 mousepos = new Vector3(a * (vec2.X - vec1.X) + vec1.X, a * (vec2.Y - vec1.Y) + vec1.Y, 0.0f);

           return mousepos;
       }

       public static int computeDistance(Vector2 pos1, Vector2 pos2, Environment.Map map)
       {
           //return statement
           int distanz = 0;

           //breadth-first search
           Vector2 tmp = new Vector2();
           Queue<Vector2> queue = new Queue<Vector2>();
           queue.Enqueue(pos1);
           map.getHexagonAt(pos1).Visited = true;
           queue.Enqueue(new Vector2(map.getPlanelength(), 0));

           while (queue.Count != 1)
           {
               tmp = queue.Dequeue();
               if (tmp == pos2) break;
               if (tmp.X == map.getPlanelength())
               {
                   ++distanz;
                   queue.Enqueue(tmp);
                   continue;
               }
               foreach (Vector2 hex in map.getHexagonAt(tmp).Neighbors)
               {
                   if (!map.getHexagonAt(hex).Visited)
                   {
                       queue.Enqueue(hex);
                       map.getHexagonAt(hex).Visited = true;
                   }
               }
           }
           //clear Hexmap for next search
           for (int i = 0; i < map.getPlanelength(); ++i)
           {
               for (int j = 0; j < map.getPlanelength(); ++j)
               {
                   map.getHexagonAt(i, j).Visited = false;
               }
           }

           return distanz;
       }
       // TODO write comment
       public static Vector2 gridColision(Vector3 position, int planeLength, float hexagonSideLength)
       {
           //magic 
           float positionX = position.X;
           float positionY = position.Y;
           int X = 0;
           int Y = 0;
           while (positionX < 0) positionX = positionX + (planeLength * 1.5f * hexagonSideLength);
           while (positionY < 0) positionY = positionY + (planeLength * 2 * 0.875f * hexagonSideLength);
           positionX = positionX % (planeLength * 1.5f * hexagonSideLength);
           positionY = positionY % (planeLength * 2 * 0.875f * hexagonSideLength);

           while (positionX >= 1.5f * hexagonSideLength)
           {
               positionX -= 1.5f;
               ++X;
           }
           while (positionY >= 0.875f * hexagonSideLength)
           {
               positionY -= 0.875f;
               ++Y;
           }

           if (positionX <= hexagonSideLength / 2)
           {
               if ((X + Y) % 2 == 0)
               {
                   if (0.875 >= positionX * 1.75 + positionY) --X;
                   if (X < 0) X += planeLength;
               }
               else
               {
                   if (0 >= positionX * 1.75 - positionY) --X;
                   if (X < 0) X += planeLength;
               }
           }
           if (X % 2 != 0)
           {
               --Y;
               if (Y < 0) Y += planeLength * 2;
           }

           return new Vector2(X, (int)(Y / 2));
       }

       public static void resetHexagonColors(Environment.Map map)
       {
           foreach (Environment.Hexagon hex in map.getMapHexagons())
            {
                hex.Color = Color.White;
            }
           //colors the room-hexagons in CreateRoom mode, MergeRooms mode, DeleteRoom mode, Build  mode, BuildUpgrade mode
           if(Interaction.GameState == Vars_Func.GameState.CreateRoom ||
               Interaction.GameState == Vars_Func.GameState.MergeRooms ||
               Interaction.GameState == Vars_Func.GameState.DeleteRoom ||
               Interaction.GameState == Vars_Func.GameState.Build ||
               Interaction.GameState == Vars_Func.GameState.BuildUpgrade)
           {
               foreach (Environment.Hexagon hex in map.getMapHexagons())
               {
                   if (hex.RoomNumber == 0) { }
                   else if (hex.RoomNumber % 10 == 0) hex.Color = Color.Red;
                   else if (hex.RoomNumber % 10 == 1) hex.Color = Color.Yellow;
                   else if (hex.RoomNumber % 10 == 2) hex.Color = Color.Blue;
                   else if (hex.RoomNumber % 10 == 3) hex.Color = Color.Black;
                   else if (hex.RoomNumber % 10 == 4) hex.Color = Color.Green;
                   else if (hex.RoomNumber % 10 == 5) hex.Color = Color.Purple;
                   else if (hex.RoomNumber % 10 == 6) hex.Color = Color.Lerp(Color.Black, Color.Green, 0.5f);
                   else if (hex.RoomNumber % 10 == 7) hex.Color = Color.Lerp(Color.Black, Color.Green, 0.5f);
                   else if (hex.RoomNumber % 10 == 8) hex.Color = Color.Lerp(Color.Black, Color.Green, 0.5f);
                   else if (hex.RoomNumber % 10 == 9) hex.Color = Color.Lerp(Color.Black, Color.Green, 0.5f);
               }
           }
           //colors the hexagons which are in the mineJobs list
           else if (Interaction.GameState == Vars_Func.GameState.Mine)
           {
               foreach(Vector2 pos in map.MineJobs)
               {
                   map.getHexagonAt(pos).Color = Color.Purple;
               }
           }
       }
    }
}
