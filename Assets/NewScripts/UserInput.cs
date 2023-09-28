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
    private bool controllerInputHandBrake;
    private bool controllerInputNitro;

    private Vector2 controllerInputAiming;

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

    public bool ControllerInputHandBrake
    {
        get { return controllerInputHandBrake; }
    }

    public Vector2 ControllerInputAiming
    {
        get { return controllerInputAiming; }
    }

    public bool ControllerInputNitro { get { return controllerInputNitro; } }
    private void OnMove(InputValue inputValue)
    {
        //_movementInput = inputValue.Get<Vector2>();
        controllerInputX = inputValue.Get<Vector2>().x;
        controllerInputY = inputValue.Get<Vector2>().y;
    }
    private void OnAiming(InputValue inputValue)
    {
        controllerInputAiming = inputValue.Get<Vector2>();
    }

    private void OnBrake(InputValue inputValue)
    {
        if (inputValue.Get<float>() > 0.5f)
        {
            controllerInputHandBrake = true;
        }
        else
        {
            controllerInputHandBrake = false;
        }
    }

    private void OnReverse(InputValue inputValue)
    {
        controllerInputReverse = inputValue.Get<float>();
    }

    private void OnNitro(InputValue inputValue)
    {
        if (inputValue.Get<float>() > 0.5f)
        {
            controllerInputNitro = true;
        }
        else
        {
            controllerInputNitro = false;
        }
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