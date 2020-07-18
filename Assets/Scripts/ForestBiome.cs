using Unity.Mathematics;
using UnityEngine;

public class ForestBiome : Biome
{
    public ForestBiome(string name, int seed, float scale, float2 offset, float baseHeight) : base(name, seed, scale, offset, baseHeight)
    {

    }

    public override Color GetColor()
    {
        return new Color32(0, 204, 0, 255);
    }
}