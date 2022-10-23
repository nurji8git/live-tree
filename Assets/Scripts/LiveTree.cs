using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class LiveTree : ScriptableObject
    {
        [SerializeField]
        public GameObject plant;
        [MenuItem("Tools/MyTool/Do It in C#")]
        static void DoIt()
        {
        }
    }
}