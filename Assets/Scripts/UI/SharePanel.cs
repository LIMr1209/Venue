using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SharePanel : UIbase
{
    TMP_Text txt;
    Button btn_Copylink;
    void Start()
    {
        txt = transform.Find("img_bg/img_linkbg/img_bg/txt_link").GetComponent<TMP_Text>();
        btn_Copylink = transform.Find("img_bg/img_linkbg/btn_Copylink").GetComponent<Button>();
        btn_Copylink.onClick.AddListener(OnCopy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCopy()
    {
        UnityEngine.GUIUtility.systemCopyBuffer = txt.text;
    }

}
