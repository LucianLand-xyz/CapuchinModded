#region using shit
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ClickableTransparentOverlay;
using ImGuiNET;
#endregion

namespace LucianLandCapuchin
{
    public class Renderer : Overlay
    {
        #region var

        public Vector2 screenSize = new Vector2(3440, 1440); // if you are debugging this, change this to your actual screen size for your moniter

        // entity copy and safe shit
        private ConcurrentQueue<Entity> entities = new ConcurrentQueue<Entity>();
        private Entity LocalPlayer = new Entity();
        private readonly object EntityLock = new object();

        // gui var
        private bool esp = false;
        private Vector4 PlayerEspColor = new Vector4(218, 0, 255, 1);


        // draw esp list
        ImDrawListPtr drawList;

        #endregion

        protected override void Render()
        {

            ImGui.Begin("LucianLand - Capuchin");
            ImGui.Checkbox("Player ESP", ref esp);

            if (ImGui.CollapsingHeader("Player ESP Color"))
                ImGui.ColorPicker4("##playercolor", ref PlayerEspColor);

            DrawOverlay(screenSize);
            drawList = ImGui.GetWindowDrawList();

            if (esp)
            {
                foreach (var entity in entities)
                {
                    if (EntityOnScreen(entity))
                    {
                        DrawBox(entity);
                        DrawLine(entity);
                    }
                }
            }
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

            Vector4 boxColor = LocalPlayer.team == entity.team ? PlayerEspColor : PlayerEspColor;

            drawList.AddRect(rectTop, rectBottom, ImGui.ColorConvertFloat4ToU32(boxColor));

        }

        private void DrawLine(Entity entity)
        {
            Vector4 lineColor = LocalPlayer.team == entity.team ? PlayerEspColor : PlayerEspColor;

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
