using UnityEngine;
using System.Collections;

public class SimpleRadio : MonoBehaviour
{
    public AudioSource radioAudio;
    public float startDelay = 10f;
    private bool isOn = false;

    void Start()
    {
        StartCoroutine(StartRadioAfterDelay());
    }

    IEnumerator StartRadioAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        radioAudio.Play();
        isOn = true;
        Debug.Log("Radio started automatically");
    }

    void OnMouseDown()
    {
        // Wordt ook getriggerd door XR-ray interactor bij ‘click/select’,
        // zolang je collider heeft en interactie toestaat
        if (isOn)
        {
            StopRadio();
        }
        else
        {
            StartRadio();
        }
    }

    public void StartRadio()
    {
        radioAudio.Play();
        isOn = true;
        Debug.Log("Radio turned on");
    }
    
    public void StopRadio()
    {
        radioAudio.Stop();
        isOn = false;
        Debug.Log("Radio turned off");
    }
}
