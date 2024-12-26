using MelonLoader;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Rendering.Universal.LibTessDotNet;

namespace LucianLand.MainMenu
{
    public enum ToggleTypeSelf
    {
        NoFog,
        NightVision,
        GodMode,
        Speed,
        Fly,
        NoWeight,
        NoFallDamage,
        LootThroughWalls,
        Invisibility
    }
    public enum ToggleTypeServer
    {
        EnemyCantBeSpawned,
        Gets_All_Scrap,
        Never_Lose_Scrap,
        Disable_All_Turrets,
        Explode_All_Landmines
    }
    public enum ToggleTypeVisuals
    {
        Object_ESP,
        Player_ESP,
        Enemy_ESP
    }
    public enum ToggleTypeHost
    {
        Revive_All_Players,
        Spawn_Enemy,
        Force_Start,
        Force_EndGame
    }


    public class MainGUI : MelonMod
    {
 

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("LucianLand Mod Loaded!");
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                if (!GameObject.Find("LucianGUI"))
                {
                    var guiObject = new GameObject("LucianGUI");
                    guiObject.AddComponent<Render>();
                    UnityEngine.Object.DontDestroyOnLoad(guiObject);
                }
            }
        }

        #region var

        public static MainGUI _instance;
        public static MainGUI Instance => _instance;

        public Dictionary<ToggleTypeSelf, bool> ToggleSelf;
        public Dictionary<ToggleTypeServer, bool> ToggleServer;
        public Dictionary<ToggleTypeVisuals, bool> ToggleVisuals;
        public Dictionary<ToggleTypeHost, bool> ToggleHost;

        public static string strTooltip = null;
        public bool didit = true;
        public Texture2D button, buttonHovered, buttonActive, windowBackground, textArea, textAreaHovered, textAreaActive, box;
        public static Rect GUIRect = new Rect(0, 0, 540, 240);
        public static int selectedTab = 0;
        public static readonly string[] tabNames = { "Self", "Server Info", "Visuals", "Troll", "Player List", "Settings" };
        public Vector2 scrollPosition = Vector2.zero;
        public bool toggled = true;
        public static Color c_Theme;
        public float toggleDelay = 0.5f;
        public float lastToggleTime;
        public KeyCode toggleKey = KeyCode.Insert;
        public GameObject directionalLightClone;
        internal static bool nightVision;
        public Dictionary<Type, List<Component>> objectCache = new Dictionary<Type, List<Component>>();
        public bool joining = false;
        public bool shouldSpawnEnemy;
        public int numberDeadLastRound;
        public static bool Cursorlock = false;
        public int SpeedSelection = 0;
        public static GameObject pointer;
        public string[] SpeedOptions = new string[] { "Slow", "Default", "Fast", "Super Fast", "FASTTTTTTTS" };
        public float[] SpeedValues = new float[] { 5.0f, 7.5f, 15.0f, 17.5f, 20.0f };
        public float Speed = 1.7f;
        internal static bool playerManagerEnabled = false;
        public string guiSelectedEnemy;
        public static bool hasGUISynced;
        public System.Random random = new System.Random();
        public float cacheRefreshInterval = 1.5f;
        public int enemyCount = 0;
        public bool addMoney = false;
        public Photon.Realtime.Player selectedPlayer;

        #endregion

        public bool ToggleButton(string text, bool toggle, string Tooltip = "")
        {
            GUIStyle buttonStyle = Render.Instance.CreateButtonStyle(toggle ? buttonActive : button, buttonHovered, buttonActive);
            if (GUILayout.Button(text, buttonStyle)) { return !toggle; }
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                strTooltip = Tooltip;
            }
            return toggle;
        }
        public void Awake()
        {
            ToggleSelf = Enum.GetValues(typeof(ToggleTypeSelf)).Cast<ToggleTypeSelf>().ToDictionary(t => t, t => false);
            ToggleServer = Enum.GetValues(typeof(ToggleTypeServer)).Cast<ToggleTypeServer>().ToDictionary(t => t, t => false);
            ToggleVisuals = Enum.GetValues(typeof(ToggleTypeVisuals)).Cast<ToggleTypeVisuals>().ToDictionary(t => t, t => false);
            ToggleHost = Enum.GetValues(typeof(ToggleTypeHost)).Cast<ToggleTypeHost>().ToDictionary(t => t, t => false);

            _instance = this;
            _instance = this;
            if (didit)
            {
                c_Theme = new Color32(75, 75, 75, 255);
                buttonActive = Render.Instance.CreateTexture(new Color32(100, 100, 100, 255));
                buttonHovered = Render.Instance.CreateTexture(new Color32(75, 75, 75, 255));
                button = Render.Instance.CreateTexture(new Color32(64, 64, 64, 255));
                windowBackground = Render.Instance.CreateTexture(new Color32(30, 30, 30, 255));
                textArea = Render.Instance.CreateTexture(new Color32(64, 64, 64, 255));
                textAreaHovered = Render.Instance.CreateTexture(new Color32(75, 75, 75, 255));
                textAreaActive = Render.Instance.CreateTexture(new Color32(100, 100, 100, 255));
                box = Render.Instance.CreateTexture(new Color32(40, 40, 40, 255));
                didit = false;
            }
        }
        public void DrawPlayerListTab()
        {
            _instance.scrollPosition = GUILayout.BeginScrollView(_instance.scrollPosition);
            int num = 1;

            foreach (Photon.Realtime.Player player in Photon.Pun.PhotonNetwork.PlayerList)
            {
                string playerUsername = player.NickName;

                GUILayout.BeginHorizontal();
                GUILayout.Label("Player " + num.ToString() + ": " + playerUsername);

                if (GUILayout.Button("i", GUILayout.Width(20), GUILayout.Height(20))) { selectedPlayer = player; playerManagerEnabled = true; }

                GUILayout.EndHorizontal();

                if (playerManagerEnabled && selectedPlayer == player)
                {

                    if (GUILayout.Button("TP")) { MelonLoader.MelonLogger.LogError("Failed to tp. Lucian is ass at coding just so uk."); }

                    if (GUILayout.Button("Back")) { selectedPlayer = null; playerManagerEnabled = false; }
                }

                num++;
            }

            GUILayout.EndScrollView();
        }
        public void DrawSettingsTab()
        {
            _instance.scrollPosition = GUILayout.BeginScrollView(_instance.scrollPosition);

            GUILayout.Label("Speed Boost: " + SpeedOptions[SpeedSelection]);
            SpeedSelection = (int)GUILayout.HorizontalSlider(SpeedSelection, 0, SpeedOptions.Length - 1);
            Speed = SpeedValues[SpeedSelection];
            Render.ColorPicker("Theme", ref c_Theme);
            GUILayout.EndScrollView();
        }
        public override void OnGUI()
        {
            GUI.skin = GUI.skin ?? new GUISkin();
            if (toggled) { 
                GUIRect = GUI.Window(69, GUIRect, (GUI.WindowFunction)OnGUII, "LucianLand.xyz - Capuchin | Toggle: " + toggleKey);
                GUILayout.BeginArea(new Rect(10, 10, GUIRect.width - 20, GUIRect.height - 20));
                GUILayout.Space(10);

                DrawTabButtons();

                if (selectedTab == 0)
                {
                    Instance.DrawTab(Instance.ToggleSelf);
                }
                else if (selectedTab == 1)
                {
                    Instance.DrawTab(Instance.ToggleServer);
                }
                else if (selectedTab == 2)
                {
                    Instance.DrawTab(Instance.ToggleVisuals);
                }
                else if (selectedTab == 3)
                {
                    Instance.DrawTab(Instance.ToggleHost);
                }
                else if (selectedTab == 4)
                {
                    _instance.DrawPlayerListTab();
                }
                else if (selectedTab == 5)
                {
                    _instance.DrawSettingsTab();
                }

                GUILayout.EndArea();
                GUI.DragWindow(new Rect(0, 0, GUIRect.width, 20));
                Render.RenderTooltip();
            }
        }
        public static void OnGUII(int windowID)
        {
            //Instance.Update();
            GUILayout.BeginArea(new Rect(10, 10, GUIRect.width - 20, GUIRect.height - 20));
            GUILayout.Space(10);

            DrawTabButtons();

            if (selectedTab == 0)
            {
                Instance.DrawTab(Instance.ToggleSelf);
            }
            else if (selectedTab == 1)
            {
                Instance.DrawTab(Instance.ToggleServer);
            }
            else if (selectedTab == 2)
            {
                Instance.DrawTab(Instance.ToggleVisuals);
            }
            else if (selectedTab == 3)
            {
                Instance.DrawTab(Instance.ToggleHost);
            }
            else if (selectedTab == 4)
            {
                _instance.DrawPlayerListTab();
            }
            else if (selectedTab == 5)
            {
                _instance.DrawSettingsTab();
            }

            GUILayout.EndArea();
            GUI.DragWindow(new Rect(0, 0, GUIRect.width, 20));
            Render.RenderTooltip();
        }

        public void DrawGUI(int windowId)
        {
            GUILayout.BeginArea(new Rect(10, 10, GUIRect.width - 20, GUIRect.height - 20));
            GUI.Label(new Rect(10, 10, 200, 20), "");

            DrawTabButtons();

            if (selectedTab == 0) DrawTab(ToggleSelf);
            else if (selectedTab == 1) DrawTab(ToggleServer);
            else if (selectedTab == 2) DrawTab(ToggleVisuals);
            else if (selectedTab == 3) DrawTab(ToggleHost);

            GUILayout.EndArea();
            GUI.DragWindow(new Rect(0, 0, GUIRect.width, 20));
        }
        public void GUIToggleCheck()
        {
            if (UnityEngine.Input.GetKey(toggleKey))
            {
                if (Time.time - lastToggleTime >= toggleDelay)
                {
                    toggled = !toggled;
                    lastToggleTime = Time.time;
                }
            }
        }
        public void DrawTab<T>(Dictionary<T, bool> toggleDictionary) where T : Enum
        {

            Vector2 scrollPosition = Vector2.zero;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(500), GUILayout.Height(400));

            foreach (var toggle in toggleDictionary.Keys.ToList())
            {
                string toggleName = toggle.ToString().Replace("_", " ");
                toggleDictionary[toggle] = GUILayout.Toggle(toggleDictionary[toggle], toggleName);
            }

            GUI.EndScrollView();
        }

        public static void DrawTabButtons()
        {
            GUILayout.BeginHorizontal();

            for (int i = 0; i < tabNames.Length; i++)
            {
                if (selectedTab == i)
                {
                    GUIStyle selectedStyle = Render.Instance.CreateButtonStyle(Instance.buttonActive, Instance.buttonHovered, Instance.buttonActive);
                    if (GUILayout.Button(tabNames[i], selectedStyle))
                    {
                        selectedTab = i;
                    }
                }
                else
                {
                    GUIStyle unselectedStyle = Render.Instance.CreateButtonStyle(Instance.button, Instance.buttonHovered, Instance.buttonActive);
                    if (GUILayout.Button(tabNames[i], unselectedStyle))
                    {
                        selectedTab = i;
                    }
                }
            }

            GUILayout.EndHorizontal();
        }

        #region Mods

        // converted my old c++ esp to c# so thats why this is here
        public static bool WorldToScreen(Camera camera, Vector3 world, out Vector3 screen)
        {
            screen = camera.WorldToViewportPoint(world);
            screen.x *= (float)Screen.width;
            screen.y *= (float)Screen.height;
            screen.y = (float)Screen.height - screen.y;
            return screen.z > 0f;
        }
        #endregion
    }

    /*
    public class Loader : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("LucianLand Mod Loaded!");
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                if (!GameObject.Find("LucianGUI"))
                {
                    var guiObject = new GameObject("LucianGUI");
                    guiObject.AddComponent<MainGUI>();
                    UnityEngine.Object.DontDestroyOnLoad(guiObject);
                }
            }
        }
    }
    */
}
