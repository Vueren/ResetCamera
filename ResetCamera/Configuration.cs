using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Numerics;

namespace ResetCamera
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public float Distance { get; set; }
        public float HRotation { get; set; }
        public float VRotation { get; set; }
        public float ZoomFoV { get; set; }  // Applies when zooming in very closely
        public float GposeFoV { get; set; } // Can be adjusted in the GPose settings menu
        public float Pan { get; set; }
        public float Tilt { get; set; }
        public float Roll { get; set; }

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private IDalamudPluginInterface? pluginInterface;

        public void Initialize(IDalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.pluginInterface!.SavePluginConfig(this);
        }
    }
}
