using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static DefaultNamespace.JsonData;

public class ListPanel : UIbase
{
    ScrollRect scrollview;
    ListResult<WorkData> workResult;


    void Start()
    {
        FindChild(this.gameObject);
        for (int i = 0; i < aaa.Count; i++)
        {
            aaa[i].gameObject.AddComponent<TextTest>();
        }


        scrollview = transform.Find("Scroll View").GetComponent<ScrollRect>();
        OnSetContentView(scrollview.content, 5, 400);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnSetContentItem();
        }
    }

    //刷新界面
    public void OnSetContentItem()
    {
        // 获取作品列表
        Dictionary<string, string> workListRequest = new Dictionary<string, string>();
        workListRequest["id"] = Globle.roomId;
        workListRequest["token"] = Globle.token; // token 
        Request.instances.HttpSend(3, "get", workListRequest, (statusCode, error, body) =>
        {
            workResult = JsonUtility.FromJson<ListResult<WorkData>>(body);

            for(int i=0;i< scrollview.content.childCount; i++)
            {
                //Transform item = Instantiate(scrollview.content.Find("Item"));
                //item.SetParent(scrollview.content);
                scrollview.content.GetChild(i).transform.Find("txt_title").GetComponent<TextMeshProUGUI>().text = workResult.data[i].title;
                scrollview.content.GetChild(i).transform.Find("txt_username").GetComponent<TextMeshProUGUI>().text = workResult.data[i].user.nickname;
                OnGetUserItem(scrollview.content.GetChild(i), workResult, i);
            }
            for (int i = scrollview.content.childCount; i < workResult.data.Length; i++)
            {
                Transform item = Instantiate(scrollview.content.Find("Item"));
                item.SetParent(scrollview.content);
                item.transform.Find("txt_title").GetComponent<TextMeshProUGUI>().text = workResult.data[i].title;
                item.transform.Find("txt_username").GetComponent<TextMeshProUGUI>().text = workResult.data[i].user.nickname;
                OnGetUserItem(item, workResult, i);
            }
        });
        int ceng = scrollview.content.childCount / 5;
        int cengnum = scrollview.content.childCount % 5 != 0 ? ceng : ceng - 1;
        int height = 400 + (cengnum * 400);
        scrollview.content.GetComponent<RectTransform>().sizeDelta = new Vector2(17, height);
    }



    public void OnSetContentView(Transform content, int index, int cengheight)
    {
        // 获取作品列表
        Dictionary<string, string> workListRequest = new Dictionary<string, string>();
        workListRequest["id"] = Globle.roomId;
        workListRequest["token"] = Globle.token; // token 
        Request.instances.HttpSend(3, "get", workListRequest, (statusCode, error, body) =>
        {
            workResult = JsonUtility.FromJson<ListResult<WorkData>>(body);
            string workUrl = workResult.data[0].cover.thumb_path.avb;
            Debug.Log("作品 url: " + workUrl);
            OnGetUserItem(content.transform.Find("Item").transform, workResult, 0);
            for (int i = content.transform.childCount; i < workResult.data.Length; i++)
            {
                Transform item = Instantiate(content.Find("Item"));
                item.SetParent(content);
                item.transform.Find("txt_title").GetComponent<TextMeshProUGUI>().text = workResult.data[i].title;
                item.transform.Find("txt_username").GetComponent<TextMeshProUGUI>().text = workResult.data[i].user.nickname;
                OnGetUserItem(item, workResult, i);
            }
        }) ;
        int ceng = content.childCount / index;
        int cengnum = content.childCount % index != 0 ? ceng : ceng - 1;
        int height = cengheight + (cengnum * cengheight);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(17, height);
    }


    public void OnGetUserItem(Transform Item, ListResult<WorkData> workResult,int i)
    {
        Item.Find("txt_title").GetComponent<TextMeshProUGUI>().text = workResult.data[i].title;
        Item.Find("txt_username").GetComponent<TextMeshProUGUI>().text = workResult.data[i].user.nickname;
        OnGetSprite(Item.Find("img_bg").GetComponent<Image>(), workResult.data[i].cover.thumb_path.avb);
        OnGetSprite(Item.Find("Image").GetComponent<Image>(), workResult.data[i].user.logo_url);
    }

    public void OnGetSprite(Image image, string url)
    {
        StartCoroutine(GameManager.instances.DownTexture(image, url));
    }
}
