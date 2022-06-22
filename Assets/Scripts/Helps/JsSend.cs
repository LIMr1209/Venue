using UnityEngine;

namespace DefaultNamespace
{
    // js send message unity 接收
    public class JsSend : MonoBehaviour
    {
        // 获取初始化参数 场景url 实例化场景对象
        public void GetInitParam(string strParams)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
        {
            string[] words = strParams.Split('#');
            Globle.sceneId = words[0];
            Debug.Log("前端发送参数 sceneId: "+Globle                                                                                                                                                                                  .sceneId);
            
            if (string.IsNullOrEmpty(Globle.sceneId))
            {
                // 默认脚本组件禁用, 接收到前端的参数 启用脚本组件
                FindObjectOfType<AbInit>().enabled = true;
                FindObjectOfType<InitialScene>().enabled = true;
            }
        }
        
        // 接收画框名字 作品id 作品url  替换场景中画框的材质贴图
        public void ReplaceArtImage()
        {
            
        }
        // 接收画框id 聚焦画框
        public void FocusArt()
        {
            
        }


    }
}