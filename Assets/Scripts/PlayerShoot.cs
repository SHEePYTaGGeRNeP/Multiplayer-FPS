namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.Networking;

    using Debug = UnityEngine.Debug;

    //    [RequireComponent(typeof(WeaponManager))]
    class PlayerShoot : NetworkBehaviour
    {
        private const string _PLAYER_TAG = "Player";

        [SerializeField]
        private LayerMask _raycastLayerMask;

        [SerializeField]
        private Camera _cam;

        private WeaponManager _weaponManager;
        private PlayerWeapon _currentWeapon;


        private void Start()
        {
            if (this._cam == null)
            {
                Debug.LogError("No camera referenced on PlayerShoot");
                this.enabled = false;
            }

            this._weaponManager = this.GetComponent<WeaponManager>();
        }

        private void Update()
        {
            this._currentWeapon = this._weaponManager.CurrentWeapon;
            if (this._currentWeapon.fireRate <= 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    this.Shoot();
                }
            }
            else
            {
                if (Input.GetButton("Fire1"))
                {
                    this.InvokeRepeating("Shoot", 0f, 1f / this._currentWeapon.fireRate);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    this.CancelInvoke("Shoot");
                }
            }
        }

        // Is called on the server when we hit something.
        [Command]
        private void CmdOnHit(Vector3 hitPosition, Vector3 normal)
        {
            this.RpcDoHitEffect(hitPosition, normal);
        }
        // is called on all clients when a client hits something.
        [ClientRpc]
        private void RpcDoHitEffect(Vector3 hitPosition, Vector3 normal)
        {
            GameObject hitEffect = (GameObject)Instantiate(this._weaponManager.CurrentGraphics.HitEffectPrefab, hitPosition, Quaternion.LookRotation(normal));
            Destroy(hitEffect, 2f);

        }

        // Is called on the server when a player shoots
        [Command]
        private void CmdOnShoot()
        {
            this.RpcDoShootEffect();
        }

        // is called on all clients when a client shoots.
        [ClientRpc]
        private void RpcDoShootEffect()
        {
            this._weaponManager.CurrentGraphics.MuzzleFlash.Play();
        }


        [Client]
        private void Shoot()
        {
            if (!this.isLocalPlayer) return;

            // we are shooting, call the OnShootMethod on hte server
            this.CmdOnShoot();

            RaycastHit hit;
            if (Physics.Raycast(this._cam.transform.position, this._cam.transform.forward, out hit, this._currentWeapon.Range, this._raycastLayerMask))
            {
                if (hit.collider.tag == _PLAYER_TAG)
                    this.CmdPlayerShot(hit.collider.name, this._currentWeapon.Damage);
                // we hit something, call the CmdOnHit method on the server.
                this.CmdOnHit(hit.point, hit.normal);
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
