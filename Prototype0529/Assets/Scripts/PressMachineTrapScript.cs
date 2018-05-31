using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// これはプレス機の罠をコントロールする関数
/// </summary>
public class PressMachineTrapScript : MonoBehaviour
{
    //参照
    GameObject parentObj;
    GameObject eff_Disapear;


    bool effPlayable = true;
    public bool ghost_Trap_Visible=false;

    // Use this for initialization
    void Start()
    {
        parentObj = transform.root.gameObject;

        if (tag == "GhostTrap")
        {
            ghost_Trap_Visible = PlayerPrefsX.GetBool("Visible" + gameObject.name);
        }
        eff_Disapear = GameObject.Find("Disapear");
    }

    // Update is called once per frame
    void Update()
    {
        if (tag == "GhostTrap")
        {
            if (ghost_Trap_Visible)
            {
                //子オブジェのRenderコンポーネントをTrueにする
                foreach (Transform child in parentObj.transform)
                {
                    if (child.GetComponent<Renderer>() != null)
                        child.GetComponent<Renderer>().enabled = true;
                    foreach (Transform secondChild in child)
                    {
                        if (secondChild.GetComponent<Renderer>() != null)
                            secondChild.GetComponent<Renderer>().enabled = true;
                    }
                }

            }
            else
            {
                //子オブジェのRenderコンポーネントをFalseにする
                foreach (Transform child in parentObj.transform)
                {
                    if (child.GetComponent<Renderer>() != null)
                        child.GetComponent<Renderer>().enabled = false;
                    foreach (Transform secondChild in child)
                    {
                        if (secondChild.GetComponent<Renderer>() != null)
                            secondChild.GetComponent<Renderer>().enabled = false;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //ghost_Trap_Visible = true;
            //PlayerPrefsX.SetBool("Visible" + gameObject.name.Insert(0, "Ghost"), ghost_Trap_Visible);
        }
        if (collision.gameObject.name == "Ghost")
        {
            if (tag == "GhostTrap")
            {
                //消失のエフェクトを再生
                if (effPlayable)
                {
                    eff_Disapear.transform.position = transform.position;
                    eff_Disapear.GetComponent<ParticleSystem>().Play();
                    effPlayable = false;
                }
                ghost_Trap_Visible = false;
                PlayerPrefsX.SetBool("Visible" + gameObject.name, ghost_Trap_Visible);
            }
        }

    }
}
