using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenePanel : MonoBehaviour
{
    private Button btn_Share;
    void Start()
    {
        btn_Share = transform.Find("OnLoadPanel/btn_share").GetComponent<Button>();
        btn_Share.onClick.AddListener(OnOpenSharePanel);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnOpenSharePanel();
        }
    }

    private void OnOpenSharePanel()
    {
        GameObject obj = OnLoadPanel("prefabs/UIPrefabs/SharePanel");
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(209, -291, 0);
    }

    private GameObject OnLoadPanel(string path)
    {
        GameObject obj = Instantiate(Resources.Load(path)) as GameObject;
        return obj;
    }

}
