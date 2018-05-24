using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// これはトラップスイッチをコントロールするクラスです。
/// </summary>
public class SwitchScript : MonoBehaviour
{
    // Use this for initialization
    public bool trapEnable = false;
    public bool ghostTrapVisible=false;

    void Start()
    {
        if(tag== "GhostSwitch")
        ghostTrapVisible = PlayerPrefsX.GetBool("ghostTrapVisible" + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (tag == "GhostSwitch")
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
            PlayerPrefsX.SetBool("ghostTrapVisible" + gameObject.name.Insert(0,"Ghost"), ghostTrapVisible);
            trapEnable = true;
            //プレイヤーがスイッチを踏んだら、スイッチの色を赤に変える。
            GetComponent<Renderer>().material.color = Color.red;
        }

    }
}
