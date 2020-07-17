using UnityEngine;

public static class Utils
{
    public static Color ColorFromRGB255(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }
    
    public static float BilinearInterpolation(float bottomLeft, float bottomRight, float topLeft, float topRight,
                                              float minX, float maxX, float minY, float maxY,
                                              float x, float y)
    {
        float tx = (x - minX) / (maxX - minX);
        float valueTop = topLeft + (topRight - topLeft) * tx;
        float valueBottom = bottomLeft + (bottomRight - bottomLeft) * tx;

        float ty = (y - minY) / (maxY - minY);
        float value = valueTop + (valueBottom - valueTop) * ty;
        return value;
    }
}