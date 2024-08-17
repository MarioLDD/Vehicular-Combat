using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using UnityEngine.Windows;
using static UnityEngine.LightAnchor;

public class Weapon : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private InputActionAsset inputAsset;
    private InputActionMap player;
    [SerializeField] private UserInput userInput;
    private float horizontalAngle;
    [SerializeField] private float horizontalAngleMin = -45;
    [SerializeField] private float horizontalAngleMax = 45;

    private float verticalAngle;
    [SerializeField] private float verticalAngleMin = -30;
    [SerializeField] private float verticalAngleMax = 7;

    [SerializeField] private float cursorSpeed = 1;
    [SerializeField] private float inputSmoothingFactor = 0.1f;
    private Vector2 inputSmoothed;


    private RaycastHit rayHitA;
    private RaycastHit rayHitB;
    private RaycastHit rayHitAim;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private int screenMinXWidth;
    [SerializeField] private int screenMaxXWidth;
    [SerializeField] private int screenMinYHeight;
    [SerializeField] private int screenMaxYHeight;
    [SerializeField] private Camera camera;

    [SerializeField] private float bulletRange;
    [SerializeField] private float fireRate, reloadTime;
    [SerializeField] private bool isAutomatic;
    [SerializeField] private int magazineSize;
    [SerializeField] private int currentAmmo;
    public int CurrentAmmo { set { currentAmmo = value; } }

    private bool isShooting, readyToShoot, reloading;

    [SerializeField] private GameObject bulletHolePrefabs;
    [SerializeField] private GameObject hitSpark;
    [SerializeField] private float bulletHoleLifeSpan;

    [SerializeField] private string EnemyTag;

    [SerializeField] private bool AddBulletSpread = true;
    [SerializeField] private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private float burstDealy;
    [SerializeField] private int bulletsPerBurst;
    private int bulletsShot;
    //[SerializeField] private float proyectileForce;
    //[SerializeField] private Rigidbody bullet_Rb;
    [SerializeField] private Transform firePointA;
    [SerializeField] private Transform firePointB, aimPoint;
    [SerializeField] private ParticleSystem muzzleFlashA;
    [SerializeField] private ParticleSystem muzzleFlashB;


    [SerializeField] private TMP_Text ammo_Text;
    [SerializeField] private Image ammo_Image;
    [SerializeField] private int bulletDamage = 5;


    /**
     * lo siguiente corresponde al agregado de trails
     */
    [SerializeField] private float BulletSpeed = 100;
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    [SerializeField] private TrailRenderer bulletTrail;

    //AutoApuntado
    [SerializeField] private List<GameObject> playerList = new List<GameObject>();
    private GameObject currentPlayer;

    private LineRenderer lineRenderer;



    private void Awake()
    {
        currentAmmo = magazineSize;
        readyToShoot = true;
        //controls = new InputPlayer();
        inputAsset = GetComponentInParent<PlayerInput>().actions;
        //player = inputAsset.FindActionMap("Player"); //este andaba bien
        player = playerInput.currentActionMap;
    }
    private void Start()
    {
        PlayerInputManager playerInputManager = FindAnyObjectByType<PlayerInputManager>();
        var totalPlayer = playerInputManager.playerCount;
        var playerIndex = playerInput.splitScreenIndex;
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        if (totalPlayer == 2)
        {

            if (playerIndex == 0)
            {
                screenMinXWidth = -screenWidth / 2;
                screenMaxXWidth = 0;

                screenMinYHeight = -screenHeight / 2;
                screenMaxYHeight = screenHeight / 2;

                crosshair.localPosition = new Vector3(screenMinXWidth / 2, 0);
            }
            else if (playerIndex == 1)
            {
                screenMinXWidth = 0;
                screenMaxXWidth = screenWidth / 2;

                screenMinYHeight = -screenHeight / 2;
                screenMaxYHeight = screenHeight / 2;

                crosshair.localPosition = new Vector3(screenMaxXWidth / 2, 0);

            }
        }
        else if (totalPlayer == 4)
        {
            if (playerIndex == 0)
            {
                screenMinXWidth = -screenWidth / 2;
                screenMaxXWidth = 0;

                screenMinYHeight = 0;
                screenMaxYHeight = screenHeight / 2;

                crosshair.localPosition = new Vector3(screenMinXWidth / 2, screenMaxYHeight / 2);
            }
            if (playerIndex == 1)
            {
                screenMinXWidth = 0;
                screenMaxXWidth = screenWidth / 2;

                screenMinYHeight = 0;
                screenMaxYHeight = screenHeight / 2;

                crosshair.localPosition = new Vector3(screenMaxXWidth / 2, screenMaxYHeight / 2);
            }
            if (playerIndex == 2)
            {
                screenMinXWidth = -screenWidth / 2;
                screenMaxXWidth = 0;

                screenMinYHeight = -screenHeight / 2;
                screenMaxYHeight = 0;

                crosshair.localPosition = new Vector3(screenMinXWidth / 2, screenMinYHeight / 2);
            }
            if (playerIndex == 3)
            {
                screenMinXWidth = 0;
                screenMaxXWidth = screenWidth / 2;

                screenMinYHeight = -screenHeight / 2;
                screenMaxYHeight = 0;

                crosshair.localPosition = new Vector3(screenMaxXWidth / 2, screenMinYHeight / 2);
            }
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        currentPlayer = gameObject.transform.parent.gameObject;
        foreach (var player in players)
        {
            if (player != currentPlayer)
            {
                playerList.Add(player);
            }
        }
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        ammo_Text.text = currentAmmo.ToString();

    }
    void Update()
    {
        Vector2 input = Vector2.zero;

        input = userInput.ControllerInputAiming;

        GunAiming(input);






        //if (currentPlayer.name == "Player 4") 


        ///<summary>
        ///el siguiente codigo contiene el auto apuntado vertical
        ///lo desactive para implementar uno manual
        ///borrar una vez que el autoapuntado manual funcione bien
        ///</summary>      

        //{
        //    foreach (var player in playerList)
        //    {
        //        RaycastHit hit;
        //        if (Physics.Raycast(aimPoint.position, aimPoint.forward, out hit))
        //        {
        //            if (hit.collider.gameObject == player.gameObject)
        //            {
        //                continue;
        //            }
        //        }
        //        Vector3 aimPointForward = aimPoint.transform.forward;
        //        Vector3 playerDirection = (player.transform.position - aimPoint.transform.position);

        //        Vector3 aimPointForwardProyectY = aimPointForward;
        //        Vector3 playerDirectionProyectY = playerDirection;

        //        aimPointForwardProyectY.y = 0f;
        //        playerDirectionProyectY.y = 0f;

        //        aimPointForwardProyectY = aimPointForwardProyectY.normalized;
        //        playerDirectionProyectY = playerDirectionProyectY.normalized;

        //        float dotAngle = Vector3.Dot(aimPointForwardProyectY, playerDirectionProyectY);
        //        float angleInDegrees = Mathf.Acos(dotAngle) * Mathf.Rad2Deg;
        //        if (angleInDegrees <= 5.5f)
        //        {
        //            //Debug.Log(player.name + "   " + angleInDegrees);
        //            float vAngle = Vector3.SignedAngle(playerDirection, aimPointForward, -transform.right);
        //            //Debug.Log(vAngle);
        //            Vector3 euler = transform.localRotation.eulerAngles;
        //            euler.x += vAngle;

        //            euler.x = Mathf.Clamp(euler.x, -30, 7);
        //            transform.localRotation = Quaternion.Euler(euler);
        //        }
        //        else
        //        {
        //            transform.localRotation = Quaternion.Euler(1.329f, 0, 0);
        //        }
        //    }
        //}





        if (isShooting && readyToShoot && currentAmmo > 0)
        {
            bulletsShot = bulletsPerBurst;
            PerformShot();
        }


    }
    private void FixedUpdate()
    {
        //Visualizador de apuntado

        if (Physics.Raycast(aimPoint.position, aimPoint.forward, out rayHitAim))
        {
            float distance = Vector3.Distance(aimPoint.position, rayHitAim.point);
            lineRenderer.SetPosition(0, aimPoint.position);//laser
            lineRenderer.SetPosition(1, rayHitAim.point);//laser
        }
        else
        {
            float distance = 100f; // Distancia razonable para el punto de destino
            Vector3 destination = aimPoint.position + aimPoint.forward * distance;
            lineRenderer.SetPosition(0, aimPoint.position);//laser
            lineRenderer.SetPosition(1, destination);//laser
        }
    }

    

    private void GunAiming(Vector2 input)
    {
        // Ajusta la posición del crosshair en la pantalla
        input *= cursorSpeed * Time.deltaTime;
        float clampedX = Mathf.Clamp(crosshair.localPosition.x + input.x, screenMinXWidth, screenMaxXWidth);
        float clampedY = Mathf.Clamp(crosshair.localPosition.y + input.y, screenMinYHeight, screenMaxYHeight);

        crosshair.localPosition = new Vector3(clampedX, clampedY);

        // Lanza un rayo desde la cámara a través de la posición del crosshair en la pantalla
        Ray ray = camera.ScreenPointToRay(crosshair.position);

        Vector3 gunDirection;
        RaycastHit hit;

        // Verifica si el rayo impacta en algún objeto en el mundo 3D
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Calcula la dirección hacia el punto de impacto
            gunDirection = hit.point - transform.position;
        }
        else
        {
            // Si no hay impacto, dispara en la dirección del rayo
            gunDirection = ray.direction;
        }

        // Apunta la torreta hacia la dirección calculada
        transform.rotation = Quaternion.LookRotation(gunDirection);

        // Obtiene la rotación actual en euler angles
        Vector3 rot = transform.localRotation.eulerAngles;
        float rotY = rot.y;
        float rotX = rot.x;

        // Aplica clamping a la rotación horizontal (Y)
        rotY = (rotY > 180) ? rotY - 360 : rotY;
        rotY = Mathf.Clamp(rotY, horizontalAngleMin, horizontalAngleMax);
        rotY = (rotY < 0) ? rotY + 360 : rotY;

        // Aplica clamping a la rotación vertical (X)
        rotX = (rotX > 180) ? rotX - 360 : rotX;
        rotX = Mathf.Clamp(rotX, verticalAngleMin, verticalAngleMax);
        rotX = (rotX < 0) ? rotX + 360 : rotX;

        // Establece la nueva rotación clamped
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0);
        transform.localRotation = localRotation;


    }










    private void StartShot()
    {
        isShooting = true;
    }

    private void EndShot()
    {
        isShooting = false;
    }

    private void PerformShot()
    {
        readyToShoot = false;

        Vector3 directionA = GetDirection();

        if (Physics.Raycast(firePointA.position, directionA, out rayHitA, bulletRange))
        {
            if (rayHitA.collider.gameObject.TryGetComponentInParents(out HealthSystem healthSystem))
            {
                if (healthSystem.gameObject != currentPlayer)
                {
                    healthSystem.TakeDamage(bulletDamage, currentPlayer);
                }
            }
            TrailRenderer trail = Instantiate(bulletTrail, firePointA.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHitA.point, rayHitA.normal, true, rayHitA.collider.transform));
        }
        else
        {
            TrailRenderer trail = Instantiate(bulletTrail, firePointA.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, firePointA.position + directionA * 100, Vector3.zero, false, null));
        }

        Vector3 directionB = GetDirection();

        if (Physics.Raycast(firePointB.position, directionB, out rayHitB, bulletRange))
        {
            if (rayHitB.collider.gameObject.TryGetComponentInParents(out HealthSystem healthSystem))
            {
                if (healthSystem.gameObject != currentPlayer)
                {
                    healthSystem.TakeDamage(bulletDamage, currentPlayer);
                }
            }
            TrailRenderer trail = Instantiate(bulletTrail, firePointB.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHitB.point, rayHitB.normal, true, rayHitB.collider.transform));
        }
        else
        {
            TrailRenderer trail = Instantiate(bulletTrail, firePointB.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, firePointB.position + directionB * 100, Vector3.zero, false, null));
        }

        muzzleFlashA.Play();
        muzzleFlashB.Play();

        currentAmmo -= 2;
        bulletsShot--;
        ammo_Text.text = currentAmmo.ToString();
        if (currentAmmo < 1)
        {
            ammo_Image.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            ammo_Text.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }

        if (bulletsShot > 0 && currentAmmo > 0)
        {
            Invoke("ResumeBurst", burstDealy);
        }
        else
        {
            Invoke("ResetShot", fireRate);

            if (!isAutomatic)
            {
                EndShot();
            }
        }
    }

    public void Addmunitions(int addAmmo)
    {
        currentAmmo = addAmmo;
        ammo_Image.color = Color.white;
        ammo_Text.color = Color.white;
        ammo_Text.text = currentAmmo.ToString();
    }
    private Vector3 GetDirection()
    {
        Vector3 direction = firePointA.forward;

        if (AddBulletSpread)
        {
            direction += new Vector3(
                UnityEngine.Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                UnityEngine.Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                UnityEngine.Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
                );
            direction.Normalize();
        }
        return direction;
    }
    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact, Transform impactedObject)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }

        Trail.transform.position = HitPoint;
        if (MadeImpact)
        {
            //Instantiate(hitSpark, HitPoint + HitNormal, Quaternion.identity);
            GameObject bulletHole = Instantiate(bulletHolePrefabs, HitPoint + HitNormal * 0.001f, Quaternion.identity);
            bulletHole.transform.LookAt(HitPoint + HitNormal);
            bulletHole.transform.parent = impactedObject;
            Destroy(bulletHole, bulletHoleLifeSpan);
        }

        Destroy(Trail.gameObject, Trail.time);
        Destroy(Trail.gameObject, Trail.time);
        Destroy(Trail.gameObject, Trail.time);
    }
    private void ResumeBurst()
    {
        readyToShoot = true;
        PerformShot();
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinish", reloadTime);
    }

    private void ReloadFinish()
    {
        currentAmmo = magazineSize;
        ammo_Image.color = Color.white;
        ammo_Text.color = Color.white;
        ammo_Text.text = currentAmmo.ToString();
        reloading = false;
    }
    private void OnEnable()
    {
        //controls.Enable();
        player.FindAction("Fire").started += ctx => StartShot();
        player.FindAction("Fire").canceled += ctx => EndShot();
        player.FindAction("Reload").performed += ctx => Reload();

    }

    private void OnDisable()
    {
        //controls.Disable();
        player.FindAction("Fire").started -= ctx => StartShot();
        player.FindAction("Fire").canceled -= ctx => EndShot();
        player.FindAction("Reload").performed -= ctx => Reload();
    }
}