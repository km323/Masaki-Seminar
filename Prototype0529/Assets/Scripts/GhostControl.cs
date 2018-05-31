
using UnityEngine;
using System.Collections;
/// <summary>
/// これはゴーストを移動させる関数
/// </summary>


// 必要なコンポーネントの列記
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class GhostControl : MonoBehaviour
{
    //参照
    GameObject outArea;
    GameObject player;

    // アニメーション再生速度設定
    float animSpeed;

    // 移動関連
    float Speed;
    bool duringRun = false;
    [HideInInspector]
    public float g_VeclocityX;
    Vector3 velocity;

    //方向変更用
    Direction playerDirection;
    bool turnOverEnable;

    // ジャンプ用
    float jumpPower;
    [HideInInspector]
    public bool g_duringJump = false;

    //コンポーネント
    private Rigidbody rb;
    private Animator anim;

    // 初期化
    void Start()
    {
        //参照
        player = GameObject.Find("Player");
        outArea = GameObject.Find("OutArea");

        //プロパティ
        playerDirection = Direction.Up;
        animSpeed = player.GetComponent<PlayerControl>().animSpeed;
        Speed = player.GetComponent<PlayerControl>().Speed;
        jumpPower = player.GetComponent<PlayerControl>().jumpPower;

        //コンポーネント
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //アニメーション設定
        anim.SetBool("Jump", g_duringJump);
        anim.SetBool("Run", duringRun);
        anim.speed = animSpeed;

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

}
