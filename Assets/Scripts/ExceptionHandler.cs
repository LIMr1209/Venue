using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ExceptionHandler : MonoBehaviour
    {
        //是否作为异常处理者  
        public bool isHandler = true;

        //是否退出程序当异常发生时  
        public bool isQuitWhenException = false;


        private void Awake()
        {
            //注册异常处理
            if (isHandler) Application.logMessageReceived += Handler;
        }

        void OnEnable()
        {
            // 取消注册
            if (isHandler) Application.logMessageReceived += Handler;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Handler;
        }


        // 异常处理事件
        private void Handler(string logString, string stackTrace, LogType type)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            if (type == LogType.Exception)
            {
                Tools.showDialog(logString);
            }
#endif
            //退出程序，bug反馈程序重启主程序  
            if (isQuitWhenException)
            {
                Application.Quit();
            }
        }
    }

    public class TempIsZeroException : ApplicationException
    {
        public TempIsZeroException(string message) : base(message)
        {
        }
    }
}