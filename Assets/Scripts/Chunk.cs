using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    JobHandle jobHandle;

    NativeArray<Vector3> vertices;
    NativeArray<Color> colors;

    int[] triangles;

    public bool needsUpdate;

    public float masterScale;
    public float precipitationScale;
    public float temperatureScale;

    public int width;
    public int height;
    public float seaLevel;
    public TerrainDisplayMode terrainDisplayMode;
    public int blending;

    private void Update()
    {
        if (needsUpdate)
        {
            jobHandle = CreateTerrain((int)transform.position.x, (int)transform.position.z, width, height, seaLevel, terrainDisplayMode);

            triangles = new int[width * height * 6];
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
        }
    }

    private void LateUpdate()
    {
        if (needsUpdate)
        {
            jobHandle.Complete();
            
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            vertices.Dispose();

            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            mesh.SetColors(colors.ToList());
            colors.Dispose();

            GetComponent<MeshFilter>().sharedMesh = mesh;

            needsUpdate = false;
        }
    }

    JobHandle CreateTerrain(int xOffset, int yOffset, int width, int height, float seaLevel, TerrainDisplayMode terrainDisplayMode)
    {
        vertices = new NativeArray<Vector3>((width + 1) * (height + 1), Allocator.TempJob);
        colors = new NativeArray<Color>((width + 1) * (height + 1), Allocator.TempJob);

        TerrainJob terrainJob = new TerrainJob()
        {
            width = width + 1,
            height = height + 1,

            xOffset = xOffset,
            zOffset = yOffset,

            masterScale = masterScale,
            precipitationScale = precipitationScale,
            temperatureScale = temperatureScale,

            seaLevel = seaLevel,
            blending = blending,

            terrainDisplayMode = terrainDisplayMode,

            vertices = vertices,
            colors = colors
        };

        JobHandle jobHandle = terrainJob.Schedule(vertices.Length, 32);
        return jobHandle;
    }
}
