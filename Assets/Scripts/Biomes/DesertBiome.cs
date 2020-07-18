using Unity.Mathematics;
using UnityEngine;

public class DesertBiome : Biome
{
    public DesertBiome(string name, int seed, float scale, float2 offset, float baseHeight) : base(name, seed, scale, offset, baseHeight)
    {
        
    }

    public override Color GetColor()
    {
        return new Color32(255, 255, 102, 255);
    }
}