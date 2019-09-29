using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private int masterSeed;
    [SerializeField] private float masterScale;

    [Range(0f, 1f)]
    [SerializeField] private float seaLevel;

    [SerializeField] private float precipitationScale;
    [SerializeField] private AnimationCurve precipitationCurve;

    [SerializeField] private float temperatureScale;
    [SerializeField] private AnimationCurve temperatureCurve;

    [SerializeField] private TerrainDisplayMode terrainDisplayMode = TerrainDisplayMode.Normal;

    [SerializeField] private bool generate;

    [SerializeField]
    private Biome[][] biomeTable = new Biome[][]
    {
        /*                                 Very cold          Cold              Average             Warm               Hot */
        new Biome[] /* Rainy */          { Biome.TundraBiome, Biome.ForestBiome, Biome.JungleBiome, Biome.JungleBiome, Biome.JungleBiome, },
        new Biome[] /* Slightly rainy */ { Biome.TundraBiome, Biome.ForestBiome, Biome.ForestBiome, Biome.JungleBiome, Biome.JungleBiome, },
        new Biome[] /* Average */        { Biome.TundraBiome, Biome.ForestBiome, Biome.ForestBiome, Biome.JungleBiome, Biome.JungleBiome, },
        new Biome[] /* Slightly dry */   { Biome.TundraBiome, Biome.TundraBiome, Biome.DesertBiome, Biome.DesertBiome, Biome.DesertBiome,  },
        new Biome[] /* Dry */            { Biome.TundraBiome, Biome.TundraBiome, Biome.DesertBiome, Biome.DesertBiome, Biome.DesertBiome, }
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

        int n = biomeTable.Length;

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

                Biome biome;
                if (master <= seaLevel)
                {
                    biome = DetermineSea(precipitation, temperature);
                }
                else
                {
                    biome = DetermineBiome(precipitation, temperature);
                }
                
                float terrainHeight = biome.GetHeight(x, y);

                vertices[i] = new Vector3(x, terrainHeight, y);

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
        if (value < 0.1f) return Color.white * 0.1f;
        if (value < 0.25f) return Color.Lerp(Color.blue, Color.white, 0.2f);
        if (value < 0.35f) return Color.Lerp(Color.green, Color.white, 0.2f);
        if (value < 0.5f) return Color.Lerp(Color.Lerp(Color.red, Color.yellow, 0.55f), Color.white, 0.2f);
        if (value < 0.75f) return Color.Lerp(Color.red, Color.black, 0.3f);
        return Color.Lerp(Color.red, Color.black, 0.7f);
    }

    Biome DetermineBiome(float precipitation, float temperature)
    {
        int rowIndex = Mathf.FloorToInt(precipitation * biomeTable.Length);
        Biome[] row = biomeTable[rowIndex];

        int columnIndex = Mathf.FloorToInt(temperature * row.Length);
        return row[columnIndex];
    }

    Biome DetermineSea(float precipitation, float temperature)
    {
        return Biome.OceanBiome;
    }
}
