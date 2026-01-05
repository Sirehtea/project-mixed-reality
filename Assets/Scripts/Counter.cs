using UnityEngine;
using System.Collections;

public class Counter : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private string productTag = "Product";
    [SerializeField] private float teleportDelay = 2f;
    [SerializeField] private float npcDelay = 4f;

    [Header("Teleport")]
    [SerializeField] private Transform playerToTeleport;
    [SerializeField] private Transform teleportTarget;

    [Header("Audio")]
    [SerializeField] private AudioClip teleportSfx;

    [Header("NPC Reaction")]
    [SerializeField] private NPCWatcher npc;
    [SerializeField] private Transform lookTarget;

    private Coroutine teleportRoutine;
    private Coroutine npcRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(productTag))
            return;

        if (teleportRoutine == null)
            teleportRoutine = StartCoroutine(TeleportAfterDelay());

        if (npcRoutine == null)
            npcRoutine = StartCoroutine(NPCReactAfterDelay());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(productTag))
            return;

        if (teleportRoutine != null)
        {
            StopCoroutine(teleportRoutine);
            teleportRoutine = null;
        }

        if (npcRoutine != null)
        {
            StopCoroutine(npcRoutine);
            npcRoutine = null;
        }

        if (npc)
            npc.SetWatching(false);
    }

    private IEnumerator TeleportAfterDelay()
    {
        yield return new WaitForSeconds(teleportDelay);

        if (playerToTeleport && teleportTarget)
        {
            // move player
            playerToTeleport.position = teleportTarget.position;
            playerToTeleport.rotation = Quaternion.Euler(0f, 0f, 0f);

            // play teleport sound
            if (teleportSfx)
                AudioSource.PlayClipAtPoint(teleportSfx, teleportTarget.position);

            // enable fog
            if (FogController.Instance)
                FogController.Instance.RequestFog(this, 0.04f);

            // stop NPC watching
            if (npc)
                npc.SetWatching(false);

            // ---- START UNDERWORLD LAMP EVENT ----
            if (UnderworldLampSequence.Instance)
                UnderworldLampSequence.Instance.BeginSequence();
        }

        teleportRoutine = null;
    }

    private IEnumerator NPCReactAfterDelay()
    {
        yield return new WaitForSeconds(npcDelay);

        if (npc)
        {
            npc.SetPlayerReference(lookTarget ? lookTarget : playerToTeleport);
            npc.SetWatching(true);
        }

        npcRoutine = null;
    }
}
