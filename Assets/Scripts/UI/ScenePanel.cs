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
    private Button btn_Interaction;
    private List<Button> btnList = new List<Button>();


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
        btn_Interaction = transform.Find("img_bottonbg/btn_interaction").GetComponent<Button>();
        btn_Interaction.onClick.AddListener(OnActionInteractionPanel);
        btnList.Add(btn_Other);
        btnList.Add(btn_Share);
        btnList.Add(btn_Interaction);
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
        btn_Share.transform.Find("ima_click").gameObject.SetActive(true);
    }

    private void OnActionOtherPanel()
    {
        //OnButtonActionPanel("uiprefabs/otherpanel.ab", "otherpanel", new Vector3(273, -148, 0));
        GameManager.instances.UIdic["otherpanel"].OnExitAction();
        btn_Other.transform.Find("ima_click").gameObject.SetActive(true);
    }

    private void OnActionInteractionPanel()
    {
        GameManager.instances.UIdic["expressionpanel"].OnExitAction();
        btn_Interaction.transform.Find("ima_click").gameObject.SetActive(true);
    }

    public void OnUIdicActionFalse()
    {
        foreach (var item in GameManager.instances.UIdic)
        {
            item.Value.gameObject.SetActive(false);
        }
        OnSeleetedButton();
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

    public void OnSeleetedButton()
    {
        for (int i = 0; i < btnList.Count; i++)
        {
            btnList[i].transform.Find("ima_click").gameObject.SetActive(false);
        }
    }


}
