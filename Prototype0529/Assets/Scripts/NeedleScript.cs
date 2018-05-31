using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// これは針をコントロールするクラス
/// </summary>
public class NeedleScript : MonoBehaviour {
    public bool hit;
    public bool hitGhost;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            hit = true;
        }
        if(collision.gameObject.name=="Ghost")
        {
            hitGhost = true;
        }
    }
}
