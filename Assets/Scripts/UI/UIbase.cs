using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIbase : MonoBehaviour
{
    bool isAction = true;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnExitAction()
    {
        ScenePanel.instance.OnUIdicActionFalse();
        gameObject.SetActive(true);
    }
}
