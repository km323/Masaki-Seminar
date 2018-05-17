using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// これはステージ4のスイッチ機能クラスです
/// 1,3,5踏んだっら、岩が落ちる
/// 8踏んだら、床が回転する
/// </summary>
public class Stage4Switch : MonoBehaviour {
    GameObject[] trapSwitch = new GameObject[6];
    GameObject[] stone = new GameObject[3];

    GameObject trapFloor;
	// Use this for initialization
	void Start () {
		for(int i=0;i<6;i++)
        {
            trapSwitch[i] = GameObject.Find("Switch" + (i+1));
        }
        for (int i = 0; i < 3; i++)
        {
            stone[i] = GameObject.Find("Stone" + (i+1));
        }

        trapFloor = GameObject.Find("trapFloor");
    }
	
	// Update is called once per frame
	void Update () {
		if(trapSwitch[0].GetComponent<SwitchScript>().trapEnable)
        {
            stone[0].GetComponent<Rigidbody>().isKinematic = false;
            stone[0].GetComponent<Rigidbody>().AddForce(0, -5, 0);
            trapSwitch[0].GetComponent<SwitchScript>().trapEnable = false;
        }
        if (trapSwitch[2].GetComponent<SwitchScript>().trapEnable)
        {
            stone[1].GetComponent<Rigidbody>().isKinematic = false;
            stone[1].GetComponent<Rigidbody>().AddForce(0, -5, 0);
            trapSwitch[2].GetComponent<SwitchScript>().trapEnable = false;
        }
        if (trapSwitch[4].GetComponent<SwitchScript>().trapEnable)
        {
            stone[2].GetComponent<Rigidbody>().isKinematic = false;
            stone[2].GetComponent<Rigidbody>().AddForce(0, -5, 0);
            trapSwitch[4].GetComponent<SwitchScript>().trapEnable = false;
        }
        if (trapSwitch[5].GetComponent<SwitchScript>().trapEnable)
        {
            trapSwitch[5].transform.Rotate(0, 0, -5);
            trapFloor.transform.Rotate(0, 0, -5);
        }
    }
}
