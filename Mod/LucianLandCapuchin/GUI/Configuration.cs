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
        // self tab


        // server tab


        // visual tab

        public static bool esp = false;
        public static Vector4 PlayerEspColor = new Vector4(218, 0, 255, 1);


        // playerlist shit

        public static bool Teleport;
        public static bool Kick;
        public static bool Lag;
        public static bool Kill;

        string Name = "UnityEngine.CapsuleCollider";
        bool Chams = false;
        bool Snapline = false;
        bool Box = false;
        bool Aimbot = false;

        public static bool EnableDeveloperOption;

        public static float FakeHeadPosDiff = 1;
        public static float FakeFeetPosDiff = 1;

    }

    public static class TestObjects
    {
        public static List<GameObject> List = new List<GameObject>();
        public static string Name = "UnityEngine.CapsuleCollider";
        public static bool Chams = false;
        public static bool Snapline = false;
        public static Color SnaplineColor = new Color(255.0f / 255, 255.0f / 255, 255.0f / 255);
        public static bool Box = false;
        public static Color BoxColor = new Color(255.0f / 255, 255.0f / 255, 255.0f / 255);
        public static bool Aimbot = false;
    }
}
