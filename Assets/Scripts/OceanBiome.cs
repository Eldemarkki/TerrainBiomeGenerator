using UnityEngine;

public class OceanBiome : Biome
{
    public OceanBiome(int seed, string name) : base(seed, name)
    {

    }

    public override Color GetColor()
    {
        return Utils.ColorFromRGB255(51, 204, 255);
    }

    public override float GetHeight(float x, float y)
    {
        return -5;
    }
}