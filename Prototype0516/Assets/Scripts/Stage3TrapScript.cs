using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// これはステージ3のスイッチ機能クラスです
/// 踏んだっら、踏み台が上がる
/// </summary>
public class Stage3TrapScript : MonoBehaviour
{
    GameObject switch1;

    GameObject springBoard;

    bool trapLiftEnable;
    // Use this for initialization
    void Start()
    {
        springBoard = GameObject.Find("Springboard");
        switch1 = transform.Find("Switch1").gameObject;
        //trapLiftEnable = PlayerPrefsX.GetBool("trapLiftEnable");
    }

    // Update is called once per frame
    void Update()
    {
        if(trapLiftEnable)
        {
            if(springBoard.transform.localScale.y<=5.0f)
            {
                springBoard.transform.localScale += new Vector3(0, 0.05f, 0);
            }
            if (springBoard.transform.position.y <= 0.8f)
            {
                springBoard.transform.position += new Vector3(0, 0.05f, 0);
            }
        }


        if (switch1.GetComponent<SwitchScript>().fallenDownFlag)
        {
            if (switch1.transform.localScale.y >= 0.02f)
            {
                //PlayerPrefsX.SetBool("trapLiftEnable", trapLiftEnable);
                trapLiftEnable = true;
                switch1.transform.localScale -= new Vector3(0, 0.01f, 0);
            }
        }
        else
        {
            if (switch1.transform.localScale.y < 0.14f)
            {
                switch1.transform.localScale += new Vector3(0, 0.01f, 0);
            }
        }
    }
}
