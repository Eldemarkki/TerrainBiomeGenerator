using UnityEngine;

[CreateAssetMenu(fileName="New Biome", menuName="Biome")]
public class Biome : ScriptableObject
{
    public new string name;
    public Gradient colorGradient;
    public int seed;

    public float GetHeight(float x, float y){
        FastNoise noise = new FastNoise(seed);

        return noise.GetPerlin(x, y);
    }
}
