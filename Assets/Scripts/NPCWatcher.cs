using UnityEngine;

public class NPCWatcher : MonoBehaviour
{
    [Header("Animator control")]
    public Animator animator;
    public bool useAnimatorStates = false; // keep this OFF for now

    [Header("Chill facing direction")]
    [Tooltip("The rotation this NPC should face when music is playing (in degrees). Only Y really matters.")]
    public Vector3 idleEulerAngles; // set this in Inspector per NPC, e.g. (0, 45, 0)

    [Header("Look behavior")]
    public float lookRotateSpeed = 5f; // how fast they turn

    // runtime state
    private bool watching = false;
    private Transform playerRef;

    // called by controller when player enters/leaves range
    public void SetWatching(bool watch)
    {
        watching = watch;
        Debug.Log(gameObject.name + " -> SetWatching(" + watch + ")");

        if (animator)
        {
            if (useAnimatorStates)
            {
                // not using this yet, but keeping fallback
                animator.SetBool("isWatching", watch);
                animator.speed = 1f;
            }
            else
            {
                if (watch)
                {
                    // freeze
                    animator.speed = 0f;
                    Debug.Log(gameObject.name + " animator.speed = 0 (FREEZE)");
                }
                else
                {
                    // unfreeze
                    animator.speed = 1f;
                    Debug.Log(gameObject.name + " animator.speed = 1 (RESUME)");
                }
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " has no Animator assigned in NPCWatcher!");
        }
    }

    // this gets called from the controller so we know who to look at
    public void SetPlayerReference(Transform player)
    {
        playerRef = player;
    }

    private void Update()
    {
        // Handle rotation every frame depending on state
        if (watching)
        {
            // WATCH MODE → face the player
            FacePlayerNow();
        }
        else
        {
            // CHILL MODE → face idleEulerAngles
            FaceIdleNow();
        }
    }

    private void FacePlayerNow()
    {
        if (playerRef == null) return;

        // direction on ground plane
        Vector3 dir = playerRef.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lookRotateSpeed * Time.deltaTime);
    }

    private void FaceIdleNow()
    {
        // We only really care about Y but we’ll allow full euler so you can tilt if you really want (but probably keep X/Z = 0)
        Quaternion targetRot = Quaternion.Euler(idleEulerAngles);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lookRotateSpeed * Time.deltaTime);
    }
}
