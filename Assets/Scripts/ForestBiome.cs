using UnityEngine;

public class ForestBiome : Biome
{
    public ForestBiome(int seed) : base(seed)
    {

    }

    public override Color GetColor()
    {
        return Utils.ColorFromRGB255(0, 204, 0);
    }

    public override float GetHeight(float x, float y)
    {
        return 5;
    }
}