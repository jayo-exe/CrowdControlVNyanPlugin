﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace CrowdControlVNyanPlugin.VNyanPluginHelper
{
    class WindowFocus : MonoBehaviour, IPointerDownHandler
    {
        public RectTransform focusRect;

        public void OnPointerDown(PointerEventData eventData)
        {
            focusRect.SetAsLastSibling();
            transform.SetAsLastSibling();
        }
    }
}
