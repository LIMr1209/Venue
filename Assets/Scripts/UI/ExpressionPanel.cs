using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ExpressionPanel : UIbase
{
    public ScrollRect scrollView;

    public Transform Content;

    public int ExpressionNum;


    void Start()
    {
       
        scrollView = gameObject.GetComponentInChildren<ScrollRect>();
        Content = scrollView.content;
        OnInitialization();
    }


    void Update()
    {
        
    }

    public void OnInitialization()
    {
        Transform item1 = Instantiate(Content.Find("item"));
        item1.SetParent(Content);
        for (int i = 0; i <GameManager.instances.ExpressionList.Count ; i++) 
        {
            Transform item = Instantiate(Content.Find("item"));
            item.SetParent(Content);
            int BeginIndex = GameManager.instances.ExpressionList[i].name.IndexOf("/") + 1;
            int LastIndex = GameManager.instances.ExpressionList[i].name.IndexOf(".");
            int len = LastIndex - BeginIndex;
            string bundleName = GameManager.instances.ExpressionList[i].name.Substring(BeginIndex, len);
            Sprite spitem = GameManager.instances.ExpressionList[i].LoadAsset<Sprite>(bundleName);
            item.transform.Find("Image").GetComponentInChildren<Image>().sprite = spitem;
            Content.GetComponent<RectTransform>().sizeDelta = new Vector2(17, 80 + (OnInitializationTest() * 64));
        }
        OnClickExpressionTest();
    }

    private int OnInitializationTest()
    {
        int height = Content.childCount / 5;
        return Content.childCount % 5 != 0 ? height : height - 1;
    }

    private void OnClickExpressionTest()
    {
        for (int i = 0; i < Content.childCount; i++)
        {
            //Content.GetChild(i).transform.GetComponent<Button>().onClick.AddListener(() => 
            //{
            //    Debug.Log(Content.GetChild(i).GetComponent<Image>().sprite.name);
            //});


            Button button = Content.GetChild(i).transform.GetComponent<Button>();
            button.onClick.AddListener(() => 
            {
                Sprite a = button.transform.Find("Image").GetComponent<Image>().sprite;
                Debug.Log(a.name);
            });
        }
    }





}
