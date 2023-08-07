using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    private InputActionAsset inputAsset;
    private InputActionMap player;

    private RaycastHit rayHitA;
    private RaycastHit rayHitB;
    private RaycastHit rayHitAim;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private Camera cam;

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
    private List<GameObject> playerList = new List<GameObject>();
    private GameObject currentPlayer;

    private LineRenderer lineRenderer;



    private void Awake()
    {
        currentAmmo = magazineSize;
        readyToShoot = true;
        //controls = new InputPlayer();
        inputAsset = GetComponentInParent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
    }
    private void Start()
    {
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
        //if (currentPlayer.name == "Player 4")
        {
            foreach (var player in playerList)
            {
                RaycastHit hit;
                if (Physics.Raycast(aimPoint.position, aimPoint.forward, out hit))
                {
                    if (hit.collider.gameObject == player.gameObject)
                    {
                        continue;
                    }
                }
                Vector3 aimPointForward = aimPoint.transform.forward;
                Vector3 playerDirection = (player.transform.position - aimPoint.transform.position);

                Vector3 aimPointForwardProyectY = aimPointForward;
                Vector3 playerDirectionProyectY = playerDirection;

                aimPointForwardProyectY.y = 0f;
                playerDirectionProyectY.y = 0f;

                aimPointForwardProyectY = aimPointForwardProyectY.normalized;
                playerDirectionProyectY = playerDirectionProyectY.normalized;

                float dotAngle = Vector3.Dot(aimPointForwardProyectY, playerDirectionProyectY);
                float angleInDegrees = Mathf.Acos(dotAngle) * Mathf.Rad2Deg;
                if (angleInDegrees <= 5.5f)
                {
                    //Debug.Log(player.name + "   " + angleInDegrees);
                    float vAngle = Vector3.SignedAngle(playerDirection, aimPointForward, -transform.right);
                    //Debug.Log(vAngle);
                    Vector3 euler = transform.localRotation.eulerAngles;
                    euler.x += vAngle;

                    euler.x = Mathf.Clamp(euler.x, -30, 7);
                    transform.localRotation = Quaternion.Euler(euler);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(1.329f, 0, 0);
                }
            }
        }




        if (isShooting && readyToShoot && currentAmmo > 0)
        {
            bulletsShot = bulletsPerBurst;
            PerfromShot();
        }


    }
    private void FixedUpdate()
    {
        if (Physics.Raycast(aimPoint.position, aimPoint.forward, out rayHitAim))
        {
            //float distance = Vector3.Distance(aimPoint.position, rayHitAim.point);
            crosshair.position = cam.WorldToScreenPoint(rayHitAim.point);
            //lineRenderer.SetPosition(0, aimPoint.position);
            //lineRenderer.SetPosition(1, rayHitAim.point);
        }
        else
        {
            float distance = 100f; // Distancia razonable para el punto de destino
            Vector3 destination = aimPoint.position + aimPoint.forward * distance;
            crosshair.position = cam.WorldToScreenPoint(destination);
            //lineRenderer.SetPosition(0, aimPoint.position);
            //lineRenderer.SetPosition(1, destination);
        }
    }
    private void StartShot()
    {
        isShooting = true;
    }

    private void EndShot()
    {
        isShooting = false;
    }

    private void PerfromShot()
    {
        readyToShoot = false;

        Vector3 directionA = GetDirection();

        if (Physics.Raycast(firePointA.position, directionA, out rayHitA, bulletRange))
        {
            if (rayHitA.collider.TryGetComponent(out HealthSystem healthSystem))
            {
                if (rayHitA.collider.gameObject != currentPlayer)
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
            if (rayHitB.collider.TryGetComponent(out HealthSystem healthSystem))
            {
                if (rayHitB.collider.gameObject != currentPlayer)
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
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
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
        PerfromShot();
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