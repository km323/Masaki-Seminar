using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// これはカメラを移動するクラス
/// カメラの注目点であるターゲットが必要
/// </summary>
public class CameraColltroller : MonoBehaviour {
    public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //カメラの位置を決める
        Camera.main.transform.position = target.transform.position + new Vector3(-2.36f,1.6f, 0);
	}
}
