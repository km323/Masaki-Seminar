using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public bool isGrounded;
    // Use this for initialization
    void Start()
    {
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGrounded)
        Debug.Log("IsG!"+isGrounded+"\n");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }
    }
}
