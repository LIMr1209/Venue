using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMap : MonoBehaviour
{
    [System.Serializable]
    struct RendererInfo
    {
        public Renderer renderer;
        public int lightmapIndex;
        public Vector4 lightmapOffsetScale;
    }



    [UnityEngine.SerializeField]
    Texture2D[] lightmapTexs;   //当前场景的灯光贴图



    [UnityEngine.SerializeField]
    Texture2D[] lightmapTextures;

    [UnityEngine.SerializeField]
    RendererInfo[] rendererList;


#if UNITY_EDITOR
    public void SaveLightmap()
    {
        Renderer[] renders = GetComponentsInChildren<Renderer>(true);
        RendererInfo rendererInfo;
        rendererList = new RendererInfo[renders.Length];

        int index = 0;

        for (int r = 0, rLength = renders.Length; r < rLength; ++r)
        {
            if (renders[r].gameObject.isStatic == false) continue;

            rendererInfo.renderer = renders[r];
            rendererInfo.lightmapIndex = renders[r].lightmapIndex;
            rendererInfo.lightmapOffsetScale = renders[r].lightmapScaleOffset;

            rendererList[index] = rendererInfo;

            ++index;
        }

        //序列化光照贴图
        LightmapData[] ldata = LightmapSettings.lightmaps;
        lightmapTexs = new Texture2D[ldata.Length];
        for (int t = 0, tLength = ldata.Length; t < tLength; ++t)
        {
            lightmapTexs[t] = ldata[t].lightmapColor;
        }
    }

    void Awake()
    {
        this.LoadLightmap();
    }

#endif


#if !UNITY_EDITOR
     public 
#endif
    void LoadLightmap()
    {
        if (null == rendererList || rendererList.Length == 0)
        {
            Debug.Log(gameObject.name + " 的 光照信息为空");
            return;
        }

        Renderer[] renders = GetComponentsInChildren<Renderer>(true);

        for (int r = 0, rLength = renders.Length; r < rLength; ++r)
        {
            renders[r].lightmapIndex = rendererList[r].lightmapIndex;
            renders[r].lightmapScaleOffset = rendererList[r].lightmapOffsetScale;
        }

#if UNITY_EDITOR
        if (null == lightmapTexs || lightmapTexs.Length == 0)
        {
            return;
        }

        LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
        LightmapData[] ldata = new LightmapData[lightmapTexs.Length];
        LightmapSettings.lightmaps = null;

        for (int t = 0, tLength = lightmapTexs.Length; t < tLength; ++t)
        {
            ldata[t] = new LightmapData();
            ldata[t].lightmapColor = lightmapTexs[t];
        }

        LightmapSettings.lightmaps = ldata;
#endif
    }










    public void OnCreatLightmapTexs(int i)
    {
        lightmapTextures = new Texture2D[i];
    }
    public int i = 0;
    public void OnAddLightmapTexs(Texture2D texture2D)
    {
        lightmapTextures[i] = texture2D;
        i++;
    }


    public void OnLoadLightmap()
    {
        //for (int i = 0; i < lightmapTextures.Length - 1; i++)
        //{
        //    string a1 = lightmapTextures[i].name;    //我们抓取当前字符当中的123
        //    string b1 = System.Text.RegularExpressions.Regex.Replace(a1, @"[^0-9]+", "");
        //    for (int j = 0; j < lightmapTextures.Length - 1 - i; j++)
        //    {
        //        string a2 = lightmapTextures[i].name;    //我们抓取当前字符当中的123
        //        string b2 = System.Text.RegularExpressions.Regex.Replace(b1, @"[^0-9]+", "");
        //        if (int.Parse(b2) > int.Parse(a2)) 
        //        {
        //            Texture2D temp = lightmapTextures[j + 1];
        //            lightmapTextures[j + 1] = lightmapTextures[j];
        //            lightmapTextures[j] = temp;
        //        }
        //    }
        //}
        //for (int i = 0; i < lightmapTexs.Length; i++)
        //{
        //    string str = lightmapTexs[i].name;    //我们抓取当前字符当中的123
        //    string result = System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", "");
        //    Debug.Log(result);
        //}
        //if (null == rendererList || rendererList.Length == 0)
        //{
        //    Debug.Log(gameObject.name + " 的 光照信息为空");
        //    return;
        //}
        Renderer[] renders = GetComponentsInChildren<Renderer>(true);

        for (int r = 0, rLength = renders.Length; r < rLength; ++r)
        {
            renders[r].lightmapIndex = rendererList[r].lightmapIndex;
            renders[r].lightmapScaleOffset = rendererList[r].lightmapOffsetScale;
        }

        if (null == lightmapTexs || lightmapTexs.Length == 0)
        {
            return;
        }
        LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
        LightmapData[] ldata = new LightmapData[lightmapTexs.Length];
        LightmapSettings.lightmaps = null;
        for (int t = 0, tLength = lightmapTexs.Length; t < tLength; ++t)
        {
            ldata[t] = new LightmapData();
            ldata[t].lightmapColor = lightmapTexs[t];
        }
        LightmapSettings.lightmaps = ldata;


        ////rendererList贴图信息
        //if (null == rendererList || rendererList.Length == 0)
        //{
        //    Debug.Log(gameObject.name + " 的 光照信息为空");
        //    return;
        //}

        //Renderer[] renders = GetComponentsInChildren<Renderer>(true);

        //for (int r = 0, rLength = renders.Length; r < rLength; ++r)
        //{
        //    renders[r].lightmapIndex = rendererList[r].lightmapIndex;
        //    renders[r].lightmapScaleOffset = rendererList[r].lightmapOffsetScale;
        //}
        ////lightmapTextures  光照贴图
        //if (null == lightmapTextures || lightmapTextures.Length == 0)
        //{
        //    return;
        //}

        //LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
        //LightmapData[] ldata = new LightmapData[lightmapTextures.Length];
        //LightmapSettings.lightmaps = null;

        //for (int t = 0, tLength = lightmapTextures.Length; t < tLength; ++t)
        //{
        //    ldata[t] = new LightmapData();
        //    ldata[t].lightmapColor = lightmapTextures[t];
        //}

        //LightmapSettings.lightmaps = ldata;
        //Debug.Log(ldata);

    }
}
