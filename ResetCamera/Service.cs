using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResetCamera
{
    public class Service
    {
#pragma warning disable CS8618
        [PluginService] public static ISigScanner SigScanner { get; private set; } = null!;
        [PluginService] public static IPluginLog PluginLog { get; private set; } = null!;
#pragma warning restore CS8618
    }
}
