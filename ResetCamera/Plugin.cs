using System;
using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ResetCamera.Structures;

namespace ResetCamera
{
    public sealed unsafe class Plugin : IDalamudPlugin
    {

        private const string SaveCommandName = "/rcsave";

        private const string ResetCommandName = "/rcreset";

        private const string ConfigCommandName = "/rcconfig";

        [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;

        [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
        [PluginService] public static ISigScanner SigScanner { get; private set; } = null!;
        [PluginService] public static IPluginLog PluginLog { get; private set; } = null!;

        public Configuration Configuration { get; init; }

        private CameraManager* cameraManager { get; init; }

        [NonSerialized]
        private static GameCamera* Camera;

        private PluginUI? ui;

        public Plugin()
        {
            this.cameraManager = (CameraManager*)FFXIVClientStructs.FFXIV.Client.Game.Control.CameraManager.Instance();
            Camera = cameraManager->WorldCamera;

            this.Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(PluginInterface);

            CommandManager.AddHandler(SaveCommandName, new CommandInfo(SaveOnCommand)
            {
                HelpMessage = "Saves the current camera position for resetting."
            });

            CommandManager.AddHandler(ResetCommandName, new CommandInfo(ResetOnCommand)
            {
                HelpMessage = "Resets the camera position to the saved position."
            });

            CommandManager.AddHandler(ConfigCommandName, new CommandInfo(ConfigOnCommand)
            {
                HelpMessage = "Opens the configuration window for the Reset Camera addon."
            });

            ui = new PluginUI(this.Configuration);

            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += ToggleDrawConfigUI;

            PluginLog.Info("Plugin load finished!");
        }

        public void Dispose()
        {
            CommandManager.RemoveHandler(SaveCommandName);
            CommandManager.RemoveHandler(ResetCommandName);
        }

        private void SaveOnCommand(string command, string args)
        {
            // Save the camera's current position
            PluginLog.Info("Saving...");

            Configuration.Distance = (Camera->Mode == 0) ? 0f : Camera->Distance;
            Configuration.HRotation = Camera->HRotation;
            Configuration.VRotation = Camera->VRotation;
            Configuration.ZoomFoV = Camera->FoV;
            Configuration.GposeFoV = Camera->AddedFoV;
            Configuration.Pan = Camera->Pan;
            Configuration.Tilt = Camera->Tilt;
            Configuration.Roll = Camera->Roll;
            Configuration.Save();

            PluginLog.Info("Saved configuration!");
        }

        private void ResetOnCommand(string command, string args)
        {
            // Reset the camera to the saved position
            PluginLog.Info("Resetting...");

            Camera->Mode = (Configuration.Distance == 0) ? 0 : 1;
            Camera->Distance = (Configuration.Distance == 0) ? 1.5f : Configuration.Distance;
            Camera->HRotation = Configuration.HRotation;
            Camera->VRotation = Configuration.VRotation;
            Camera->FoV = Configuration.ZoomFoV;
            Camera->AddedFoV = Configuration.GposeFoV;
            Camera->Pan = Configuration.Pan;
            Camera->Tilt = Configuration.Tilt;
            Camera->Roll = Configuration.Roll;

            PluginLog.Info("Position is reset!");
        }

        private void DrawUI()
        {
            ui?.Draw();
        }

        private void ToggleDrawConfigUI()
        {
            if (ui == null) return;

            ui.SettingsVisible = !ui.SettingsVisible;

            PluginLog.Info("UI window toggled!");
        }

        private void ConfigOnCommand(string command, string args)
        {
            ToggleDrawConfigUI();
        }
    }
}
