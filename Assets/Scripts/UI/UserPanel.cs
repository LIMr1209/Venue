using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using DefaultNamespace;
using static DefaultNamespace.JsonData;
using TMPro;

public class UserPanel : UIbase
{
    ScrollRect teamscrollview;
    ScrollRect invitedscrollview;
    ScrollRect strangersscrollview;
    TextMeshProUGUI text_num;

    Button btn_Close;

    ViewResult<memberData> memberResult;

    void Start()
    {
        FindChild(this.gameObject);
        for (int i = 0; i < aaa.Count; i++)
        {
            aaa[i].gameObject.AddComponent<TextTest>();
        }
        text_num= transform.Find("img_topbg/txt_num").GetComponent<TextMeshProUGUI>();
        teamscrollview = transform.Find("img_contentbg/img_teambg/Scroll View").GetComponent<ScrollRect>();
        invitedscrollview = transform.Find("img_contentbg/img_invitedbg/Scroll View").GetComponent<ScrollRect>();
        strangersscrollview = transform.Find("img_contentbg/img_strangersbg/Scroll View").GetComponent<ScrollRect>();
        memberResult = GameManager.instances.memberResult;
        text_num.text = GameManager.instances.OnGetMemberRequestNum() + "人正在观看";
        OnSetContentView(teamscrollview.content, 1, 110, false, "主办方");
        OnSetContentView(invitedscrollview.content, 3, 102, true, "我邀请的");
        OnSetContentView(strangersscrollview.content, 4, 102, true, "陌生人");

        btn_Close = transform.Find("img_topbg/btn_close").GetComponent<Button>();
        btn_Close.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }


    void Update()
    {
    }

    public override void OnExitAction()
    {
        base.OnExitAction();
        OnSetContentItem();
    }

    //刷新界面
    public void OnSetContentItem()
    {
        if (!teamscrollview || !invitedscrollview || !invitedscrollview)
        {
            return;
        }
        memberResult = GameManager.instances.OnMemberRequest();
        OnGetUserItem(teamscrollview.content.GetChild(0), memberResult.data.host_team);
        OnFush(memberResult.data.invited_user, invitedscrollview.content,3,120);
        OnFush(memberResult.data.stranger, strangersscrollview.content, 4, 120);
    }

    private void OnFush(UserData[] Data ,Transform content,int index,int cengheight)
    {
        for (int i = 0; i < content.childCount; i++)
        {
            OnGetUserItem(content.GetChild(i), Data[i]);
        }
        for (int i = content.childCount; i < Data.Length; i++)
        {
            Transform item = Instantiate(content.Find("Item"));
            item.SetParent(content);
            OnGetUserItem(item, Data[i]);
        }
        int ceng = content.childCount / index;
        int cengnum = content.childCount % index != 0 ? ceng : ceng - 1;
        int height = cengheight + (cengnum * cengheight);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(17, height);
    }



    public void OnSetContentView(Transform content, int index, int cengheight, bool isheight,string Pcalss)
    {
        Transform Item = content.transform.Find("Item");
        switch (Pcalss) 
        {
            case "主办方":
                OnGetUserItem(Item, memberResult.data.host_team);
                break;
            case "我邀请的":
                OnGetUserItem(Item, memberResult.data.invited_user[0]);
                for (int i = 1; i < memberResult.data.invited_user.Length; i++)
                {
                    Transform item = Instantiate(content.Find("Item"));
                    item.SetParent(content);
                    OnGetUserItem(item, memberResult.data.invited_user[i]);
                }
                break;
            case "陌生人":
                OnGetUserItem(Item, memberResult.data.stranger[0]);
                for (int i = 1; i < memberResult.data.stranger.Length; i++)
                {
                    Transform item = Instantiate(content.Find("Item"));
                    item.SetParent(content);
                    OnGetUserItem(item, memberResult.data.stranger[i]);
                }
                break;
        }
        
        
        if (isheight)
        {
            int ceng = content.childCount / index;
            int cengnum= content.childCount % index != 0 ? ceng : ceng - 1;
            int height = cengheight + (cengnum * cengheight);
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(17, height);
        }
        else
        {
            int width = content.childCount / index * cengheight + 16;
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 0);
        }
    }

    public void OnGetUserItem(Transform Item, UserData date)
    {
        Item.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = date.nickname;
        OnGetSprite(Item.Find("Image").GetComponent<Image>(), date.logo_url);
    }

    public void OnGetSprite(Image image, string url)
    {
        StartCoroutine(GameManager.instances.DownTexture(image, url));
    }

}
