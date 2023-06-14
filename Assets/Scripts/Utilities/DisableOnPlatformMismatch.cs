using UnityEngine;

namespace Utilities
{
    public class DisableOnPlatformMismatch : MonoBehaviour
    {
        public RuntimePlatform runtimePlatform;

        private void OnEnable()
        {
            if (runtimePlatform == RuntimePlatform.WindowsPlayer &&
                Application.platform == RuntimePlatform.WindowsEditor) return;
            
            if (Application.platform != runtimePlatform) gameObject.SetActive(false);
        }
    }
}