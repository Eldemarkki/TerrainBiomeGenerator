using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private int masterSeed;
    [SerializeField] private float masterScale;

    [SerializeField] private float precipitationScale;
    [SerializeField] private AnimationCurve precipitationCurve;

    [SerializeField] private float temperatureScale;
    [SerializeField] private AnimationCurve temperatureCurve;

    [SerializeField] private TerrainDisplayMode terrainDisplayMode = TerrainDisplayMode.Normal;

    [SerializeField] private bool generate;

    [SerializeField] private Biome[] biomes = new Biome[]
    {
        new ForestBiome(0, new Climate(0.2f, 0.3f)),
        new DesertBiome(0, new Climate(0f, 1f)),
        new OceanBiome(0, new Climate(0.05f, 0.2f)),
        new JungleBiome(0, new Climate(0.8f, 0.9f)),
    };

    private void Start()
    {
        CreateTerrain(width, height);
    }

    private void Update()
    {
        if (generate)
        {
            CreateTerrain(width, height);
            generate = false;
        }
    }

    void CreateTerrain(int width, int height)
    {
        Noise masterNoise = new Noise(masterSeed, masterScale, 4, 1);

        Noise precipitationNoise = new Noise(masterSeed + 1, precipitationScale, 4, 1);
        Noise temperatureNoise = new Noise(masterSeed + 2, temperatureScale, 4, 1);

        int n = biomes.Length;

        var vertices = new Vector3[(width + 1) * (height + 1)];
        var colors = new Color[(width + 1) * (height + 1)];

        int i = 0;
        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                float precipitation = precipitationCurve.Evaluate((precipitationNoise.GetValue(x, y) + 1) / 2f);
                float temperature = temperatureCurve.Evaluate((temperatureNoise.GetValue(x, y) + 1) / 2f);

                float master = (masterNoise.GetValue(x, y) + 1) / 2f;

                float terrainHeight = 0;
                for (int biomeIndex = 0; biomeIndex < n; biomeIndex++)
                {
                    if ((biomeIndex - 1f) / n <= master && master <= (biomeIndex + 1f) / n)
                    {
                        Biome b = biomes[biomeIndex];
                        float w = b.GetWeight(precipitation, temperature);

                        float h = (-Mathf.Abs(n * master - biomeIndex) + 1) * b.GetHeight(x, y);
                        terrainHeight += h;
                    }
                }

                vertices[i] = new Vector3(x, terrainHeight, y);

                Color color;
                switch (terrainDisplayMode)
                {
                    case TerrainDisplayMode.Normal:
                        Biome biome = DetermineBiome(precipitation, temperature);
                        float weight = biome.GetWeight(precipitation, temperature);
                        color = biome.GetColor(weight);
                        break;
                    case TerrainDisplayMode.Biome:
                        color = DetermineBiome(precipitation, temperature).GetColor(1);
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

                colors[i] = color;
                i++;
            }
        }

        int[] triangles = new int[width * height * 6];
        for (int t = 0, v = 0, y = 0; y < height; y++, v++)
        {
            for (int x = 0; x < width; x++, t = t + 6, v++)
            {
                triangles[t] = v;
                triangles[t + 3] = triangles[t + 2] = v + 1;
                triangles[t + 4] = triangles[t + 1] = v + width + 1;
                triangles[t + 5] = v + width + 2;
            }
        }

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.SetColors(colors.ToList());

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private Color GetMapColor(float value)
    {
        if(value < 0.1f) return Color.white * 0.1f;
        if(value < 0.25f) return Color.Lerp(Color.blue, Color.white, 0.2f);
        if(value < 0.35f) return Color.Lerp(Color.green, Color.white, 0.2f);
        if(value < 0.5f) return Color.Lerp(Color.Lerp(Color.red, Color.yellow, 0.55f), Color.white, 0.2f);
        if(value < 0.75f) return Color.Lerp(Color.red, Color.black, 0.3f);
        return Color.Lerp(Color.red, Color.black, 0.7f);
    }

    Biome DetermineBiome(float precipitation, float temperature)
    {
        // Get the biome with the highest weight with these parameters
        return biomes.OrderByDescending(biome => biome.GetWeight(precipitation, temperature)).First();
    }
}
