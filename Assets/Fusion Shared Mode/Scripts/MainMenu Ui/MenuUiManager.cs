using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MaAvatar.Utils;
using MaAvatar.Extentions;
using Random = UnityEngine.Random;

namespace MaAvatar
{
    public class MenuUiManager : MonoBehaviour
    {
        [Header("Variables")]
        [SerializeField] GameObject rootObject;
        [SerializeField] TMP_InputField userNameInputField;
        [SerializeField] TMP_Dropdown regionDropdown;
        [SerializeField] Button startBtn;

        GameManager _gm;

        int currentSelectedRegion = 0;

        private void Awake()
        {
            _gm = GameManager.Instance;

            // If in the editor and both the main menu and gameplay scene are open, Unity will try to unload the gameplay scene.  This should only be able to occur when in the editor.
            if (!Application.isEditor)
                return;

            try
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_gm.GameConfig.GameSceneName);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        private void Start()
        {
            userNameInputField.text = $"Guest_{Random.Range(1000, 9999)}";

            Debug.Log(_gm);
            Debug.Log(_gm.GameConfig);
            regionDropdown.SetNetworkOptions(_gm.GameConfig.networkRegions);
            regionDropdown.AssignListerner((_value) => currentSelectedRegion = _value);

            startBtn.AssignListerner(OnClickStartBtn);
        }

        void OnClickStartBtn()
        {
            StartGameUtils.AddPlayer(_gm.GameConfig.GameSceneName, true, _gm.GameConfig.networkRegions[currentSelectedRegion].regionCode, OnSceneLoaded);
        }

        void OnSceneLoaded()
        {
            //hide loading
            rootObject.SetActive(false);
        }
    }
}
