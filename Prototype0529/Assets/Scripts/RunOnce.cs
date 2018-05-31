using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Hack:必要か？
public class RunOnce : MonoBehaviour {
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        PlayerPrefs.DeleteAll();
    }
}
