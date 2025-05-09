using System;
using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using ResetCamera.Structures;

namespace ResetCamera
{
    public sealed unsafe class Plugin : IDalamudPlugin
    {

        private const string SaveCommandName = "/rcsave";
        private const string ResetCommandName = "/rcreset";
        private const string DeleteCommandName = "/rcdelete";
        private const string ConfigCommandName = "/rcconfig";

        [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;

        [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
        [PluginService] public static ISigScanner SigScanner { get; private set; } = null!;
        [PluginService] public static IPluginLog PluginLog { get; private set; } = null!;

        public Configuration Configuration { get; init; }

        private CameraManager* cameraManager { get; init; }

        [NonSerialized]
        private static GameCamera* Camera;

        private readonly PluginUI? ui;

        public Plugin()
        {
            this.cameraManager = (CameraManager*)FFXIVClientStructs.FFXIV.Client.Game.Control.CameraManager.Instance();
            Camera = cameraManager->WorldCamera;

            this.Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(PluginInterface);

            CommandManager.AddHandler(SaveCommandName, new CommandInfo(SaveOnCommand)
            {
                HelpMessage = "Saves the current camera direction to the specified name for resetting back to later on. Name is optional."
            });

            CommandManager.AddHandler(ResetCommandName, new CommandInfo(ResetOnCommand)
            {
                HelpMessage = "Resets the camera direction to the saved direction with the specified name. Name is optional."
            });

            CommandManager.AddHandler(DeleteCommandName, new CommandInfo(DeleteOnCommand)
            {
                HelpMessage = "Deletes the previously saved camera direction with the specified name. Name is optional."
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
            CommandManager.RemoveHandler(DeleteCommandName);
            CommandManager.RemoveHandler(ConfigCommandName);

            ui?.Dispose();
        }

        private void SaveOnCommand(string command, string args)
        {
            // Save the camera's current direction
            args = args.Trim().Replace(" ", "").ToLower();

            if (args.IsNullOrWhitespace())
            {
                PluginLog.Info("Saving over the default unnamed saved direction...");

                Configuration.Distance = (Camera->Mode == 0) ? 0f : Camera->Distance;
                Configuration.HRotation = Camera->HRotation;
                Configuration.VRotation = Camera->VRotation;
                Configuration.ZoomFoV = Camera->FoV;
                Configuration.GposeFoV = Camera->AddedFoV;
                Configuration.Pan = Camera->Pan;
                Configuration.Tilt = Camera->Tilt;
                Configuration.Roll = Camera->Roll;

                PluginLog.Info("Saved over the default unnamed saved direction!");
            }
            else
            {
                PluginLog.Info("Saving [" + args + "]...");
                
                var savedCameraInfo = new Configuration.SavedCameraInfo();
                savedCameraInfo.Distance = (Camera->Mode == 0) ? 0f : Camera->Distance;
                savedCameraInfo.HRotation = Camera->HRotation;
                savedCameraInfo.VRotation = Camera->VRotation;
                savedCameraInfo.ZoomFoV = Camera->FoV;
                savedCameraInfo.GposeFoV = Camera->AddedFoV;
                savedCameraInfo.Pan = Camera->Pan;
                savedCameraInfo.Tilt = Camera->Tilt;
                savedCameraInfo.Roll = Camera->Roll;

                // If it already exists, set it. Otherwise, create it.
                if(Configuration.OtherSavedCameraInfos.ContainsKey(args))
                {
                    Configuration.OtherSavedCameraInfos[args] = savedCameraInfo;
                }
                else
                {
                    Configuration.OtherSavedCameraInfos.TryAdd(args, savedCameraInfo);
                }

                PluginLog.Info("Saved [" + args + "]!");
            }

            Configuration.Save();
        }

        private void ResetOnCommand(string command, string args)
        {
            // Reset the camera to the saved direction
            args = args.Trim().Replace(" ", "").ToLower();

            if (args.IsNullOrWhitespace())
            {
                PluginLog.Info("Resetting back to the default unnamed saved direction...");

                Camera->Mode = (Configuration.Distance == 0) ? 0 : 1;
                Camera->Distance = (Configuration.Distance == 0) ? 1.5f : Configuration.Distance;
                Camera->HRotation = Configuration.HRotation;
                Camera->VRotation = Configuration.VRotation;
                Camera->FoV = Configuration.ZoomFoV;
                Camera->AddedFoV = Configuration.GposeFoV;
                Camera->Pan = Configuration.Pan;
                Camera->Tilt = Configuration.Tilt;
                Camera->Roll = Configuration.Roll;

                PluginLog.Info("Direction has been reset back to the default unnamed saved direction!");
            }
            else
            {
                PluginLog.Info("Resetting to [" + args + "]...");

                if(Configuration.OtherSavedCameraInfos.TryGetValue(args, out var savedCameraInfo))
                {
                    Camera->Mode = (savedCameraInfo.Distance == 0) ? 0 : 1;
                    Camera->Distance = (savedCameraInfo.Distance == 0) ? 1.5f : savedCameraInfo.Distance;
                    Camera->HRotation = savedCameraInfo.HRotation;
                    Camera->VRotation = savedCameraInfo.VRotation;
                    Camera->FoV = savedCameraInfo.ZoomFoV;
                    Camera->AddedFoV = savedCameraInfo.GposeFoV;
                    Camera->Pan = savedCameraInfo.Pan;
                    Camera->Tilt = savedCameraInfo.Tilt;
                    Camera->Roll = savedCameraInfo.Roll;
                }

                PluginLog.Info("Direction has been reset to [" + args + "]!");
            }
        }

        private void DeleteOnCommand(string command, string args)
        {
            // Delete the saved direction specified
            args = args.Trim().Replace(" ", "").ToLower();

            if (args.IsNullOrWhitespace())
            {
                PluginLog.Info("Deleting any changes made to the default unnamed saved direction...");

                Configuration.Distance = 6;
                Configuration.HRotation = 0;
                Configuration.VRotation = -0.34906587f;
                Configuration.ZoomFoV = 0.78f;
                Configuration.GposeFoV = 0;
                Configuration.Pan = 0;
                Configuration.Tilt = 0;
                Configuration.Roll = 0;

                PluginLog.Info("Any changes made to the default unnamed saved direction have been deleted!");
            }
            else
            {
                PluginLog.Info("Deleting [" + args + "]...");

                Configuration.OtherSavedCameraInfos.Remove(args);

                PluginLog.Info("Direction [" + args + "] is deleted!");
            }
        }

        private void DrawUI()
        {
            ui?.Draw();
        }

        private void ToggleDrawConfigUI()
        {
            if (ui == null) return;

            ui.SettingsVisible = !ui.SettingsVisible;

            PluginLog.Info("Config UI window toggled!");
        }

        private void ConfigOnCommand(string command, string args)
        {
            ToggleDrawConfigUI();
        }
    }
}
