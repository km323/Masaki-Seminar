using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// これは落石を移動させるクラスです
/// </summary>
public class StoneScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

        if (SceneManager.GetActiveScene().name == "Stage3")
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(-400, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.one.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            //床に落ちたら、左へのパワーを与える
            if (collision.gameObject.name == "Floor3")
            {
                transform.GetComponent<Rigidbody>().AddForce(-8, 0, 0);
            }

            if (collision.gameObject.name == "OutArea")
            {
                GetComponent<Renderer>().enabled=false;
            }

            if(gameObject.tag=="GhostTrap"&&collision.gameObject.name=="Ghost")
            {
                collision.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
        if (SceneManager.GetActiveScene().name == "Stage3")
        {
            if (collision.gameObject.name == "Player")
            {
                if (GetComponent<Rigidbody>().velocity.magnitude >= 2)
                    collision.gameObject.GetComponent<PlayerLifeControl>().lifeCount=0;
            }
        }
    }
}
