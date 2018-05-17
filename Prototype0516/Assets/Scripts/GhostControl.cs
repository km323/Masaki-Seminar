
using UnityEngine;
using System.Collections;

// 必要なコンポーネントの列記
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class GhostControl : MonoBehaviour
{
    GameObject outArea;
    public float animSpeed = 1.5f;              // アニメーション再生速度設定

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
    
    // 初期化
    void Start()
    {
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
        anim.SetBool("Jump", g_duringJump);
        anim.SetBool("Run", duringRun);
        anim.speed = animSpeed;


        rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする
        
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

        // 以下、キャラクターの移動処理
        velocity = new Vector3(g_VeclocityX, 0, 0);        // 左右のキー入力からX軸方向の移動量を取得

        velocity *= Speed;       // 移動速度を掛ける

        if (!turnOverEnable)
            // 左右のキー入力でキャラクターを移動させる
            transform.localPosition += velocity * Time.fixedDeltaTime;

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
    void ChangeDirection()
    {
        if (playerDirection == Direction.Right)
        {
            if (g_VeclocityX < 0)
            {
                turnOverEnable = true;
            }
            if (turnOverEnable)
            {
                if (transform.eulerAngles.y < 240)
                    transform.Rotate(new Vector3(0, 15, 0));
                else
                {
                    playerDirection = Direction.Left;
                    turnOverEnable = false;
                }
            }
        }
        if (playerDirection == Direction.Left)
        {
            if (g_VeclocityX > 0)
            {
                turnOverEnable = true;
            }
            if (turnOverEnable)
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="GhostTrap")
        {
            outArea.gameObject.GetComponent<ReplayScript>().GhostDisapear();
        }
    }
}
