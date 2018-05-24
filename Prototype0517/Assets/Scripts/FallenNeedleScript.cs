using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// これは落ちる針をコントロールするクラスです
/// UI_TEXT二つが必要です。
/// </summary>
public class FallenNeedleScript : MonoBehaviour
{
    public bool trapEnable;
    public bool ghostTrapVisible;
    public int fallenCount;
    // Use this for initialization
    void Start()
    {
        if (tag == "GhostTrap")
        {
            string num = gameObject.name.Remove(0, gameObject.name.Length - 1);
            ghostTrapVisible = PlayerPrefsX.GetBool("ghostTrapVisible"+num);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (trapEnable)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            trapEnable = false;
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
            string num = gameObject.name.Remove(0, gameObject.name.Length-1);
            ghostTrapVisible = true;
            PlayerPrefsX.SetBool("ghostTrapVisible"+num, ghostTrapVisible);
        }

        if (collision.gameObject.name == "OutArea")
        {
            Destroy(this);
        }
    }
}
