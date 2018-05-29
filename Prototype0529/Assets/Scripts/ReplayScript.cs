using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// これはReplay機能のクラスです
/// </summary>


enum PlayerState
{
    Move,
    Idle,
}
public class ReplayScript : MonoBehaviour
{
    #region フィールド
    /// <summary>
    /// 時間
    /// </summary>
    //これは現在の時間を記録するカウンターです
    //シーンが再ロードされても、0からスタートする
    float now_timeCount;

    //ねっぽ　ここから
    /// <summary>
    /// 今のステージナンバー
    /// </summary>
    public string nowStageNum;

    public float p_NowTimeCount
    {
        get { return now_timeCount; }
    }

    //これは前回のプレイ時間を記録するカウンターです
    float lastDeathCount;
    //ねっぽ　ここまで


    /// <summary>
    ///プレイヤーオブジェ
    /// </summary>
    GameObject player;

    /// <summary>
    /// ゴーストオブジェ
    /// </summary>
    GameObject ghost;
    GameObject ghostBody;
    GameObject ghostHead;
    bool ghostExist_Flag;

    /// <summary>
    /// ゴーストが消えるエフェクト
    /// </summary>
    GameObject Eff_Disapear;
    bool setGhostLaternEnable;
    public GameObject ghostLatern;
    ParticleSystem ParSys_Disapear;
    bool eff_Playable;

    /// <summary>
    /// REPLAY機能
    /// </summary>
    bool doReplay;

    //REPLAYするときに、一回だけ値をセットするフラグ
    bool setReplayValue;
    //REPLAYするした時間
    float replayCount;
    bool deadForZeroLife;
    Vector3 death_Pos;

    #region プレイヤーの情報を再保存＆＆取り出す配列
    //プレイヤー情報を保存する配列
    public const int MAXTIME = 5000;

    int arrayCount;
    //velocity
    public float[] velocitys = new float[MAXTIME];
    //horizontal
    public float[] horizontals = new float[MAXTIME];
    //m_jump
    public bool[] jumps = new bool[MAXTIME];
    //Time
    public float[] times = new float[MAXTIME];

    //h
    float[] new_velocitys = new float[MAXTIME];
    //horizontal
    float[] new_horizontals = new float[MAXTIME];
    //m_jump
    bool[] new_jumps = new bool[MAXTIME];
    //Time
    float[] new_times = new float[MAXTIME];
    #endregion

    #endregion

    #region 初期化
    void Start()
    {
        InitializeObj();
        InitializeFlag();
        InitializeTime();
        InitializeOtherValue();

        InitializePlayerDataArray();
    }

    #region 初期化関数
    void InitializeObj()
    {
        Eff_Disapear = GameObject.Find("Disapear");
        player = GameObject.Find("Player");
        ghost = GameObject.Find("Ghost");
        ghostBody = GameObject.FindGameObjectWithTag("GhostBody");
        ghostHead = GameObject.FindGameObjectWithTag("GhostHead");
        ParSys_Disapear = Eff_Disapear.GetComponent<ParticleSystem>();
        ParSys_Disapear.Stop();
    }
    void InitializeFlag()
    {
        ghostExist_Flag = false;
        deadForZeroLife = false;
        setReplayValue = true;
        doReplay = false;
        eff_Playable = true;
        deadForZeroLife = PlayerPrefsX.GetBool("deadForZeroLife");
    }
    void InitializeTime()
    {
        now_timeCount = 0;
    }
    void InitializeOtherValue()
    {
        replayCount = 0;
        //nowStageNum = "1";
        Time.fixedDeltaTime = 0.016f;
        death_Pos = PlayerPrefsX.GetVector3("DeathPos");
        ghost.SetActive(false);
    }
    void InitializePlayerDataArray()
    {
        for (int i = 0; i < MAXTIME; i++)
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
    }

    #region　Fixed更新
    // Update is called once per frame
    void FixedUpdate()
    {

        #region Replay
        //REPLAYするかどうかをチェック
        doReplay = PlayerPrefsX.GetBool("ReplayFlag");

        if (doReplay)
        {
            DoReplay();

        }
        #endregion



    }
    #endregion

    #region 関数
    /// <summary>
    /// これはライフ数が０になったときの処理メソッド
    /// </summary>
    public void CheckPlayerLife()
    {
        if (player.GetComponent<PlayerLifeControl>().lifeCount <= 0)
        {
            //移動情報をplayerPrefsXに保存
            PlayerPrefsX.SetFloatArray("Velocitys", velocitys);
            PlayerPrefsX.SetFloatArray("Horizontals", horizontals);
            PlayerPrefsX.SetFloatArray("Times", times);
            PlayerPrefsX.SetBoolArray("jumps", jumps);
            PlayerPrefs.SetInt("Count", player.GetComponent<PlayerControl>().dataList.Count);

            //死亡位置を保存
            death_Pos = player.transform.position;
            PlayerPrefsX.SetVector3("DeathPos", death_Pos);

            //Playerprefsxの中のreplayFlagをon
            doReplay = true;
            PlayerPrefsX.SetBool("ReplayFlag", doReplay);

            //特定の位置で消滅させるフラグをオンにする
            deadForZeroLife = true;
            PlayerPrefsX.SetBool("deadForZeroLife", deadForZeroLife);

            //前回の死亡時間を記録
            lastDeathCount = player.GetComponent<PlayerControl>().addCount;
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
            setGhostLaternEnable = true;
            //リプレイするとき取り出す
            ghostExist_Flag = true;
            //新しい配列に入れる
            new_velocitys = PlayerPrefsX.GetFloatArray("Velocitys");
            new_horizontals = PlayerPrefsX.GetFloatArray("Horizontals");
            new_times = PlayerPrefsX.GetFloatArray("Times");
            new_jumps = PlayerPrefsX.GetBoolArray("jumps");
            arrayCount = PlayerPrefs.GetInt("Count");
            //前回のプレイ時間を取得
            lastDeathCount = PlayerPrefs.GetFloat("lastDeathCount");
            ghost.SetActive(true);
            ghostBody.SetActive(true);
            //ghostHead.SetActive(true);
            setReplayValue = false;
        }
        //配列内の要素をチェックする
        //現在の時間と一致したら、その情報量をゴーストに代入する
        for (int i = 0; i < arrayCount; i++)
        {
            if (now_timeCount - new_times[i] >= Time.deltaTime)
            {
                MoveGhost(i);

                ghost.GetComponent<TrailRenderer>().time += Time.time;
            }
        }
        replayCount++;

        if (replayCount - lastDeathCount >= 0.1)
        {
            doReplay = false;
            PlayerPrefsX.SetBool("ReplayFlag", doReplay);
        }

        if (!doReplay)
        {
            //移動を停止
            ghost.GetComponent<GhostControl>().g_VeclocityX = 0;
            ghost.GetComponent<GhostControl>().g_VeclocityY = 0;
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
            //ghostHead.SetActive(false);

            if (!ParSys_Disapear.isPlaying)
                ParSys_Disapear.Stop();
            if (setGhostLaternEnable)
            {
                Instantiate(ghostLatern, ghost.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                setGhostLaternEnable = false;
            }
        }
        //Debug.Log("replayCount" + replayCount);
    }

    /// <summary>
    /// これはゴーストを移動させるメソッド
    /// </summary>
    /// <param name="i"></param>
    public void MoveGhost(int i)
    {
        if (ghostExist_Flag)
        {
            ghost.GetComponent<GhostControl>().g_VeclocityX = new_velocitys[i];
            ghost.GetComponent<GhostControl>().g_VeclocityY = new_horizontals[i];
            ghost.GetComponent<GhostControl>().g_duringJump = new_jumps[i];
        }
    }

    /// <summary>
    /// これはプレイヤーかゴーストが範囲外にいるときの処理メソッドです
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーのライフ数をゼロまで減らせる
            player.GetComponent<PlayerLifeControl>().lifeCount = 0;
        }
        if (collision.gameObject.tag == "Ghost")
        {
            //移動を停止
            ghost.GetComponent<GhostControl>().g_VeclocityX = 0;
            ghost.GetComponent<GhostControl>().g_duringJump = false;

            ghostBody.SetActive(false);
            //ghostHead.SetActive(false);

            doReplay = false;
            PlayerPrefsX.SetBool("ReplayFlag", doReplay);
        }
    }
    #endregion
}