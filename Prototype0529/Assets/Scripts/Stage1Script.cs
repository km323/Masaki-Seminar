using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Script : MonoBehaviour
{
    const int fallenNeedle_count = 3;
    GameObject[] fallenNeedle = new GameObject[fallenNeedle_count];
    
    const int remoteFloor_Count = 2;
    GameObject[] remoteFloor = new GameObject[remoteFloor_Count];
    GameObject remoteFloorSwitch;

    GameObject fallenStoneSwitch;
    GameObject stone;

    public float Needle_FallenVeloc = -2;

    public float Stone_FallenVeloc = -3;
    public float Stone_First_Hit_Force = -15;
    public float Stone_Second_Hit_Force = -9;

    public float FallenNeedle_To_Next_distance = 2;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < fallenNeedle_count; i++)
        {
            fallenNeedle[i] = GameObject.Find("FallenNeedle" + (i + 1));
            fallenNeedle[i].GetComponent<FallenNeedleScript>().fallen_Veloc= Needle_FallenVeloc;
        }

        for (int i = 0; i < remoteFloor_Count; i++)
        {
            remoteFloor[i] = GameObject.Find("RemoteFloor" + (i + 1));
        }
        remoteFloorSwitch = GameObject.Find("RemoteSwitch");

        fallenStoneSwitch = GameObject.Find("FallenStoneSwitch");

        stone = GameObject.Find("Stone");
    }

    // Update is called once per frame
    void Update()
    {

        //スイッチを踏んだら、岩を移動させる
        if (fallenStoneSwitch.GetComponent<SwitchScript>().trapEnable)
        {
            stone.GetComponent<FallenStoneScript>().fallenEnable = true;
            stone.GetComponent<FallenStoneScript>().
                            SetFallenStone(Stone_FallenVeloc, Stone_First_Hit_Force, Stone_Second_Hit_Force);
        }

        //スイッチを踏んだら、針と床両方を落下させる
        if (remoteFloorSwitch.GetComponent<SwitchScript>().trapEnable)
        {
            //床
            for (int i = 0; i < remoteFloor_Count; i++)
            {
                remoteFloor[i].GetComponent<RemoteFloorScript>().fallenDownEnable = true;
            }
            //矢を降らせる
            fallenNeedle[0].GetComponent<FallenNeedleScript>().trapEnable = true;

            remoteFloorSwitch.GetComponent<SwitchScript>().trapEnable = false;
        }

        FallenByDistance(fallenNeedle_count);
    }
    /// <summary>
    /// これは針を一個ずつ落下させる関数
    /// </summary>
    /// <param name="fallenNeedle_count"></param>
    public void FallenByDistance(int fallenNeedle_count)
    {
        if (!fallenNeedle[fallenNeedle_count - 1].GetComponent<FallenNeedleScript>().trapEnable)
        {
            for (int i = 0; i < fallenNeedle_count; i++)
            {
                if (i != 0)
                {
                    if (fallenNeedle[i].transform.position.y - fallenNeedle[i - 1].transform.position.y > FallenNeedle_To_Next_distance)
                    {
                        fallenNeedle[i].GetComponent<FallenNeedleScript>().trapEnable = true;
                    }
                }
            }
        }
    }

}
