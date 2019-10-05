using UnityEngine;

public class DesertBiome : Biome
{
    public DesertBiome(int seed, string name) : base(seed, name)
    {
        
    }

    public override Color GetColor()
    {
        return Utils.ColorFromRGB255(255, 255, 102);
    }

    public override float GetHeight(float x, float y)
    {
        return 7;
    }
}