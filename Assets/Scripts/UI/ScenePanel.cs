using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using TMPro;

public class ScenePanel : MonoBehaviour
{
    public static ScenePanel instance;

    private Button btn_Other;
    private Button btn_Share;

    public Dictionary<string, UIbase> UIdic = new Dictionary<string, UIbase>();



    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        

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
        OnButtonActionPanel("SharePanel", new Vector3(209, -290, 0));
    }

    private void OnActionOtherPanel()
    {
        OnButtonActionPanel("OtherPanel", new Vector3(273, -148, 0));
    }

    private void OnButtonActionPanel(string name,Vector3 point)
    {
        if (UIdic.ContainsKey(name))
        {
            UIdic[name].OnExitAction();
        }
        else
        {
            OnSetPanel(name, point);
        }
    }

    public void OnUIdicActionFalse()
    {
        foreach (var item in UIdic)
        {
            item.Value.gameObject.SetActive(false);
        }
    }

    private void OnSetPanel(string _path,Vector3 point)
    {
        //string path = "prefabs/UIPrefabs/" + _path;
        //GameObject obj = Instantiate(Resources.Load(path)) as GameObject;
        //UIdic.Add(_path, obj.GetComponent<UIbase>());
        //obj.transform.SetParent(transform);
        //obj.transform.localPosition = point;
        //OnUIdicActionFalse();
        //obj.SetActive(true);

        string path = Application.dataPath + "/AssetsBundles/assetbundel/uiprefabs/" + _path;
        GameObject obj = Instantiate( GameManager.instances.OnLoadUIPanel(path,_path));
        UIdic.Add(_path, obj.GetComponent<UIbase>());
        obj.transform.SetParent(transform);
        obj.transform.localPosition = point;
        OnUIdicActionFalse();
        obj.SetActive(true);
    }


}
