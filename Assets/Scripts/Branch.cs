using UnityEngine;

namespace System
{
    public class Branch : MonoBehaviour
    {
        public float thick;
        public float thick1;
        readonly float[] _thickness = new float[2];

         void Start()
        {
            _thickness[0] = thick;
            _thickness[1] = thick1;
        }
    }
}