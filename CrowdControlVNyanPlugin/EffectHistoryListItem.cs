using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CrowdControlVNyanPlugin.CrowdControl.TRPC.Entities;

namespace CrowdControlVNyanPlugin
{
    class EffectHistoryListItem: MonoBehaviour
    {
        public GameObject nameTextObject;
        public GameObject senderTextObject;
        public GameObject triggerTextObject;
        public GameObject timeTextObject;
        public GameObject copyButtonObject;

        private TriggerHistoryRecord triggerHistoryRecord;


        public void PrepareUI(TriggerHistoryRecord record, int index)
        {
            triggerHistoryRecord = record;
            nameTextObject.GetComponent<Text>().text = triggerHistoryRecord.effectName;
            senderTextObject.GetComponent<Text>().text = triggerHistoryRecord.effectSender;
            triggerTextObject.GetComponent<Text>().text = triggerHistoryRecord.triggerName;
            timeTextObject.GetComponent<Text>().text = triggerHistoryRecord.timestamp;

            copyButtonObject.GetComponent<Button>().onClick.AddListener(() => { CopyTrigger(); });

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2(5, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(-5, rectTransform.offsetMax.y);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, ((index * (64 + 5)) + 5) * -1);

        }

        //copy the currently-selected trigger name to the clipboard
        public void CopyTrigger()
        {
            GUIUtility.systemCopyBuffer = triggerHistoryRecord.triggerName;
        }
    }
}
