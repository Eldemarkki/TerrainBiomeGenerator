using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public struct TerrainJob : IJobParallelFor
{
    public int width;
    public int height;

    public int xOffset;
    public int zOffset;

    public float masterScale;
    public float precipitationScale;
    public float temperatureScale;

    public float seaLevel;
    public int blending;

    public TerrainDisplayMode terrainDisplayMode;

    public NativeArray<Vector3> vertices;
    public NativeArray<Color> colors;

    public void Execute(int index)
    {
        int localX = index % width;
        int localZ = index / width % height;

        // X and Z offsets are just the chunk's world position
        int worldX = localX + xOffset;
        int worldZ = localZ + zOffset;

        int solidGroundHeight = 42;
        float sumOfHeights = 0;
        int count = 0;
        float strongestWeight = float.NegativeInfinity;
        int strongestBiomeIndex = 0;

        for (int i = 0; i < Biome.AllBiomes.Length; i++)
        {
            Biome biome = Biome.AllBiomes[i];
            float weight = (noise.snoise(new float2(worldX, worldZ) * biome.scale + biome.offset) + 1) / 2f;

            if (weight > strongestWeight)
            {
                strongestWeight = weight;
                strongestBiomeIndex = i;
            }

            float biomeHeight = biome.GetHeight(worldX, worldZ) * weight;

            sumOfHeights += biomeHeight;
            count++;
        }

        Biome strongestBiome = Biome.AllBiomes[strongestBiomeIndex];
        float terrainHeight = (sumOfHeights / count) + solidGroundHeight;

        vertices[index] = new Vector3(localX, terrainHeight, localZ);

        // Range 0-1 for all of these
        float precipitation = GetPrecipitation(worldX, worldZ);
        float temperature = GetTemperature(worldX, worldZ);
        float master = GetMaster(worldX, worldZ);

        Color color;
        switch (terrainDisplayMode)
        {
            case TerrainDisplayMode.Normal:
                //color = DetermineBiome(precipitation, temperature, master, seaLevel).GetColor();
                color = strongestBiome.GetColor();
                break;
            case TerrainDisplayMode.Precipitation:
                color = GetMapColor(precipitation);
                break;
            case TerrainDisplayMode.Temperature:
                color = GetMapColor(temperature);
                break;
            case TerrainDisplayMode.MasterNoise:
                color = Color.white * master;
                break;
            default:
                color = Color.magenta;
                break;
        };

        colors[index] = color;
    }

    private float OctavePerlinNoise(float x, float y, float xScale, float yScale, int octaves, int seed){
        float value = 0;
        for (int i = 0; i < octaves; i++)
        {
            float e = Mathf.Pow(2, i);
            value += noise.snoise(new float2(e * x * xScale + seed, e * y * yScale + seed)) / e;
        }
        return value;
    }

    private Biome DetermineBiomeAtPosition(int worldX, int worldZ)
    {
        float precipitation = GetPrecipitation(worldX, worldZ);
        float temperature = GetTemperature(worldX, worldZ);
        float elevation = GetMaster(worldX, worldZ);

        return DetermineBiome(precipitation, temperature, elevation, seaLevel);
    }

    private float GetHeightAt(int worldX, int worldY)
    {
        float precipitation = GetPrecipitation(worldX, worldY);
        float temperature = GetTemperature(worldX, worldY);
        float elevation = GetMaster(worldX, worldY);

        Biome biome = DetermineBiome(precipitation, temperature, elevation, seaLevel);
        return biome.GetHeight(worldX, worldY);
    }

    private float GetMaster(int worldX, int worldY)
    {
        return (noise.snoise(new float2(worldX * masterScale, worldY * masterScale)) + 1) / 2f;
    }

    private float GetPrecipitation(int worldX, int worldY)
    {
        return (noise.snoise(new float2(worldX * precipitationScale, worldY * precipitationScale) + new float2(50000, 50000)) + 1) / 2f;
    }

    private float GetTemperature(int worldX, int worldY)
    {
        return (noise.snoise(new float2(worldX * temperatureScale, worldY * temperatureScale) + new float2(100000, 100000)) + 1) / 2f;
    }

    private Color GetMapColor(float value)
    {
        if (value < 0.1f) return Color.white * 0.1f;
        if (value < 0.25f) return Color.Lerp(Color.blue, Color.white, 0.2f);
        if (value < 0.35f) return Color.Lerp(Color.green, Color.white, 0.2f);
        if (value < 0.5f) return Color.Lerp(Color.Lerp(Color.red, Color.yellow, 0.55f), Color.white, 0.2f);
        if (value < 0.75f) return Color.Lerp(Color.red, Color.black, 0.3f);
        return Color.Lerp(Color.red, Color.black, 0.7f);
    }

    Biome DetermineBiome(float precipitation, float temperature, float elevation, float seaLevel)
    {
        if (elevation <= seaLevel) return DetermineSea(precipitation, temperature);

        int rowIndex = Mathf.FloorToInt(precipitation * (Biome.BiomeTable.Length - 1));

        Biome[] row = Biome.BiomeTable[rowIndex];

        int columnIndex = Mathf.FloorToInt(temperature * (row.Length - 1));
        return row[columnIndex];
    }

    Biome DetermineSea(float precipitation, float temperature)
    {
        return Biome.OceanBiome;
    }
}
