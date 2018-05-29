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
    GameObject[] ghostRemoteFloor = new GameObject[remoteFloor_count];
    GameObject remoteSwitch;
    GameObject ghostRemoteSwitch;

    GameObject fallenStoneSwitch;
    GameObject ghostFallenStoneSwitch;
    GameObject stone;
    GameObject ghostStone;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < fallenNeedle_count; i++)
        {
            fallenNeedle[i] = GameObject.Find("FallenNeedle" + (i + 1));
            ghost_fallenNeedle[i] = GameObject.Find("GhostFallenNeedle" + (i + 1));
        }

        for (int i = 0; i < remoteFloor_count; i++)
        {
            remoteFloor[i] = GameObject.Find("RemoteFloor" + (i + 1));
            ghostRemoteFloor[i] = GameObject.Find("GhostRemoteFloor" + (i + 1));
        }
        remoteSwitch = GameObject.Find("RemoteSwitch");
        ghostRemoteSwitch = GameObject.Find("GhostRemoteSwitch");

        fallenStoneSwitch = GameObject.Find("FallenStoneSwitch");
        ghostFallenStoneSwitch = GameObject.Find("GhostFallenStoneSwitch");

        stone = GameObject.Find("Stone");
        ghostStone = GameObject.Find("GhostStone");
    }

    // Update is called once per frame
    void Update()
    {
        //落石
        if (fallenStoneSwitch.GetComponent<SwitchScript>().trapEnable)
        {
            stone.GetComponent<StoneScript>().fallenEnable = true;
        }
        if (ghostFallenStoneSwitch.GetComponent<SwitchScript>().ghostTrapEnable)
        {
            ghostStone.GetComponent<StoneScript>().fallenEnable = true;
        }

        //落下する床
        if (remoteSwitch.GetComponent<SwitchScript>().trapEnable)
        {
            for (int i = 0; i < remoteFloor_count; i++)
            {
                remoteFloor[i].GetComponent<RemoteFloorScript>().fallenDownEnable = true;
                PlayerPrefsX.SetBool("ghostTrapVisible" + remoteFloor[i].name.Insert(0, "Ghost"), true);
            }
            fallenNeedle[0].GetComponent<FallenNeedleScript>().trapEnable = true;

            remoteSwitch.GetComponent<SwitchScript>().trapEnable = false;
        }

        FallenByDistance(fallenNeedle_count);

        if (ghostRemoteSwitch.GetComponent<SwitchScript>().ghostTrapEnable)
        {
            for (int i = 0; i < remoteFloor_count; i++)
            {
                ghostRemoteFloor[i].GetComponent<RemoteFloorScript>().fallenDownEnable = true;
            }
            //矢を降らせる
            ghost_fallenNeedle[0].GetComponent<FallenNeedleScript>().trapEnable = true;

            ghostRemoteSwitch.GetComponent<SwitchScript>().ghostTrapEnable = false;
        }

        GhostFallenByDistance(fallenNeedle_count);
    }

    public void FallenByDistance(int fallenNeedle_count)
    {
        //矢を降らせる
        int fallenNeedle_distance = 2;

        if (!fallenNeedle[fallenNeedle_count - 1].GetComponent<FallenNeedleScript>().trapEnable)
        {
            for (int i = 0; i < fallenNeedle_count; i++)
            {
                if (i != 0)
                {
                    if (fallenNeedle[i].transform.position.y - fallenNeedle[i - 1].transform.position.y > fallenNeedle_distance)
                    {
                        fallenNeedle[i].GetComponent<FallenNeedleScript>().trapEnable = true;
                    }
                }
            }
        }
    }

    public void GhostFallenByDistance(int fallenNeedle_count)
    {
        //矢を降らせる
        int fallenNeedle_distance = 2;

        if (!ghost_fallenNeedle[fallenNeedle_count - 1].GetComponent<FallenNeedleScript>().trapEnable)
        {
            for (int i = 0; i < fallenNeedle_count; i++)
            {
                if (i != 0)
                {
                    if (ghost_fallenNeedle[i].transform.position.y - ghost_fallenNeedle[i - 1].transform.position.y > fallenNeedle_distance)
                    {
                        ghost_fallenNeedle[i].GetComponent<FallenNeedleScript>().trapEnable = true;
                    }
                }
            }
        }
    }
}
