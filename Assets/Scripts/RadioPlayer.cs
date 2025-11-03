using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RadioPlayer : MonoBehaviour
{
    [Tooltip("List of songs the radio can play.")]
    public AudioClip[] songs;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (songs.Length == 0)
        {
            Debug.LogWarning("No songs assigned to the RadioPlayer!");
            return;
        }

        PlayRandomSong();
    }

    void Update()
    {
        // Check if current song ended
        if (!audioSource.isPlaying)
        {
            PlayRandomSong();
        }
    }

    void PlayRandomSong()
    {
        if (songs.Length == 0) return;

        int randomIndex = Random.Range(0, songs.Length);
        AudioClip nextSong = songs[randomIndex];
        audioSource.clip = nextSong;
        audioSource.Play();
    }
}
