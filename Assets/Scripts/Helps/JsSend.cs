using UnityEngine;

namespace DefaultNamespace
{
    // js send message unity 接收
    public class JsSend : MonoBehaviour
    {
        // 获取初始化参数 场景id 项目id 邀请码 
        public void GetInitParam(string strParams)
        {
            string[] words = strParams.Split('#');
            Globle.sceneId = words[0];
            Globle.projectId = words[1];
            Globle.inviteCode = words[2];
            Globle.token = words[3];
            Debug.Log("前端发送参数 sceneId: "+Globle.sceneId);
            Debug.Log("前端发送参数 projectId: "+Globle.projectId);
            Debug.Log("前端发送参数 inviteCode: "+Globle.inviteCode);
            Debug.Log("前端发送参数 token: "+Globle.token);
            
            if (string.IsNullOrEmpty(Globle.sceneId))
            {
                // 默认脚本组件禁用, 接收到前端的参数 启用脚本组件
                FindObjectOfType<AbInit>().enabled = true;
                FindObjectOfType<InitialScene>().enabled = true;
            }
        }
    }
}