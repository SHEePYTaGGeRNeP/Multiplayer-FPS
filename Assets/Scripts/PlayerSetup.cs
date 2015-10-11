using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class PlayerSetup : NetworkBehaviour
    {
        [SerializeField]
        private Behaviour[] _componentsToDisable;

        private Camera _sceneCamera;

        void Start()
        {
            if (!this.isLocalPlayer)
            {
                foreach (Behaviour lvT in this._componentsToDisable)
                    lvT.enabled = false;
            }
            else
            {
                this._sceneCamera = Camera.main;
                if (this._sceneCamera != null) this._sceneCamera.gameObject.SetActive(false);
                Camera.main.gameObject.SetActive(false);
            }
        }

        void OnDisable()
        {
            if (this._sceneCamera != null)
                this._sceneCamera.gameObject.SetActive(true);
        }
    }
}
