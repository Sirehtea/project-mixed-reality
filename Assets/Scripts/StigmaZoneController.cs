using UnityEngine;

public class StigmaZoneController : MonoBehaviour
{
    [Header("Player detection")]
    public Transform player;
    public float triggerDistance = 6f;

    [Header("NPCs")]
    public NPCWatcher[] npcs;

    [Header("Radio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip radioClickOn;
    public AudioClip radioClickOff;

    private bool isWatching = false;

    void Start()
    {
        foreach (var npc in npcs)
            npc?.SetPlayerReference(player);
    }

    void Update()
    {
        if (player == null || npcs == null || npcs.Length == 0)
            return;

        float closest = Mathf.Infinity;

        foreach (var npc in npcs)
        {
            if (npc == null) continue;
            float d = Vector3.Distance(player.position, npc.transform.position);
            if (d < closest) closest = d;
        }

        bool shouldWatch = closest < triggerDistance;

        if (shouldWatch == isWatching)
            return;

        isWatching = shouldWatch;

        if (isWatching)
        {
            if (musicSource && musicSource.isPlaying)
                musicSource.Pause();

            var rp = musicSource ? musicSource.GetComponent<RadioPlayer>() : null;
            if (rp) rp.enabled = false;

            if (sfxSource && radioClickOff)
                sfxSource.PlayOneShot(radioClickOff);

            foreach (var npc in npcs)
                npc?.SetWatching(true);
        }
        else
        {
            var rp = musicSource ? musicSource.GetComponent<RadioPlayer>() : null;
            if (rp) rp.enabled = true;

            if (musicSource && !musicSource.isPlaying)
                musicSource.UnPause();

            if (sfxSource && radioClickOn)
                sfxSource.PlayOneShot(radioClickOn);

            foreach (var npc in npcs)
                npc?.SetWatching(false);
        }
    }
}
