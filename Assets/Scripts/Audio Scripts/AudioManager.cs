using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    public AudioClip music_Bg;
    //public AudioClip ambient_Rain;

    [Header("SFX")]
    public AudioClip open_Tile_SFX;
    public AudioClip player_Step_On_Tile;
    public AudioClip lose_Fuel_SFX;
    public AudioClip gain_Fuel_SFX;



    [Header("Settings")]
    [SerializeField] public float minTimeBetween = 0.3f;
    [SerializeField] public float maxTimeBetween = 0.6f;

    [Header("Pitch Settings")]
    [SerializeField] public float minPitch = 0.9f;
    [SerializeField] public float maxPitch = 1.1f;

    [Header("Fade Settings")]
    [SerializeField] private float fadeOutDuration = 1.0f;
    [SerializeField] private float fadeInDuration = 1.0f;

    private float timeSinceLast;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        PlayMusic(music_Bg);
        //PlayAmbient(ambient_Rain);
    }

    public void PlayMusic(AudioClip audioClip)
    {
        musicSource.clip = audioClip;
        musicSource.volume = 0; // Start with volume at 0 for fade-in
        musicSource.Play();
        StartCoroutine(FadeInAudio(musicSource)); // Fade in the music
    }

    public void PlayAmbient(AudioClip audioClip)
    {
        ambientSource.clip = audioClip;
        ambientSource.volume = 0; // Start with volume at 0 for fade-in
        ambientSource.Play();
        StartCoroutine(FadeInAudio(ambientSource)); // Fade in the ambient sound
    }

    public void PlaySFXWithRandomPitch(AudioClip audioClip)
    {
        // Randomize pitch for SFX
        sfxSource.pitch = Random.Range(minPitch, maxPitch);
        sfxSource.PlayOneShot(audioClip);
    }

    public void PlaySFX(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }

    // public void PlayWalkingSFX()
    // {
    //     if (Time.time - timeSinceLast >= Random.Range(minTimeBetween, maxTimeBetween))
    //     {
    //         AudioClip sliceStepSound = walking_sfx[Random.Range(0, walking_sfx.Length)];
    //         // Randomize pitch for walking SFX
    //         sfxSource.pitch = Random.Range(minPitch, maxPitch);
    //         sfxSource.PlayOneShot(sliceStepSound);
    //         timeSinceLast = Time.time;
    //     }
    // }

    // New function to stop music by fading out
    public void StopMusicFadeOut()
    {
        StartCoroutine(FadeOutMusic(musicSource));
    }

    public void StopAmbientFadeOut()
    {
        StartCoroutine(FadeOutMusic(ambientSource));
    }

    private IEnumerator FadeOutMusic(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset volume to original value
    }

    private IEnumerator FadeInAudio(AudioSource audioSource)
    {
        float targetVolume = 1.0f; // Target volume for fade-in

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeInDuration;
            yield return null;
        }

        audioSource.volume = targetVolume; // Ensure the volume is exactly 1 after fade-in
    }
}
