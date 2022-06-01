using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIbase : MonoBehaviour
{
    // Start is called before the first frame update
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
