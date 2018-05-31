using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleSetScript : MonoBehaviour {
    GameObject needle;
    public bool ghostTrapVisible = false;
    // Use this for initialization
    void Start () {
        needle = transform.Find("Needle").gameObject;
        if (tag == "GhostTrap")
        {
            ghostTrapVisible = PlayerPrefsX.GetBool("ghostTrapVisible" + gameObject.name);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(needle.GetComponent<NeedleScript>().hit)
        {
            //ghostTrapVisible = true;
            //PlayerPrefsX.SetBool("ghostTrapVisible" + gameObject.name.Insert(0, "Ghost"), ghostTrapVisible);
            needle.GetComponent<NeedleScript>().hit = false;
        }
        if (tag == "GhostTrap")
        {
            if (ghostTrapVisible)
            {
                needle.GetComponent<Renderer>().enabled = true;
                transform.Find("Trap_Needle").GetComponent<Renderer>().enabled = true;
            }
            else
            {
                needle.GetComponent<Renderer>().enabled = false;
                transform.Find("Trap_Needle").GetComponent<Renderer>().enabled = false;
            }
            if(needle.GetComponent<NeedleScript>().hitGhost)
            {
                transform.Find("Disapear").GetComponent<ParticleSystem>().Play();
                ghostTrapVisible = false;
                PlayerPrefsX.SetBool("ghostTrapVisible" + gameObject.name, ghostTrapVisible);
                needle.GetComponent<NeedleScript>().hitGhost = false;
            }
        }
	}
}
