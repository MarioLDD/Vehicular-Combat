using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


    public class UserInput : MonoBehaviour
    {

        private float controllerInputX;
        private float controllerInputY;
        private float controllerInputReverse;
        private float controllerInputHandBrake;

        public float ControllerInputX
        {
            get { return controllerInputX; }
        }

        public float ControllerInputY
        {
            get { return controllerInputY; }
        }

        public float ControllerInputReverse
        {
            get { return controllerInputReverse; }
        }

        public float ControllerInputHandBrake
        {
            get { return controllerInputHandBrake; }
        }

        private void OnMove(InputValue inputValue)
        {
            //_movementInput = inputValue.Get<Vector2>();
            controllerInputX = inputValue.Get<Vector2>().x;
            controllerInputY = inputValue.Get<Vector2>().y;
        }

        private void OnBrake(InputValue inputValue)
        {
            controllerInputHandBrake = inputValue.Get<float>();
        }

        private void OnReverse(InputValue inputValue)
        {
            controllerInputReverse = inputValue.Get<float>();
        }
    }





// Update is called once per frame
//void Update()
//{
//controllerInputX = Input.GetAxis("Horizontal");
//controllerInputY = Input.GetAxis("Vertical");
//controllerInputReverse = Input.GetAxis("Reverse");
//controllerInputHandBrake = Input.GetAxis("HandBrake");
//}