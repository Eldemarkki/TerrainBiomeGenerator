using UnityEngine;

public abstract class Biome
{
    public static ForestBiome ForestBiome = new ForestBiome(0);
    public static DesertBiome DesertBiome = new DesertBiome(0);
    public static OceanBiome OceanBiome = new OceanBiome(0);
    public static JungleBiome JungleBiome = new JungleBiome(0);
    public static TundraBiome TundraBiome = new TundraBiome(0);

    public Noise noise;

    public Biome(int seed){
        noise = new Noise(seed, 1, 4, 1);
    }

    public virtual float GetHeight(float x, float y) {
        return noise.GetValue(x, y);
    }

    public virtual Color GetColor()
    {
        return Color.magenta;
    }
}