using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;

namespace ResetCamera
{
    internal class PluginUI : IDisposable
    {
        // reference fields
        private Configuration configuration;

        public PluginUI(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public bool SettingsVisible;

        public void Dispose()
        {
        }

        public void Draw()
        {
            try
            {
                DrawSettingsWindow();
            }
            catch (Exception ex)
            {
                Plugin.PluginLog.Error(ex, "Prevented GUI Crash");
            }
        }

        private static bool ImGuiFloatInput(string text, string label, ref float f, float step, float min, float max)
        {
            ImGui.Text(text);
            ImGui.PushItemWidth(160);
            var result = ImGui.DragFloat(label, ref f, step, min, max, "%.2f");
            ImGui.PopItemWidth();
            return result;
        }

        private void DrawSettingsWindow()
        {
            if (!SettingsVisible) return;

            ImGui.SetNextWindowSize(new Vector2(400, 500), ImGuiCond.FirstUseEver);
            if (!ImGui.Begin("Reset Camera Config", ref SettingsVisible))
            {
                ImGui.End();
                return;
            }

            ImGui.Spacing();

            if (ImGui.CollapsingHeader("[No Name] Settings", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Text("This data is for /rcreset when it is used without a name.");
                ImGui.Spacing();
                ImGui.Text("Use /rcsave without a name to overwrite these numbers.");
                ImGui.Spacing();
                ImGui.Text("Then, come in here to tweak them further to your liking.");
                ImGui.Spacing();
                ImGui.Text("Use /rcsave <name> to set or create a new profile.");
                ImGui.Spacing();
                ImGui.Text("Use /rcdelete <name> to delete the profile.");
                ImGui.Spacing();

                if(ImGuiFloatInput("Distance", "##distance", ref configuration.Distance, 1, 0, 100)) configuration.Save();
                
                float hRotationDegrees = float.RadiansToDegrees(configuration.HRotation);
                if(ImGuiFloatInput("HRotation Degrees", "##hrotation", ref hRotationDegrees, 1f, 0, 360f))
                {
                    configuration.HRotation = float.DegreesToRadians(hRotationDegrees);
                    configuration.Save();
                }
                
                if(ImGuiFloatInput("VRotation", "##vrotation", ref configuration.VRotation, 0.01f, -1.49f, 0.79f)) configuration.Save();
                
                float rollDegrees = float.RadiansToDegrees(configuration.Roll);
                if(ImGuiFloatInput("Roll Degrees", "##roll", ref rollDegrees, 1f, 0, 360f))
                {
                    configuration.Roll = float.DegreesToRadians(rollDegrees);
                    configuration.Save();
                }
            }

            ImGui.Spacing();

            foreach(KeyValuePair<string, Configuration.SavedCameraInfo> entry in configuration.OtherSavedCameraInfos)
            {
                if (ImGui.CollapsingHeader(entry.Key + " Settings"))
                {
                    if(ImGuiFloatInput("Distance", "##distance" + entry.Key, ref entry.Value.Distance, 1, 0, 100)) configuration.Save();
                    
                    float hRotationDegrees = float.RadiansToDegrees(entry.Value.HRotation);
                    if(ImGuiFloatInput("HRotation Degrees", "##hrotation" + entry.Key, ref hRotationDegrees, 1f, 0, 360f))
                    {
                        entry.Value.HRotation = float.DegreesToRadians(hRotationDegrees);
                        configuration.Save();
                    }
                    
                    if(ImGuiFloatInput("VRotation", "##vrotation" + entry.Key, ref entry.Value.VRotation, 0.01f, -1.49f, 0.79f)) configuration.Save();
                    
                    float rollDegrees = float.RadiansToDegrees(entry.Value.Roll);
                    if(ImGuiFloatInput("Roll Degrees", "##roll" + entry.Key, ref rollDegrees, 1f, 0, 360f))
                    {
                        entry.Value.Roll = float.DegreesToRadians(rollDegrees);
                        configuration.Save();
                    }
                }
                ImGui.Spacing();
            }

            ImGui.End();
        }
    }
}
