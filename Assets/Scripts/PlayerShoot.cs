namespace Assets.Scripts
{
    using System.Diagnostics;

    using UnityEngine;
    using UnityEngine.Networking;

    using Debug = UnityEngine.Debug;

    class PlayerShoot : NetworkBehaviour
    {
        private const string _PLAYER_TAG = "Player";

        public PlayerWeapon Weapon;

        [SerializeField]
        private LayerMask _raycastLayerMask;

        [SerializeField]
        private Camera _cam;

        private void Start()
        {
            if (this._cam == null)
            {
                Debug.LogError("No camera referenced on PlayerShoot");
                this.enabled = false;
            }
        }

        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                this.Shoot();
            }
        }

        [Client]
        private void Shoot()
        {
            RaycastHit hit;
            if (Physics.Raycast(this._cam.transform.position, this._cam.transform.forward, out hit, this.Weapon.Range, this._raycastLayerMask ))
            {
                if (hit.collider.tag == _PLAYER_TAG)
                    this.CmdPlayerShot(hit.collider.name, this.Weapon.Damage);
            }
        }

        // only called on the server
        [Command]
        private void CmdPlayerShot(string playerID, int damage)
        {
            Debug.Log(playerID + " has been shot.");

            Player player = GameManager.GetPlayer(playerID);
            player.RpcTakeDamage(damage);
        }

    }
}
