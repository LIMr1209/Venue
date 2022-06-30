using System;

namespace DefaultNamespace
{
    public class JsonData
    {
        [Serializable]
        public class Meta
        {
            public int status_code;
            public string message;
        }
        [Serializable]
        public class ThumbPath
        {
            public string avb;
        }

        [Serializable]
        public class CoverData
        {
            public string id;
            public string name;
            // public Dictionary<string, string> thumb_path;
            public ThumbPath thumb_path;
        }
        [Serializable]
        public class UserData
        {
            public string nickname;
            public string logo_url;
            public string account;
            public string id;
        }
        [Serializable]
        public class ProjectData
        {
            public string id;
            public int scene_id;
            public RoomData room_info;
        }
        [Serializable]
        public class RoomData
        {
            public int id;
            public string user_project_id;
        }
        [Serializable]
        public class WorkData
        {
            public int id;
            public string title;
            public string description;
            public int is_nft;
            public CoverData cover;
            public UserData user;
            public string user_project_id;
        }
        
        [Serializable]
        public class memberData
        {
            public UserData host_team;
            public UserData[] invited_user;
            public UserData[] stranger;
        }
        
        
        [Serializable]
        public class sceneData
        {
            public string name;
            public string qiniu_path_url; // 七牛 根目录
            public string fbx_file_url; // fbx url
            public string fbx_name; // fbx 
            public string ab_name; // 场景ab包
            public string qiniu_path; //不带域名的根目录
        }
        
        [Serializable]
        public class ArtData
        {
            public string name; // 画框名字
            public string id; // 作品id
            public string imageUrl; // 画框图片名
            public float[] scale; // 画框缩放值
            public float[] rotate; // 画框旋转值
            public float[] position; // 画框坐标值
            public float[] quaternion;//画框旋转四元数
        }

        [Serializable]
        public class ListResult<T>
        {
            public Meta meta;
            public T[] data;
        }
        public class ViewResult<T>
        {
            public Meta meta;
            public T data;
        }
        
        public class ErrorResult
        {
            public Meta meta;
        }
    }
}