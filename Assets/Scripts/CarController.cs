using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    //  [SerializeField] private int maxHealth;
    private HealthSystem healthSystem;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private WheelColliders wheelColliders;
    [SerializeField] private WheelVisual wheelVisual;
    [SerializeField] private float maxMotorTorque;
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private float maxBrakeTorque_F;
    [SerializeField] private float maxBrakeTorque_R;
    private float brakeInput;
    private Vector2 _movementInput;

    [SerializeField] private Vector2 centerOfMass_OffSet;

    private Weapon weapon;
    //public float fuerzaDown = 0;



    [SerializeField] private float timeToMaxTorque = 5f;
    [SerializeField] private float timeToZeroTorque = 1f;
    [SerializeField] private float torque = 0f;
    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        weapon = GetComponentInChildren<Weapon>();
        playerRb.centerOfMass = new Vector3(playerRb.centerOfMass.x, centerOfMass_OffSet.y, centerOfMass_OffSet.x);

        // healthSystem = GetComponent<HealthSystem>();
        // healthSystem.MaxHealth = maxHealth;
    }

    private void Update()
    {
        //playerRb.centerOfMass = new Vector3(playerRb.centerOfMass.x, centerOfMass_OffSet.y, centerOfMass_OffSet.x);

        //Debug.Log("Vel: " + playerRb.velocity.magnitude + "  RPM: " + wheelColliders.BRWheel.rpm + " Brake: " + wheelColliders.BRWheel.brakeTorque);
        //Debug.Log("velocidad: " + (wheelColliders.BLWheel.radius * 2) * Mathf.PI * wheelColliders.BLWheel.rpm);
    }
    //private void OnFire()
    //{
    //    weapon.Fire();
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(playerRb.centerOfMass), 0.05f);
    }
    public void FixedUpdate()
    {
        Motor();
        Steering();
        ApplyBrake();
        ApplyWheelPositions();

        //playerRb.AddRelativeForce(Vector3.down * fuerzaDown, ForceMode.Force);
    }



    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void Motor()
    {

        float targetTorque;
        float torqueRate;

        if (Mathf.Abs(_movementInput.y) >= 0f)
        {
            targetTorque = maxMotorTorque * _movementInput.y;
            torqueRate = maxMotorTorque / timeToMaxTorque;
        }
        else
        {
            targetTorque = 0f;
            torqueRate = maxMotorTorque / timeToZeroTorque;
        }

        torque = Mathf.MoveTowards(torque, targetTorque, torqueRate * Time.deltaTime);

        wheelColliders.R_Wheel_L.motorTorque = torque;
        wheelColliders.R_Wheel_R.motorTorque = torque;
        wheelColliders.F_Wheel_L.motorTorque = torque;
        wheelColliders.F_Wheel_R.motorTorque = torque;




        //wheelColliders.BLWheel.motorTorque = maxMotorTorque * _movementInput.y;
        //wheelColliders.BRWheel.motorTorque = maxMotorTorque * _movementInput.y;

    }

    private void Steering()
    {
        wheelColliders.F_Wheel_L.steerAngle = maxSteeringAngle * _movementInput.x;
        wheelColliders.F_Wheel_R.steerAngle = maxSteeringAngle * _movementInput.x;
    }

    private void OnBrake(InputValue inputValue)
    {
        brakeInput = inputValue.Get<float>();
    }

    private void ApplyBrake()
    {
        if (brakeInput != 0)
        {
            wheelColliders.R_Wheel_L.brakeTorque = brakeInput * maxBrakeTorque_R;
            wheelColliders.R_Wheel_R.brakeTorque = brakeInput * maxBrakeTorque_R;
            wheelColliders.F_Wheel_L.brakeTorque = brakeInput * maxBrakeTorque_F;
            wheelColliders.F_Wheel_R.brakeTorque = brakeInput * maxBrakeTorque_F;
        }
        else
        {
            wheelColliders.R_Wheel_L.brakeTorque = 0;
            wheelColliders.R_Wheel_R.brakeTorque = 0;
            wheelColliders.F_Wheel_L.brakeTorque = 0;
            wheelColliders.F_Wheel_R.brakeTorque = 0;
        }
        Debug.Log(wheelColliders.R_Wheel_L.brakeTorque);
    }

    private void ApplyWheelPositions()
    {
        UpdateWheel(wheelColliders.F_Wheel_L, wheelVisual.F_Tire_L);
        UpdateWheel(wheelColliders.F_Wheel_R, wheelVisual.F_Tire_R);
        UpdateWheel(wheelColliders.R_Wheel_L, wheelVisual.R_Tire_L);
        UpdateWheel(wheelColliders.R_Wheel_R, wheelVisual.R_Tire_R);
    }

    private void UpdateWheel(WheelCollider wCollider, GameObject wGameObject)
    {
        Quaternion rotation;
        Vector3 position;

        wCollider.GetWorldPose(out position, out rotation);
        wGameObject.transform.position = position;
        wGameObject.transform.rotation = rotation;
    }
    private void OnReset()
    {
        transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0));
    }
    //public void DestroyCar()
    //{
    //    Debug.Log("El " + gameObject.name + " ha muerto");
    //    Destroy(gameObject);
    //}


}

[System.Serializable]
public class WheelColliders
{
    public WheelCollider F_Wheel_L;
    public WheelCollider F_Wheel_R;
    public WheelCollider R_Wheel_L;
    public WheelCollider R_Wheel_R;
}

[System.Serializable]
public class WheelVisual
{
    public GameObject F_Tire_L;
    public GameObject F_Tire_R;
    public GameObject R_Tire_L;
    public GameObject R_Tire_R;
}