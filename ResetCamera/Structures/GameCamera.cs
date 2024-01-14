using System.Numerics;
using System.Runtime.InteropServices;

namespace ResetCamera.Structures
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct GameCamera
    {
        [FieldOffset(0x60)] public Vector3 Position;
        [FieldOffset(0x114)] public float Distance;     // default is 6
        [FieldOffset(0x118)] public float MinDistance;  // 1.5
        [FieldOffset(0x11C)] public float MaxDistance;  // 20
        [FieldOffset(0x120)] public float FoV;          // default is 0.78
        [FieldOffset(0x124)] public float MinFoV;       // 0.69
        [FieldOffset(0x128)] public float MaxFoV;       // 0.78
        [FieldOffset(0x12C)] public float AddedFoV;     // -0.5 to 0.5, default is 0
        [FieldOffset(0x130)] public float HRotation;    // -pi -> pi, default is pi
        [FieldOffset(0x134)] public float VRotation;    // default is -0.349066 (?)
        [FieldOffset(0x148)] public float MinVRotation; // -1.483530
        [FieldOffset(0x14C)] public float MaxVRotation; // 0.785398 (pi/4)
        [FieldOffset(0x150)] public float Pan;          // -0.872664 to 0.872664, default is 0
        [FieldOffset(0x154)] public float Tilt;         // -0.646332 to 0.3417, affected by third person angle. default is 0
        [FieldOffset(0x160)] public float Roll;         // -pi -> pi, default is 0
        [FieldOffset(0x170)] public int Mode;           // 0 = 1st Person. 1 = 3rd Person. 2+ = Restrictive camera control 
    }
}
