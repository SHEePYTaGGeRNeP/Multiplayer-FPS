using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        private Vector3 _velocity = Vector3.zero;
        private Vector3 _rotation = Vector3.zero;
        private float _cameraRotationX = 0f;
        private float _currentCameraRotationX = 0f;
        private Vector3 _thrusterForce = Vector3.zero;

        [SerializeField]
        private float _cameraRotationLimit = 85f;

        private Rigidbody rb;

        private void Start()
        {
            this.rb = this.GetComponent<Rigidbody>();
        }
        
        public void Move(Vector3 velocity)
        {
            this._velocity = velocity;
        }
        public void Rotate(Vector3 rotation)
        {
            this._rotation = rotation;
        }
        public void RotateCamera(float cameraRotationX)
        {
            this._cameraRotationX = cameraRotationX;
        }
        public void ApplyThruster(Vector3 thrusterForce)
        {
            this._thrusterForce = thrusterForce;
        }


        void FixedUpdate()
        {
            this.PerformMovement();
            this.PerformRotation();
        }
        

        private void PerformMovement()
        {
            if (this._velocity != Vector3.zero)
            {
                this.rb.MovePosition(this.transform.position + this._velocity * Time.fixedDeltaTime);
            }

            if (this._thrusterForce != Vector3.zero)
            {
                this.rb.AddForce(this._thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }

        private void PerformRotation()
        {
            this.rb.MoveRotation(this.rb.rotation * Quaternion.Euler(this._rotation));
            if (this._camera == null) return;
            this._currentCameraRotationX -= this._cameraRotationX;
            this._currentCameraRotationX = Mathf.Clamp(this._currentCameraRotationX, -this._cameraRotationLimit, this._cameraRotationLimit);

            this._camera.transform.localEulerAngles = new Vector3(this._currentCameraRotationX, 0f, 0f);
        }

    }
}
