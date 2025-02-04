using Fusion;
using System;
using UnityEngine;
using Fusion.Sockets;
using System.Collections.Generic;

namespace MaAvatar.Utils
{
    public class PlayerSpawner : SimulationBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] GameObject playerPrefab;

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.LocalPlayer == player)
            {
                runner.Spawn(playerPrefab, inputAuthority: player);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsSharedModeMasterClient)
                return;

            var networkObjects = runner.GetAllNetworkObjects();
            foreach (var obj in networkObjects)
            {
                // If the state authority cannot be overridden, it is skipped.
                if ((obj.Flags & NetworkObjectFlags.AllowStateAuthorityOverride) != NetworkObjectFlags.AllowStateAuthorityOverride ||
                    (obj.Flags & NetworkObjectFlags.MasterClientObject) == NetworkObjectFlags.MasterClientObject ||
                    (obj.Flags & NetworkObjectFlags.DestroyWhenStateAuthorityLeaves) == NetworkObjectFlags.DestroyWhenStateAuthorityLeaves)
                {
                    continue;
                }

                // If the state authority of the object is equal to the player who left, we trasnfer ownership to the shared mode master client.
                if (obj.StateAuthority == player)
                    obj.RequestStateAuthority();
            }
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            // Clears the static dictionarys that use runners as keys.
            /*Player.LocalPlayerDictionary?.Remove(runner);
            Player.PlayerListDictionary?.Remove(runner);
            PlayerTracker.PlayerTrackers?.Remove(runner);
            PlayerUI.UIInstances?.Remove(runner);*/

            if (NetworkRunner.Instances.Count == 0)
            {
                // show loading
                var async = (UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0, UnityEngine.SceneManagement.LoadSceneMode.Single));
                // Stop loading when scene loading complete
                //async.completed += (AsyncOperation op) => { LoadingScreen.HideLoadingScreen(); MusicManager.PlaySong(-1); };
            }
        }

        #region Network Runner Callbacks
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        #endregion
    }
}