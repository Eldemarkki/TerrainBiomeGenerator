using UnityEngine;

public class DesertBiome : Biome
{
    public DesertBiome(int seed) : base(seed)
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