using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// これはUI全般をコントロールするクラスです
/// UI_TEXT二つが必要です。
/// </summary>
public class UIScript : MonoBehaviour {
    public Text lightSwitch_Text;
    public Text playerlife_Text;
    
    private GameObject worldLight;
    private GameObject player;

    private bool turnLightOn;
    // Use this for initialization
    void Start () {
        turnLightOn = false;

        worldLight = GameObject.FindGameObjectWithTag("MainLight");

        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        //ライフ数をテキストで表示
        playerlife_Text.text = "Life:1 " + player.GetComponent<PlayerLifeControl>().lifeCount.ToString();
        //Debug.Log(playerlife_Text.text);
        //turnlightonフラグを頼って、ワールドライトの輝度を調整する
        if (!turnLightOn)
        {
            lightSwitch_Text.text = "LightON(Num1)";
            worldLight.GetComponent<Light>().intensity = 0.01f;
            worldLight.GetComponent<Light>().color = Color.black;
        }
        else
        {
            lightSwitch_Text.text = "LightOFF(Num1)";
            worldLight.GetComponent<Light>().intensity = 2f;
            worldLight.GetComponent<Light>().color = Color.white;
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha1)||Input.GetButtonDown("LightOn"))
        {
            turnLightOn = !turnLightOn;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)||Input.GetButtonDown("Retry"))
        {
            player.GetComponent<PlayerLifeControl>().lifeCount = 0;
        }

    }

    /// <summary>
    /// これはLIGHT ONボタンでコントロールする関数です
    /// 呼び出したら、輝度を反転させる；
    /// </summary>
    public void ChangeTheLightIndensity()
    {
        turnLightOn = !turnLightOn;
    }

    /// <summary>
    /// これはリトライボタンでコントロールする関数です
    /// 呼び出したら、プレイヤーのライフ数を減らすをゼロまで減らす；
    /// </summary>
    public void RetryFunction()
    {
        player.GetComponent<PlayerLifeControl>().lifeCount=0;
    }
}
