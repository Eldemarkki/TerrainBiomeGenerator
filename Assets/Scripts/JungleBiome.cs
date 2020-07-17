using Unity.Mathematics;
using UnityEngine;

public class JungleBiome : Biome
{
    public JungleBiome(string name, int seed, float scale, float2 offset, float baseHeight) : base(name, seed, scale, offset, baseHeight)
    {

    }

    public override Color GetColor()
    {
        return Utils.ColorFromRGB255(0, 144, 33);
    }
}