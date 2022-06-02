using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager instances;

    bool loadFont = true;

    private GameManager()
    {
       
    }

    private void Awake()
    {
        instances = this;
    }

    private void Start()
    {
        
    }


    public GameObject OnLoadUIPanel(string path,string name)
    {
        AssetBundle ab = AssetBundle.LoadFromFile(path+".ab");
        GameObject obj = ab.LoadAsset<GameObject>(name);

        if (loadFont)
        {
            string fontPath = "/AssetsBundles/assetbundel/";
            AssetBundle abFonta = AssetBundle.LoadFromFile(Application.dataPath + fontPath + "pingfangsca.ab");
            TMP_FontAsset fonta = abFonta.LoadAsset<TMP_FontAsset>("PingFangSCA");

            AssetBundle abFontaa = AssetBundle.LoadFromFile(Application.dataPath + fontPath + "pingfangscafont.ab");
            Font fontaa = abFontaa.LoadAsset<Font>("PingFangSCAfont");

            AssetBundle abFontb = AssetBundle.LoadFromFile(Application.dataPath + fontPath + "pingfangscb.ab");
            TMP_FontAsset fontb = abFontb.LoadAsset<TMP_FontAsset>("PingFangSCB");

            AssetBundle abFontbb = AssetBundle.LoadFromFile(Application.dataPath + fontPath + "pingfangscbfont.ab");
            Font fontbb = abFontbb.LoadAsset<Font>("PingFangSCBfont");

            loadFont = false;
        }
        
        return obj;
    }

}
