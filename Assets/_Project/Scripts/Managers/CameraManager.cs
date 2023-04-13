using UnityEngine;

public class CameraManager : Manager
{
    public static Range VerticalLimit;
    public static Range HorizontalLimit;

    private static bool _CameraShake;
    private static float _Strength;
    private static float _CameraShakeTime;
    private static Vector3 _OriginalPosition;

    public static Camera MainCamera;

    public override void Initiate()
    {
        SetMainCamera();
    }

    public override void Initialize()
    {
        
    }

    public override void Restart()
    {
        
    }

    private void Update()
    {
        if (_CameraShake && MainCamera.enabled)
        {
            Vector3 __randomVector = Random.insideUnitSphere;

            MainCamera.transform.localPosition = new Vector3(__randomVector.x * _Strength, __randomVector.y * _Strength, -10f) + _OriginalPosition;

            _CameraShakeTime -= Time.deltaTime;

            if (_CameraShakeTime <= 0)
            {
                StopShake();
            }
        }
    }

    public static void ShakeCamera(float p_duration, float p_strength)
    {
        _Strength = p_strength;
        _CameraShakeTime += p_duration;
        _CameraShakeTime = Mathf.Clamp(_CameraShakeTime, 0f, p_duration);

        _CameraShake = true;
    }

    public static void StopShake()
    {
        _CameraShake = false;
        _CameraShakeTime = 0f;

        MainCamera.transform.localPosition = _OriginalPosition;
    }

    public static bool InsideCamera(Vector2 p_position)
    {
        if (p_position.x < HorizontalLimit.min || p_position.x > HorizontalLimit.max)
            return false;

        if (p_position.y < VerticalLimit.min || p_position.y > VerticalLimit.max)
            return false;

        return true;
    }

    public static Vector2 RandomOutside()
    {
        float __x, __y = 0;

        if (Random.Range(0, 2) == 1)
        {
            __x = Random.Range(HorizontalLimit.min, HorizontalLimit.max);
            __y = Random.Range(0, 2) == 1 ? VerticalLimit.min * 1.1f : VerticalLimit.max * 1.1f;
        }
        else
        {
            __x = Random.Range(0, 2) == 1 ? HorizontalLimit.min * 1.1f : HorizontalLimit.max * 1.1f;
            __y = Random.Range(VerticalLimit.min, VerticalLimit.max);
        }

        return new Vector2(__x, __y);
    }

    public static Vector2 RandomInside()
    {
        float __x, __y = 0;

        __x = Random.Range(HorizontalLimit.min, HorizontalLimit.max);
        __y = Random.Range(VerticalLimit.min, VerticalLimit.max);

        return new Vector2(__x, __y);
    }

    private static void SetMainCamera()
    {
        MainCamera = Camera.main;
        _OriginalPosition = MainCamera.transform.localPosition;

        Vector2 __minLimits = MainCamera.ViewportToWorldPoint(new Vector2(0.0f, 0.0f));
        Vector2 __maxLimits = MainCamera.ViewportToWorldPoint(new Vector2(1.0f, 1.0f));

        VerticalLimit = new Range(__minLimits.y, __maxLimits.y);
        HorizontalLimit = new Range(__minLimits.x, __maxLimits.x);
    }
}