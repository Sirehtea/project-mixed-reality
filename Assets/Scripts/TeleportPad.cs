using UnityEngine;

public class TeleportPad : MonoBehaviour
{
    [Header("Who to teleport")]
    [SerializeField] private Transform playerToTeleport;
    [SerializeField] private string playerTag = "Player";

    [Header("Where to teleport")]
    [SerializeField] private Transform teleportTarget;

    [Header("Rotation Reset")]
    [SerializeField] private bool resetRotationToZero = true;

    [Header("Enable Existing Prefab On Teleport")]
    [SerializeField] private GameObject prefabToEnable;
    [SerializeField] private Transform optionalRepositionTarget;

    private void OnTriggerEnter(Collider other)
    {
        // Only react to the player
        if (other.transform != playerToTeleport && !other.CompareTag(playerTag))
            return;

        if (playerToTeleport == null || teleportTarget == null)
            return;

        // --- TELEPORT ---
        playerToTeleport.position = teleportTarget.position;

        if (resetRotationToZero)
            playerToTeleport.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            playerToTeleport.rotation = teleportTarget.rotation;

        // --- STOP WHITE LAMP AUDIO ---
        if (UnderworldLampSequence.Instance)
            UnderworldLampSequence.Instance.StopWhiteLampAudio();

        // --- ENABLE EXISTING PREFAB IN SCENE ---
        if (prefabToEnable)
        {
            // optionally move it first
            if (optionalRepositionTarget)
            {
                prefabToEnable.transform.position = optionalRepositionTarget.position;
                prefabToEnable.transform.rotation = optionalRepositionTarget.rotation;
            }

            prefabToEnable.SetActive(true);
        }

        // --- CLEAR UNDERWORLD FOG ---
        if (FogController.Instance)
            FogController.Instance.ReleaseFog(this);

        // --- RESET ANY NPCS THAT WERE WATCHING ---
        NPCWatcher npc = FindObjectOfType<NPCWatcher>();
        if (npc != null)
            npc.SetWatching(false);
    }
}
