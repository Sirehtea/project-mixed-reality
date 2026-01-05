using UnityEngine;
using System.Collections;

public class NPCRespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Vector3 respawnRotationEuler = Vector3.zero;
    [SerializeField] private string npcTag = "NPC";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(npcTag)) return;
        if (respawnPoint == null) return;

        NPCCooldown cooldown = other.GetComponent<NPCCooldown>();
        if (cooldown == null)
            cooldown = other.gameObject.AddComponent<NPCCooldown>();

        if (cooldown.IsOnCooldown)
            return;

        cooldown.StartCooldown(0.25f);

        other.transform.SetPositionAndRotation(
            respawnPoint.position,
            Quaternion.Euler(respawnRotationEuler)
        );
    }
}

public class NPCCooldown : MonoBehaviour
{
    public bool IsOnCooldown { get; private set; }

    public void StartCooldown(float time)
    {
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(Cooldown(time));
    }

    private IEnumerator Cooldown(float time)
    {
        IsOnCooldown = true;
        yield return new WaitForSeconds(time);
        IsOnCooldown = false;
    }
}
