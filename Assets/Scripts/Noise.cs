using UnityEngine;

public class Noise
{
    FastNoise noise;

    public float frequency;
    public int octaves;
    public float amplitude;

    public Noise(int seed, float frequency, int octaves, float amplitude)
    {
        noise = new FastNoise(seed);

        this.frequency = frequency;
        this.octaves = octaves;
        this.amplitude = amplitude;
    }

    public float GetValue(float x, float y)
    {
        float value = 0;

        for (int i = 0; i < octaves; i++)
        {
            int e = (int)Mathf.Pow(2, i);
            value += (amplitude * noise.GetPerlin(e * x * frequency, e * y * frequency)) / e;
        }

        return value;
    }
}