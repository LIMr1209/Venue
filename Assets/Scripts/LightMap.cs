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


#if UNITY_EDITOR
    [UnityEngine.SerializeField]
    Texture2D[] lightmapTexs;   //��ǰ�����ĵƹ���ͼ

#endif

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

        //���л�������ͼ
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
            Debug.Log(gameObject.name + " �� ������ϢΪ��");
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
        if (null == rendererList || rendererList.Length == 0)
        {
            Debug.Log(gameObject.name + " �� ������ϢΪ��");
            return;
        }

        Renderer[] renders = GetComponentsInChildren<Renderer>(true);

        for (int r = 0, rLength = renders.Length; r < rLength; ++r)
        {
            renders[r].lightmapIndex = rendererList[r].lightmapIndex;
            renders[r].lightmapScaleOffset = rendererList[r].lightmapOffsetScale;
        }


        if (null == lightmapTextures || lightmapTextures.Length == 0)
        {
            return;
        }

        LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
        LightmapData[] ldata = new LightmapData[lightmapTextures.Length];
        LightmapSettings.lightmaps = null;

        for (int t = 0, tLength = lightmapTextures.Length; t < tLength; ++t)
        {
            ldata[t] = new LightmapData();
            ldata[t].lightmapColor = lightmapTextures[t];
        }

        LightmapSettings.lightmaps = ldata;
    }
}
