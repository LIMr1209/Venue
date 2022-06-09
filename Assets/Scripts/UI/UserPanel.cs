using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class UserPanel : UIbase
{
    ScrollRect teamscrollview;
    ScrollRect invitedscrollview;
    ScrollRect strangersscrollview;

    Button btn_Close;

    void Start()
    {
        FindChild(this.gameObject);
        for (int i = 0; i < aaa.Count; i++)
        {
            aaa[i].gameObject.AddComponent<TextTest>();
        }

        teamscrollview = transform.Find("img_contentbg/img_teambg/Scroll View").GetComponent<ScrollRect>();
        invitedscrollview = transform.Find("img_contentbg/img_invitedbg/Scroll View").GetComponent<ScrollRect>();
        strangersscrollview = transform.Find("img_contentbg/img_strangersbg/Scroll View").GetComponent<ScrollRect>();
        OnSetContentView(teamscrollview.content, 1, 110, false);
        OnSetContentView(invitedscrollview.content, 3, 102, true);
        OnSetContentView(strangersscrollview.content, 4, 102, true);

        btn_Close = transform.Find("img_topbg/btn_close").GetComponent<Button>();
        btn_Close.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }


    void Update()
    {
        
    }


    public void OnSetContentView(Transform content, int index, int cengheight, bool isheight)
    {
        for (int i = 0; i < 20; i++)
        {
            Transform item = Instantiate(content.Find("Item"));
            item.SetParent(content);
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

}
