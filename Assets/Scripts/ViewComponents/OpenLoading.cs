using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenLoading : MonoBehaviour
{
    public Image loadingBar;

    public float timeLoading;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer + Time.deltaTime < timeLoading)
        {
            timer += Time.deltaTime;
            loadingBar.fillAmount = (float)timer / (float)timeLoading;
        }
        else
            OpenGame();
    }

    void OpenGame()
    {
        gameObject.SetActive(false);
    }
}
