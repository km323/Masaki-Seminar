using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public static bool canControlPlayer;
    GameObject ClearEff;
    GameObject worldLight;
    GameObject ui;
    bool playable;
    // Use this for initialization
    void Start()
    {
        canControlPlayer = true;
        ui = GameObject.Find("UI");
        worldLight = GameObject.Find("Directional light");
        ClearEff = GameObject.Find("ClearEff");
    }

    // Update is called once per frame
    void Update()
    {
        if (playable)
        {
            if (!ClearEff.GetComponent<ParticleSystem>().isPlaying)
            {
                ClearEff.GetComponent<ParticleSystem>().Play();
            }
            ui.GetComponent<UIScript>().turnLightOn = true;
            worldLight.GetComponent<Light>().intensity = 2f;
            worldLight.GetComponent<Light>().color = Color.white;
            playable = false;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            playable = true;
            Animator doorAnimator = GameObject.Find("door").GetComponent<Animator>();
            doorAnimator.SetTrigger("OpenDoor");
            canControlPlayer = false;
        }
    }
}
