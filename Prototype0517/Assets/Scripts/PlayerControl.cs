using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
enum Direction
{
    Right,
    Left,
    Up,
}
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
    GameObject outArea;
    /// <summary>
    /// プレイヤー情報を一時保存するList動的配列
    /// </summary>
    public List<Data> dataList = new List<Data>();
    public float addCount;
    //立ち留まる時間
    private float idle_Time;

    PlayerState player_NowState;

    //プレイヤー情報を保存する配列
    public const int MAXTIME = 5000;

    // アニメーション再生速度設定
    public float animSpeed = 1.5f;

    // 以下キャラクターコントローラ用パラメタ
    // 前進速度
    public float Speed = 7.0f;
    bool duringRun = false;
    public float g_VeclocityX;

    //方向変更用
    Direction playerDirection;
    bool turnOverEnable = false;

    // ジャンプ威力
    public float jumpPower = 8.0f;
    public bool g_duringJump = false;

    //rRigidbody
    private Rigidbody rb;
    // キャラクターコントローラ（カプセルコライダ）の移動量
    private Vector3 velocity;
    // キャラにアタッチされるアニメーターへの参照
    private Animator anim;

    /// <summary>
    /// 接地判定用Ray
    /// </summary>
    //　レイを飛ばす位置
    Ray ray;
    public const float DistanceToGround = 0.07f;
    //　レイが地面に到達しているかどうか
    private bool isGround = false;


    // 初期化
    void Start()
    {
        //capsuleCollider = GetComponent<CapsuleCollider>();
        addCount = 0;
        idle_Time = 0;
        outArea = GameObject.Find("OutArea");
        playerDirection = Direction.Right;
        // Animatorコンポーネントを取得する
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        // Animator側で設定している"Speed"パラメタにg_VeclocityXを渡す
        anim.SetFloat("Speed", g_VeclocityX);
        anim.SetBool("Jump", false);
        anim.SetBool("Run", duringRun);
        anim.speed = animSpeed;                             // Animatorのモーション再生速度に animSpeedを設定する

        rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする

        CheckisGrounded();

        g_VeclocityX = Input.GetAxis("Vertical");              // 入力デバイスの水平軸をg_VeclocityXで定義
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                g_duringJump = true;
                anim.SetBool("Jump", true);
            }
        }
        if (g_VeclocityX > 0 || g_VeclocityX < 0)
        {
            duringRun = true;
        }
        else
        {
            duringRun = false;
        }

        //Debug.Log("AddCount"+addCount);

    }

    // 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
    void FixedUpdate()
    {

        #region 情報保存
        //プレイヤーが移動しているかどうかをチェック
        if (player_NowState == PlayerState.Move)
        {
            //もし保存できる最大値以下だったら
            //フレームごとに、プレイヤーの情報を保存する
            if (dataList.Count < MAXTIME)
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

        // 以下、キャラクターの移動処理
        velocity = new Vector3(g_VeclocityX, 0, 0);        // 左右のキー入力からX軸方向の移動量を取得

        velocity *= Speed;       // 移動速度を掛ける

        // 左右のキー入力でキャラクターを移動させる
        if (!turnOverEnable)
            transform.localPosition += velocity * Time.fixedDeltaTime;

        CheckPlayerIdleState();

        JumpFunction();

        ChangeDirection();



    }
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
    void CheckisGrounded()
    {
        RaycastHit hit;
        ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, DistanceToGround))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        Debug.DrawLine(ray.origin, ray.origin - new Vector3(0, DistanceToGround, 0), Color.red, 0.1f);
    }
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
            if (transform.eulerAngles.y < 250)
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
        addCount++;
        return d;
    }

    /// <summary>
    /// これはプレイヤーの移動状態をチェックするメソッドです。
    /// 停止ならlayer_NowState = PlayerState.Idle;逆ー＞layer_NowState = PlayerState.Move
    /// </summary>
    public void CheckPlayerIdleState()
    {
        if (g_VeclocityX == 0 && isGround)
        {
            player_NowState = PlayerState.Idle;
        }
        else if(!isGround)
        {
            player_NowState = PlayerState.Move;
        }
        else if(g_VeclocityX!=0)
        {
            player_NowState = PlayerState.Move;
        }
    }
}
