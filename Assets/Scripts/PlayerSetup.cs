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
            }

            this.GetComponent<Player>().Setup();
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
            if (this._sceneCamera != null)
                this._sceneCamera.gameObject.SetActive(true);
            GameManager.UnRegisterPlayer(this.transform.name);
        }
    }
}
