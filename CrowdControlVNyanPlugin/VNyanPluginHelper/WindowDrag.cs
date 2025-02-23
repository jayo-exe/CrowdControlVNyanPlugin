﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace CrowdControlVNyanPlugin.VNyanPluginHelper
{
    class WindowDrag : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        public RectTransform dragRect;

        public void OnDrag(PointerEventData eventData)
        {
            dragRect.anchoredPosition += eventData.delta;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            dragRect.SetAsLastSibling();
            transform.SetAsLastSibling();
        }
    }
    
}
