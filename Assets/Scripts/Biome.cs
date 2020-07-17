using Unity.Mathematics;
using UnityEngine;

public abstract class Biome
{
    public static ForestBiome ForestBiome = new ForestBiome("Forest", 1, 1, new float2(100, 100), 10);
    public static DesertBiome DesertBiome = new DesertBiome("Desert", 2, 2, new float2(-3000, 3000), 5);
    public static OceanBiome OceanBiome = new OceanBiome("Ocean", 3, 10, new float2(0, 0), 0);
    public static JungleBiome JungleBiome = new JungleBiome("Jungle", 4, 3, new float2(5600, 3400), 30);
    public static TundraBiome TundraBiome = new TundraBiome("Tundra", 5, 2, new float2(-4000, 7000), 5);

    public static Biome[][] BiomeTable = new Biome[][]
    {
        /*                                 Very cold    Cold         Average      Warm         Hot */
        new Biome[] /* Rainy */          { TundraBiome, ForestBiome, JungleBiome, JungleBiome, JungleBiome, },
        new Biome[] /* Slightly rainy */ { TundraBiome, ForestBiome, ForestBiome, JungleBiome, JungleBiome, },
        new Biome[] /* Average */        { TundraBiome, ForestBiome, ForestBiome, JungleBiome, JungleBiome, },
        new Biome[] /* Slightly dry */   { TundraBiome, TundraBiome, DesertBiome, DesertBiome, DesertBiome, },
        new Biome[] /* Dry */            { TundraBiome, TundraBiome, DesertBiome, DesertBiome, DesertBiome, }
    };

    public static Biome[] AllBiomes = { ForestBiome, DesertBiome, OceanBiome, JungleBiome, TundraBiome };

    public Noise noise;

    public string name;

    public int seed;
    public float scale;
    public float baseHeight;
    public float2 offset;

    protected Biome(string name, int seed, float scale, float2 offset, float baseHeight)
    {
        this.scale = scale;
        this.offset = offset;
        this.baseHeight = baseHeight;
        this.seed = seed;
        this.name = name;

        noise = new Noise(seed, scale, 4, 6);
    }

    public virtual float GetHeight(float x, float y)
    {
        return noise.GetValue(x, y) + baseHeight;
    }

    public virtual Color GetColor()
    {
        return Color.magenta;
    }
}