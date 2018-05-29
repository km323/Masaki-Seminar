﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// これは落石を移動させるクラスです
/// </summary>
public class StoneScript : MonoBehaviour
{
    public bool fallenEnable;
    public bool ghostTrapVisible = false;
    // Use this for initialization
    void Start()
    {
        if (tag == "GhostTrap")
        {
            ghostTrapVisible = PlayerPrefsX.GetBool("ghostTrapVisible" + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fallenEnable)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.GetComponent<Rigidbody>().AddForce(0, -3, 0);
            fallenEnable = false;
        }
        if (tag == "GhostTrap")
        {
            if (ghostTrapVisible)
            {
                GetComponent<Renderer>().enabled = true;
            }
            else
            {
                GetComponent<Renderer>().enabled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            ghostTrapVisible = true;
            PlayerPrefsX.SetBool("ghostTrapVisible" + gameObject.name.Insert(0, "Ghost"), ghostTrapVisible);
        }
        if (collision.gameObject.name == "Ghost")
        {
            if (tag == "GhostTrap")
            {
                transform.Find("Disapear").GetComponent<ParticleSystem>().Play();
                ghostTrapVisible = false;
                PlayerPrefsX.SetBool("ghostTrapVisible" + gameObject.name, ghostTrapVisible);
            }
        }
        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            //床に落ちたら、左へのパワーを与える
            if (collision.gameObject.name == "Floor2")
            {
                transform.GetComponent<Rigidbody>().AddForce(-10, 0, 0,ForceMode.VelocityChange);
            }
            //床に落ちたら、左へのパワーを与える
            if (collision.gameObject.name == "Floor1")
            {
                transform.GetComponent<Rigidbody>().AddForce(-6, 0, 0, ForceMode.VelocityChange);
            }
        }
    }
}