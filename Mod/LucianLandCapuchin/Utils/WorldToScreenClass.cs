using LucianLand.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class WorldToScreenClass
    {

        public static bool WorldToScreen(Vector3 world, out UnityEngine.Vector2 screen)
        {
            screen = Vector2.zero;

            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return false;
            }

            Vector3 buffer = mainCamera.WorldToScreenPoint(world);

            if (buffer.x > Configuration.ScreenSize.X || buffer.y > Configuration.ScreenSize.Y || buffer.x < 0 || buffer.y < 0 || buffer.z < 0)
            {
                return false;
            }

            if (buffer.z > 0.0f)
            {
                screen = new Vector2(buffer.x, Configuration.ScreenSize.Y - buffer.y); // Invert y for screen space coordinates
            }

            if (screen.x > 0 || screen.y > 0)
            {
                return true;
            }

            return false;
        }
    }
}
