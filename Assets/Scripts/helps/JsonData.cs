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
        public class UserData
        {
            public string account;
            public string id;
        }

        [Serializable]
        public class UserResult
        {
            public Meta meta;
            public UserData[] data;
        }
    }
}