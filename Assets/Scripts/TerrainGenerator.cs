using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("World Settings")]
    [SerializeField] private int worldWidth;
    [SerializeField] private int worldDepth;
    [SerializeField] private GameObject chunkPrefab;

    [Header("Chunk Settings")]
    [SerializeField] private int width;
    [SerializeField] private int height;

    [Header("Biome Generation Settings")]
    [SerializeField] private int masterSeed;
    [SerializeField] private float masterScale;

    [Range(0, 256)]
    [SerializeField] private int blending = 4;

    [Range(0f, 1f)]
    [SerializeField] private float seaLevel;

    [SerializeField] private float precipitationScale;

    [SerializeField] private float temperatureScale;

    [Header("Rendering Settings")]
    [SerializeField] private TerrainDisplayMode terrainDisplayMode = TerrainDisplayMode.Normal;

    [Header("Generate World")]
    [SerializeField] private bool generate;

    private Chunk[] chunks;

    private void Awake()
    {
        chunks = new Chunk[worldWidth * worldDepth];
    }

    private void Start()
    {
        CreateWorld();
    }

    private void CreateWorld()
    {
        for (int chunkIndex = 0; chunkIndex < chunks.Length; chunkIndex++)
        {
            Chunk chunk = chunks[chunkIndex];
            if (chunk)
                Destroy(chunk.gameObject);
        }

        chunks = new Chunk[worldWidth * worldDepth];

        int i = 0;
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldDepth; y++)
            {
                Chunk chunk = Instantiate(chunkPrefab, new Vector3(x * width, 0, y * height), Quaternion.identity).GetComponent<Chunk>();
                chunk.needsUpdate = true;

                chunk.width = width;
                chunk.height = height;
                chunk.seaLevel = seaLevel;
                chunk.terrainDisplayMode = terrainDisplayMode;
                chunk.blending = blending;

                chunk.masterScale = masterScale;
                chunk.precipitationScale = precipitationScale;
                chunk.temperatureScale = temperatureScale;

                chunks[i++] = chunk;
            }
        }
    }

    private void Update()
    {
        if (generate)
        {
            CreateWorld();
            generate = false;
        }
    }
}