namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.Networking;

    public class Player : NetworkBehaviour
    {
        [SerializeField]
        private int maxHealth = 100;

        // When this value changes sends it to all clients
        [SyncVar]
        private int currentHealth;

        void Awake()
        {
            this.SetDefaults();
        }

        public void SetDefaults()
        {
            this.currentHealth = this.maxHealth;
        }

        public void TakeDamage(int damage)
        {
            int newHealth = this.currentHealth - damage;
            if (newHealth < 0)
                newHealth = 0;
            this.currentHealth = newHealth;
            Debug.Log("Player " + this.transform.name + " now has " + this.currentHealth + " health.");
        }
    }
}
