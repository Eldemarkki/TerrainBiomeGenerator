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
        /*                                 Very cold          Cold              Average             Warm               Hot */
        new Biome[] /* Rainy */          { Biome.TundraBiome, Biome.ForestBiome, Biome.JungleBiome, Biome.JungleBiome, Biome.JungleBiome, },
        new Biome[] /* Slightly rainy */ { Biome.TundraBiome, Biome.ForestBiome, Biome.ForestBiome, Biome.JungleBiome, Biome.JungleBiome, },
        new Biome[] /* Average */        { Biome.TundraBiome, Biome.ForestBiome, Biome.ForestBiome, Biome.JungleBiome, Biome.JungleBiome, },
        new Biome[] /* Slightly dry */   { Biome.TundraBiome, Biome.TundraBiome, Biome.DesertBiome, Biome.DesertBiome, Biome.DesertBiome, },
        new Biome[] /* Dry */            { Biome.TundraBiome, Biome.TundraBiome, Biome.DesertBiome, Biome.DesertBiome, Biome.DesertBiome, }
    };

    public static Biome[] AllBiomes = { Biome.ForestBiome, Biome.DesertBiome, Biome.OceanBiome, Biome.JungleBiome, Biome.TundraBiome };

    public Noise noise;

    public string name;

    public int seed;
    public float scale;
    public float baseHeight;
    public float2 offset;

    public Biome(string name, int seed, float scale, float2 offset, float baseHeight){
        this.scale = scale;
        this.offset = offset;
        this.baseHeight = baseHeight;
        this.seed = seed;
        this.name = name;

        noise = new Noise(seed, scale, 4, 15);
    }

    public virtual float GetHeight(float x, float y) {
        return noise.GetValue(x, y);
    }

    public virtual Color GetColor()
    {
        return Color.magenta;
    }
}