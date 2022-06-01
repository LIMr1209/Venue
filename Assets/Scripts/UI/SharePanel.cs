using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharePanel : UIbase
{
    Text txt;

    void Start()
    {
        txt = transform.Find("img_bg/img_linkbg/img_bg/img_bg").GetComponent<Text>();
        transform.Find("img_bg/img_linkbg/btn_Copylink").GetComponent<Button>().onClick.AddListener(()=> 
        {
            UnityEngine.GUIUtility.systemCopyBuffer = txt.text;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCopy()
    {
        
    }

}
