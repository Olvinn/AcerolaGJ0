using UnityEngine;

namespace Commands
{
    public struct UpdateAim
    {
        public Vector3 Pos;
        public bool Show;
    }
    
    public struct ShowHint
    {
        public string Text;
        public Vector3 Pos;
        public int Id;
    }

    public struct HideHint
    {
        public int Id;
    }

    public struct UpdatePlayer
    {
        public float CurrentHP;
        public float MaxHP;
    }

    public struct UpdatePlayerWeapon
    {
        public float CurrentMag;
        public float MaxMag;
    }
}
