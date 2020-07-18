using Unity.Mathematics;
using UnityEngine;

public class OceanBiome : Biome
{
    public OceanBiome(string name, int seed, float scale, float2 offset, float baseHeight) : base(name, seed, scale, offset, baseHeight)
    {

    }

    public override Color GetColor()
    {
        return new Color32(51, 204, 255, 255);
    }

    public override float GetHeight(float x, float y)
    {
        return baseHeight;
    }
}