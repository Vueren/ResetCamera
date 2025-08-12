using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;

namespace ResetCamera
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        // These fields are what the commands work from when no args are provided.
        // They remain in the addon for backwards compatibility purposes from previous versions.
        // Otherwise everything would be handled from the Saved Camera Info list.
        public float Distance = 6;
        public float HRotation = 0;
        public float VRotation = -0.34906587f;
        public float ZoomFoV = 0.78f;
        public float GposeFoV = 0;
        public float Roll = 0;

        [Serializable]
        public class SavedCameraInfo
        {
            public float Distance;
            public float HRotation;
            public float VRotation;
            public float ZoomFoV;
            public float GposeFoV;
            public float Roll;
        }

        public Dictionary<string, SavedCameraInfo> OtherSavedCameraInfos = new();

        // The below exists to make Saving less cumbersome
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
