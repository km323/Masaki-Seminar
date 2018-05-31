using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// これはプレイヤーを移動させる、プレイヤーの情報を記録するクラス
/// </summary>

/// <summary>
/// これは方向をコントロールする列挙体
/// </summary>
public enum Direction
{
    Right,
    Left,
    Up,
}
/// <summary>
/// これは移動状態をコントロールする列挙体
/// </summary>
public enum PlayerState
{
    Move,
    Idle,
}
/// <summary>
/// これはプレイヤーのデータを保存する列挙体
/// </summary>
public class Data
{
    public bool m_Jump;
    public float v;
    public float time;
}
// 必要なコンポーネントの列記
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class PlayerControl : MonoBehaviour
{
    //参照
    GameObject outArea;

    /// <summary>
    ///情報保存用
    /// </summary>
    public List<Data> dataList = new List<Data>();// プレイヤー情報を一時保存するList動的配列

    public int add_Data_Count;
   
    public const int MAX_DATA_STORGE_COUNT = 5000;

    float idle_Time;

    PlayerState player_NowState;

    //アニメーション関連
    public float animSpeed=1.5f;
    bool duringRun = false;

    //移動関連
    public float Speed;
    [HideInInspector]
    public float g_VeclocityX;
    Vector3 velocity;

    //方向変更用
    Direction playerDirection;
    bool turnOverEnable = false;

    // ジャンプ用
    public float jumpPower;
    [HideInInspector]
    public bool g_duringJump = false;

    //コンポーネント
    Rigidbody rb;
    Animator anim;

    /// <summary>
    /// 接地判定
    /// <summary>
    Ray ray;
    bool isGround = false;
    public const float DISTANCE_TO_GROUND = 0.15f;


    // 初期化
    void Start()
    {
        //参照
        outArea = GameObject.Find("OutArea");

        //カウンター
        add_Data_Count = 0;
        idle_Time = 0;

        //プロパティ
        playerDirection = Direction.Right;

        //コンポーネント
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
//ねっぽ
//クリア時、操作禁止
        if (!DoorControl.canControlPlayer)
            return;

        //アニメーション設定
        anim.SetBool("Jump", false);
        anim.SetBool("Run", duringRun);
        anim.speed = animSpeed;

        //接地判定
        CheckisGrounded();

        //入力
        g_VeclocityX = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
                g_duringJump = true;
                anim.SetBool("Jump", true);
            }
        }

        //移動判定
        if (g_VeclocityX > 0 || g_VeclocityX < 0)
        {
            duringRun = true;
        }
        else
        {
            duringRun = false;
        }
        

    }

    // 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
    void FixedUpdate()
    {
//ねっぽ
//クリア時方向を上に向かわせる
        if (!DoorControl.canControlPlayer)
        {
                transform.eulerAngles = new Vector3(0, 0, 0);
                transform.localPosition += Vector3.forward * Time.fixedDeltaTime;
            return;
        }
        
        #region 情報保存
        //プレイヤーが移動しているかどうかをチェック
        if (player_NowState == PlayerState.Move||g_duringJump)
        {
            //もし保存できる最大値以下だったら
            //フレームごとに、プレイヤーの情報を保存する
            if (dataList.Count < MAX_DATA_STORGE_COUNT)
            {
                dataList.Add(GetData());
            }
            else
            {
                Debug.Log("MaxWarining");
            }
        }
        //保存用の配列に値を再代入する
        for (int i = 0; i < dataList.Count; i++)
        {
            outArea.GetComponent<ReplayScript>().velocitys[i] = dataList[i].v;
            outArea.GetComponent<ReplayScript>().jumps[i] = dataList[i].m_Jump;
            outArea.GetComponent<ReplayScript>().times[i] = dataList[i].time;
        }
        if (player_NowState == PlayerState.Idle)
        {
            //止まったら、移動するまでの時間を記録する。
            idle_Time += Time.deltaTime;
        }
        #endregion

        CheckPlayerIdleState();

        ChangeDirection();

        MoveFunction();

        JumpFunction();
       
    }

    /// <summary>
    /// これは移動をコントロールする関数
    /// g_VeclocityXを頼って、横に移動させている
    /// </summary>
    void MoveFunction()
    {
        // 以下、キャラクターの移動処理
        velocity = new Vector3(g_VeclocityX, 0, 0);        // 左右のキー入力からX軸方向の移動量を取得
        velocity *= Speed;       // 移動速度を掛ける
        transform.localPosition += velocity * Time.fixedDeltaTime;        // 左右のキー入力でキャラクターを移動させる

    }

    /// <summary>
    /// これはジャンプをコントロールする関数
    /// g_duringJumpがtrueの時、上に向く力を与える,それ以外の場合は重力を与える
    /// </summary>
    void JumpFunction()
    {
        if (g_duringJump)
        {
            // スペースキーを入力したら
            rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            g_duringJump = false;
        }
        else
        {
            rb.velocity += Physics.gravity * Time.deltaTime;
        }
    }

    /// <summary>
    /// これは接地判定用の関数
    /// ↓に向く射線を発射させ、何かとぶつかったら、接地とみなす
    /// </summary>
    void CheckisGrounded()
    {
        RaycastHit hit;
        ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, DISTANCE_TO_GROUND))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        Debug.DrawLine(ray.origin, ray.origin - new Vector3(0, DISTANCE_TO_GROUND, 0), Color.red, 0.1f);
    }

    /// <summary>
    /// これは方向を変える関数
    /// HACK:方向を変える速度を編集できるようにする
    /// </summary>
    void ChangeDirection()
    {
        if (playerDirection == Direction.Right)
        {
            if (g_VeclocityX < 0)
            {
                turnOverEnable = true;
            }
        }
        if (playerDirection == Direction.Left)
        {
            if (g_VeclocityX > 0)
            {
                turnOverEnable = true;
            }
        }
        if (turnOverEnable && playerDirection == Direction.Right)
        {
            if (transform.eulerAngles.y < 270)
                transform.Rotate(new Vector3(0, 15, 0));
            else
            {
                playerDirection = Direction.Left;
                turnOverEnable = false;
            }
        }

        if (turnOverEnable && playerDirection == Direction.Left)
        {
            if (transform.eulerAngles.y >= 20)
            {
                transform.Rotate(new Vector3(0, 15, 0));
            }
            else
            {
                playerDirection = Direction.Up;
            }
        }
        if (playerDirection == Direction.Up)
        {
            if (transform.eulerAngles.y < 80)
            {
                transform.Rotate(new Vector3(0, 15, 0));
            }
            else
            {
                playerDirection = Direction.Right;
                turnOverEnable = false;
            }
        }
    }


    /// <summary>
    /// これは移動情報を動的配列に保存するメソッドです
    /// Data型を返す
    /// </summary>
    /// <returns></returns>
    public Data GetData()
    {
        Data d = new Data();
        //現在の時間-停止状態の全時間帯=ゴーストが実際に動く時間
        d.time = outArea.GetComponent<ReplayScript>().p_NowTimeCount - idle_Time;
        d.v = g_VeclocityX;
        d.m_Jump = g_duringJump;
        add_Data_Count++;
        return d;
    }

    /// <summary>
    /// これはプレイヤーの移動状態をチェックするメソッドです。
    /// </summary>
    public void CheckPlayerIdleState()
    {
        //全部保存
        //player_NowState = PlayerState.Move;

        //移動するときだけ保存
        if (g_VeclocityX == 0 && isGround)
        {
            player_NowState = PlayerState.Idle;
        }
        else if (!isGround)
        {
            player_NowState = PlayerState.Move;
        }
        else if (g_VeclocityX != 0)
        {
            player_NowState = PlayerState.Move;
        }
    }
}
