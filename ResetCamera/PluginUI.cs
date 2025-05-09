using System;
using System.Numerics;
using ImGuiNET;

namespace ResetCamera
{
    internal class PluginUI : IDisposable
    {
        // reference fields
        private Configuration configuration;
        private float hRotationDegrees;
        private float rollDegrees;

        public PluginUI(Configuration configuration)
        {
            this.configuration = configuration;
            hRotationDegrees = float.RadiansToDegrees(configuration.HRotation);
            rollDegrees = float.RadiansToDegrees(configuration.Roll);
            configuration.OnSave += () => {
                hRotationDegrees = float.RadiansToDegrees(configuration.HRotation);
                rollDegrees = float.RadiansToDegrees(configuration.Roll);
            };
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

            ImGui.SetNextWindowSize(new Vector2(400, 350));
            if (!ImGui.Begin("Reset Camera Config", ref SettingsVisible, ImGuiWindowFlags.NoResize))
            {
                ImGui.End();
                return;
            }

            ImGui.Spacing();

            if (ImGui.CollapsingHeader("Settings", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Text("This data is what /rcreset references.");
                ImGui.Spacing();
                ImGui.Text("Use /rcsave to overwrite these numbers.");
                ImGui.Spacing();
                ImGui.Text("Then, come in here to tweak them further to your liking.");
                ImGui.Spacing();

                if(ImGuiFloatInput("Distance", "##distance", ref configuration.Distance, 1, 0, 100)) configuration.Save();
                
                if(ImGuiFloatInput("HRotation Degrees", "##hrotation", ref hRotationDegrees, 1f, 0, 360f))
                {
                    configuration.HRotation = float.DegreesToRadians(hRotationDegrees);
                    configuration.Save();
                }
                
                if(ImGuiFloatInput("VRotation", "##vrotation", ref configuration.VRotation, 0.01f, -1.49f, 0.79f)) configuration.Save();
                
                if(ImGuiFloatInput("Roll Degrees", "##roll", ref rollDegrees, 1f, 0, 360f))
                {
                    configuration.Roll = float.DegreesToRadians(rollDegrees);
                    configuration.Save();
                }
            }

            ImGui.End();
        }
    }
}
