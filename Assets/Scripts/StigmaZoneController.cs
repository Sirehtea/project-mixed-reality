using UnityEngine;

public class StigmaZoneController : MonoBehaviour
{
    [Header("Player detection")]
    public Transform player;                // drag player root here
    public float triggerDistance = 6f;      // how close before they react

    [Header("NPCs")]
    public NPCWatcher[] npcs;               // assign both NPCWatcher components in inspector

    [Header("Radio")]
    public AudioSource musicSource;         // the music AudioSource (radio songs)
    public AudioSource sfxSource;           // the click sound AudioSource
    public AudioClip radioClickOn;          // sound when music resumes
    public AudioClip radioClickOff;         // sound when music stops

    private bool isWatching = false;        // are they currently in "watch mode"?

    void Start()
    {
        // pass the player reference to the NPCs so they can track you
        foreach (var npc in npcs)
        {
            if (npc != null)
                npc.SetPlayerReference(player);
        }
    }

    void Update()
    {
        if (player == null || npcs == null || npcs.Length == 0)
            return;

        // 1. Find distance from player to the closest NPC
        float closest = Mathf.Infinity;
        foreach (var npc in npcs)
        {
            if (npc == null) continue;
            float d = Vector3.Distance(player.position, npc.transform.position);
            if (d < closest) closest = d;
        }

        bool shouldWatch = closest < triggerDistance;

        // 2. If state changed, handle transition
        if (shouldWatch != isWatching)
        {
            isWatching = shouldWatch;
            Debug.Log("STATE CHANGE. isWatching = " + isWatching);

            if (isWatching)
            {
                Debug.Log("Player close -> PAUSE music + FREEZE NPCs");

                // Pause/disable music
                if (musicSource && musicSource.isPlaying)
                {
                    musicSource.Pause();
                }

                // Disable the RadioPlayer so it doesn't auto-start new songs
                if (musicSource)
                {
                    RadioPlayer rp = musicSource.GetComponent<RadioPlayer>();
                    if (rp) rp.enabled = false;
                }

                // Click OFF SFX
                if (sfxSource && radioClickOff)
                {
                    sfxSource.PlayOneShot(radioClickOff);
                }

                // Tell all NPCs to enter watch mode
                foreach (var npc in npcs)
                {
                    if (npc != null)
                        npc.SetWatching(true);
                }
            }
            else
            {
                Debug.Log("Player far -> RESUME music + UNFREEZE NPCs");

                // Re-enable RadioPlayer so it can keep doing random songs
                if (musicSource)
                {
                    RadioPlayer rp = musicSource.GetComponent<RadioPlayer>();
                    if (rp) rp.enabled = true;
                }

                // Resume music
                if (musicSource && !musicSource.isPlaying)
                {
                    musicSource.UnPause();
                }

                // Click ON SFX
                if (sfxSource && radioClickOn)
                {
                    sfxSource.PlayOneShot(radioClickOn);
                }

                // Tell all NPCs to chill again
                foreach (var npc in npcs)
                {
                    if (npc != null)
                        npc.SetWatching(false);
                }
            }
        }
    }
}
