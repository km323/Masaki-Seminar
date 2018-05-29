using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleControl : MonoBehaviour {
    public GameObject light;
    bool reverseIndensityEnable;
	// Use this for initialization
	void Start () {
        reverseIndensityEnable = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(reverseIndensityEnable)
        {
            if(light.GetComponent<Light>().intensity>6)
            {
                light.GetComponent<Light>().intensity -= 0.03f;
            }
            else
            {
                reverseIndensityEnable = !reverseIndensityEnable;
            }
        }
        if(!reverseIndensityEnable)
        {
            if (light.GetComponent<Light>().intensity < 10)
            {
                light.GetComponent<Light>().intensity += 0.03f;
            }
            else
            {
                reverseIndensityEnable = !reverseIndensityEnable;
            }
        }
	}
}
