using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostItem : MonoBehaviour
{
    public GameObject countObj;

    public GameObject lockObj;

    public GameObject addObj;

    public Text countText;

    public enum STATE
    {
        LOCK,
        AVAILABLE,
        UNAVAILABLE
    }

    public STATE currentState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLock()
    {
        currentState = STATE.LOCK;

        lockObj.SetActive(true);
        countObj.SetActive(false);
        addObj.SetActive(false);
    }

    public void UnLock()
    {
        currentState = STATE.AVAILABLE;
        lockObj.SetActive(false);
        countObj.SetActive(true);
        addObj.SetActive(false);
    }

    public void SetUnAvailable()
    {
        currentState = STATE.UNAVAILABLE;
        lockObj.SetActive(false);
        countObj.SetActive(false);
        addObj.SetActive(true);
    }

    public void SetAvailable(int count)
    {
        currentState = STATE.AVAILABLE;
        lockObj.SetActive(false);
        countObj.SetActive(true);
        addObj.SetActive(false);
        countText.text = count.ToString();
    }

    public void ShowCountText(int count)
    {
        countText.text = count.ToString();
    }
}
