using UnityEngine;

public class OceanBiome : Biome
{
    public OceanBiome(int seed, Climate climate) : base(seed, climate)
    {

    }

    public override Color GetColor(float height)
    {
        Color color = Color.Lerp(Color.blue * 0.2f, Color.blue, height);
        color.a = 1;
        return color;
    }

    public override float GetHeight(float x, float y)
    {
        return 0;
    }
}