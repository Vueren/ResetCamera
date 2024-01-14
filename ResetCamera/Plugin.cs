using System;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Plugin.Services;
using ResetCamera.Structures;

namespace ResetCamera
{
    public sealed unsafe class Plugin : IDalamudPlugin
    {

        private const string SaveCommandName = "/rcsave";

        private const string ResetCommandName = "/rcreset";

        private DalamudPluginInterface PluginInterface { get; init; }

        private ICommandManager CommandManager { get; init; }
        
        public Configuration Configuration { get; init; }

        private CameraManager* cameraManager { get; init; }

        [NonSerialized]
        private static GameCamera* Camera;

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager)
        {
            pluginInterface.Create<Service>();

            this.cameraManager = (CameraManager*)Service.SigScanner.GetStaticAddressFromSig("4C 8D 35 ?? ?? ?? ?? 85 D2"); // g_ControlSystemCameraManager
            Camera = cameraManager->WorldCamera;

            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            this.CommandManager.AddHandler(SaveCommandName, new CommandInfo(SaveOnCommand)
            {
                HelpMessage = "Saves the current camera position for resetting."
            });

            this.CommandManager.AddHandler(ResetCommandName, new CommandInfo(ResetOnCommand)
            {
                HelpMessage = "Resets the camera position to the saved position."
            });
        }

        public void Dispose()
        {            
            this.CommandManager.RemoveHandler(SaveCommandName);
            this.CommandManager.RemoveHandler(ResetCommandName);
        }

        private void SaveOnCommand(string command, string args)
        {
            // Save the camera's current position
            Service.PluginLog.Info("Saving...");

            Configuration.Distance = (Camera->Mode == 0) ? 0f : Camera->Distance;
            Configuration.HRotation = Camera->HRotation;
            Configuration.VRotation = Camera->VRotation;
            Configuration.ZoomFoV = Camera->FoV;
            Configuration.GposeFoV = Camera->AddedFoV;
            Configuration.Pan = Camera->Pan;
            Configuration.Tilt = Camera->Tilt;
            Configuration.Roll = Camera->Roll;
            Configuration.Save();
            Service.PluginLog.Info("Saved configuration!");
        }

        private void ResetOnCommand(string command, string args)
        {
            // Reset the camera to the saved position
            Service.PluginLog.Info("Resetting...");

            Camera->Mode = (Configuration.Distance == 0) ? 0 : 1;
            Camera->Distance = (Configuration.Distance == 0) ? 1.5f : Configuration.Distance;
            Camera->HRotation = Configuration.HRotation;
            Camera->VRotation = Configuration.VRotation;
            Camera->FoV = Configuration.ZoomFoV;
            Camera->AddedFoV = Configuration.GposeFoV;
            Camera->Pan = Configuration.Pan;
            Camera->Tilt = Configuration.Tilt;
            Camera->Roll = Configuration.Roll;
            Service.PluginLog.Info("Position is reset!");
        }
    }
}
