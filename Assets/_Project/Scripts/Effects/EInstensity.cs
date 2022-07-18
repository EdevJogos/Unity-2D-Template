using UnityEngine;


public class EInstensity : MonoBehaviour
{
    public bool playOnce;
    public float from, to, speed;

    private bool _going;
    private float _target;

    private void Start()
    {
        GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = from;

        _target = to;
    }

    private void Update()
    {
        GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = Mathf.MoveTowards(GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity, _target, speed * Time.deltaTime);

        if(Mathf.Abs(_target - GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity) <= 0)
        {
            if (playOnce) enabled = false;

            _target = _going ? from : to;
            _going = !_going;
        }
    }
}
