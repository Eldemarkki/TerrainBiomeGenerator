using UnityEngine;

public abstract class Biome
{
    public Noise noise;

    public Climate climate;

    public Biome(int seed, Climate climate){
        noise = new Noise(seed, 1, 4, 1);
        this.climate = climate;
    }

    public virtual float GetHeight(float x, float y) {
        return noise.GetValue(x, y);
    }

    public virtual Color GetColor(float height)
    {
        return Color.Lerp(Color.black, Color.white, height);
    }

    public virtual float GetWeight(float precipitation, float temperature)
    {
        Vector2 input = new Vector2(precipitation, temperature);
        Vector2 target = new Vector2(climate.precipitation, climate.temperature);
        float dist = Vector2.Distance(input, target);

        float weight = 1f - dist;
        return weight;
        
        // float precipitationSimilarity = Mathf.Min(precipitation, biome.GetTargetPrecipitation()) / Mathf.Max(precipitation, biome.GetTargetPrecipitation());
        // float temperatureSimilarity = Mathf.Min(temperature, biome.GetTargetTemperature()) / Mathf.Max(temperature, biome.GetTargetTemperature());
        // return (precipitationSimilarity + temperatureSimilarity) / 2f;
    }
}