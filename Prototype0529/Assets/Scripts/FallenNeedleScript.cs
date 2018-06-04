using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// これは落ちる針をコントロールするクラスです
/// </summary>
public class FallenNeedleScript : MonoBehaviour
{
    [HideInInspector]
    public bool trapEnable;

    public float fallen_Veloc;
    
    
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (trapEnable)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(0, fallen_Veloc, 0);
            trapEnable = false;
        }
    }
}
