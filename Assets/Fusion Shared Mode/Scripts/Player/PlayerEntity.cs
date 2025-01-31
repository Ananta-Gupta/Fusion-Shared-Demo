using Fusion;
using System.Collections.Generic;

namespace MaAvatar
{
    public class PlayerEntity : NetworkBehaviour
    {
        /// <summary>
        /// A static dictionary of the local player, using the NetworkRunner as the key to account for multi-peer mode.
        /// </summary>
        public static Dictionary<NetworkRunner, PlayerEntity> LocalPlayerDictionary { get; set; }

        /// <summary>
        /// A static dictionary that contains lists of all the players in the game, using NetworkRunners as the keys to account for multi-peer mode
        /// </summary>
        public static Dictionary<NetworkRunner, List<PlayerEntity>> PlayerListDictionary { get; set; }

        public override void Spawned()
        {
            base.Spawned();

            if (HasInputAuthority)
            {
                if (LocalPlayerDictionary == null)
                    LocalPlayerDictionary = new Dictionary<NetworkRunner, PlayerEntity>() { { Runner, this } };
                else
                    LocalPlayerDictionary.Add(Runner, this);
            }

            PlayerListDictionary ??= new Dictionary<NetworkRunner, List<PlayerEntity>>();

            if (PlayerListDictionary.ContainsKey(Runner))
                PlayerListDictionary[Runner].Add(this);
            else
                PlayerListDictionary.Add(Runner, new List<PlayerEntity>() { this });
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            if (LocalPlayerDictionary.ContainsKey(runner))
            {
                if (LocalPlayerDictionary[runner] == this)
                    LocalPlayerDictionary.Remove(runner);
            }

            if (PlayerListDictionary != null && PlayerListDictionary.ContainsKey(runner))
                PlayerListDictionary[runner].Remove(this);
        }

        private void OnDestroy()
        {

        }
    }
}
