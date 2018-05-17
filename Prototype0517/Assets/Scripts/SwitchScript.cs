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
    GameObject stone;
    GameObject ghostStone;
    public bool fallenDownFlag = false;
    public bool trapEnable = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Stage2" ||
            SceneManager.GetActiveScene().name == "Stage1")
        {
            ghostStone = GameObject.Find("GhostStone");
            stone = GameObject.Find("Stone");

            //ghostStone.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter(Collision collision)
    {
        #region Stage1
        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            if (collision.gameObject.name == "Player")
            {

                //プレイヤーがスイッチを踏んだら、岩を落下させ、スイッチの色を赤に変える。
                if (this.GetComponent<Renderer>().material.color.r < 20)
                {
                    GetComponent<Renderer>().material.color += new Color(1, 0, 0);
                    stone.GetComponent<Rigidbody>().isKinematic = false;
                    stone.GetComponent<Rigidbody>().AddForce(0, -10, 0);
                }
            }

            if(gameObject.name=="GhostSwitch"&&collision.gameObject.name=="Ghost")
            {
                //ゴーストがスイッチを踏んだら、ゴースト岩を落下させる。
                ghostStone.SetActive(true);
                ghostStone.GetComponent<Rigidbody>().isKinematic = false;
                ghostStone.GetComponent<Rigidbody>().AddForce(0, -10, 0);
                
            }
        }
        #endregion

        #region Stage2||Stage3
        if (SceneManager.GetActiveScene().name == "Stage2" ||
            SceneManager.GetActiveScene().name == "Stage3")
        {
            if (collision.gameObject.name == "Player")
            {
                fallenDownFlag = true;
            }
            if (collision.gameObject.name == "Ghost")
            {
                fallenDownFlag = true;
            }
        }
        #endregion

        #region Stage4
        if (SceneManager.GetActiveScene().name == "Stage4")
        {
            if(collision.gameObject.name=="Player")
            {
                switch(gameObject.name)
                {
                    case "Switch1":
                        gameObject.GetComponent<Renderer>().material.color = Color.red;
                        trapEnable = true;
                        break;
                    case "Switch3":
                        gameObject.GetComponent<Renderer>().material.color = Color.red;
                        trapEnable = true;
                        break;
                    case "Switch5":
                        gameObject.GetComponent<Renderer>().material.color = Color.red;
                        trapEnable = true;
                        break;
                    case "Switch6":
                        gameObject.GetComponent<Renderer>().material.color = Color.red;
                        trapEnable = true;
                        break;
                    case "Switch2":
                    case "Switch4":
                        gameObject.GetComponent<Renderer>().material.color = Color.green;
                        break;
                }
            }
        }
        #endregion
    }
    private void OnCollisionStay(Collision collision)
    {
        if (SceneManager.GetActiveScene().name == "Stage2" ||
            SceneManager.GetActiveScene().name == "Stage3")
        {
            if (collision.gameObject.name == "Player")
            {
                fallenDownFlag = true;
            }

            if (collision.gameObject.name == "Ghost")
            {
                fallenDownFlag = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (SceneManager.GetActiveScene().name == "Stage2" ||
            SceneManager.GetActiveScene().name == "Stage3")
        {
            if (collision.gameObject.name == "Player")
            {
                fallenDownFlag = false;
            }

            if (collision.gameObject.name == "Ghost")
            {
                fallenDownFlag = false;
            }
        }
    }
}
