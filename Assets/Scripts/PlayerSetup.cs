using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
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
                Camera.main.gameObject.SetActive(false);
            }
            this.RegisterPlayer();
        }

        private void RegisterPlayer()
        {
            string id = "Player " + this.GetComponent<NetworkIdentity>().netId;
            this.transform.name = id;
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
        }
    }
}
