using UnityEngine;

public class SFXSource : MonoBehaviour
{
    public bool IsPlaying => GetComponent<AudioSource>().isPlaying;
    public float Pitch { get { return GetComponent<AudioSource>().pitch; } set { GetComponent<AudioSource>().pitch = value; } }
    public AudioSource Speaker { get { if (_speaker == null) _speaker = GetComponent<AudioSource>(); return _speaker; } }

    private bool _permanent = false;
    private float _volumeTarget = 1f, _volumeSpeed = 1f;
    private AudioSource _speaker;
    private AudioManager.ClipData _clipData;

    private void Start()
    {
        enabled = !_permanent;
    }

    private void Update()
    {
        if (_permanent)
            return;

        Speaker.volume = Mathf.MoveTowards(Speaker.volume, _volumeTarget, _volumeSpeed * Time.deltaTime);

        if (!Speaker.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public void SetClipData(AudioManager.ClipData p_clipData, bool p_permanent)
    {
        _clipData = p_clipData;
        _permanent = p_permanent;

        _volumeTarget = _clipData.volume;
    }

    public void Play(int p_index = 0, float p_pan = 0f)
    {
        AudioManager.PlaySFX(Speaker, _clipData, p_index, p_pan);
    }

    public void UnPause()
    {
        Speaker.UnPause();
    }

    public void Pause()
    {
        Speaker.Pause();
    }

    public void Stop()
    {
        Speaker.Stop();
    }

    public void ResetVolume()
    {
        _volumeTarget = _clipData.volume;
    }

    public void SetVolume(float p_volume)
    {
        _volumeTarget = p_volume;
        Speaker.volume = p_volume;
    }

    public void SetVolumeSmooth(float p_volume, float p_speed)
    {
        _volumeTarget = p_volume;
        _volumeSpeed = p_speed;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}