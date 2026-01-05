using UnityEngine;
using System.Collections;

[System.Serializable]
public class LampStep
{
    public Light lampLight;
    public GameObject prefabToSpawn;
    public Transform spawnPoint;

    public float prefabLifetime = 2f;     // duration prefab exists
    public float postLightDelay = 1f;     // duration lamp stays on AFTER prefab disappears

    [Header("Vicinity Radius (for THIS lamp)")]
    public float triggerRadius = 5f;      // <<< PER-LAMP vicinity radius

    [Header("Audio")]
    [Range(0f, 1f)]
    public float sfxVolume = 1f;          // volume for this lamp
}

public class UnderworldLampSequence : MonoBehaviour
{
    public static UnderworldLampSequence Instance;

    [Header("Red Lamps Sequence")]
    [SerializeField] private LampStep[] redLampSteps; // size = 3

    [Header("Final White Lamp")]
    [SerializeField] private Light whiteLamp;
    [SerializeField] private AudioClip whiteLampSfx;
    [Range(0f, 1f)]
    [SerializeField] private float whiteLampVolume = 0.5f;

    [Header("Timing")]
    [SerializeField] private float firstDelay = 5f;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Audio")]
    [SerializeField] private AudioClip lampOnSfx;

    private int currentIndex = 0;
    private bool running = false;
    private AudioSource currentLampAudio; // tracks the currently playing red lamp audio

    // --- TRACK WHITE LAMP AUDIO ---
    private AudioSource whiteLampAudio;

    void Awake()
    {
        Instance = this;

        foreach (var step in redLampSteps)
            if (step.lampLight) step.lampLight.enabled = false;

        if (whiteLamp) whiteLamp.enabled = false;
    }

    public void BeginSequence()
    {
        if (running) return;
        running = true;

        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        // --- Initial delay ---
        yield return new WaitForSeconds(firstDelay);

        // --- Loop through red lamps ---
        while (currentIndex < redLampSteps.Length)
        {
            var step = redLampSteps[currentIndex];

            // --- Turn ON lamp + play SFX ---
            if (step.lampLight)
            {
                step.lampLight.enabled = true;

                if (currentLampAudio)
                {
                    currentLampAudio.Stop();
                    Destroy(currentLampAudio);
                }

                // create audio source
                currentLampAudio = step.lampLight.gameObject.AddComponent<AudioSource>();
                currentLampAudio.clip = lampOnSfx;
                currentLampAudio.volume = step.sfxVolume;
                currentLampAudio.loop = true;

                // --- 3D audio settings ---
                currentLampAudio.spatialBlend = 1f;      // fully 3D
                currentLampAudio.dopplerLevel = 0f;      // no Doppler effect
                currentLampAudio.minDistance = 120f;
                currentLampAudio.maxDistance = 200f;

                currentLampAudio.Play();
            }

            // --- WAIT FOR PLAYER NEAR SPAWNPOINT (NOT LAMP) ---
            yield return new WaitUntil(() =>
            {
                if (!player) return false;

                Vector3 checkPos;

                if (step.spawnPoint)
                    checkPos = step.spawnPoint.position;
                else if (step.lampLight)
                    checkPos = step.lampLight.transform.position;
                else
                    return false;

                return Vector3.Distance(player.position, checkPos) <= step.triggerRadius;
            });

            // --- Spawn prefab ---
            if (step.prefabToSpawn)
            {
                Vector3 pos = step.spawnPoint ? step.spawnPoint.position : step.lampLight.transform.position;
                Quaternion rot = step.spawnPoint ? step.spawnPoint.rotation : Quaternion.identity;

                var obj = Instantiate(step.prefabToSpawn, pos, rot);
                yield return new WaitForSeconds(step.prefabLifetime);
                Destroy(obj);
            }
            else
            {
                yield return new WaitForSeconds(step.prefabLifetime);
            }

            // --- Keep lamp on for a bit ---
            yield return new WaitForSeconds(step.postLightDelay);

            // --- Turn OFF lamp + stop SFX ---
            if (step.lampLight)
            {
                step.lampLight.enabled = false;

                if (currentLampAudio)
                {
                    currentLampAudio.Stop();
                    Destroy(currentLampAudio);
                    currentLampAudio = null;
                }
            }

            currentIndex++;
        }

        // --- Final white lamp ---
        if (whiteLamp)
        {
            whiteLamp.enabled = true;

            if (whiteLampSfx)
            {
                whiteLampAudio = whiteLamp.gameObject.AddComponent<AudioSource>();
                whiteLampAudio.clip = whiteLampSfx;
                whiteLampAudio.volume = whiteLampVolume;
                whiteLampAudio.loop = false;

                // --- 3D audio settings ---
                whiteLampAudio.spatialBlend = 1f;
                whiteLampAudio.dopplerLevel = 0f;
                whiteLampAudio.minDistance = 120f;
                whiteLampAudio.maxDistance = 200f;

                whiteLampAudio.Play();
                Destroy(whiteLampAudio, whiteLampSfx.length + 0.1f);
            }
        }

        running = false;
    }

    // --- PUBLIC STOPPER FOR WHITE LAMP AUDIO ---
    public void StopWhiteLampAudio()
    {
        if (whiteLampAudio)
        {
            whiteLampAudio.Stop();
            Destroy(whiteLampAudio);
            whiteLampAudio = null;
        }
    }
}
