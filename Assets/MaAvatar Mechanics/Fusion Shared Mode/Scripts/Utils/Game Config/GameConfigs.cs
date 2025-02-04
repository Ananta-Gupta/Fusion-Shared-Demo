using System;
using UnityEngine;
using System.Collections.Generic;

namespace MaAvatar.Utils
{
    [Serializable]
    public struct NetworkRegions
    {
        public string regionName;
        public string regionCode;
    }

    [CreateAssetMenu(fileName = "Game Config", menuName = "Utils/GameConfig")]
    public class GameConfigs : ScriptableObject
    {
        [Header("Scenes")]
        public string MenuSceneName = "MainMenuScene";
        public string GameSceneName = "GameplayScene";

        [Header("Region")]
        public List<NetworkRegions> networkRegions;
    }
}
