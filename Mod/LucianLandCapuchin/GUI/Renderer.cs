#region using shit
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ClickableTransparentOverlay;
using LucianLand.Settings;
using ImGuiNET;
using LucianLand.Settings;
using Microsoft.VisualBasic;
using LucianLandCapuchin;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;
using Vector4 = System.Numerics.Vector4;
using Utils;
#endregion

namespace LucianLamd
{

    public class RendererTest : Overlay
    {
        #region var

        public Vector2 screenSize = new Vector2(3440, 1440); // if you are debugging this, change this to your actual screen size for your moniter
        public static Vector2 WindowSize = new Vector2 { X = 620f, Y = 335f };
        public static uint TabBarFlags = (uint)ImGuiTabBarFlags.Reorderable | (uint)ImGuiTabBarFlags.TabListPopupButton | (uint)ImGuiTabBarFlags.NoCloseWithMiddleMouseButton;
        public static int Tab = 0;
        static bool[] TabBools = { true, true, true, true, }; // Persistent user state
        public static bool _isMyUIOpen = true;
        public static sbyte TextBuffer = 12;

        // entity copy and safe shit
        private ConcurrentQueue<Entity> entities = new ConcurrentQueue<Entity>();
        private Entity LocalPlayer = new Entity();
        private readonly object EntityLock = new object();

        // draw esp list
        ImDrawListPtr drawList;

        // tab shit
        private bool SelfTab;
        private bool ServerTab;
        private bool VisualTab;
        private bool PlayerListTab;


        private bool Teleport;


        #endregion

        protected override void Render()
        {
            if (Configuration.ShowInspector)
            {
                Utils.DrawInspector();
            }

            ImGuiThemes.ApplyTheme();

            string[] names = { "Self", "Server", "Visual", "Developer" };
            var dummy2 = true;
            ImGui.SetNextWindowSize(WindowSize, 0);

            if (ImGui.Begin($"LucianLand - Capuchin v2.0.0", ref dummy2, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize))
            {
                if (ImGui.BeginTabBar("#Tabs", (ImGuiTabBarFlags)TabBarFlags | ImGuiTabBarFlags.NoTabListScrollingButtons))
                {
                    for (int n = 0; n < Math.Min(TabBools.Length, names.Length); n++)
                    {
                        if (ImGui.BeginTabItem(names[n], ref TabBools[n], 0))
                        {
                            Tab = n;
                            if (n == 0)
                            {
                                ImGui.Text("Self");
                            }
                            else if (n == 1)
                            {
                                ImGui.Text("Server");
                            }
                            else if(n == 2)
                            {
                                ImGui.Text("Visual");
                            }
                            else if (n == 3)
                            {
                                ImGui.Checkbox("Enable Developer Options", ref Configuration.EnableDeveloperOption);

                                if (Configuration.EnableDeveloperOption)
                                {
                                    ImGui.Indent();
                                   // ImGui.Checkbox("Show Inspector", ref Configuration.ShowInspector);
                                    ImGui.Spacing();

                                    { // test things
                                        ImGui.Text("Test Objects");
                                        ImGui.SameLine();
                                        ImGui.InputTextWithHint("##SearchObject", "Name of a component...", ref TestObjects.Name, 200);

                                        ImGui.Checkbox("Test Objects Chams", ref TestObjects.Chams);
                                        ImGui.SameLine();
                                        ImGui.Checkbox("Test Objects Snapline", ref TestObjects.Snapline);
                                        ImGui.Checkbox("Test Objects Box", ref TestObjects.Box);
                                        ImGui.SameLine();
                                        ImGui.Checkbox("Test Objects Aimbot", ref TestObjects.Aimbot);
                                    }
                                    ImGui.Unindent();

                                }
                            }

                            ImGui.EndTabItem();
                        }
                    }
                    ImGui.EndTabBar();
                }
            }

            #region Developer Shit

           

            #endregion

            #region Watermark
            var dummyBool = true;
            ImGui.SetNextWindowSize(new Vector2 { X = 220f, Y = 25f }, 0);
            ImGui.SetNextWindowPos(new Vector2 { X = 10f, Y = 10f }, 0, new Vector2 { X = 0f, Y = 0f });
            if (ImGui.Begin("#WoahWatermark", ref dummyBool, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize))
            {
                ImGui.Text($" LucianLand - Capuchin | v2.0");
            }
            ImGui.End();
            #endregion
        }


        #region draw methords

        bool EntityOnScreen(Entity entity)
        {
            if (entity.pos2D.X > 0 && entity.pos2D.X < screenSize.X && entity.pos2D.Y > 0 && entity.pos2D.Y < screenSize.Y)
            {
                return true;
            }
            return false;
        }

        private void DrawBox(Entity entity)
        {
            float entityHeight = entity.pos2D.Y - entity.pos2D.Y;

            Vector2 rectTop = new Vector2(entity.viewPos2D.X - entityHeight / 3, entity.viewPos2D.Y);

            Vector2 rectBottom = new Vector2(entity.pos2D.X + entityHeight / 3, entity.pos2D.Y);

            Vector4 boxColor = LocalPlayer.team == entity.team ? Configuration.PlayerEspColor : Configuration.PlayerEspColor;

            drawList.AddRect(rectTop, rectBottom, ImGui.ColorConvertFloat4ToU32(boxColor));

        }

        private void DrawLine(Entity entity)
        {
            Vector4 lineColor = LocalPlayer.team == entity.team ? Configuration.PlayerEspColor : Configuration.PlayerEspColor;

            drawList.AddLine(new Vector2(screenSize.X / 2, screenSize.Y), entity.pos2D, ImGui.ColorConvertFloat4ToU32(lineColor));
        }


        #endregion

        #region transfer entities
        public void UpdateEntities(IEnumerable<Entity> NewEntities)
        {
            entities = new ConcurrentQueue<Entity>(NewEntities);
        }

        public void UpdateLocalPlayer(Entity newentity)
        {
            lock (EntityLock)
            {
                LocalPlayer = newentity;
            }
        }

        public Entity GetLocalPlayer()
        {
            lock (EntityLock)
            {
                return LocalPlayer;
            }
        }

        void DrawOverlay(Vector2 ScreenSize)
        {
            ImGui.SetNextWindowSize(ScreenSize);
            ImGui.SetNextWindowPos(new Vector2(0, 0));
            ImGui.Begin("overlay", ImGuiWindowFlags.NoDecoration
                | ImGuiWindowFlags.NoBackground
                | ImGuiWindowFlags.NoBringToFrontOnFocus
                | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoInputs
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoScrollWithMouse
                );
        }

        #endregion


    }
}
