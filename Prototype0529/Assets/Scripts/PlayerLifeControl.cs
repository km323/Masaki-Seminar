using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// これはプレイヤーのライフをコントロールするクラス
/// </summary>
public class PlayerLifeControl : MonoBehaviour {

    public int lifeCount;

	// Use this for initialization
	void Start () {
        //ライフ初期化
        lifeCount = 1;
	}
	// Update is called once per frame
	void Update () {

        //キーボードLでライフ数を減らす
        if(Input.GetKeyDown(KeyCode.L))
        {
            lifeCount--;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //tag名が”トラップ”のオブジェと遭遇したら、ライフ数を減らす
        if (collision.gameObject.tag == "Trap")
        {
            lifeCount--;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //tag名が”トラップ”のオブジェと遭遇したら、ライフ数を減らす
        if (other.gameObject.tag == "Trap")
        {
            lifeCount--;
        }
    }
}
