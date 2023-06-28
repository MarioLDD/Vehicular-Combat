using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewCarController : MonoBehaviour
{
    public Rigidbody playerRb;
    public WheelColliders wheelColliders;
    public WheelVisual wheelVisual;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float maxBrakeTorque_F;
    public float maxBrakeTorque_B;


    private float brakeInput;
    private Vector2 _movementInput;
    public float fuerzaDown =0;
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
        Motor();
        Steering();
        ApplyBrake();
        ApplyWheelPositions();

        playerRb.AddRelativeForce(Vector3.down * fuerzaDown, ForceMode.Force);
    }

    

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void Motor()
    {
        wheelColliders.BLWheel.motorTorque = maxMotorTorque * _movementInput.y;
        wheelColliders.BRWheel.motorTorque = maxMotorTorque * _movementInput.y;
    }

    private void Steering()
    {
        wheelColliders.FLWheel.steerAngle = maxSteeringAngle * _movementInput.x;
        wheelColliders.FRWheel.steerAngle = maxSteeringAngle * _movementInput.x;
    }

    private void OnBrake(InputValue inputValue)
    {
        brakeInput = inputValue.Get<float>();

    }

    private void ApplyBrake()
    {
        wheelColliders.BLWheel.brakeTorque = brakeInput * maxBrakeTorque_B;
        wheelColliders.BRWheel.brakeTorque = brakeInput * maxBrakeTorque_B;
        wheelColliders.FLWheel.brakeTorque = brakeInput * maxBrakeTorque_F;
        wheelColliders.FRWheel.brakeTorque = brakeInput * maxBrakeTorque_F;
    }

    private void ApplyWheelPositions()
    {
        UpdateWheel(wheelColliders.FLWheel, wheelVisual.FLWheelVisual);
        UpdateWheel(wheelColliders.FRWheel, wheelVisual.FRWheelVisual);
        UpdateWheel(wheelColliders.BLWheel, wheelVisual.BLWheelVisual);
        UpdateWheel(wheelColliders.BRWheel, wheelVisual.BRWheelVisual);
    }

    private void UpdateWheel(WheelCollider wCollider, GameObject wGameObject)
    {
        Quaternion rotation;
        Vector3 position;

        wCollider.GetWorldPose(out position, out rotation);
        wGameObject.transform.position = position;
        wGameObject.transform.rotation = rotation;
    
    
    }










}

[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider BRWheel;
    public WheelCollider BLWheel;
}

[System.Serializable]
public class WheelVisual
{
    public GameObject FRWheelVisual;
    public GameObject FLWheelVisual;
    public GameObject BRWheelVisual;
    public GameObject BLWheelVisual;
}