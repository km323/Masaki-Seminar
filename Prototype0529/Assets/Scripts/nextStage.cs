using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextStage : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
    }

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player") {
            //一秒後に実行
			Invoke ("LoadNextScene",0.2f);
		}
	}

	void LoadNextScene()
	{
        //OutAreaゲームオブジェクトにあるシーンナンバーをもとに、次のステージを決める
        GameObject OutArea = GameObject.Find("OutArea");
        int i_nowStage = int.Parse(OutArea.GetComponent<ReplayScript>().nowStageNum);
        string nextStage = (i_nowStage + 1).ToString();

        //セーブデータの初期化
        PlayerPrefs.DeleteAll();

        //ロードシーン
        SceneManager.LoadScene ("Stage" + nextStage);
    }
}
