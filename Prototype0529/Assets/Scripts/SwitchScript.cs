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

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            trapEnable = true;
            //プレイヤーがスイッチを踏んだら、スイッチの色を赤に変える。
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
