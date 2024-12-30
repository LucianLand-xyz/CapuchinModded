using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vector4 = System.Numerics.Vector4;

namespace LucianLand.Settings
{
    public class Configuration 
    {
        public static bool CameraFovChanger = false;
        public static float CameraCustomFOV = 80.0f;

        public static bool esp = false;
        public static Vector4 PlayerEspColor = new Vector4(218, 0, 255, 1);
        public static System.Numerics.Vector3 BoxColor = new System.Numerics.Vector3(218, 0, 255);
        public static bool PlayersSnapline = false;
        public static bool RainbowPlayersSnapline = false;
        public static System.Numerics.Vector3 PlayersSnaplineColor = new System.Numerics.Vector3(218, 0, 255);
        public static int PlayersSnaplineType = 2;

        public static System.Numerics.Vector2 ScreenSize = new System.Numerics.Vector2( 0, 0 );
        public static System.Numerics.Vector2 ScreenCenter = new System.Numerics.Vector2(0, 0 );


        public static bool Teleport;
        public static bool Kick;
        public static bool Lag;
        public static bool Kill;

        public static string Name = "UnityEngine.CapsuleCollider";
        public static bool Chams = false;
        public static bool Snapline = false;
        public static bool Box = false;
        public static bool Aimbot = false;

        public static bool EnableDeveloperOption;

        public static float FakeHeadPosDiff = 1;
        public static float FakeFeetPosDiff = 1;

    }

    public static class TestObjects
    {
        public static List<GameObject> List = new List<GameObject>();
        public static string Name = "UnityEngine.CapsuleCollider";
        public static bool Chams = false;
        public static bool ShowInspector = false;
        public static bool Snapline = false;
        public static Color SnaplineColor = new Color(255.0f / 255, 255.0f / 255, 255.0f / 255);
        public static bool Box = false;
        public static Color BoxColor = new Color(255.0f / 255, 255.0f / 255, 255.0f / 255);
        public static bool Aimbot = false;
    }
}
