using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Biome[] biomes;

    [SerializeField] private int precipitationSeed;
    [SerializeField] private float precipitationScale;

    [SerializeField] private int temperatureSeed;
    [SerializeField] private float temperatureScale;

    private SpriteRenderer spriteRenderer;
    private bool requestedUpdate;

    void OnValidate()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        requestedUpdate = true;
    }

    private void Update()
    {
        if (requestedUpdate)
        {
            spriteRenderer.sprite = Sprite.Create(GetTexture(width, height), new Rect(0, 0, width, height), Vector2.one * 0.5f);
            requestedUpdate = false;
        }
    }

    Texture2D GetTexture(int width, int height)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Color[] colors = new Color[width * height];

        FastNoise precipitationNoise = new FastNoise(precipitationSeed);
        FastNoise temperatureNoise = new FastNoise(temperatureSeed);

        int i = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float precipitation = (precipitationNoise.GetPerlin(x*precipitationScale, y*precipitationScale) + 1) / 2f;
                float temperature = (temperatureNoise.GetPerlin(x*temperatureScale, y*temperatureScale) + 1) / 2f;

                Biome biome = DetermineBiome(precipitation, temperature);

                float b = (precipitationNoise.GetPerlin(x * precipitationScale, y * precipitationScale) + 1f) / 2f;

                colors[i] = new Color(b,b,b);//biome.colorGradient.Evaluate(biome.GetHeight(x, y));
                i++;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        return texture;
    }

    Biome DetermineBiome(float precipitation, float temperature)
    {
        if (precipitation <= 0.5f)
        {
            if (temperature <= 0.5f)
            {
                return biomes[3];
            }
            else
            {
                return biomes[2];
            }
        }
        else
        {
            if (temperature <= 0.5f)
            {
                return biomes[0];
            }
            else
            {
                return biomes[1];
            }
        }
    }
}
