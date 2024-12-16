using System;

[Flags]
public enum DamageType
{
    None = 0,
    Fire = 1,
    Ice = 2,
    Earth = 4,
    Poison = 8,
    Void = 16
}