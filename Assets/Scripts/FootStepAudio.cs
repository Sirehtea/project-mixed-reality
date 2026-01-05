using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepAudio : MonoBehaviour
{
    [Header("Movement")]
    public Transform xrOrigin;            // XR Origin root
    public float stepDistance = 0.5f;    // Distance per step

    [Header("Default Footstep")]
    public AudioClip defaultFootstep;
    [Range(0f, 1f)] public float volume = 0.8f;
    public Vector2 randomPitch = new Vector2(0.95f, 1.05f);

    [Header("Surface Footsteps")]
    public AudioClip grassFootstep;
    public AudioClip storeFootstep;

    private Vector3 lastPos;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        if (!xrOrigin) xrOrigin = transform;
        lastPos = xrOrigin.position;
    }

    void Update()
    {
        if (!defaultFootstep || !source) return;

        float dist = Vector3.Distance(xrOrigin.position, lastPos);
        if (dist >= stepDistance)
        {
            AudioClip clipToPlay = defaultFootstep;

            // --- Raycast downward to detect surface ---
            Ray ray = new Ray(xrOrigin.position + Vector3.up * 0.1f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 1f))
            {
                if (hit.collider.CompareTag("Grass") && grassFootstep)
                    clipToPlay = grassFootstep;
                else if (hit.collider.CompareTag("Store") && storeFootstep)
                    clipToPlay = storeFootstep;
            }

            source.pitch = Random.Range(randomPitch.x, randomPitch.y);
            source.volume = volume;
            source.PlayOneShot(clipToPlay);

            lastPos = xrOrigin.position;
        }
    }
}
