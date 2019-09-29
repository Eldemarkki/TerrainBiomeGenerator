using UnityEngine;

public class JungleBiome : Biome
{
    public JungleBiome(int seed, Climate climate) : base(seed, climate)
    {

    }

    public override Color GetColor(float height)
    {
        Color color = Color.Lerp(Color.green * 0.2f, Color.green * 0.5f, height);
        color.a = 1;
        return color;
    }

    public override float GetHeight(float x, float y)
    {
        return 10;
    }
}