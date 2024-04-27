using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using CrowdControlVNyanPlugin.CrowdControl.TRPC.Entities;

namespace CrowdControlVNyanPlugin
{
    public class CrowdControlVNyanPlugin : MonoBehaviour, VNyanInterface.IButtonClickedHandler
    {
        public GameObject windowPrefab;
        public GameObject triggerBrowserItemPrefab;
        public GameObject triggerHistoryItemPrefab;
        public Material screenMaterial;
        public Texture baseScreenTexture;

        public GameObject window;
        
        public GameObject triggerBrowserTab;
        public GameObject triggerBrowserBody;
        public GameObject triggerBrowserContent;
        public GameObject triggerBrowserSessionText;
        public Button triggerBrowserButton;
        
        public GameObject triggerHistoryTab;
        public GameObject triggerHistoryBody;
        public GameObject triggerHistoryContent;
        public GameObject triggerHistorySessionText;
        public Button triggerHistoryButton;
        public RawImage displayScreenRenderer;
        public string ccToken;
        public MainThreadDispatcher mainThread;

        private VNyanHelper _VNyanHelper;
        private VNyanTriggerDispatcher triggerDispatcher;
        private CrowdControlManager crowdControlManager;
        

        public void Start()
        {
        }

        public void Awake()
        {
            
            Debug.Log($"Crowd Control Plugin is Awake!");
            _VNyanHelper = new VNyanHelper();
            ccToken = "";
            Debug.Log($"Loading Settings");
            // Load settings
            loadPluginSettings();

            Debug.Log($"Beginning Plugin Setup");
            
            mainThread = gameObject.AddComponent<MainThreadDispatcher>();
            triggerDispatcher = gameObject.AddComponent<VNyanTriggerDispatcher>();
            
            try
            {
                window = _VNyanHelper.pluginSetup(this, "Crowd Control", windowPrefab);
            } catch(Exception e)
            {
                Debug.Log(e.ToString());
            }
            
            // Hide the window by default
            if (window != null)
            {

                GameObject displayScreen = window.transform.Find("Panel/SessionDetails/GameImage").gameObject;
                displayScreenRenderer = displayScreen.GetComponent<RawImage>();

                triggerBrowserTab = window.transform.Find("Panel/Tabs/TriggerBrowser").gameObject;
                triggerBrowserBody = window.transform.Find("Panel/Tabs/TriggerBrowser/ScrollView").gameObject;
                triggerBrowserContent = window.transform.Find("Panel/Tabs/TriggerBrowser/ScrollView/Viewport/Content").gameObject;
                triggerBrowserSessionText = window.transform.Find("Panel/Tabs/TriggerBrowser/SessionText").gameObject;
                triggerBrowserButton = window.transform.Find("Panel/Tabs/TriggerBrowserButton").GetComponent<Button>();
                
                triggerHistoryTab = window.transform.Find("Panel/Tabs/TriggerHistory").gameObject;
                triggerHistoryBody = window.transform.Find("Panel/Tabs/TriggerHistory/ScrollView").gameObject;
                triggerHistoryContent = window.transform.Find("Panel/Tabs/TriggerHistory/ScrollView/Viewport/Content").gameObject;
                triggerHistorySessionText = window.transform.Find("Panel/Tabs/TriggerHistory/SessionText").gameObject;
                triggerHistoryButton = window.transform.Find("Panel/Tabs/TriggerHistoryButton").GetComponent<Button>();
                
                crowdControlManager = gameObject.AddComponent<CrowdControlManager>();
                window.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                window.SetActive(false);

                setStatusTitle("Initializing");

                try
                {
                    Debug.Log($"Preparing Plugin Window");
                    setGameSessionInfo("[No Active Session]", "", "");

                    window.transform.Find("Panel/TitleBar/CloseButton").GetComponent<Button>().onClick.AddListener(() => { closePluginWindow(); });
                    window.transform.Find("Panel/StatusControls/AuthorizeButton").GetComponent<Button>().onClick.AddListener(() => {
                        initCrowdControl();
                    });
                    window.transform.Find("Panel/StatusControls/DeauthorizeButton").GetComponent<Button>().onClick.AddListener(() => {
                        deInitCrowdControl();
                    });

                    triggerBrowserButton.onClick.AddListener(() => {
                        tabToTriggerBrowser();
                    });
                    triggerHistoryButton.onClick.AddListener(() => {
                        tabToTriggerHistory();
                    });


                    tabToTriggerBrowser();
                }
                catch (Exception e)
                {
                    Debug.Log($"Couldn't prepare Plugin Window: {e.Message}");
                    setStatusTitle("Couldn't prepare Plugin Window");
                    Debug.Log(e.ToString());
                }

                try
                {
                    if(ccToken != null)
                    {
                        initCrowdControl();
                    }
                }
                catch (Exception e)
                {
                    setStatusTitle("Couldn't auto-initialize Crowd Control Connection");
                }
            }

            
        }

        public void tabToTriggerBrowser()
        {
            triggerBrowserTab.SetActive(true);
            triggerHistoryTab.SetActive(false);
            triggerBrowserButton.interactable = false;
            triggerHistoryButton.interactable = true;
        }

        public void tabToTriggerHistory()
        {
            triggerBrowserTab.SetActive(false);
            triggerHistoryTab.SetActive(true);
            triggerBrowserButton.interactable = true;
            triggerHistoryButton.interactable = false;
        }

        public void initCrowdControl()
        {
            mainThread.Enqueue(() => {
                window.transform.Find("Panel/StatusControls/AuthorizeButton").gameObject.SetActive(false);
                window.transform.Find("Panel/StatusControls/DeauthorizeButton").gameObject.SetActive(true);
                crowdControlManager.initCrowdControl();
            }); 
        }

        public void deInitCrowdControl()
        {
            mainThread.Enqueue(() => {
                window.transform.Find("Panel/StatusControls/AuthorizeButton").gameObject.SetActive(true);
                window.transform.Find("Panel/StatusControls/DeauthorizeButton").gameObject.SetActive(false);
                crowdControlManager.deInitCrowdControl();
            });
        }

        public void Update()
        {

        }

        private void OnApplicationQuit()
        {
            // Save settings
            savePluginSettings();
        }

        public void loadPluginSettings()
        {
            // Get settings in dictionary
            Dictionary<string, string> settings = _VNyanHelper.loadPluginSettingsData("CrowdControlPlugin.cfg");
            if (settings != null)
            {
                // Read string value
                settings.TryGetValue("CCToken", out ccToken);

            } else
            {
                ccToken = "";
            }
        }

        public void savePluginSettings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings["CCToken"] = ccToken;

            _VNyanHelper.savePluginSettingsData("CrowdControlPlugin.cfg", settings);
        }

        public void pluginButtonClicked()
        {
            // Flip the visibility of the window when plugin window button is clicked
            Debug.Log("plugin button clicked");
            if (window != null)
            {
                window.SetActive(!window.activeSelf);
                if(window.activeSelf)
                    window.transform.SetAsLastSibling();
            }
                
        }

        public void closePluginWindow()
        {
            window.SetActive(false);
        }

        public void setStatusTitle(string titleText)
        {
            Text StatusTitle = window.transform.Find("Panel/StatusControls/Status Indicator").GetComponent<Text>();
            StatusTitle.text = titleText;
        }

        public void setConnectionStatusTitle(string titleText)
        {
            Text StatusTitle = window.transform.Find("Panel/StatusControls/Connection Status Indicator").GetComponent<Text>();
            StatusTitle.text = titleText;
        }

        public void setGameSessionInfo(string titleText, string releaseText, string platformText)
        {
            Text GameTitle = window.transform.Find("Panel/SessionDetails/SessionGameText").GetComponent<Text>();
            GameTitle.text = titleText;
            Text ReleaseDate = window.transform.Find("Panel/SessionDetails/SessionReleaseText").GetComponent<Text>();
            ReleaseDate.text = releaseText;
            Text PlatformText = window.transform.Find("Panel/SessionDetails/SessionPlatformsText").GetComponent<Text>();
            PlatformText.text = platformText;
        }

        public void clearBrowserList()
        {
            var children = new List<GameObject>();
            foreach (Transform child in triggerBrowserContent.transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }

        public void populateBrowserList()
        {
            clearBrowserList();
            int itemIndex = 0;
            foreach (KeyValuePair<string, GameEffectEntity> effectItem in crowdControlManager.getGamePack().effects.game)
            {
                GameObject listItem = GameObject.Instantiate(triggerBrowserItemPrefab);
                listItem.transform.SetParent(triggerBrowserContent.transform);
                listItem.GetComponent<EffectBrowserListItem>().PrepareUI(effectItem.Key, effectItem.Value, itemIndex);
                itemIndex++;
            }
            triggerBrowserContent.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, ((72 + 5) * itemIndex + 5));
        }

        public void clearHistoryList()
        {
            var children = new List<GameObject>();
            foreach (Transform child in triggerHistoryContent.transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }

        public void populateHistoryList()
        {
            clearHistoryList();
            int itemIndex = 0;
            List<TriggerHistoryRecord> flippedHistory = new List<TriggerHistoryRecord>(crowdControlManager.triggerHistory);
            flippedHistory.Reverse();
            foreach (TriggerHistoryRecord triggerItem in flippedHistory)
            {
                GameObject listItem = GameObject.Instantiate(triggerHistoryItemPrefab);
                listItem.transform.SetParent(triggerHistoryContent.transform);
                listItem.GetComponent<EffectHistoryListItem>().PrepareUI(triggerItem, itemIndex);
                itemIndex++;
            }
            triggerHistoryContent.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, ((64 + 5) * itemIndex + 5));
        }



    }
}
