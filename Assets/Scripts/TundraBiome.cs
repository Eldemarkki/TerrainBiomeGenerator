using Unity.Mathematics;
using UnityEngine;

public class TundraBiome : Biome
{
    public TundraBiome(string name, int seed, float scale, float2 offset, float baseHeight) : base(name, seed, scale, offset, baseHeight)
    {

    }

    public override Color GetColor()
    {
        return Utils.ColorFromRGB255(102, 153, 153);
    }

    public override float GetHeight(float x, float y)
    {
        return noise.GetValue(x, y);
    }
}