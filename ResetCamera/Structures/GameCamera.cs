using System.Numerics;
using System.Runtime.InteropServices;

namespace ResetCamera.Structures
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct GameCamera
    {
        [FieldOffset(0x60)] public Vector3 Position;
        [FieldOffset(0x124)] public float Distance;     // default is 6
        [FieldOffset(0x128)] public float MinDistance;  // 1.5
        [FieldOffset(0x12C)] public float MaxDistance;  // 20
        [FieldOffset(0x130)] public float FoV;          // default is 0.78
        [FieldOffset(0x134)] public float MinFoV;       // 0.69
        [FieldOffset(0x138)] public float MaxFoV;       // 0.78
        [FieldOffset(0x13C)] public float AddedFoV;     // -0.5 to 0.5, default is 0
        [FieldOffset(0x140)] public float HRotation;    // -pi -> pi, default is pi
        [FieldOffset(0x144)] public float VRotation;    // default is -0.349066 (?)
        [FieldOffset(0x158)] public float MinVRotation; // -1.483530
        [FieldOffset(0x15C)] public float MaxVRotation; // 0.785398 (pi/4)
        [FieldOffset(0x170)] public float Roll;         // -pi -> pi, default is 0
        [FieldOffset(0x180)] public int Mode;           // 0 = 1st Person. 1 = 3rd Person. 2+ = Restrictive camera control 
    }
}
