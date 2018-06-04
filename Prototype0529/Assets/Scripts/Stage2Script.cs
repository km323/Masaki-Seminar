using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Script : MonoBehaviour {
    GameObject FallenTrap;
    GameObject FallenTrap2;

    GameObject GhostFallenTrap;
    GameObject GhostFallenTrap2;


    bool fallenEnable2;

    public float fallenSpeed=0.5f;
    public float raiseSpeed=0.1f;

    public float distanceToTop = 5.5f;
    public float distanceToBottom = 1;

    public float fallenSpeed2 = 0.5f;
    public float raiseSpeed2 = 0.5f;

    public float distanceToTop2 = 5.5f;
    public float distanceToBottom2 = 1;
    // Use this for initialization
    void Start () {
        FallenTrap = GameObject.Find("FallenTrapSet");
        FallenTrap2 = GameObject.Find("FallenTrapSet2");

    }

    // Update is called once per frame
    void Update()
    {
        FallenTrap.GetComponent<PressMachineTrapScript>().
            SetTwoWaysTrap(distanceToTop, distanceToBottom, fallenSpeed, raiseSpeed);

        FallenTrap2.GetComponent<PressMachineTrapScript>().
            SetTwoWaysTrap(distanceToTop2, distanceToBottom2, fallenSpeed2, raiseSpeed2);
        
    }
}
