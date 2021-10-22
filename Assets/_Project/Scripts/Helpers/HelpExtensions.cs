using UnityEngine;

public  static class HelpExtensions
{
    public static System.Collections.Generic.List<string> IntStringLookup = new System.Collections.Generic.List<string>(290);

    public static void Initiate()
    {
        for (int __i = -60; __i < 230; __i++)
        {
            IntStringLookup.Add(__i.ToString());
        }
    }

    public static int ClampCircle(int p_value, int p_min, int p_max)
    {
        if (p_value > p_max)
        {
            return p_min;
        }
        else if (p_value < p_min)
        {
            return p_max;
        }
        else
        {
            return p_value;
        }
    }

    public static float ClampMin0(float p_value)
    {
        return p_value <= 0 ? 0 : p_value;
    }

    public static Vector2 Clamp(Vector2 p_position, Vector2 p_min, Vector2 p_max)
    {
        return new Vector2(
            Mathf.Clamp(p_position.x, p_min.x, p_max.x),
            Mathf.Clamp(p_position.y, p_min.y, p_max.y)
            );
    }

    public static Vector2 ClampMin(Vector2 p_position, Vector2 p_min)
    {
        return new Vector2(
            Mathf.Clamp(p_position.x, p_min.x, Mathf.Infinity),
            Mathf.Clamp(p_position.y, p_min.y, Mathf.Infinity)
            );
    }

    public static void SetAlpha(this SpriteRenderer p_renderer, float p_alpha)
    {
        Color __color = p_renderer.color;

        p_renderer.color = new Color(__color.r, __color.g, __color.b, p_alpha);
    }

    public static void SetAlpha<T>(this T p_renderer, float p_alpha) where T : UnityEngine.UI.Graphic
    {
        Color __color = p_renderer.color;

        p_renderer.color = new Color(__color.r, __color.g, __color.b, p_alpha);
    }

    public static int RandomSelect(int p_value, int p_value2)
    {
        return Random.Range(0, 2) == 1 ? p_value : p_value2; 
    }

    public static float GetStereoPan(float p_x)
    {
        return (p_x / 12.5f) * 0.5f;
    }
}
