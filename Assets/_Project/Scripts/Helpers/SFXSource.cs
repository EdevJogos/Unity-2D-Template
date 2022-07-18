using UnityEngine;

public class SFXSource : MonoBehaviour
{
    public bool IsPlaying => GetComponent<AudioSource>().isPlaying;
    public float Pitch { get { return GetComponent<AudioSource>().pitch; } set { GetComponent<AudioSource>().pitch = value; } }
    public AudioSource audioSource => GetComponent<AudioSource>();

    private AudioManager.ClipData _clipData;

    private void Start()
    {
        enabled = !GetComponent<AudioSource>().loop;
    }

    private void Update()
    {
        if (GetComponent<AudioSource>().loop)
            return;

        if (!GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public void SetClipData(AudioManager.ClipData p_clipData)
    {
        _clipData = p_clipData;
    }

    public void Play(int p_index = 0, float p_pan = 0f)
    {
        AudioManager.PlaySFX(audioSource, _clipData, p_index, p_pan);
    }

    public void Pause()
    {
        GetComponent<AudioSource>().Pause();
    }

    public void Stop()
    {
        GetComponent<AudioSource>().Stop();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}