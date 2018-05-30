using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenTrapScript : MonoBehaviour
{
    GameObject ghostFallenTrapSet;
    bool ghost_Trap_Visible;
    // Use this for initialization
    void Start()
    {
        ghostFallenTrapSet = GameObject.Find("GhostFallenTrapSet");
        ghost_Trap_Visible = PlayerPrefsX.GetBool("Visible" + gameObject.name);

    }

    // Update is called once per frame
    void Update()
    {
        if (tag == "GhostTrap")
        {
            if (ghost_Trap_Visible)
            {
                foreach (Transform child in ghostFallenTrapSet.transform)
                {
                    if (child.GetComponent<Renderer>() != null)
                        child.GetComponent<Renderer>().enabled = true;
                    foreach (Transform secondChild in child)
                    {
                        if (secondChild.GetComponent<Renderer>() != null)
                            secondChild.GetComponent<Renderer>().enabled = true;
                    }
                }

            }
            else
            {
                foreach (Transform child in ghostFallenTrapSet.transform)
                {
                    if (child.GetComponent<Renderer>() != null)
                        child.GetComponent<Renderer>().enabled = false;
                    foreach (Transform secondChild in child)
                    {
                        if (secondChild.GetComponent<Renderer>() != null)
                            secondChild.GetComponent<Renderer>().enabled = false;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            ghost_Trap_Visible = true;
            PlayerPrefsX.SetBool("Visible" + gameObject.name.Insert(0, "Ghost"), ghost_Trap_Visible);
        }
        if (collision.gameObject.name == "Ghost")
        {
            if (tag == "GhostTrap")
            {
                transform.Find("Disapear").GetComponent<ParticleSystem>().Play();
                ghost_Trap_Visible = false;
                PlayerPrefsX.SetBool("Visible" + gameObject.name, ghost_Trap_Visible);
            }
        }

    }
}
