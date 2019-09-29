using UnityEngine;

public class JungleBiome : Biome
{
    public JungleBiome(int seed) : base(seed)
    {

    }

    public override Color GetColor()
    {
        return Utils.ColorFromRGB255(0, 144, 33);
    }

    public override float GetHeight(float x, float y)
    {
        return 10;
    }
}