using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Script : MonoBehaviour
{
    const int fallenNeedle_count = 3;
    GameObject[] fallenNeedle = new GameObject[fallenNeedle_count];
    GameObject[] ghost_fallenNeedle = new GameObject[fallenNeedle_count];

    const int remoteFloor_count = 2;
    GameObject[] remoteFloor = new GameObject[remoteFloor_count];
    GameObject remoteSwitch;

    GameObject fallenStoneSwitch;
    GameObject stone;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < fallenNeedle_count; i++)
        {
            fallenNeedle[i] = GameObject.Find("FallenNeedle" + (i + 1));
            ghost_fallenNeedle[i] = GameObject.Find("GhostFallenNeedle" + (i + 1));
        }
        fallenNeedle[0].GetComponent<FallenNeedleScript>().trapEnable = true;
        ghost_fallenNeedle[0].GetComponent<FallenNeedleScript>().trapEnable = true;

        for (int i = 0; i < remoteFloor_count; i++)
        {
            remoteFloor[i] = GameObject.Find("RemoteFloor" + (i + 1));
            remoteFloor[i].SetActive(false);
        }
        remoteSwitch = GameObject.Find("RemoteSwitch");

        fallenStoneSwitch = GameObject.Find("FallenStoneSwitch");
        stone = GameObject.Find("Stone");
    }

    // Update is called once per frame
    void Update()
    {
        int fallenNeedle_distance = 2;
        for (int i = 0; i < fallenNeedle_count; i++)
        {
            if (i != 0)
            {
                if (fallenNeedle[i].transform.position.y - fallenNeedle[i - 1].transform.position.y > fallenNeedle_distance)
                {
                    fallenNeedle[i].GetComponent<FallenNeedleScript>().trapEnable = true;
                    ghost_fallenNeedle[i].GetComponent<FallenNeedleScript>().trapEnable = true;
                }
            }
        }

        if(fallenStoneSwitch.GetComponent<SwitchScript>().trapEnable)
        {
            stone.GetComponent<StoneScript>().fallenEnable = true;
        }

        if(remoteSwitch.GetComponent<SwitchScript>().trapEnable)
        {
            for(int i=0;i<remoteFloor_count;i++)
            {
                remoteFloor[i].SetActive(true);
            }
            remoteSwitch.GetComponent<SwitchScript>().trapEnable = false;
        }
    }
}
