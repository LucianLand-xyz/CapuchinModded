using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class DrawInspectorClass
    {
        private static List<string> components = new List<string>();
        private static List<string> classes = new List<string>();
        private static List<string> methods = new List<string>();
        private static string currentComp = "";

        public static void DrawInspector()
        {
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(600.0f, 1000.0f), ImGuiCond.Once);

            if (!ImGui.Begin("Inspector", ImGuiWindowFlags.None))
            {
                ImGui.End();
                return;
            }

            ImGui.Text("Currently Work In Progress! Come Back Soon.");

            /*
            if (ImGui.Button("Update##comp"))
            {
                components = Dumper.DumpComponentsString(); 
            }

            ImGui.SetNextItemWidth(150f);
            int componentCurrentIdx = 0; 
            ImGuiTextFilter cFilter = new ImGuiTextFilter();
            cFilter.Draw("Search##compfilter");

            if (ImGui.BeginListBox("##Components", new System.Numerics.Vector2(-1, 200)))
            {
                for (int i = 0; i < components.Count; i++)
                {
                    if (!cFilter.PassFilter(components[i])) continue;

                    bool isSelected = componentCurrentIdx == i;
                    if (ImGui.Selectable(components[i], isSelected))
                        componentCurrentIdx = i;

                    if (isSelected)
                        ImGui.SetItemDefaultFocus();
                }
                ImGui.EndListBox();
            }

            */
          

            ImGui.End();
        }
    }
}
