namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.Networking;
    using System.Collections;

    public class Player : NetworkBehaviour
    {
        [SyncVar]
        private bool _isDead = false;

        public bool IsDead
        {
            get { return this._isDead; }
            protected set { this._isDead = value; }
        }

        [SerializeField]
        private int maxHealth = 100;

        // When this value changes sends it to all clients
        [SyncVar]
        private int currentHealth;

        [SerializeField]
        private Behaviour[] _disableOnDeath;

        private bool[] _wasEnabled;

        void Awake()
        {
            this.Setup();
        }

        void Update()
        {
            if (!this.isLocalPlayer)
                return;
            if (Input.GetKeyDown(KeyCode.K))
                this.RpcTakeDamage(9999);
        }

        public void Setup()
        {
            this._wasEnabled = new bool[this._disableOnDeath.Length];
            for (int i = 0; i < this._wasEnabled.Length; i++)
            {
                this._wasEnabled[i] = this._disableOnDeath[i].enabled;
            }

            this.SetDefaults();
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(GameManager.Instance.MatchSettings.respawnTime);

            this.SetDefaults();
            Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
            this.transform.position = spawnPoint.position;
            this.transform.rotation = spawnPoint.rotation;
            Debug.Log(this.transform.name + " has respawned");
        }

        public void SetDefaults()
        {
            this._isDead = false;
            this.currentHealth = this.maxHealth;

            for (int i = 0; i < this._disableOnDeath.Length; i++)
            {
                this._disableOnDeath[i].enabled = this._wasEnabled[i];
            }

            // Colliders are not Behaviours
            Collider col = this.GetComponent<Collider>();
            if (col != null)
                col.enabled = true;
        }

        [ClientRpc]
        public void RpcTakeDamage(int damage)
        {
            if (this.IsDead) return;
            int newHealth = this.currentHealth - damage;
            if (newHealth < 0)
                newHealth = 0;
            this.currentHealth = newHealth;
            if (this.currentHealth <= 0)
                this.Die();
                
            Debug.Log("Player " + this.transform.name + " now has " + this.currentHealth + " health.");
        }

        private void Die()
        {
            this._isDead = true;

            for (int i = 0; i < this._disableOnDeath.Length; i++)
            {
                this._disableOnDeath[i].enabled = false;
            }
            // Colliders are not Behaviours
            Collider col = this.GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            Debug.Log(this.transform.name + " has died.");
            
            this.StartCoroutine(this.Respawn());

        }
    }
}
