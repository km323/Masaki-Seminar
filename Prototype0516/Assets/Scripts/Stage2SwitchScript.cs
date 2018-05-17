using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// これはステージ２のスイッチ機能クラスです
/// 同時に押されると、トラップが解除され、クリアできるようになる
/// プレイヤーが踏むと、床に嵌る；離れると、元に戻す
/// </summary>
public class Stage2SwitchScript : MonoBehaviour {
    GameObject switch1;
    GameObject switch2;
    
    GameObject fireWall;
    bool trapLiftEnable;
	// Use this for initialization
	void Start () {

        if (SceneManager.GetActiveScene().name == "Stage2")
        {
            switch1 = transform.Find("Switch1").gameObject;
            switch2 = transform.Find("Switch2").gameObject;

            fireWall = GameObject.Find("FireWall");

            fireWall.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
        if(switch1.GetComponent<SwitchScript>().fallenDownFlag)
        {
            if(switch1.transform.localScale.y>=0.02f)
            {
                switch1.transform.localScale -= new Vector3(0, 0.01f, 0);
            }
        }
        else
        {
            if (!trapLiftEnable)
            {
                if (switch1.transform.localScale.y < 0.14f)
                {
                    switch1.transform.localScale += new Vector3(0, 0.01f, 0);
                }
            }
        }

        if (switch2.GetComponent<SwitchScript>().fallenDownFlag)
        {
            if (switch2.transform.localScale.y >= 0.02f)
            {
                switch2.transform.localScale -= new Vector3(0, 0.01f, 0);
            }
        }
        else
        {
            if (!trapLiftEnable)
            {
                if (switch2.transform.localScale.y < 0.14f)
                {
                    switch2.transform.localScale += new Vector3(0, 0.01f, 0);
                }
            }
        }

        if(switch1.GetComponent<SwitchScript>().fallenDownFlag&&
            switch2.GetComponent<SwitchScript>().fallenDownFlag)
        {
            trapLiftEnable = true;
        }

        if (trapLiftEnable)
            fireWall.SetActive(false);
    }
}
