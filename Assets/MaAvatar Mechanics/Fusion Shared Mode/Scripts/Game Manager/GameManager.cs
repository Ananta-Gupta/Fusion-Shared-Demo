using UnityEngine;
using MaAvatar.Utils;

namespace MaAvatar
{
    public class GameManager : MonoBehaviour
    {
        internal static GameManager Instance { get; private set; }

        [SerializeField] GameConfigs gameConfig;
        public GameConfigs GameConfig => gameConfig;

        private void Awake()
        {
            Debug.Log(this.name);

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                {
                    Destroy(this.gameObject);
                    return;
                }
            }
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
