using UnityEngine;

namespace DefaultNamespace
{
    public class LayerHelp
    {
        public static int focusArtLayerNum = LayerMask.NameToLayer(Globle.focusArtLayer);
        public static int lockArtLayerNum = LayerMask.NameToLayer(Globle.lockArtlayer);
        public static int playerLayerNum = LayerMask.NameToLayer(Globle.playerLayer);
        public static int groundLayerNum = LayerMask.NameToLayer(Globle.groundLayer);
        public static int wallLayerNum = LayerMask.NameToLayer(Globle.wallLayer);
    }
}