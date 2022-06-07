using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIbase : MonoBehaviour
{
    bool isAction = true;

    public List<TextMeshProUGUI> aaa = new List<TextMeshProUGUI>();


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

    public void FindChild(GameObject child)
    {
        for (int c = 0; c < child.transform.childCount; c++)
        {
            if (child.transform.GetChild(c).childCount > 0)
            {
                FindChild(child.transform.GetChild(c).gameObject);
            }
            if (child.transform.GetChild(c).transform.GetComponent<TextMeshProUGUI>() != null)
            {
                aaa.Add(child.transform.GetChild(c).gameObject.GetComponent<TextMeshProUGUI>());
            }

        }
    }
}
