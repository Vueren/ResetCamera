using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace ResetCamera
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public float Distance;
        public float HRotation;
        public float VRotation;
        public float ZoomFoV;
        public float GposeFoV;
        public float Pan;
        public float Tilt;
        public float Roll;



        // The below exists to make Saving less cumbersome
        [NonSerialized]
        private IDalamudPluginInterface? pluginInterface;

        public void Initialize(IDalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public delegate void OnSaveHandler();
        public event OnSaveHandler? OnSave;

        public void Save()
        {
            this.pluginInterface!.SavePluginConfig(this);
            this.OnSave?.Invoke();
        }
    }
}
