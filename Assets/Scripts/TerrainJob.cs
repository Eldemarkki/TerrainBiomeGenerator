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

        // Range 0-1 for all of these
        float precipitation = GetPrecipitation(worldX, worldZ);
        float temperature = GetTemperature(worldX, worldZ);
        float master = GetMaster(worldX, worldZ);
        Biome biome = DetermineBiome(precipitation, temperature, master, seaLevel);
        float terrainHeight = biome.GetHeight(worldX, worldZ) + solidGroundHeight;

        float heightSum = 0;
        if (!(biome is OceanBiome))
        {
            // Perform blending
            for (int offsetX = -blending; offsetX <= blending; offsetX++)
            {
                for (int offsetY = -blending; offsetY <= blending; offsetY++)
                {
                    float offsetPrecipitation = GetPrecipitation(worldX + offsetX, worldZ + offsetY);
                    float offsetTemperature = GetTemperature(worldX + offsetX, worldZ + offsetY);
                    float offsetMaster = GetMaster(worldX + offsetX, worldZ + offsetY);
                    Biome offsetBiome = DetermineBiome(offsetPrecipitation, offsetTemperature, offsetMaster, seaLevel);
                    float offsetTerrainHeight = offsetBiome.GetHeight(worldX + offsetX, worldZ + offsetY) + solidGroundHeight;

                    heightSum += offsetTerrainHeight;
                }
            }

            int sampleCount = (blending * 2 + 1) * (blending * 2 + 1);
            terrainHeight = heightSum / sampleCount;
        }

        vertices[index] = new Vector3(localX, terrainHeight, localZ);

        Color color;
        switch (terrainDisplayMode)
        {
            case TerrainDisplayMode.Normal:
                color = biome.GetColor();
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
        if (value < 0.1f) { return Color.white * 0.1f; }
        if (value < 0.25f) { return Color.Lerp(Color.blue, Color.white, 0.2f); }
        if (value < 0.35f) { return Color.Lerp(Color.green, Color.white, 0.2f); }
        if (value < 0.5f) { return Color.Lerp(Color.Lerp(Color.red, Color.yellow, 0.55f), Color.white, 0.2f); }
        if (value < 0.75f) { return Color.Lerp(Color.red, Color.black, 0.3f); }
        return Color.Lerp(Color.red, Color.black, 0.7f);
    }

    Biome DetermineBiome(float precipitation, float temperature, float elevation, float seaLevel)
    {
        if (elevation <= seaLevel) { return DetermineSea(); }

        int rowIndex = Mathf.FloorToInt(precipitation * (Biome.BiomeTable.Length - 1));

        int columnIndex = Mathf.FloorToInt(temperature * (Biome.BiomeTable[rowIndex].Length - 1));
        return Biome.BiomeTable[rowIndex][columnIndex];
    }

    // This would determine the type of sea, e.g. deep ocean, shallow ocean, warm ocean...
    // It should also take the precipitation and temperature as parameters
    private static Biome DetermineSea()
    {
        return Biome.OceanBiome;
    }
}
