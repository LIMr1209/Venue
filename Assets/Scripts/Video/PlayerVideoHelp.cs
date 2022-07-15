using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace DefaultNamespace
{
    public class PlayerVideoHelp
    {

        public static IEnumerator AddVideoCompent(GameObject obj, string url)
        {
            VideoPlayer videoPlayer = obj.AddComponent<VideoPlayer>();

            videoPlayer.playOnAwake = false;

            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;

            videoPlayer.targetCameraAlpha = 1.0f;

            videoPlayer.url = url;
            videoPlayer.audioOutputMode = VideoAudioOutputMode.None;


            videoPlayer.isLooping = true;

            videoPlayer.prepareCompleted += PreparedPlayer;
            videoPlayer.errorReceived += ErrorPlayer;
            videoPlayer.Prepare();
            yield return null;
        }

        static void PreparedPlayer(VideoPlayer videoPlayer)
        {
            videoPlayer.Play();
            SizeAdaptation.SetSize(videoPlayer.gameObject, new Vector2(videoPlayer.width, videoPlayer.height));
        }

        static void ErrorPlayer(VideoPlayer videoPlayer, string message)
        {

        }
    }

}