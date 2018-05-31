using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{

    //参照
    GameObject ClearEff;
    GameObject worldLight;
    GameObject ui;

    [HideInInspector]
    public static bool canControlPlayer;
    bool eff_Playable;
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
        if (eff_Playable)
        {
            if (!ClearEff.GetComponent<ParticleSystem>().isPlaying)
            {
                ClearEff.GetComponent<ParticleSystem>().Play();
            }
            ui.GetComponent<UIScript>().turnLightOn = true;
            //HACK:必要ないかもしれない
            worldLight.GetComponent<Light>().intensity = 2f;
            worldLight.GetComponent<Light>().color = Color.white;
            eff_Playable = false;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            eff_Playable = true;
            Animator doorAnimator = GameObject.Find("door").GetComponent<Animator>();
            doorAnimator.SetTrigger("OpenDoor");
            canControlPlayer = false;
        }
    }
}
