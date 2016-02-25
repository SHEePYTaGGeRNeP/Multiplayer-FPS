using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Player))]
    public class PlayerSetup : NetworkBehaviour
    {
        [SerializeField]
        private Behaviour[] _componentsToDisable;

        [SerializeField]
        private string _remoteLayerName = "RemotePlayer";
        [SerializeField]
        private string _dontDrawLayerName = "DontDraw";
        [SerializeField]
        private GameObject _playerGraphics;
        [SerializeField]
        private GameObject _playerUIPrefab;
        private GameObject _playerUIInstance;


        private Camera _sceneCamera;

        void Start()
        {
            if (!this.isLocalPlayer)
            {
                this.DisableComponents();
                this.AssignRemoteLayer();
            }
            else
            {
                this._sceneCamera = Camera.main;
                if (this._sceneCamera != null) this._sceneCamera.gameObject.SetActive(false);
                if (Camera.main != null)
                    Camera.main.gameObject.SetActive(false);

                // Disable player graphics for local player
                this.SetLayerRecursively(this._playerGraphics, LayerMask.NameToLayer(this._dontDrawLayerName));

                // Instantiate PlayerUI
                this._playerUIInstance = Instantiate(this._playerUIPrefab);
                this._playerUIInstance.name = this._playerUIPrefab.name;
            }

            this.GetComponent<Player>().Setup();
        }

        private void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
                this.SetLayerRecursively(child.gameObject, layer);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            string netIDParam = this.GetComponent<NetworkIdentity>().netId.ToString();
            Player player = this.GetComponent<Player>();

            GameManager.RegisterPlayer(netIDParam, player);
        }

        private void AssignRemoteLayer()
        {
            this.gameObject.layer = LayerMask.NameToLayer(this._remoteLayerName);
        }
        private void DisableComponents()
        {
            foreach (Behaviour lvT in this._componentsToDisable)
                lvT.enabled = false;
        }

        void OnDisable()
        {
            if (this._playerUIInstance != null)
                Destroy(this._playerUIInstance);
            if (this._sceneCamera != null)
                this._sceneCamera.gameObject.SetActive(true);
            GameManager.UnRegisterPlayer(this.transform.name);
        }
    }
}
