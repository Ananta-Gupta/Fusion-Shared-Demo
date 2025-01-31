using Fusion;
using UnityEngine;
using Fusion.Async;
using Fusion.Photon.Realtime;

namespace MaAvatar.Utils
{
    public class StartGameUtils : MonoBehaviour
    {
        internal static StartGameUtils Instance { get; private set; }

        [SerializeField] NetworkRunner networkRunner;

        /// <summary>
        /// The maximum number of players available in this sample.
        /// </summary>
        public const int MAX_PLAYER_COUNT = 8;  // to be set in game config api 

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
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

        private void OnDestroy()
        {
            // Clears the global app settings to an empty string or "best region" when leaving the game.
            PhotonAppSettings.Global.AppSettings.FixedRegion = string.Empty;
        }

        public static async void AddPlayer(string sceneToLoad, bool setRegionCode, string regionCode, System.Action OnFinishAction)
        {
            // If this is the first network runner, we show the loading screen.
            /*if (NetworkRunner.Instances.Count == 0)
                LoadingScreen.ShowLoadingScreen();*/

            var newRunner = Instantiate(Instance.networkRunner);

            // Assign a random name to differentiate the runners.
            newRunner.name = "NetworkRunner:  " + System.Guid.NewGuid().ToString();

            // We set the region to the global settings
            if (setRegionCode)
            {
                // These are the global 
                var appSettings = PhotonAppSettings.Global;
                appSettings.AppSettings.FixedRegion = regionCode;
            }

            StartGameArgs startGameArgs = new()
            {
                GameMode = GameMode.Shared,
                PlayerCount = MAX_PLAYER_COUNT,
            };

            var result = await newRunner.StartGame(startGameArgs);

            if (!result.Ok)
            {
                Debug.LogError(result.ErrorMessage);
                //LoadingScreen.HideLoadingScreen();
                return;
            }

            if (newRunner.IsSharedModeMasterClient)
            {
                NetworkSceneAsyncOp sceneLoad = newRunner.LoadScene(sceneToLoad, UnityEngine.SceneManagement.LoadSceneMode.Single);

                // We wait until the scene is done loading.
                while (!sceneLoad.IsDone)
                {
                    // TaskManager.Delay is to prevent issues with threading and WebGL.
                    await TaskManager.Delay(1000);
                }

                //Debug.Break();
            }
            else if (NetworkRunner.Instances.Count > 1)
            {
                newRunner.ProvideInput = false;
                newRunner.SetVisible(false);
            }

            OnFinishAction?.Invoke();
        }
    }
}
