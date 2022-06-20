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
    private Button btn_num;
    private Button btn_list;
    private Button btn_speech;
    private Button btn_chat;

    private TextMeshProUGUI txt_num;



    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        transform.SetAsFirstSibling();
        FindChild(this.gameObject);
        for (int i = 0; i < aaa.Count; i++)
        {
            aaa[i].gameObject.AddComponent<TextTest>();
        }

        OnButtonfindAdd();

        txt_num = transform.Find("btn_num/Text (TMP)").GetComponent<TextMeshProUGUI>();

            txt_num.text = GameManager.instances.OnGetMemberRequestNum().ToString();

        



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

    //查找添加按钮的点击事件
    private void OnButtonfindAdd()
    {
        btn_Other = transform.Find("img_bottonbg/btn_other").GetComponent<Button>();
        btn_Other.onClick.AddListener(OnActionOtherPanel);
        btn_Share = transform.Find("img_bottonbg/btn_share").GetComponent<Button>();
        btn_Share.onClick.AddListener(OnActionSharePanel);
        btn_Interaction = transform.Find("img_bottonbg/btn_interaction").GetComponent<Button>();
        btn_Interaction.onClick.AddListener(OnActionInteractionPanel);
        btn_list = transform.Find("img_bottonbg/btn_list").GetComponent<Button>();
        btn_list.onClick.AddListener(OnActionListPanel);
        btn_speech = transform.Find("img_bottonbg/btn_speech").GetComponent<Button>();
        btn_speech.onClick.AddListener(OnActionSpeechPanel);
        btn_chat = transform.Find("img_bottonbg/btn_chat").GetComponent<Button>();
        btn_chat.onClick.AddListener(OnActionChatPanel);
        btn_num = transform.Find("btn_num").GetComponent<Button>();
        btn_num.onClick.AddListener(OnActionUserPanel);
    }

    private void OnActionSharePanel()
    {
        if (GameManager.instances.UIdic["sharepanel"].gameObject.activeInHierarchy)
        {
            OnUIdicActionFalse();
            btn_Share.image.color = Color.white;
        }
        else
        {
            GameManager.instances.UIdic["sharepanel"].OnExitAction();
            btn_Share.image.color = Color.blue;
        }
    }

    private void OnActionOtherPanel()
    {
        if (GameManager.instances.UIdic["otherpanel"].gameObject.activeInHierarchy)
        {
            OnUIdicActionFalse();
            btn_Other.image.color = Color.white;
        }
        else
        {
            GameManager.instances.UIdic["otherpanel"].OnExitAction();
            btn_Other.image.color = Color.blue;
        }
    }

    private void OnActionInteractionPanel()
    {
        if (GameManager.instances.UIdic["expressionpanel"].gameObject.activeInHierarchy)
        {
            OnUIdicActionFalse();
            btn_Interaction.image.color = Color.white;
        }
        else
        {
            GameManager.instances.UIdic["expressionpanel"].OnExitAction();
            btn_Interaction.image.color = Color.blue;
        }
    }

    private void OnActionUserPanel()
    {
        if (GameManager.instances.UIdic["userpanel"].gameObject.activeInHierarchy)
        {
            OnUIdicActionFalse();
        }
        else
        {
            GameManager.instances.UIdic["userpanel"].OnExitAction();
        }
    }

    private void OnActionListPanel()
    {
        if (GameManager.instances.UIdic["listpanel"].gameObject.activeInHierarchy)
        {
            OnUIdicActionFalse();
            btn_list.image.color = Color.white;
        }
        else
        {
            GameManager.instances.UIdic["listpanel"].OnExitAction();
            btn_list.image.color = Color.blue;
        }
    }

    private void OnActionSpeechPanel()
    {
        //GameManager.instances.UIdic["speechpanel"].OnExitAction();
        OnUIdicActionFalse();
        btn_speech.image.color = Color.blue;
    }

    private void OnActionChatPanel()
    {
        //GameManager.instances.UIdic["chatpanel"].OnExitAction();
        OnUIdicActionFalse();
        btn_chat.image.color = Color.blue;
    }



    public void OnUIdicActionFalse()
    {
        foreach (var item in GameManager.instances.UIdic)
        {
            item.Value.gameObject.SetActive(false);
        }
        btn_chat.image.color = Color.white;
        btn_Interaction.image.color = Color.white;
        btn_list.image.color = Color.white;
        btn_Other.image.color = Color.white;
        btn_Share.image.color = Color.white;
        btn_speech.image.color = Color.white;
    }



}
