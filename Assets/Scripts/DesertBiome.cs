using UnityEngine;

public class DesertBiome : Biome
{
    public DesertBiome(int seed, Climate climate) : base(seed, climate)
    {

    }

    public override Color GetColor(float height)
    {
        Color color = Color.Lerp(Color.yellow * 0.2f, Color.yellow, height);
        color.a = 1;
        return color;
    }

    public override float GetHeight(float x, float y)
    {
        return 7;
    }
}