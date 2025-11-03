using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class HingedDoor : MonoBehaviour
{
    [Header("Door limits (degrees)")]
    [Tooltip("Angle when the door is fully closed (local Y euler).")]
    public float closedAngleY = 0f;

    [Tooltip("Max angle you can push the door open (positive).")]
    public float maxOpenAngleY = 90f;

    [Tooltip("Max angle you can pull the door inward (negative).")]
    public float maxBackAngleY = -10f;

    [Header("Door swing smoothing")]
    public float followSpeed = 15f;

    private XRGrabInteractable grab;
    private Rigidbody rb;

    private bool isGrabbed = false;

    private float targetAngleY;
    private float grabStartAngleY;
    private Vector3 grabStartHandPos;
    private Transform currentInteractor;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        targetAngleY = transform.localEulerAngles.y;

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void Update()
    {
        if (isGrabbed && currentInteractor != null)
        {
            UpdateTargetAngleFromHand();
        }

        float clamped = Mathf.Clamp(targetAngleY, closedAngleY + maxBackAngleY, closedAngleY + maxOpenAngleY);

        // --- Enforce X = -90 always ---
        float lockedX = -90f;
        float lockedZ = 0f;

        Quaternion current = transform.localRotation;
        Quaternion desired = Quaternion.Euler(lockedX, clamped, lockedZ);

        transform.localRotation = Quaternion.Slerp(current, desired, Time.deltaTime * followSpeed);

        // keep target angle synced with actual Y
        float normalizedY = NormalizeAngle(transform.localEulerAngles.y);
        targetAngleY = normalizedY;
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        currentInteractor = args.interactorObject.transform;
        grabStartAngleY = NormalizeAngle(transform.localEulerAngles.y);
        grabStartHandPos = currentInteractor.position;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        currentInteractor = null;
    }

    void UpdateTargetAngleFromHand()
    {
        Vector3 hingePos = transform.position;

        Vector3 handOffsetNow = currentInteractor.position - hingePos;
        Vector3 handOffsetStart = grabStartHandPos - hingePos;

        handOffsetNow.y = 0f;
        handOffsetStart.y = 0f;

        if (handOffsetNow.sqrMagnitude < 0.0001f || handOffsetStart.sqrMagnitude < 0.0001f)
            return;

        float deltaAngle = SignedAngleOnPlane(handOffsetStart, handOffsetNow, Vector3.up);
        targetAngleY = grabStartAngleY + deltaAngle;
    }

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        if (angle < -180f) angle += 360f;
        return angle;
    }

    float SignedAngleOnPlane(Vector3 v1, Vector3 v2, Vector3 planeNormal)
    {
        v1 = Vector3.ProjectOnPlane(v1, planeNormal);
        v2 = Vector3.ProjectOnPlane(v2, planeNormal);
        return Vector3.SignedAngle(v1, v2, planeNormal);
    }
}
