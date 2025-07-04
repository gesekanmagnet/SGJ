using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance { get; private set; }

    [Header("Audio Settings")]
    public AudioSource audioSource;

    [Tooltip("Set true to prevent restarting BGM if it's already playing")]
    public bool dontRestartSameTrack = true;

    public AudioClip bgmMainMenu;
    public AudioClip bgmGameplay;
    public AudioClip bgmGameOver;
    public AudioClip bgmGameWin;

    private AudioClip currentClip;

    private void Awake()
    {
        //if (Instance != null && Instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
        }
    }

    /// <summary>
    /// Plays the given BGM AudioClip.
    /// </summary>
    public void PlayBGM(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        if (dontRestartSameTrack && audioSource.isPlaying && clip == currentClip)
            return;

        currentClip = clip;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    /// <summary>
    /// Stops the current BGM.
    /// </summary>
    public void StopBGM()
    {
        audioSource.Stop();
        currentClip = null;
    }

    /// <summary>
    /// Fades out the BGM over time.
    /// </summary>
    public void FadeOut(float duration)
    {
        StartCoroutine(FadeOutRoutine(duration));
    }

    private System.Collections.IEnumerator FadeOutRoutine(float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
