using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;

public class TVControllerXR : MonoBehaviour
{
    [Header("Components")]
    public VideoPlayer videoPlayer;          // Assign your Video Player here
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable; // Assign your XR Grab Interactable (same object)

    [Header("Settings")]
    public float startDelay = 5f;

    private bool isOn = false;

    void Start()
    {
        // Make sure both references exist
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        if (grabInteractable == null)
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // Subscribe to XR grab/activate event
        if (grabInteractable != null)
            grabInteractable.selectEntered.AddListener(OnGrab);

        // Start video after delay
        Invoke(nameof(StartTV), startDelay);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    public void StartTV()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            isOn = true;
        }
    }
    public void StopTV()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Pause();
            isOn = false;
        }
    }
    private void OnGrab(SelectEnterEventArgs args)
    {
        ToggleTV();
    }

    public void ToggleTV()
    {
        if (videoPlayer == null) return;

        if (isOn)
        {
            StopTV();
        }
        else
        {
            StartTV();
        }
    }
}
