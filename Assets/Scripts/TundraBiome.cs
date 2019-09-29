using UnityEngine;

public class TundraBiome : Biome
{
    public TundraBiome(int seed) : base(seed)
    {

    }

    public override Color GetColor()
    {
        return Utils.ColorFromRGB255(102, 153, 153);
    }

    public override float GetHeight(float x, float y){
        return 5;
    }
}