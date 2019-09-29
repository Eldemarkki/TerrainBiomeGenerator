using UnityEngine;

public static class Utils
{
    public static Color ColorFromRGB255(int r, int g, int b){
        return new Color(r / 255f, g / 255f, b / 255f);
    }
}