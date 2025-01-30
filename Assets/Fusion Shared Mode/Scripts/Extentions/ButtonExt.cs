using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MaAvatar.Extentions
{
    public static class ButtonExt
    {
        public static void AssignListerner(this Button button, UnityAction action)
        {
            if (button == null)
            {
                Debug.LogError("Button not assigned");
                return;
            }

            button.onClick.RemoveListener(action);
            button.onClick.AddListener(action);
        }
    }
}
