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
        private Vector3 _cameraRotation = Vector3.zero;

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
        public void RotateCamera(Vector3 cameraRotation)
        {
            this._cameraRotation = cameraRotation;
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
        }

        private void PerformRotation()
        {
            this.rb.MoveRotation(this.rb.rotation * Quaternion.Euler(this._rotation));
            if (this._camera != null)
                this._camera.transform.Rotate(-this._cameraRotation);
        }



    }
}
