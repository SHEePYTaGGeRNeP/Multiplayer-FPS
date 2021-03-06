﻿using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerMotor), typeof(ConfigurableJoint))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 5f;

        [SerializeField]
        private float _mouseSensitivity = 3f;

        [SerializeField]
        private float _thrusterForce = 1000f;

        

        // Header in the Unity inspector
        [Header("Joint options:")]
        [SerializeField]
        private float _jointSpring = 20f;
        [SerializeField]
        private float _jointMaxForce = 40f;

        // component caching
        private PlayerMotor _motor;
        private ConfigurableJoint _joint;
        private Animator _animator;

        void Start()
        {
            this._motor = this.GetComponent<PlayerMotor>();
            this._joint = this.GetComponent<ConfigurableJoint>();
            this._animator = this.GetComponent<Animator>();
            this.SetJointSettings(this._jointSpring);
        }

        void Update()
        {
            // calculate movement velocity as a 3D vector.
            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");

            Vector3 moveHorizontal = this.transform.right * xMove;
            Vector3 moveVertical = this.transform.forward * zMove;

            Vector3 velocity = (moveHorizontal + moveVertical) * this._speed * Time.deltaTime;

            // Animate movement
            this._animator.SetFloat("ForwardVelocity", zMove);

            this._motor.Move(velocity);

            // Only for turning - Rotating around X is for camera
            float yrot = Input.GetAxisRaw("Mouse X");
            Vector3 rotation = new Vector3(0, yrot, 0) * this._mouseSensitivity;

            this._motor.Rotate(rotation);

            // Calculate camera rotation as a 3D vector
            float xrot = Input.GetAxisRaw("Mouse Y");
            float cameraRotationX = xrot * this._mouseSensitivity;

            this._motor.RotateCamera(cameraRotationX);


            Vector3 thrusterForce = Vector3.zero;
            if (Input.GetButton("Jump"))
            {
                thrusterForce = Vector3.up * this._thrusterForce;
                this.SetJointSettings(0f);
            }
            else
                this.SetJointSettings(this._jointSpring);

            // Apply thuster force
            this._motor.ApplyThruster(thrusterForce);
        }

        private void SetJointSettings(float jointSpring)
        {
            this._joint.yDrive = new JointDrive { positionSpring = jointSpring, maximumForce = this._jointMaxForce };


        }
    }
}
