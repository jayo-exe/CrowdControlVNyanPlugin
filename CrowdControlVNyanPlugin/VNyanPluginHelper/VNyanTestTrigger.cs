using System;
using System.Collections.Generic;
using System.Text;
using VNyanInterface;
using UnityEngine;

namespace CrowdControlVNyanPlugin.VNyanPluginHelper
{
    
    class VNyanTestTriggerData
    {
        public string triggerName = "";

        public int value1 = 0;
        public int value2 = 0;
        public int value3 = 0;

        public string text1 = "";
        public string text2 = "";
        public string text3 = "";
    }

    class VNyanTestTrigger: MonoBehaviour, ITriggerInterface
    {
        private VNyanTestTrigger _instance;
        private Action<string> triggerFired;
        private Queue<VNyanTestTriggerData> triggerQueue = new Queue<VNyanTestTriggerData>();

        public void registerTriggerListener(ITriggerHandler triggerHandler)
        {
            triggerFired += triggerHandler.triggerCalled;
        }

        public void callTrigger(string triggerName)
        {
            Debug.Log($"Enqueueing trigger {triggerName}");
            if (_instance == null)
            {
                return;
            }

            lock (triggerQueue)
            {
                VNyanTestTriggerData trigger = new VNyanTestTriggerData
                {
                    triggerName = triggerName,
                    value1 = 0,
                    value2 = 0,
                    value3 = 0,
                    text1 = "",
                    text2 = "",
                    text3 = ""
                };
                triggerQueue.Enqueue(trigger);
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Update()
        {
            
            if (_instance == null) return;
            

            lock (triggerQueue)
            {
                while (triggerQueue.Count > 0)
                {
                    VNyanTestTriggerData trigger = triggerQueue.Dequeue();
                    if (triggerFired != null) triggerFired.Invoke(trigger.triggerName);
                }
            }
        }
    }
}
