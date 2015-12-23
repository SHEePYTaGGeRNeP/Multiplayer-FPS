namespace Assets.Scripts
{
    using System.Collections.Generic;

    using UnityEngine;

    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance;

        public MatchSettings MatchSettings;


        void Awake()
        {
            if (Instance != null)
                Debug.LogError("More than one GameManager in scene.");
            else
                Instance = this;
        }

        #region Player tracking

        private const string PLAYER_ID_PREFIX = "Player ";

        private static Dictionary<string, Player> _players = new Dictionary<string, Player>();

        public static void RegisterPlayer(string netID, Player player)
        {
            string playerID = PLAYER_ID_PREFIX + netID;
            _players.Add(playerID, player);
            player.transform.name = playerID;
        }

        public static void UnRegisterPlayer(string playerID)
        {
            _players.Remove(playerID);
        }

        public static Player GetPlayer(string playerID)
        {
            return _players[playerID];
        }

        //void OnGUI()
        //{
        //    GUILayout.BeginArea(new Rect(200,200, 200, 500));
        //    GUILayout.BeginVertical();

        //    foreach (string playerID in _players.Keys)
        //    {
        //        GUILayout.Label(playerID + "  - " + _players[playerID].transform.name);
        //    }


        //    GUILayout.EndVertical();
        //    GUILayout.EndArea();
        //}


        #endregion


    }
}
