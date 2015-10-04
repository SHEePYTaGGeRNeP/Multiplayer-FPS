using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerMotor))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 5f;

        private PlayerMotor _motor;

        void Start()
        {
            this._motor = this.GetComponent<PlayerMotor>();
        }

        void Update()
        {
            // calculate movement velocity as a 3D vector.
            float xMove = Input.GetAxisRaw("Horizontal");
            float zMove = Input.GetAxisRaw("Vertical");

            Vector3 moveHorizontal = this.transform.right * xMove;
            Vector3 moveVertical = this.transform.forward * zMove;

            Vector3 velocity = (moveHorizontal + moveVertical).normalized * this._speed * Time.deltaTime;

            this._motor.Move(velocity);
            
        }
    }
}
