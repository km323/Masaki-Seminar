using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// これは落石を移動させるクラスです
/// </summary>
public class FallenStoneScript : MonoBehaviour
{
    int hitCount;
    [HideInInspector]
    public bool fallenEnable;
    bool addForceEnable;
    // Use this for initialization
    void Start()
    {
        hitCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// これは岩を落下させるメソッド
    /// 床に落ちたら、力を与える
    /// </summary>
    /// <param name="fallen_Veloc"></param>
    /// <param name="hitOnceForce"></param>
    /// <param name="hitTwiceForce"></param>
    public void SetFallenStone(float fallen_Veloc, float hitOnceForce, float hitTwiceForce)
    {
        GetComponent<Rigidbody>().isKinematic = false;

        transform.GetComponent<Rigidbody>().AddForce(0, fallen_Veloc, 0);

        if (hitCount == 1&&addForceEnable)
        {
            transform.GetComponent<Rigidbody>().AddForce(hitOnceForce, 0, 0, ForceMode.VelocityChange);
            addForceEnable = false;
        }
        if (hitCount == 2 && addForceEnable)
        {
            transform.GetComponent<Rigidbody>().AddForce(hitTwiceForce, 0, 0, ForceMode.VelocityChange);
            addForceEnable = false;
            fallenEnable = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            addForceEnable = true;
            hitCount++;
        }

    }
}
