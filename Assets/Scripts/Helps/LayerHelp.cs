using UnityEngine;

namespace DefaultNamespace
{
    public class LayerHelp
    {
        public static int focusArtLayerNum = LayerMask.NameToLayer(Globle.focusArtLayer);
        public static int lockArtLayerNum = LayerMask.NameToLayer(Globle.lockArtlayer);
        public static int frameLayerNum = LayerMask.NameToLayer(Globle.frameLayer);
        public static int playerLayerNum = LayerMask.NameToLayer(Globle.playerLayer);
        public static int navMeshLayerNum = LayerMask.NameToLayer(Globle.navMeshLayer);
    }
}