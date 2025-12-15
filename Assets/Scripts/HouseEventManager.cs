using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HouseEventManager : MonoBehaviour
{
    public GameObject player;
    public GameObject radio;
    public GameObject walkieTalkie;

    // Even 1
    private bool radioOff = false;
    public List<GameObject> lights = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        walkieTalkie.GetComponent<Light>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!radioOff && (player.transform.position - radio.transform.position).magnitude < 2f)
        {
            radioOff = true;
            radio.GetComponent<AudioSource>().Stop();
            walkieTalkie.GetComponent<AudioSource>().Play();
            walkieTalkie.GetComponent<Light>().enabled = true;
            foreach (GameObject light in lights)
            {
                light.GetComponent<Light>().enabled = false;
            }
        }
        else if (radioOff) { }
        
    }

    
}