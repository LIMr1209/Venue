using System.Runtime.InteropServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class Tools : MonoBehaviour
    {
        
        [DllImport("__Internal")]
        public static extern int GetWindowWidth();
        [DllImport("__Internal")]
        public static extern int GetWindowHeight();
        [DllImport("__Internal")]
        public static extern void ResetCanvasSize(int width, int height);
        [DllImport("__Internal")]
        public static extern string GetProjectId();
        [DllImport("__Internal")]
        private static extern string GetUserToken(string tokenName);
        
        [DllImport("__Internal")]
        public static extern string GetInviteCode();
        
        [DllImport("__Internal")]
        public static extern void showDialog(string message);
        

        public static string GetToken()
        {
            return GetUserToken(Globle.tokenName);
        }
    }
}