using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using TMPro;

public class ScenePanel : UIbase
{
    public static ScenePanel instance;

    private Button btn_Other;
    private Button btn_Share;


    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        FindChild(this.gameObject);
        for (int i = 0; i < aaa.Count; i++)
        {
            aaa[i].gameObject.AddComponent<TextTest>();
        }

        btn_Other = transform.Find("img_bottonbg/btn_other").GetComponent<Button>();
        btn_Other.onClick.AddListener(OnActionOtherPanel);
        btn_Share = transform.Find("img_bottonbg/btn_share").GetComponent<Button>();
        btn_Share.onClick.AddListener(OnActionSharePanel);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                OnUIdicActionFalse();
            }
        }
    }

    private void OnActionSharePanel()
    {
        //OnButtonActionPanel("uiprefabs/sharepanel.ab", "sharepanel", new Vector3(209, -290, 0));
        GameManager.instances.UIdic["sharepanel"].OnExitAction();
    }

    private void OnActionOtherPanel()
    {
        //OnButtonActionPanel("uiprefabs/otherpanel.ab", "otherpanel", new Vector3(273, -148, 0));
        GameManager.instances.UIdic["otherpanel"].OnExitAction();
    }

    private void OnButtonActionPanel(string _path, string name, Vector3 point)
    {
        if (GameManager.instances.UIdic.ContainsKey(name))
        {
            GameManager.instances.UIdic[name].OnExitAction();
        }
        else
        {
            //OnButtonClick(name);
            StartCoroutine(GameManager.instances.OnWebRequestAssetBundleUIPanel(name, point, transform,false));
            OnUIdicActionFalse();
        }
    }

    public void OnUIdicActionFalse()
    {
        foreach (var item in GameManager.instances.UIdic)
        {
            item.Value.gameObject.SetActive(false);
        }
    }

    //public void OnButtonClick(string btnName)
    //{
        //switch (btnName)
        //{
        //    case "sharepanel":
        //        btn_Share.enabled = !btn_Share.enabled;
        //        break;
        //    case "otherpanel":
        //        btn_Other.enabled = !btn_Other.enabled;
        //        break;
        //}

    //}


}
