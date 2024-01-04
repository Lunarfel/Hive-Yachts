using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.NaturalMotion;
using Client.Deps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client.Handlers
{
    class Yachts : YachtClient
    {
        HUD hud = new HUD();
        // How many yachts are there?
        List<string> yachtList = new List<string>();
        List<string> yachtInts = new List<string>();
        List<string> yachtLods = new List<string>();
        List<Vector3> yachtCoords = new List<Vector3>();
        List<Vector4> yachtQuat = new List<Vector4>();
        float propHeight = 20.006f;

        static List<int> yacht_options = new List<int>()
        {
            1, // The Orion
            2, // The Pisces
            3 // The Aquarius
            // apa_mp_apa_yacht_option{upgrade 1/2/3}
        };

        static List<string> yacht_flags = new List<string>()
        {
            "apa_prop_flag_argentina",
            "apa_prop_flag_australia",
            "apa_prop_flag_austria",
            "apa_prop_flag_belgium",
            "apa_prop_flag_brazil",
            "apa_prop_flag_canadat_yt",
            "apa_prop_flag_china",
            "apa_prop_flag_columbia",
            "apa_prop_flag_croatia",
            "apa_prop_flag_czechrep",
            "apa_prop_flag_denmark",
            "apa_prop_flag_england",
            "apa_prop_flag_eu_yt",
            "apa_prop_flag_finland",
            "apa_prop_flag_france",
            "apa_prop_flag_german_yt",
            "apa_prop_flag_hungary",
            "apa_prop_flag_ireland",
            "apa_prop_flag_israel",
            "apa_prop_flag_italy",
            "apa_prop_flag_jamaica",
            "apa_prop_flag_japan_yt",
            "apa_prop_flag_lstein",
            "apa_prop_flag_malta",
            "apa_prop_flag_mexico_yt",
            "apa_prop_flag_netherlands",
            "apa_prop_flag_newzealand",
            "apa_prop_flag_nigeria",
            "apa_prop_flag_norway",
            "apa_prop_flag_palestine",
            "apa_prop_flag_poland",
            "apa_prop_flag_portugal",
            "apa_prop_flag_puertorico",
            "apa_prop_flag_russia_yt",
            "apa_prop_flag_scotland_yt",
            "apa_prop_flag_script",
            "apa_prop_flag_slovakia",
            "apa_prop_flag_slovenia",
            "apa_prop_flag_southafrica",
            "apa_prop_flag_southkorea",
            "apa_prop_flag_spain",
            "apa_prop_flag_sweden",
            "apa_prop_flag_switzerland",
            "apa_prop_flag_turkey",
            "apa_prop_flag_uk_yt",
            "apa_prop_flag_us_yt",
            "apa_prop_flag_wales"
        };

        static List<string> yacht_lights = new List<string>()
        {
            "1a", // Gold
            "1b", // Blue
            "1c", // Pink
            "1d", // Green
            "2a", // Gold (sidebars)
            "2b", // Blue (sidebars)
            "2c", // Pink (sidebars)
            "2d" // Green (sidebars)
        };

        static List<string> yacht_rails = new List<string>()
        {
            "a", // Silver
            "b" // Gold
            // apa_mp_apa_yacht_o{upgrade1/2/3}_rail_a
        };

        static List<string> yacht_launchers = new List<string>()
        {
            "apa_mp_apa_yacht_launcher_01a", // only first yacht model
            "apa_mp_apa_yacht_launcher_02a", // 2nd and third tier
        };

        static int selectedYachtOption = yacht_options[2];
        static int selectedYachtTexture = 16;
        static string selectedYachtLights1 = yacht_lights[0];
        static string selectedYachtLights2 = yacht_lights[4];
        static string selectedYachtRails = yacht_rails[1]; 

        List<int> yachts = new List<int>();
        List<int> yachtsInts = new List<int>();
        List<int> yachtsLods = new List<int>();
        static YachtEnt[] YachtArr = new YachtEnt[35]
        {
            new YachtEnt(0),
            new YachtEnt(1),
            new YachtEnt(2),
            new YachtEnt(3),
            new YachtEnt(4),
            new YachtEnt(5),
            new YachtEnt(6),
            new YachtEnt(7),
            new YachtEnt(8),
            new YachtEnt(9),
            new YachtEnt(10),
            new YachtEnt(11),
            new YachtEnt(12),
            new YachtEnt(13),
            new YachtEnt(14),
            new YachtEnt(15),
            new YachtEnt(16),
            new YachtEnt(17),
            new YachtEnt(18),
            new YachtEnt(19),
            new YachtEnt(20),
            new YachtEnt(21),
            new YachtEnt(22),
            new YachtEnt(23),
            new YachtEnt(24),
            new YachtEnt(25),
            new YachtEnt(26),
            new YachtEnt(27),
            new YachtEnt(27),
            new YachtEnt(28),
            new YachtEnt(29),
            new YachtEnt(30),
            new YachtEnt(31),
            new YachtEnt(32),
            new YachtEnt(33),
        };
         
        /*
            this.HeliPadAOffset = new Vector3(-51.5f, -1.999345f, 3f);
            this.HeliPadBOffset = new Vector3(-37.78258f, -1.999345f, -0.5f);
 
            this.HeliPadAOffset = new Vector3(-29.5f, -1.999345f, 6f);
            this.HeliPadBOffset = new Vector3(36f, -1.999345f, 4.5f);
        */

 

        List<Vector4> _yachtPos = new List<Vector4>();

        bool isInJacuzzi = false;

        List<string> jacuzziObject = new List<string>()
        {
            "apa_mp_apa_yacht_jacuzzi_ripple1",
            "apa_mp_apa_yacht_jacuzzi_ripple2"
        };

        List<string> jacuzziStandingAnims = new List<string>()
        {
            // male
            "anim@amb@yacht@jacuzzi@standing@male@variation_01@",
            "anim@amb@yacht@jacuzzi@standing@male@variation_02@",


            // female
            "anim@amb@yacht@jacuzzi@standing@female@variation_01@",
            "anim@amb@yacht@jacuzzi@standing@female@variation_02@"
        };

        List<string> jacuzziSeatedAnims = new List<string>()
        {
            // male
            "anim@amb@yacht@jacuzzi@seated@male@variation_01@",
            "anim@amb@yacht@jacuzzi@seated@male@variation_02@",
            "anim@amb@yacht@jacuzzi@seated@male@variation_03@",
            "anim@amb@yacht@jacuzzi@seated@male@variation_04@",
            "anim@amb@yacht@jacuzzi@seated@male@variation_05@",


            // female
            "anim@amb@yacht@jacuzzi@seated@female@variation_01@",
            "anim@amb@yacht@jacuzzi@seated@female@variation_02@",
            "anim@amb@yacht@jacuzzi@seated@female@variation_03@",
            "anim@amb@yacht@jacuzzi@seated@female@variation_04@",
            "anim@amb@yacht@jacuzzi@seated@female@variation_05@"
        };

        List<string> jacuzziParticles = new List<string>()
        {
            "scr_apa_jacuzzi_drips",
            "scr_apa_jacuzzi_steam",
            "scr_apa_jacuzzi_steam_sp",
            "scr_apa_jacuzzi_wade"
        };

        public Yachts()
        {
            Init();

        }

        public async Task Init()
        {

            // temp, disable ocean storms because our yachts are static (they dont move)
            API.SetDeepOceanScaler(0.0f);

            sLog("Cleaning up...");
            await DESPAWNAllYachtsAsync();

            sLog("Creating Yacht Lists...");
            UpdateYachtLists();

            sLog("Loading IPLs...");
            for (int q = 0; q < yachtList.Count(); q++)
            {
                RequestIpl(yachtList[q]);
                RequestIpl(yachtInts[q]);
                RequestIpl(yachtLods[q]);
            } 

            sLog("Spawning all yachts...");
            SpawnHandler();


            sLog("Customizing all yachts...");

            sLog("All yachts spawned and customized, have fun!");

             
        } 


        private void SpawnHandler()
        {
            for (int x = 0; x < yachtList.Count(); x++)
            {
                // broken IDs, perhaps not existing in GTA:O as well?
                if(x == 0 || x == 7 || x == 11 || x == 13 || x == 21 || x == 26)
                    continue;

                SpawnYachtWithID(x);
                Vector3 yachtPoz = yachtCoords[x];
                Blip yachtBlip = World.CreateBlip(yachtPoz);
                yachtBlip.Sprite = (BlipSprite)455;
                yachtBlip.Color = BlipColor.MichaelBlue;
                yachtBlip.Name = "Yacht " + x;
            }
        }


        private void UpdateYachtGarbage(int e, Vector3 pos, Vector4 quat, float head, int w, bool yachtitself=false, bool flag=false, bool jacuzzi=false)
        {
            SetEntityAsMissionEntity(e, true, true);
            if(!yachtitself && !flag && !jacuzzi) SetEntityCoordsNoOffset(e, pos.X, pos.Y, propHeight, true, false, false);
            
            if (yachtitself)
                SetEntityCoordsNoOffset(e, pos.X, pos.Y, pos.Z, true, false, false);

            SetEntityCollision(e, true, true);

            switch (w)
            {
                case 1:
                    SetEntityHeading(e, 181.99990844727f - 90.05f);
                    break;
                case 2:
                    SetEntityHeading(e, 341.99990844727f - 90.05f);
                    break;
                case 3:
                    SetEntityHeading(e, 171.99989318848f - 90.05f);
                    break;
                case 4:
                    SetEntityHeading(e, 316.99993896484f - 90.05f);
                    break;
                case 5:
                    SetEntityHeading(e, 266.99993896484f - 90.05f);
                    break;
                case 6:
                    SetEntityHeading(e, 6.9999494552612f - 90.05f);
                    break;
                case 8:
                    SetEntityHeading(e, 51.999965667725f - 90.05f);
                    break;
                case 9:
                    SetEntityHeading(e, 126.99996948242f - 90.05f);
                    break;
                case 10:
                    SetEntityHeading(e, 21.999952316284f - 90.05f);
                    break;
                case 12:
                    SetEntityHeading(e, 326.99996948242f - 90.05f);
                    break;
                case 14:
                    SetEntityHeading(e, 331.99996948242f - 90.05f);
                    break;
                case 15:
                    SetEntityHeading(e, 282.0f - 90.05f);
                    break;
                case 16:
                    SetEntityHeading(e,121.9998626709f - 90.05f);
                    break;
                case 17:
                    SetEntityHeading(e, 341.99990844728f - 90.05f);
                    break;
                case 18:
                    SetEntityHeading(e, 171.99989318848f - 90.05f);
                    break;
                case 19:
                    SetEntityHeading(e, 316.99993896484f - 90.05f);
                    break;
                case 20:
                    SetEntityHeading(e, 346.99996948242f - 90.05f);
                    break;
                case 22:
                    SetEntityHeading(e, 176.99998474121f - 90.05f);
                    break;
                case 23:
                    SetEntityHeading(e, 151.99992370605f - 90.05f);
                    break;
                case 24:
                    SetEntityHeading(e, 201.99993896484f - 90.05f);
                    break;
                case 27:
                    SetEntityHeading(e, 246.99993896484f - 90.05f);
                    break;
                case 28:
                    SetEntityHeading(e, 31.999906539917f - 90.05f);
                    break;
                case 29:
                    SetEntityHeading(e, 111.99996185303f + 9.965f);
                    break;
                case 30:
                    SetEntityHeading(e, 96.999969482422f - 90.05f);
                    break;
                case 31:
                    SetEntityHeading(e, 266.99993896484f - 90.05f);
                    break;
                case 33:
                    SetEntityHeading(e, 116.99998474121f - 90.05f);
                    break;
                case 34:
                    SetEntityHeading(e, 161.99989318848f - 90.05f);
                    break;
            }

             

        }

        private async void SpawnYachtFlag(int w)
        {

            Object yacht = YachtArr[w].Entity;
            int yachtHandle = YachtArr[w].Entity.Handle; 
            Prop YachtRails = YachtArr[w].g;
            Vector3 yachtRotation = GetEntityRotation(yachtHandle, 5);
            float yachtZRotation = yachtRotation.Z;
            Vector3 flagsRotation = new Vector3(0.0f, -50f, yachtZRotation);

                // < X >
                // ^ Y v
                Vector3 FlagPositionA = GetOffsetFromEntityInWorldCoords(yachtHandle, -3f, -0.45f, 10.8f); // roof left
                Vector3 FlagPositionB = GetOffsetFromEntityInWorldCoords(yachtHandle, -3f, -3.55f, 10.8f); // roof right
                Vector3 FlagPositionC = GetOffsetFromEntityInWorldCoords(yachtHandle, -56.5f, -1.996299f, 1.5f); // behind the jacuzzi

                Prop FlagA = new Prop(CreateObject(GetHashKey(yacht_flags[0]), 0, 0, 0, false, false, false))
                {
                    Position = FlagPositionA,
                    Rotation = flagsRotation,
                    LodDistance = 100000,
                    IsInvincible = true,
                    IsCollisionEnabled = true,
                };

                Prop FlagB = new Prop(CreateObject(GetHashKey(yacht_flags[0]), 0, 0, 0, false, false, false))
                {
                    Position = FlagPositionB,
                    Rotation = flagsRotation,
                    LodDistance = 100000,
                    IsInvincible = true,
                    IsCollisionEnabled = true,
                };
            
                Prop FlagC = new Prop(CreateObject(GetHashKey(yacht_flags[0]), 0, 0, 0, false, false, false))
                {
                    Position = FlagPositionC,
                    Rotation = flagsRotation,
                    LodDistance = 100000,
                    IsInvincible = true,
                    IsCollisionEnabled = true,
                };

            SetEntityRotation(FlagA.Handle, flagsRotation.X, flagsRotation.Y, flagsRotation.Z, 5, true);
            SetEntityRotation(FlagB.Handle, flagsRotation.X, flagsRotation.Y, flagsRotation.Z, 5, true);
            SetEntityRotation(FlagC.Handle, flagsRotation.X, flagsRotation.Y, flagsRotation.Z, 5, true);
        }

        private void SpawnYachtJacuzzi(int w)
        { 
            int yachtHandleId = YachtArr[w].Entity.Handle;
            int maxSeats = 6;
            Vector3 yachtRotation = GetEntityRotation(yachtHandleId, 5);
            float yachtZRotation = yachtRotation.Z;
            Vector3 jacuzziSeat1, jacuzziSeat2, jacuzziSeat3, jacuzziSeat4, jacuzziSeat5, jacuzziSeat6; 

            Vector3 position_seat_1 = new Vector3(-49f, -1.999345f, -1.1f);
            jacuzziSeat1 = API.GetOffsetFromEntityInWorldCoords(yachtHandleId, position_seat_1.X, position_seat_1.Y, position_seat_1.Z);

            Vector3 position_seat_2 = new Vector3(-50f, -4f, -1.1f);
            jacuzziSeat2 = API.GetOffsetFromEntityInWorldCoords(yachtHandleId, position_seat_1.X, position_seat_1.Y, position_seat_1.Z);

            Vector3 position_seat_3 = new Vector3(-50f, 0.0f, -1.1f);
            jacuzziSeat3 = API.GetOffsetFromEntityInWorldCoords(yachtHandleId, position_seat_1.X, position_seat_1.Y, position_seat_1.Z);

            Vector3 position_seat_4 = new Vector3(-52f, 0.0f, -1.1f);
            jacuzziSeat4 = API.GetOffsetFromEntityInWorldCoords(yachtHandleId, position_seat_1.X, position_seat_1.Y, position_seat_1.Z);
            
            Vector3 position_seat_5 = new Vector3(-52f, -4f, -1.1f);
            jacuzziSeat5 = API.GetOffsetFromEntityInWorldCoords(yachtHandleId, position_seat_1.X, position_seat_1.Y, position_seat_1.Z);
            
            Vector3 position_seat_6 = new Vector3(-53f, -1.999345f, -1.1f);
            jacuzziSeat6 = API.GetOffsetFromEntityInWorldCoords(yachtHandleId, position_seat_1.X, position_seat_1.Y, position_seat_1.Z);

            Vector3 RipplePos = GetOffsetFromEntityInWorldCoords(yachtHandleId, -50.8033f, -1.9774f, 0.1368f);
            Vector3 RippleRotation = new Vector3(0.0f, -50f, yachtZRotation);

            /*Prop RippleA = new Prop(CreateObject(GetHashKey(jacuzziObject[0]), 0, 0, 0, false, false, false))
            {
                Position = RipplePos,
                //Rotation = RippleRotation,
                LodDistance = 100000,
                IsInvincible = true,
                IsCollisionEnabled = true,
            };

            Prop RippleB = new Prop(CreateObject(GetHashKey(jacuzziObject[1]), 0, 0, 0, false, false, false))
            {
                Position = RipplePos,
                //Rotation = RippleRotation,
                LodDistance = 100000,
                IsInvincible = true,
                IsCollisionEnabled = true,
            }; */

            ///this.SpawnProp("apa_mp_apa_yacht_jacuzzi_ripple1", this.YachtPos[this.Location], new Vector3(0.0f, 0.0f, 0.0f), 7);
            ///this.SpawnProp("apa_mp_apa_yacht_jacuzzi_ripple2", this.YachtPos[this.Location], new Vector3(0.0f, 0.0f, 0.0f), 7);

        }

        private async Task JacuzziHandler()
        {
  
        }


        private async void SpawnYachtWithID(int x)
        {
            Vector3 propPos = yachtCoords[x];
            float properHeading = 180;

            // Yacht itself
            YachtArr[x].Entity = new Prop(CreateObject(GetHashKey("apa_mp_apa_yacht"), propPos.X, propPos.Y, propPos.Z, false, false, false));
            UpdateYachtGarbage(YachtArr[x].Entity.Handle, yachtCoords[x], yachtQuat[x], properHeading, x, true); 
            SetObjectTextureVariant(YachtArr[x].Entity.Handle, selectedYachtTexture);
            YachtArr[x].Index = x; 

            // lights - side of yacht
            YachtArr[x].b = new Prop(CreateObject(GetHashKey("apa_mp_apa_y" + selectedYachtOption + "_l" + selectedYachtLights2), 0, 0, 0, false, false, false))
                {
                    Position = new Vector3(propPos.X, propPos.Y, propPos.Z),
                    Rotation = new Vector3(360, 0, 0),
                    //Heading = properHeading,
                    IsPositionFrozen = true,
                    LodDistance = 100000,
                    IsInvincible = true,
                    IsCollisionEnabled = true,
                };
            UpdateYachtGarbage(YachtArr[x].b.Handle, yachtCoords[x], yachtQuat[x], properHeading, x);

            // rails
            YachtArr[x].g = new Prop(CreateObject(GetHashKey("apa_mp_apa_yacht_o"+selectedYachtOption+"_rail_"+selectedYachtRails), 0, 0, 0, false, false, false))
                {
                    Position = new Vector3(propPos.X, propPos.Y, propHeight),
                    Rotation = new Vector3(360, 0, 0),
                    //Heading = properHeading,
                    IsPositionFrozen = true,
                    LodDistance = 100000,
                    IsInvincible = true,
                    IsCollisionEnabled = true,
                };
            UpdateYachtGarbage(YachtArr[x].g.Handle, yachtCoords[x], yachtQuat[x], properHeading, x);

            // yacht upgrade
            YachtArr[x].i = new Prop(CreateObject(GetHashKey("apa_mp_apa_yacht_option"+ selectedYachtOption), 0, 0, 0, false, false, false))
                {
                    Position = new Vector3(propPos.X, propPos.Y, propHeight),
                    Rotation = new Vector3(360, 0, 0),
                    //Heading = properHeading,
                    IsPositionFrozen = true,
                    LodDistance = 100000,
                    IsInvincible = true,
                    IsCollisionEnabled = true,
                };
            UpdateYachtGarbage(YachtArr[x].i.Handle, yachtCoords[x], yachtQuat[x], properHeading, x);
            SetObjectTextureVariant(YachtArr[x].i.Handle, selectedYachtTexture);

            // firework launcher
            YachtArr[x].j = new Prop(CreateObject(GetHashKey("apa_mp_apa_yacht_launcher_02a"), 0, 0, 0, false, false, false))
                {
                    Position = new Vector3(propPos.X, propPos.Y, propHeight),
                    Rotation = new Vector3(360, 0, 0),
                    //Heading = properHeading,
                    IsPositionFrozen = true,
                    LodDistance = 100000,
                    IsInvincible = true,
                    IsCollisionEnabled = true,
                };
            UpdateYachtGarbage(YachtArr[x].j.Handle, yachtCoords[x], yachtQuat[x], properHeading, x);
            SetObjectTextureVariant(YachtArr[x].j.Handle, selectedYachtTexture);

            // Yacht Flag
            SpawnYachtFlag(x);


            // Door 1
            YachtArr[x].ac = new Prop(CreateObject(GetHashKey("apa_mp_apa_yacht_door"), 0, 0, 0, false, false, false))
                {
                    Position = new Vector3(propPos.X, propPos.Y, propHeight),
                    //Rotation = new Vector3(360, 0, 0),
                    //Heading = properHeading,
                    IsPositionFrozen = true,
                    LodDistance = 100000,
                    IsInvincible = true,
                    IsCollisionEnabled = true,
                };
            UpdateYachtGarbage(YachtArr[x].ac.Handle, yachtCoords[x], yachtQuat[x], properHeading, x);

            // Door 2
            YachtArr[x].ad = new Prop(CreateObject(GetHashKey("apa_mp_apa_yacht_door2"), 0, 0, 0, false, false, false))
                {
                    Position = new Vector3(propPos.X, propPos.Y, propHeight),
                    //Rotation = new Vector3(360, 0, 0),
                    //Heading = properHeading,
                    IsPositionFrozen = true,
                    LodDistance = 100000,
                    IsInvincible = true,
                    IsCollisionEnabled = true,
                };
            UpdateYachtGarbage(YachtArr[x].ad.Handle, yachtCoords[x], yachtQuat[x], properHeading, x);

            // Jacuzzi
            SpawnYachtJacuzzi(x);

            await DrawYachtText(x);
        }

        private async Task DrawYachtText(int w)
        {
            Vector2 res = new Vector2(0.1f, 0.0889f);
            string imgDict = "dock_dlc_model";
            int yachtHandleId = YachtArr[w].Entity.Handle;
            Vector3 yachtRotation = GetEntityRotation(yachtHandleId, 5);
            float yachtZRotation = yachtRotation.Z;

            float x_coord = 0.324f + yachtRotation.X;
            float y_coord = 0.044f + yachtRotation.Y;


            DrawRect(x_coord, y_coord, yachtRotation.X + 0.003f, yachtRotation.Y + 0.003f, 0, 0, 0, 212);
             
            if (!HasStreamedTextureDictLoaded(imgDict))
                RequestStreamedTextureDict(imgDict, true);
            else
                DrawSprite(imgDict, "Cola", x_coord, y_coord, yachtRotation.X, yachtRotation.Y, 0, 255, 255, 255, 255);
        }

        private void spawnAllYachtInteriors()
        {
            for (int x = 0; x < yachtInts.Count(); x++)
            {
                int yachtHash = GetHashKey(yachtInts[x]);
                //RequestIpl(yachtList[x]);

                CreateObject(yachtHash, yachtCoords[x].X, yachtCoords[x].Y, yachtCoords[x].Z, true, true, true);
            }
        }

        private void spawnAllYachtLods()
        {
            for (int x = 0; x < yachtLods.Count(); x++)
            {
                int yachtHash = GetHashKey(yachtLods[x]);
                //RequestIpl(yachtList[x]);
                CreateObject(yachtHash, yachtCoords[x].X, yachtCoords[x].Y, yachtCoords[x].Z, true, true, true);
            }
        }
         
        public async Task DESPAWNAllYachtsAsync()
        {
            await Delay(1000);

            for (int x = 0; x < yachtList.Count(); x++)
            {
                Log("remove");
                RemoveIpl(yachtList[x]);
                RemoveIpl(yachtInts[x]);
                RemoveIpl(yachtLods[x]);
            }

            await Delay(1000);
            Log("Cleaned up...");
        }

        public enum PlayerComponents
        {
            COMPONENT_PANTS     = 4,
            COMPONENT_TSHIRT    = 8,
            COMPONENT_VEST      = 9,
            COMPONENT_SHOES     = 6,
            COMPONENT_BAG       = 5,
        }
         

        void OnJacuzziEnter(bool enter)
        {
            int ped = API.PlayerPedId();
            /*int idx_Pants = API.GetPedPropIndex(ped, (int)PlayerComponents.COMPONENT_PANTS);
            int idx_TShirt = API.GetPedPropIndex(ped, (int)PlayerComponents.COMPONENT_TSHIRT);
            int idx_Vest = API.GetPedPropIndex(ped, (int)PlayerComponents.COMPONENT_VEST);
            int idx_Shoes = API.GetPedPropIndex(ped, (int)PlayerComponents.COMPONENT_SHOES);
            int idx_Bag = API.GetPedPropIndex(ped, (int)PlayerComponents.COMPONENT_BAG);*/

            int drawable_comp_id_Pants = API.GetNumberOfPedDrawableVariations(ped, (int)PlayerComponents.COMPONENT_BAG);
            int drawable_comp_id_TShirt = API.GetNumberOfPedDrawableVariations(ped, (int)PlayerComponents.COMPONENT_BAG);
            int drawable_comp_id_Vest = API.GetNumberOfPedDrawableVariations(ped, (int)PlayerComponents.COMPONENT_BAG);
            int drawable_comp_id_Shoes = API.GetNumberOfPedDrawableVariations(ped, (int)PlayerComponents.COMPONENT_BAG);
            int drawable_comp_id_bag = API.GetNumberOfPedDrawableVariations(ped, (int)PlayerComponents.COMPONENT_BAG);
            
            int texture_comp_id_Pants = API.GetNumberOfPedTextureVariations(ped, drawable_comp_id_Pants,(int)PlayerComponents.COMPONENT_BAG);
            int texture_comp_id_TShirt = API.GetNumberOfPedTextureVariations(ped, drawable_comp_id_TShirt, (int)PlayerComponents.COMPONENT_BAG);
            int texture_comp_id_Vest = API.GetNumberOfPedTextureVariations(ped, drawable_comp_id_Vest, (int)PlayerComponents.COMPONENT_BAG);
            int texture_comp_id_Shoes = API.GetNumberOfPedTextureVariations(ped, drawable_comp_id_Shoes, (int)PlayerComponents.COMPONENT_BAG);
            int texture_comp_id_bag = API.GetNumberOfPedTextureVariations(ped, drawable_comp_id_bag, (int)PlayerComponents.COMPONENT_BAG);


            if (enter)
            {
                SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_PANTS, 0, 0, 0);
                SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_TSHIRT, 0, 0, 0);
                SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_VEST, 0, 0, 0);
                SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_SHOES, 0, 0, 0); 
                SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_BAG, 0, 0, 0); 
            } else
            {
                SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_PANTS, drawable_comp_id_Pants, texture_comp_id_Pants, 0);
                SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_TSHIRT, drawable_comp_id_TShirt, texture_comp_id_TShirt, 0);
                SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_VEST, drawable_comp_id_Vest, texture_comp_id_Vest, 0);
                SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_SHOES, drawable_comp_id_Shoes, texture_comp_id_Shoes, 0);
                SetPedComponentVariation(ped, (int)PlayerComponents.COMPONENT_BAG, drawable_comp_id_bag, texture_comp_id_bag, 0);
            }
        }

        // 35 yachts, 35 interiors, 35 lods
        // 36?
        public void UpdateYachtLists()
        {
            Log("...updating yacht lists...");

            yachtList.Add("apa_yacht_grp01_1");
            yachtInts.Add("apa_yacht_grp01_1_int");
            yachtLods.Add("apa_yacht_grp01_1_lod"); 
            yachtCoords.Add(new Vector3(-2814.489f, 4072.740f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0.87900420f, 0.47681410f)); 
 
            yachtList.Add("apa_yacht_grp01_2");
            yachtInts.Add("apa_yacht_grp01_2_int");
            yachtLods.Add("apa_yacht_grp01_2_lod");
            yachtCoords.Add(new Vector3(-3148.37900000f, 2807.55500000f, 5.42995500f));
            yachtQuat.Add(new Vector4(0f, 0f, 0.71906690f, -0.69494090f));

            yachtList.Add("apa_yacht_grp02_1");
            yachtInts.Add("apa_yacht_grp02_1_int");
            yachtLods.Add("apa_yacht_grp02_1_lod");
            yachtCoords.Add(new Vector3(-2814.489f, 4072.740f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0.80924760f, 0.58746780f));

            yachtList.Add("apa_yacht_grp02_2");
            yachtInts.Add("apa_yacht_grp02_2_int");
            yachtLods.Add("apa_yacht_grp02_2_lod");
            yachtCoords.Add(new Vector3(-3254.552f, 3685.676f, 5.42995500f));
            yachtQuat.Add(new Vector4(0f, 0f, -0.65576280f, 0.75496690f));

            yachtList.Add("apa_yacht_grp02_3");
            yachtInts.Add("apa_yacht_grp02_3_int");
            yachtLods.Add("apa_yacht_grp02_3_lod");
            yachtCoords.Add(new Vector3(-2368.441f, 4697.874f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp03_1");
            yachtInts.Add("apa_yacht_grp03_1_int");
            yachtLods.Add("apa_yacht_grp03_1_lod");
            yachtCoords.Add(new Vector3(-3205.344f, -219.0104f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp03_2");
            yachtInts.Add("apa_yacht_grp03_2_int");
            yachtLods.Add("apa_yacht_grp03_2_lod");
            yachtCoords.Add(new Vector3(-3448.254f, 311.5018f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp03_3");
            yachtInts.Add("apa_yacht_grp03_3_int");
            yachtLods.Add("apa_yacht_grp03_3_lod");
            yachtCoords.Add(new Vector3(-1995.725f, -1523.694f, 5.42997f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp04_1");
            yachtInts.Add("apa_yacht_grp04_1_int");
            yachtLods.Add("apa_yacht_grp04_1_lod");
            yachtCoords.Add(new Vector3(-1995.725f, -1523.694f, 5.42997f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp04_2");
            yachtInts.Add("apa_yacht_grp04_2_int");
            yachtLods.Add("apa_yacht_grp04_2_lod");
            yachtCoords.Add(new Vector3(-2117.581f, -2543.346f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp04_3");
            yachtInts.Add("apa_yacht_grp04_3_int");
            yachtLods.Add("apa_yacht_grp04_3_lod");
            yachtCoords.Add(new Vector3(-1605.074f, -1872.468f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp05_1");
            yachtInts.Add("apa_yacht_grp05_1_int");
            yachtLods.Add("apa_yacht_grp05_1_lod");
            yachtCoords.Add(new Vector3(-753.0817f, -3919.068f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp05_2");
            yachtInts.Add("apa_yacht_grp05_2_int");
            yachtLods.Add("apa_yacht_grp05_2_lod");
            yachtCoords.Add(new Vector3(-351.0608f, -3553.323f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp05_3");
            yachtInts.Add("apa_yacht_grp05_2_int");
            yachtLods.Add("apa_yacht_grp05_2_lod");
            yachtCoords.Add(new Vector3(-1460.536f, -3761.467f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp06_1");
            yachtInts.Add("apa_yacht_grp06_1_int");
            yachtLods.Add("apa_yacht_grp06_1_lod");
            yachtCoords.Add(new Vector3(1546.892f, -3045.627f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp06_2");
            yachtInts.Add("apa_yacht_grp06_2_int");
            yachtLods.Add("apa_yacht_grp06_2_lod");
            yachtCoords.Add(new Vector3(2490.886f, -2428.848f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp06_3");
            yachtInts.Add("apa_yacht_grp06_3_int");
            yachtLods.Add("apa_yacht_grp06_3_lod");
            yachtCoords.Add(new Vector3(2049.79f, -2821.624f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp07_1");
            yachtInts.Add("apa_yacht_grp07_1_int");
            yachtLods.Add("apa_yacht_grp07_1_lod");
            yachtCoords.Add(new Vector3(3029.018f, -1495.702f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp07_2");
            yachtInts.Add("apa_yacht_grp07_2_int");
            yachtLods.Add("apa_yacht_grp07_2_lod");
            yachtCoords.Add(new Vector3(3021.254f, -723.3903f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp07_3");
            yachtInts.Add("apa_yacht_grp07_3_int");
            yachtLods.Add("apa_yacht_grp07_3_lod");
            yachtCoords.Add(new Vector3(2976.622f, -1994.76f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp08_1");
            yachtInts.Add("apa_yacht_grp08_1_int");
            yachtLods.Add("apa_yacht_grp08_1_lod");
            yachtCoords.Add(new Vector3(3404.51f, 1977.0440f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp08_2");
            yachtInts.Add("apa_yacht_grp08_1_int");
            yachtLods.Add("apa_yacht_grp08_1_lod");
            yachtCoords.Add(new Vector3(3411.100f, 1193.445f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp08_3");
            yachtInts.Add("apa_yacht_grp08_3_int");
            yachtLods.Add("apa_yacht_grp08_3_lod");
            yachtCoords.Add(new Vector3(3784.802f, 2548.541f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp09_1");
            yachtInts.Add("apa_yacht_grp09_1_int");
            yachtLods.Add("apa_yacht_grp09_1_lod");
            yachtCoords.Add(new Vector3(4225.0280f, 3988.002f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp09_2");
            yachtInts.Add("apa_yacht_grp09_2_int");
            yachtLods.Add("apa_yacht_grp09_2_lod");
            yachtCoords.Add(new Vector3(4250.581f, 4576.565f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp09_3");
            yachtInts.Add("apa_yacht_grp09_2_int");
            yachtLods.Add("apa_yacht_grp09_2_lod");
            yachtCoords.Add(new Vector3(4204.3560f, 3373.70f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp10_1");
            yachtInts.Add("apa_yacht_grp10_2_int");
            yachtLods.Add("apa_yacht_grp10_2_lod");
            yachtCoords.Add(new Vector3(3751.681f, 5753.501f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp10_2");
            yachtInts.Add("apa_yacht_grp10_2_int");
            yachtLods.Add("apa_yacht_grp10_2_lod");
            yachtCoords.Add(new Vector3(3490.105f, 6305.785f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp10_3");
            yachtInts.Add("apa_yacht_grp10_3_int");
            yachtLods.Add("apa_yacht_grp10_3_lod");
            yachtCoords.Add(new Vector3(3684.853f, 5212.238f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp11_1");
            yachtInts.Add("apa_yacht_grp11_1_int");
            yachtLods.Add("apa_yacht_grp11_1_lod");
            yachtCoords.Add(new Vector3(581.5955f, 7124.558f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp11_2");
            yachtInts.Add("apa_yacht_grp11_2_int");
            yachtLods.Add("apa_yacht_grp11_2_lod");
            yachtCoords.Add(new Vector3(2004.462f, 6907.157f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp11_3");
            yachtInts.Add("apa_yacht_grp11_3_int");
            yachtLods.Add("apa_yacht_grp11_3_lod");
            yachtCoords.Add(new Vector3(1396.638f, 6860.203f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp12_1");
            yachtInts.Add("apa_yacht_grp12_1_int");
            yachtLods.Add("apa_yacht_grp12_1_lod");
            yachtCoords.Add(new Vector3(-1170.690f, 5.429955f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp12_2");
            yachtInts.Add("apa_yacht_grp12_2_int");
            yachtLods.Add("apa_yacht_grp12_2_lod");
            yachtCoords.Add(new Vector3(-777.4865f, 6566.907f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            yachtList.Add("apa_yacht_grp12_3");
            yachtInts.Add("apa_yacht_grp12_3_int");
            yachtLods.Add("apa_yacht_grp12_3_lod");
            yachtCoords.Add(new Vector3(-381.7739f, 6946.960f, 5.429955f));
            yachtQuat.Add(new Vector4(0f, 0f, 0f, 0f));

            Log("...updating yacht lists...");
        }
    }

    internal class YachtEnt : YachtClient
    {
        public Prop Entity = new Prop(0);
        public int Index;
        public Prop a;
        public Prop b;
        public Prop c;
        public Prop d;
        public Prop e;
        public Prop f;
        public Prop g;
        public Prop i;
        public Prop j;
        public Prop aa;
        public Prop ab;
        public Prop ac;
        public Prop ad;
        public Prop ae;
        public Prop af;
        public Prop ag;
        public Prop ai;
        public float head_with_offset;
        public YachtEnt(int index)
        {
            Index = index;
        }
    }
}
