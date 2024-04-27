using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CrowdControlVNyanPlugin.CrowdControl.TRPC.Entities;

namespace CrowdControlVNyanPlugin
{
    class EffectBrowserListItem: MonoBehaviour
    {
        public GameObject nameTextObject;
        public GameObject descriptionTextObject;
        public GameObject triggerTextObject;
        public GameObject copyButtonObject;
        public GameObject triggerDropdownObject;

        private string gameEffectID;
        private GameEffectEntity gameEffect;
        private Dictionary<string, string> triggerNames;
        private List<Dropdown.OptionData> triggerList;
        private string selectedtriggerName;

        public void PrepareUI(string effectID, GameEffectEntity effect, int index)
        {
            triggerNames = new Dictionary<string, string>();
            triggerList = new List<Dropdown.OptionData>();
            gameEffect = effect;
            gameEffectID = effectID;
            nameTextObject.GetComponent<Text>().text = gameEffect.name;
            descriptionTextObject.GetComponent<Text>().text = gameEffect.description;
            BuildTriggerDictionary();
            triggerDropdownObject.GetComponent<Dropdown>().AddOptions(triggerList);
            copyButtonObject.GetComponent<Button>().onClick.AddListener(() => { CopySelectedTrigger(); });
            triggerDropdownObject.GetComponent<Dropdown>().onValueChanged.AddListener((v) => { HandleTriggerKeyChange(v); });
            HandleTriggerKeyChange(0);
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2(5, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(-5, rectTransform.offsetMax.y);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, ((index * (72 + 5)) + 5) * -1);

        }

        //populate trigger name dictionary
        private void BuildTriggerDictionary()
        {
            Debug.Log("Adding Initial Trigger Items");
            triggerNames.Add("success", $"_xcc_esc_{gameEffectID}");
            triggerList.Add(new Dropdown.OptionData("success"));
            triggerNames.Add("request", $"_xcc_erq_{gameEffectID}");
            triggerList.Add(new Dropdown.OptionData("request"));
            triggerNames.Add("retry", $"_xcc_ert_{gameEffectID}");
            triggerList.Add(new Dropdown.OptionData("retry"));
            triggerNames.Add("refund", $"_xcc_erf_{gameEffectID}");
            triggerList.Add(new Dropdown.OptionData("refund"));
            triggerNames.Add("failure", $"_xcc_efl_{gameEffectID}");
            triggerList.Add(new Dropdown.OptionData("failure"));

            Debug.Log("Checking for Duration");
            if (gameEffect.duration == null || gameEffect.duration.value == 0)
            {
                return;
            }

            Debug.Log("Adding Timed Trigger Items");
            triggerNames.Add("timed-begin", $"_xcc_teu_{gameEffectID}_begin");
            triggerList.Add(new Dropdown.OptionData("timed-begin"));
            triggerNames.Add("timed-pause", $"_xcc_teu_{gameEffectID}_pause");
            triggerList.Add(new Dropdown.OptionData("timed-pause"));
            triggerNames.Add("timed-resume", $"_xcc_teu_{gameEffectID}_resume");
            triggerList.Add(new Dropdown.OptionData("timed-resume"));
            triggerNames.Add("timed-stop", $"_xcc_teu_{gameEffectID}_stop");
            triggerList.Add(new Dropdown.OptionData("timed-stop"));


        }

        //copy the currently-selected trigger name to the clipboard
        public void CopySelectedTrigger()
        {
            GUIUtility.systemCopyBuffer = selectedtriggerName;
        }

        //handle the selection change for the dropdown
        public void HandleTriggerKeyChange(int value)
        {
            selectedtriggerName = triggerNames[triggerList[value].text];
            triggerTextObject.GetComponent<Text>().text = selectedtriggerName;
        }


    }
}
