using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteFloorScript : MonoBehaviour
{
    public bool ghostTrapVisible = true;
    public bool fallenDownEnable = false;
    // Use this for initialization
    void Start()
    {
        if (tag == "GhostFloor")
        {
            ghostTrapVisible = PlayerPrefsX.GetBool("ghostTrapVisible" + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tag == "GhostFloor")
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
        if (fallenDownEnable)
        {
            if (name == "RemoteFloor2" || name == "GhostRemoteFloor2")
            {
                FallenDown(-1.43f);
            }
            if (name == "RemoteFloor1" || name == "GhostRemoteFloor1")
            {
                FallenDown(0.58f);
            }
        }

    }

    void FallenDown(float fallDistance_Y)
    {
        if (transform.localPosition.y - fallDistance_Y > 0.1f)
        {
            transform.localPosition -= new Vector3(0, 0.01f, 0);
        }
        else
        {
            if (tag == "GhostFloor")
            {

                transform.Find("Disapear").GetComponent<ParticleSystem>().Play();
                GetComponent<Renderer>().enabled = false;
                ghostTrapVisible = false;
                PlayerPrefsX.SetBool("ghostTrapVisible" + gameObject.name, ghostTrapVisible);
            }
            fallenDownEnable = false;
        }
    }
}
