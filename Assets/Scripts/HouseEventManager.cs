using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class HouseEventManager : MonoBehaviour
{
    public GameObject player;
    public GameObject human;
    public GameObject monster;

    // Event 1: radio playing
    private bool radioOff = false;
    public List<GameObject> lights = new List<GameObject>();
    public GameObject radio;
    public GameObject walkieTalkie;
    public GameObject fridgeDoor;

    // Event 2: after frigde door opened -> move to second scene
    private bool fridgeOpened = false;
    private bool fadeIn = false;
    private bool fadeOut = false;
    public Image screenCanvas;
    public CanvasGroup ScreenFade;
    private float fadeDuration = 5f;
    public GameObject storeTeleport;

    // Event 3: static starts playing
    private bool playerReturns = false;
    private bool tvStatic = false;
    public GameObject tvScreenStatic;
    public List<GameObject> tvLights = new List<GameObject>();

    // Event 4: tv starts playing
    private bool tvPlaying = false;
    public GameObject tvScreenPlaying;
    public List<GameObject> blockades = new List<GameObject>();

    // Event 5: final mirror event
    public GameObject finalMirror;
    private bool finale = false;
    public GameObject house;
    public GameObject store;
    public GameObject tvScene;
    public GameObject mirror;

    void Start()
    {
        monster.SetActive(false);
        walkieTalkie.GetComponent<Light>().enabled = false;
        tvScreenStatic.SetActive(false);
        tvScreenPlaying.SetActive(false);
        tvLights.ForEach(light => light.GetComponent<Light>().enabled = false);
        blockades.ForEach(blockade => blockade.SetActive(false));
        finalMirror.SetActive(false);
    }

    void Update()
    {
        if (!radioOff && (player.transform.position - radio.transform.position).magnitude < 1.2f)
        {
            ApproachRadio();
        }
        else if (fridgeOpened) 
        {
            if (fadeIn)
            {
                FadeInWhite();
            }
            else if (fadeOut)
            {
                FadeOutBlack();
            }
        }
        else if (tvPlaying)
        {
            if (tvScreenPlaying.GetComponent<VideoPlayer>().frame >= (long)tvScreenPlaying.GetComponent<VideoPlayer>().frameCount-1)
                FinalMirror();
        }
        
    }

    public void ApproachRadio()
    {
        radioOff = true;
        radio.GetComponent<AudioSource>().Stop();
        walkieTalkie.GetComponent<AudioSource>().Play();
        walkieTalkie.GetComponent<Light>().enabled = true;
        foreach (GameObject light in lights)
        {
            light.GetComponent<Light>().enabled = false;
        }
        fridgeDoor.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void FridgeOpened()
    {
        if (!fridgeOpened)
        {
            fridgeOpened = true;
            walkieTalkie.GetComponent<AudioSource>().Stop();
            walkieTalkie.GetComponent<Light>().enabled = false;
            fadeIn = true;
            // Code to move to player to second scene
        }
    }

    public void PlayerReturns()
    {
        if (!playerReturns)
        {
            playerReturns = true;
            // Code to trigger TV static
            tvLights.ForEach(light => light.GetComponent<Light>().enabled = true);
            tvScreenStatic.SetActive(true);
        }
    }

    public void TvTriggered()
    {
        if (!tvPlaying && !finale)
        {
            tvPlaying = true;
            tvScreenStatic.SetActive(false);
            tvScreenPlaying.SetActive(true);
            blockades.ForEach(blockade => blockade.SetActive(true));
        }
    }

    public void FinalMirror()
    {
        human.SetActive(false);
        monster.SetActive(true);
        tvScreenPlaying.SetActive(false);
        finalMirror.SetActive(true);
        finale = true;
        house.SetActive(false);
        store.SetActive(false);
        tvScene.SetActive(false);
        mirror.SetActive(false);
    }

    public void FadeInWhite()
    {
        ScreenFade.alpha += Time.deltaTime / fadeDuration;
        if (ScreenFade.alpha >= 1 && fadeIn)
        {
            fadeIn = false;
            screenCanvas.color = Color.black;
            player.transform.position = storeTeleport.transform.position;
            fadeOut = true;
        }
    }

    public void FadeOutBlack()
    {
        ScreenFade.alpha -= Time.deltaTime / fadeDuration;
        if (ScreenFade.alpha <= 0)
        {
            fadeOut = false;
            fridgeOpened = false;
            screenCanvas.color = Color.white;
        }
    }
}