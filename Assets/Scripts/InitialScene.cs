using System;
using System.Collections.Generic;
using UnityEngine;
using static DefaultNamespace.JsonData;

namespace DefaultNamespace
{
    public class InitialScene : MonoBehaviour
    {
        // 初始化 加载场景预制体
        // 在Unity3D中Project视窗中创建文件夹：Resources。
        // 将需要动态加载的文件放入其中，例如Texture，Sprite，prefab等等。
        // 在脚本中调用API接口Resources.Load()相关接口即可。
        // 此种方式只能访问Resources文件夹下的资源。
        public string sceneModel = "scene";

        private void Awake()
        {
            // int width = Tools.GetWindowWidth();
            // int height = Tools.GetWindowHeight();
            // Screen.SetResolution(width, height, false);
        }

        private void Start()
        {
            TestRequest();
            StartCoroutine(
                GameManager.instances.OnWebRequestLoadAssetBundleGameObject(sceneModel, "", (obj) =>
                    {
                        AddController controller = FindObjectOfType<AddController>();
                        controller.AddThird();
                    }
                ));
        }

        private void Update()
        {
        }

        private void TestRequest()
        {
            if (!Application.isEditor)
            {
                string token = "wsDBAB2J6kwzqEnDZ1jNnAz94UjIEJbRAOBjYZen3PSGpixLAhqq7DwiuyRv";
                // string token = Tools.GetToken();
                Debug.Log("token: " + token);
                string projectId = "629f0cdc0ada86dc36e9c4cd";
                // string projectId = Tools.GetProjectId();
                Debug.Log("项目Id: " + projectId);
                Dictionary<string, string> requestData = new Dictionary<string, string>();
                requestData["user_project_id"] = projectId;
                Request.instances.HttpSend(2, "get", requestData, (statusCode, error, body) =>
                {
                    ViewResult<ProjectData> projectResult = JsonUtility.FromJson<ViewResult<ProjectData>>(body);
                    int sceneId = projectResult.data.scene_id;
                    Debug.Log("场景Id: " + sceneId);
                    int roomId = projectResult.data.room_info.id;
                    Debug.Log("房间Id: " + roomId);
                    // 进入房间
                    Dictionary<string, string> requestData = new Dictionary<string, string>();
                    requestData["id"] = roomId.ToString();
                    requestData["invite_code"] = "M7RzMi"; // 邀请码 
                    requestData["token"] = token; // token 
                    Request.instances.HttpSend(4, "get", requestData, (statusCode, error, body) =>
                    {
                        ViewResult<RoomData> roomResult = JsonUtility.FromJson<ViewResult<RoomData>>(body);
                        string projectId = roomResult.data.user_project_id;

                        // 获取作品列表
                        Dictionary<string, string> workListRequest = new Dictionary<string, string>();
                        workListRequest["id"] = roomId.ToString();
                        workListRequest["token"] = token; // token 
                        Request.instances.HttpSend(3, "get", workListRequest, (statusCode, error, body) =>
                        {
                            ListResult<WorkData> workResult = JsonUtility.FromJson<ListResult<WorkData>>(body);
                            string workUrl = workResult.data[0].cover.thumb_path.avb;
                            Debug.Log("作品 url: "+workUrl);
                        });
                        // 获取房间成员
                        Dictionary<string, string> memberRequest = new Dictionary<string, string>();
                        memberRequest["id"] = roomId.ToString();
                        memberRequest["token"] = token; // token 
                        Request.instances.HttpSend(5, "get", memberRequest, (statusCode, error, body) =>
                        {
                            ViewResult<memberData> memberResult = JsonUtility.FromJson<ViewResult<memberData>>(body);
                            string nickname = memberResult.data.host_team.nickname;
                            Debug.Log("主办: "+nickname);
                        });
                    });
                });
            }
        }
    }
}