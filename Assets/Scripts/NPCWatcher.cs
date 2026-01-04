using UnityEngine;
using System.Collections;

public class NPCWatcher : MonoBehaviour
{
    [Header("Watching Behaviour")]
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float fogValue = 0.5f;

    [Header("Idle Pose")]
    [SerializeField] private Vector3 idleEulerRotation;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController idleController;
    [SerializeField] private RuntimeAnimatorController watchingController;

    [Header("Creepy Voice Lines")]
    [SerializeField] private bool enableVoices = true;
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private AudioClip[] voiceClips;
    [SerializeField] private float voiceIntervalSeconds = 4f;
    [SerializeField] private bool randomOrder = true;

    private Transform player;
    private bool isWatching = false;
    private Coroutine voiceLoopRoutine;
    private int clipIndex = 0;

    void Start()
    {
        if (idleEulerRotation == Vector3.zero)
            idleEulerRotation = transform.rotation.eulerAngles;

        if (animator && idleController)
            animator.runtimeAnimatorController = idleController;

        if (!voiceSource)
            voiceSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isWatching)
        {
            Quaternion idleRot = Quaternion.Euler(idleEulerRotation);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                idleRot,
                turnSpeed * Time.deltaTime
            );
            return;
        }

        if (!player) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion target = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            target,
            turnSpeed * Time.deltaTime
        );
    }

    public void SetPlayerReference(Transform p)
    {
        player = p;
    }

    public void SetWatching(bool watching)
    {
        if (isWatching == watching)
            return;

        isWatching = watching;

        // Fog
        if (watching)
            FogController.Instance?.RequestFog(this, fogValue);
        else
            FogController.Instance?.ReleaseFog(this);

        // Animation
        if (animator)
        {
            animator.runtimeAnimatorController =
                watching && watchingController ? watchingController : idleController;
        }

        // Voices
        HandleVoices(watching);
    }

    private void HandleVoices(bool watching)
    {
        if (!enableVoices || voiceSource == null || voiceClips == null || voiceClips.Length == 0)
            return;

        if (watching)
        {
            if (voiceLoopRoutine == null)
                voiceLoopRoutine = StartCoroutine(VoiceLoop());
        }
        else
        {
            if (voiceLoopRoutine != null)
            {
                StopCoroutine(voiceLoopRoutine);
                voiceLoopRoutine = null;
            }

            voiceSource.Stop();
        }
    }

    private IEnumerator VoiceLoop()
    {
        while (true)
        {
            if (!voiceSource.isPlaying)
            {
                AudioClip clip;

                if (randomOrder)
                    clip = voiceClips[Random.Range(0, voiceClips.Length)];
                else
                {
                    clip = voiceClips[clipIndex];
                    clipIndex = (clipIndex + 1) % voiceClips.Length;
                }

                voiceSource.clip = clip;
                voiceSource.Play();
            }

            yield return new WaitForSeconds(voiceIntervalSeconds);
        }
    }
}
