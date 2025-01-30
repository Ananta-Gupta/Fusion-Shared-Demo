using TMPro;
using UnityEngine;
using MaAvatar.Utils;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MaAvatar.Extentions
{
    public static class DropdownExt
    {
        public static void AssignListerner(this TMP_Dropdown dropdown, UnityAction<int> action)
        {
            if (dropdown == null)
            {
                Debug.LogError("Dropdown not assigned");
                return;
            }

            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.onValueChanged.AddListener(action);
        }

        public static void SetNetworkOptions(this TMP_Dropdown dropdown, List<NetworkRegions> networkList)
        {
            if (dropdown == null)
            {
                Debug.LogError("Dropdown not assigned");
                return;
            }

            dropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> regionOptionList = new List<TMP_Dropdown.OptionData>();
            foreach (var region in networkList)
                regionOptionList.Add(new TMP_Dropdown.OptionData(region.regionName));
            dropdown.AddOptions(regionOptionList);
        }
    }
}
