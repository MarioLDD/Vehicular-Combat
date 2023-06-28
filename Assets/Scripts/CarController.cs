using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public Rigidbody playerRb;
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    private float brakeInput;
    public float maxBrakeTorque;
    private Vector2 _movementInput;
    public float fuerza;
    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log(brakeInput);

    }
    public void FixedUpdate()
    {
        float motor = maxMotorTorque * _movementInput.y;
        float steering = maxSteeringAngle * _movementInput.x;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
        playerRb.AddRelativeForce(Vector3.down * fuerza, ForceMode.Force);
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>(); 
    }

    private void OnBrake(InputValue inputValue)
    {
        brakeInput = inputValue.Get<float>();

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.brakeTorque = brakeInput * maxBrakeTorque;
                axleInfo.rightWheel.brakeTorque = brakeInput * maxBrakeTorque;
            }

            if (axleInfo.steering)
            {
                axleInfo.leftWheel.brakeTorque = brakeInput * maxBrakeTorque;
                axleInfo.rightWheel.brakeTorque = brakeInput * maxBrakeTorque;
            }
        }
    }












}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public MeshRenderer meshRenderer_LW;
    public MeshRenderer meshRenderer_RW;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
