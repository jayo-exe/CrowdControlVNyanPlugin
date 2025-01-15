using UnityEngine;
using UnityEngine.EventSystems;

namespace CrowdControlVNyanPlugin.VNyanPluginHelper
{
    class WindowSize : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        public RectTransform dragRect;
        public float minWidth;
        public float maxWidth;
        public float minHeight;
        public float maxHeight;

        public void OnDrag(PointerEventData eventData)
        {

            Vector2 mousePos = eventData.position - new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 aMin = dragRect.offsetMin;
            Vector2 aMax = dragRect.offsetMax;
            float newX = aMin.x + Mathf.Clamp((aMax.x + eventData.delta.x) - aMin.x, minWidth, maxWidth);
            float newY = aMax.y - Mathf.Clamp((aMax.y - aMin.y) - eventData.delta.y, minHeight, maxHeight);

            if (
                !(eventData.delta.x < 0 && mousePos.x > newX) 
                && !(eventData.delta.x > 0 && mousePos.x < newX)
            ) dragRect.offsetMax = new Vector2(newX, aMax.y);
            if (
                !(eventData.delta.y > 0 && mousePos.y < newY) 
                && !(eventData.delta.y < 0 && mousePos.y > newY)
            ) dragRect.offsetMin = new Vector2(aMin.x, newY);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            dragRect.SetAsLastSibling();
            transform.SetAsLastSibling();
        }
    }
}
