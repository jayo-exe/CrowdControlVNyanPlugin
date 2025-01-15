using UnityEngine;

namespace CrowdControlVNyanPlugin.VNyanPluginHelper
{
    class PluginLogger
    {
        protected string identifier = "Plugin";
        public void LogInfo(object message) => Debug.Log($"[{identifier}] {message}");
        public void LogWarning(object message) => Debug.LogWarning($"[{identifier}] {message}");
        public void LogError(object message) => Debug.LogError($"[{identifier}] {message}");

        public PluginLogger(string newIdentifier) {
            identifier = newIdentifier;
        }
    }
}