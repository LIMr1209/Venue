using System.Collections.Generic;
using UnityEngine;

// 方案一
//发现unity存在使用assetbundle加载资源会丢失shader的问题
// 第一步，在将用到的Shader加到Editor->Graphics Settings的Shader列表里再进行打包（依赖打包）  
// 第二步 在代码中给shader再次赋值，代码如下：
namespace DefaultNamespace
{
    public class ShaderProblem
    {
        public static void ResetShader(Object obj)

        {
            List<Material> listMat = new List<Material>();

            listMat.Clear();


            if (obj is Material)

            {
                Material m = obj as Material;

                listMat.Add(m);
            }

            else if (obj is GameObject)

            {
                GameObject go = obj as GameObject;

                SkinnedMeshRenderer[] rends = go.GetComponentsInChildren<SkinnedMeshRenderer>();

                if (null != rends)

                {
                    foreach (SkinnedMeshRenderer item in rends)

                    {
                        Material[] materialsArr = item.sharedMaterials;

                        foreach (Material m in materialsArr)

                            listMat.Add(m);
                    }
                }
            }


            for (int i = 0; i < listMat.Count; i++)

            {
                Material m = listMat[i];

                if (null == m)

                    continue;

                var shaderName = m.shader.name;

                var newShader = Shader.Find(shaderName);

                if (newShader != null)

                    m.shader = newShader;
            }
        }
    }
}

// 方案二
// 第一步，在将用到的Shader加到Editor->Graphics Settings的Shader列表里再进行打包（依赖打包）  
// shader放到Resources文件夹，必须是Resources文件夹里。