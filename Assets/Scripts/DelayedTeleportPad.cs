using UnityEngine;
using System.Collections;

public class DelayedTeleportPad : MonoBehaviour
{
    public HouseEventManager eventManagerScript;

    [Header("Teleport")]
    [SerializeField] private Transform playerToTeleport;
    [SerializeField] private Transform teleportTarget;

    [Header("Delay before teleport (seconds)")]
    [SerializeField] private float teleportDelay = 1f;

    [Header("Rotation")]
    [SerializeField] private bool useCustomRotation = false;
    [SerializeField] private Vector3 customRotationEuler = Vector3.zero;

    private Coroutine teleportRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (playerToTeleport == null || teleportTarget == null)
            return;

        // only react when the player enters
        if (other.transform != playerToTeleport)
            return;

        if (teleportRoutine == null)
            teleportRoutine = StartCoroutine(TeleportAfterDelay());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform != playerToTeleport)
            return;

        // cancel teleport if they walk off the pad
        if (teleportRoutine != null)
        {
            StopCoroutine(teleportRoutine);
            teleportRoutine = null;
        }
    }

    private IEnumerator TeleportAfterDelay()
    {
        yield return new WaitForSeconds(teleportDelay);

        playerToTeleport.position = teleportTarget.position;
        eventManagerScript.PlayerReturns();
        if (useCustomRotation)
            playerToTeleport.rotation = Quaternion.Euler(customRotationEuler);
        else
            playerToTeleport.rotation = teleportTarget.rotation;

        teleportRoutine = null;
    }
}
