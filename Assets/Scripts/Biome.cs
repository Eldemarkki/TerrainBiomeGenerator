using UnityEngine;

public abstract class Biome
{
    public static ForestBiome ForestBiome = new ForestBiome(1, "Forest");
    public static DesertBiome DesertBiome = new DesertBiome(2, "Desert");
    public static OceanBiome OceanBiome = new OceanBiome(3, "Ocean");
    public static JungleBiome JungleBiome = new JungleBiome(4, "Jungle");
    public static TundraBiome TundraBiome = new TundraBiome(5, "Tundra");

    public Noise noise;

    public string name;

    public Biome(int seed, string name){
        noise = new Noise(seed, 1, 4, 15);
        this.name = name;
    }

    public virtual float GetHeight(float x, float y) {
        return noise.GetValue(x, y);
    }

    public virtual Color GetColor()
    {
        return Color.magenta;
    }
}