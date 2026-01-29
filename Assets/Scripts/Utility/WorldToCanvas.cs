using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class WorldToCanvas : MonoBehaviour
    {
        public Transform m_anchorTransform;
        public Vector3 m_offset = Vector3.zero;
        private Canvas m_canvas;
        // Start is called before the first frame update
        void Start()
        {
            m_canvas = GetComponentInParent<Canvas>();
        }

        // Update is called once per frame
        void Update()
        {
            if ( m_canvas.enabled )
                ForceUpdate();
        }

        private void OnEnable()
        {
            ForceUpdate();
        }

        public void ForceUpdate()
        {
            if ( m_anchorTransform != null && Camera.main != null )
            {
                Vector3 pos = Camera.main.WorldToScreenPoint( m_offset + m_anchorTransform.position );
                ( transform as RectTransform ).position = pos;
            }
        }
    }
}