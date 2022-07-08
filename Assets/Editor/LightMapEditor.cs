using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightMapEditor : MonoBehaviour
{
    [MenuItem("Ojcgames Tools/����ó���Ԥ�Ƽ��ĺ決��Ϣ", false, 0)]
      static void SaveLightmapInfoByGameObject()
      {  
          GameObject go = Selection.activeGameObject;
  
         if(null == go)return;

        LightMap data = go.GetComponent<LightMap>();
         if (data == null)
         {
             data = go.AddComponent<LightMap>();
         }
         //save lightmapdata info by mesh.render
         data.SaveLightmap();

         EditorUtility.SetDirty(go);
         //applay prefab
         PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(go), ReplacePrefabOptions.ConnectToPrefab);
      }
}
