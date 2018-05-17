using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunOnce : MonoBehaviour {
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        PlayerPrefs.DeleteAll();
    }
}
