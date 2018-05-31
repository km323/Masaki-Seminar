using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// これはReplay機能のクラスです
/// </summary>

public class ReplayScript : MonoBehaviour
{
    #region フィールド
    //ねっぽ
    public string nowStageNum;    // 今のステージナンバー

    public float p_NowTimeCount
    {
        get { return now_timeCount; }
    }
    float now_timeCount;

    //これは前回のプレイ時間を記録するカウンターです
    float lastDeathCount;


    //参照
    GameObject player;
    GameObject ghost;
    GameObject ghostBody;
    GameObject Eff_Disapear;

    //プレハブ
    public GameObject ghostLatern;

    //コンポーネント
    ParticleSystem ParSys_Disapear;

    /// <summary>
    /// REPLAY機能
    /// </summary>
    bool doReplay;

    bool setReplayValue; //一回だけ値をセットするフラグ

    float now_Replay_Count;

    bool set_Ghost_Latern_Enable;

    bool ghostLaternLight_Disapear;

    bool eff_Playable;

    #region プレイヤーの情報を再保存＆＆取り出す配列
    //プレイヤー情報を保存する配列
    public const int MAX_DATA_STORGE_COUNT = 5000;

    int now_Storge_Count;

    //移動情報を再代入する配列
    public float[] velocitys = new float[MAX_DATA_STORGE_COUNT];
    public bool[] jumps = new bool[MAX_DATA_STORGE_COUNT];
    public float[] times = new float[MAX_DATA_STORGE_COUNT];

    //移動情報を取り出す配列
    float[] new_velocitys = new float[MAX_DATA_STORGE_COUNT];
    bool[] new_jumps = new bool[MAX_DATA_STORGE_COUNT];
    float[] new_times = new float[MAX_DATA_STORGE_COUNT];
    #endregion

    #endregion

    #region 初期化
    void Start()
    {
        InitializeObj();

        InitializeFlag();

        InitializeTime();

        InitializeStorgeDataArray();
    }

    #region 初期化関数
    void InitializeObj()
    {
        player = GameObject.Find("Player");
        ghost = GameObject.Find("Ghost");
        ghost.SetActive(false);
        Eff_Disapear = ghost.transform.Find("Eff_Disapear").Find("Disapear").gameObject;

        ParSys_Disapear = Eff_Disapear.GetComponent<ParticleSystem>();
    }
    void InitializeFlag()
    {
        setReplayValue = true;
        doReplay = PlayerPrefsX.GetBool("ReplayFlag");
        eff_Playable = true;
    }
    void InitializeTime()
    {
        now_timeCount = 0;
        now_Replay_Count = 0;
    }
    void InitializeStorgeDataArray()
    {
        for (int i = 0; i < MAX_DATA_STORGE_COUNT; i++)
        {
            velocitys[i] = new float();
            jumps[i] = new bool();
            times[i] = new float();

            new_velocitys[i] = new float();
            new_jumps[i] = new bool();
            new_times[i] = new float();
        }
    }
    #endregion

    #endregion

    private void Update()
    {
        //ねっぽ　ここから
        //現在の時刻を記録
        if (Time.timeSinceLevelLoad < 0)
            return;
        now_timeCount = Time.timeSinceLevelLoad;
        //ねっぽ　ここまで

        CheckPlayerLife();

        if (ghostLaternLight_Disapear)
        {
            GhostLatternLightDisapear();
        }
    }

    #region　Fixed更新
    // Update is called once per frame
    void FixedUpdate()
    {
        if (doReplay)
        {
            DoReplay();
        }


    }
    #endregion

    #region 関数

    public void GhostLatternLightDisapear()
    {
        GameObject ghost_Latern = GameObject.Find("GhostLatern 1(Clone)");
        Light ghost_Latern_Light = ghost_Latern.transform.Find("Area Light").GetComponent<Light>();
        if (ghost_Latern_Light.intensity > 0)
        {
            ghost_Latern_Light.intensity -= 0.025f;
        }

    }


    /// <summary>
    /// これはライフ数が０になったときの処理メソッド
    /// </summary>
    public void CheckPlayerLife()
    {
        if (player.GetComponent<PlayerLifeControl>().lifeCount <= 0)
        {
            //移動情報をplayerPrefsXに保存
            PlayerPrefsX.SetFloatArray("Velocitys", velocitys);
            PlayerPrefsX.SetFloatArray("Times", times);
            PlayerPrefsX.SetBoolArray("jumps", jumps);
            PlayerPrefs.SetInt("Count", player.GetComponent<PlayerControl>().dataList.Count);

            //Playerprefsxの中のreplayFlagをon
            doReplay = true;
            PlayerPrefsX.SetBool("ReplayFlag", doReplay);

            //前回の死亡時間を記録
            lastDeathCount = player.GetComponent<PlayerControl>().add_Data_Count;
            PlayerPrefs.SetFloat("lastDeathCount", lastDeathCount);

            //シーンをロード
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    /// <summary>
    /// これはリプレイ時にゴーストを再生させるメソッド
    /// </summary>
    public void DoReplay()
    {
        //一回だけ値を保存する
        if (setReplayValue)
        {

            //新しい配列に入れる
            new_velocitys = PlayerPrefsX.GetFloatArray("Velocitys");
            new_times = PlayerPrefsX.GetFloatArray("Times");
            new_jumps = PlayerPrefsX.GetBoolArray("jumps");

            //カウンタを取得
            lastDeathCount = PlayerPrefs.GetFloat("lastDeathCount");
            now_Storge_Count = PlayerPrefs.GetInt("Count");

            ghost.SetActive(true);
            ghostBody = GameObject.Find("character_ghost");
            ghostBody.SetActive(true);

            set_Ghost_Latern_Enable = true;
            setReplayValue = false;
        }
        //配列内の要素をチェックする
        //現在の時間と一致したら、その情報量をゴーストに代入する
        for (int i = 0; i < now_Storge_Count; i++)
        {
            if (now_timeCount - new_times[i] >= Time.deltaTime)
            {
                MoveGhost(i);
            }
        }

        //リプレイカウンタを増やす
        now_Replay_Count++;


        //前回のプレイ時間になったら、関数を抜ける
        if (now_Replay_Count - lastDeathCount >= 0.1)
        {
            doReplay = false;
            PlayerPrefsX.SetBool("ReplayFlag", doReplay);
        }

        if (!doReplay)
        {
            //移動を停止
            ghost.GetComponent<GhostControl>().g_VeclocityX = 0;
            ghost.GetComponent<GhostControl>().g_duringJump = false;
            //エフェクト位置をゴースト現在位置まで移動
            Eff_Disapear.transform.position = ghost.transform.position;

            //エフェクト再生
            if (eff_Playable)
            {
                ParSys_Disapear.Play();
                eff_Playable = false;
            }

            ghostBody.SetActive(false);

            //死んだところに、Latternを残す
            if (set_Ghost_Latern_Enable)
            {
                Instantiate(ghostLatern, ghost.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                ghostLaternLight_Disapear = true;
                set_Ghost_Latern_Enable = false;
            }


        }
    }

    /// <summary>
    /// これはゴーストを移動させるメソッド
    /// </summary>
    /// <param name="i"></param>
    public void MoveGhost(int i)
    {
        ghost.GetComponent<GhostControl>().g_VeclocityX = new_velocitys[i];
        ghost.GetComponent<GhostControl>().g_duringJump = new_jumps[i];

    }

    /// <summary>
    /// これはプレイヤーかゴーストがOutAreaに入ったときの処理
    /// </summary>
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーのライフ数をゼロまで減らせる
            player.GetComponent<PlayerLifeControl>().lifeCount = 0;
        }
    }
    #endregion
}