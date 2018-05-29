
using UnityEngine;
using System.Collections;

// 必要なコンポーネントの列記
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class GhostControl : MonoBehaviour
{
    GameObject outArea;
    float animSpeed;              // アニメーション再生速度設定

    // 以下キャラクターコントローラ用パラメタ
    // 前進速度
    float Speed;
    bool duringRun = false;
    public float g_VeclocityX;
    public float g_VeclocityY;
    //方向変更用
    Direction playerDirection;
    bool turnOverEnable = false;

    // ジャンプ威力
    float jumpPower;
    public bool g_duringJump = false;

    //rRigidbody
    private Rigidbody rb;
    // キャラクターコントローラ（カプセルコライダ）の移動量
    private Vector3 velocity;
    // キャラにアタッチされるアニメーターへの参照
    private Animator anim;

    //Playerの参照
    GameObject player;
    // 初期化
    void Start()
    {
        player = GameObject.Find("Player");
        outArea = GameObject.Find("OutArea");
        playerDirection = Direction.Up;
        // Animatorコンポーネントを取得する
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        animSpeed = player.GetComponent<PlayerControl>().animSpeed;
        Speed = player.GetComponent<PlayerControl>().Speed;
        jumpPower = player.GetComponent<PlayerControl>().jumpPower;
    }
    private void Update()
    {
        anim.SetBool("Jump", g_duringJump);
        anim.SetBool("Run", duringRun);
        anim.speed = animSpeed;


        rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする
        if (g_VeclocityX > 0 || g_VeclocityX < 0 || g_VeclocityY < 0 || g_VeclocityY > 0)
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
        // 以下、キャラクターの移動処理
        velocity = new Vector3(g_VeclocityX, 0, -g_VeclocityY);        // 左右のキー入力からX軸方向の移動量を取得

        velocity *= Speed;       // 移動速度を掛ける

        if (!turnOverEnable)
            // 左右のキー入力でキャラクターを移動させる
            transform.localPosition += velocity * Time.fixedDeltaTime;

        JumpFunction();
        
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
    void ChangeDirection()
    {
        if (g_VeclocityX != 0)
        {
            g_VeclocityY = 0;
        }
        if (g_VeclocityY != 0)
        {
            g_VeclocityX = 0;
        }
        if (playerDirection == Direction.Up)
        {
            if (g_VeclocityX < 0)
            {
                playerDirection = Direction.Down;
                transform.Rotate(0, 180, 0);
            }
            if (g_VeclocityY < 0)
            {
                playerDirection = Direction.Left;
                transform.Rotate(0, -90, 0);
                //g_VeclocityY = 0;
            }
            if (g_VeclocityY > 0)
            {
                playerDirection = Direction.Right;
                transform.Rotate(0, 90, 0);
                //g_VeclocityY = 0;
            }
        }
        if (playerDirection == Direction.Down)
        {
            if (g_VeclocityX > 0)
            {
                playerDirection = Direction.Up;
                transform.Rotate(0, -180, 0);
            }
            if (g_VeclocityY < 0)
            {
                playerDirection = Direction.Left;
                transform.Rotate(0, 90, 0);
                //g_VeclocityY = 0;
            }
            if (g_VeclocityY > 0)
            {
                playerDirection = Direction.Right;
                transform.Rotate(0, -90, 0);
                //g_VeclocityY = 0;
            }
        }

        if (playerDirection == Direction.Left)
        {
            if (g_VeclocityX > 0)
            {
                playerDirection = Direction.Up;
                transform.Rotate(0, 90, 0);
                //g_VeclocityX = 0;
            }
            if (g_VeclocityX < 0)
            {
                playerDirection = Direction.Down;
                transform.Rotate(0, -90, 0);
                //g_VeclocityX = 0;
            }
            if (g_VeclocityY > 0)
            {
                playerDirection = Direction.Right;
                transform.Rotate(0, 180, 0);
            }
        }

        if (playerDirection == Direction.Right)
        {
            if (g_VeclocityX > 0)
            {
                playerDirection = Direction.Up;
                transform.Rotate(0, -90, 0);
                // g_VeclocityX = 0;
            }
            if (g_VeclocityX < 0)
            {
                playerDirection = Direction.Down;
                transform.Rotate(0, 90, 0);
                // g_VeclocityX = 0;
            }
            if (g_VeclocityY < 0)
            {
                playerDirection = Direction.Left;
                transform.Rotate(0, 180, 0);
            }
        }
    }
    
}
