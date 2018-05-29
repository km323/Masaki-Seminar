using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loading : MonoBehaviour {
    public GameObject loadingUI;
    public GameObject startButton;

    private AsyncOperation async;
    private Slider slider;

    // Use this for initialization
    private void Start()
    {
        PlayerPrefs.DeleteAll();
    }
    public void nextScene () {
        loadingUI.SetActive(true);
        startButton.SetActive(false);
        slider = loadingUI.GetComponentInChildren<Slider>();
        StartCoroutine("LoadNextScene");
	}
	
    IEnumerator LoadNextScene()
    {
        async = SceneManager.LoadSceneAsync("Stage1");

        while(!async.isDone)
        {
            float progressVal = Mathf.Clamp01(async.progress);
            slider.value = async.progress;
            yield return null;
        }
    }
}
