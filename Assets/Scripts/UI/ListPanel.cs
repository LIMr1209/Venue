using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListPanel : UIbase
{
    ScrollRect scrollview;


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
        
    }



    public void OnSetContentView(Transform content, int index, int cengheight)


    {
        for (int i = 0; i < 20; i++)
        {
            Transform item = Instantiate(content.Find("Item"));
            item.SetParent(content);
        }
        int ceng = content.childCount / index;
        int cengnum = content.childCount % index != 0 ? ceng : ceng - 1;
        int height = cengheight + (cengnum * cengheight);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(17, height);
    }
}
