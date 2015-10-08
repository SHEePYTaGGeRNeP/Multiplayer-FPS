using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerMotor))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 5f;

        [SerializeField]
        private float _mouseSensitivity = 3f;

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

            // Only for turning - Rotating around X is for camera
            float yrot = Input.GetAxisRaw("Mouse X");
            Vector3 rotation = new Vector3(0, yrot, 0) * this._mouseSensitivity;

            this._motor.Rotate(rotation);

            // Calculate camera rotation as a 3D vector
            float xrot = Input.GetAxisRaw("Mouse Y");
            Vector3 cameraRotation = new Vector3(xrot, 0, 0) * this._mouseSensitivity;

            this._motor.RotateCamera(cameraRotation);
        }
    }
}
