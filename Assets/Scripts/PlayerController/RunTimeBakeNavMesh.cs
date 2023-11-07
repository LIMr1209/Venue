using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class RunTimeBakeNavMesh : MonoBehaviour
    {
        private NavMeshSurface _nm;
        private void Awake()
        {
            _nm = GetComponent<NavMeshSurface>();
            
        }

        public void BakeNav()
        {
            _nm.BuildNavMesh(); // 动态烘培导航区域
        }
    }
}