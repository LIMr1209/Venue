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
            public string qiniu_path_url; // ��ţ ��Ŀ¼
            public string fbx_file_url; // fbx url
            public string fbx_name; // fbx 
            public string ab_name; // ����ab��
            public string qiniu_path; //���������ĸ�Ŀ¼
        }
        
        [Serializable]
        public class ArtData
        {
            public string name; // ��������
            public string id; // ��Ʒid
            public string imageUrl; // ����ͼƬ��
            public float[] scale; // ��������ֵ
            public float[] rotate; // ������תֵ
            public float[] position; // ��������ֵ
            public float[] quaternion;//������ת��Ԫ��
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