using UnityEngine;
using System.Collections;

public class Counter : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private string productTag = "Product";
    [SerializeField] private float teleportDelay = 2f;

    [Header("Teleport")]
    [SerializeField] private Transform playerToTeleport;
    [SerializeField] private Transform teleportTarget;

    [Header("Audio")]
    [SerializeField] private AudioClip teleportSfx;

    [Header("NPC Reaction")]
    [SerializeField] private NPCWatcher npc;
    [SerializeField] private Transform lookTarget;

    private bool triggered = false;
    private Coroutine teleportRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(productTag))
            return;

        if (triggered)
            return; // don't run twice

        triggered = true;

        // --- NPC reacts IMMEDIATELY ---
        if (npc)
        {
            npc.SetPlayerReference(lookTarget ? lookTarget : playerToTeleport);
            npc.SetWatching(true);
        }

        // --- Schedule teleport ---
        teleportRoutine = StartCoroutine(TeleportAfterDelay());
    }

    private IEnumerator TeleportAfterDelay()
    {
        yield return new WaitForSeconds(teleportDelay);

        if (playerToTeleport && teleportTarget)
        {
            playerToTeleport.position = teleportTarget.position;
            playerToTeleport.rotation = Quaternion.Euler(0f, 0f, 0f);

            // Sound still plays
            if (teleportSfx)
                AudioSource.PlayClipAtPoint(teleportSfx, teleportTarget.position);

            // Fog still happens
            if (FogController.Instance)
                FogController.Instance.RequestFog(this, 0.04f);

            // NPC stops again here
            if (npc)
                npc.SetWatching(false);

            // Lamps still start
            if (UnderworldLampSequence.Instance)
                UnderworldLampSequence.Instance.BeginSequence();
        }

        teleportRoutine = null;
    }

    // --- Exit no longer cancels anything ---
    private void OnTriggerExit(Collider other) { }
}
