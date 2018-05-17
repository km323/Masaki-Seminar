using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorAnimation : MonoBehaviour {
    Animator door;

	// Use this for initialization
	void Start () {
        door = GameObject.Find("Door").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            door.SetTrigger ("start");
        }
    }
}
