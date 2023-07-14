using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    private InputActionAsset inputAsset;
    private InputActionMap player;

    private RaycastHit rayHitA;
    private RaycastHit rayHitB;

    [SerializeField] private float bulletRange;
    [SerializeField] private float fireRate, reloadTime;
    [SerializeField] private bool isAutomatic;
    [SerializeField] private int magazineSize;
    [SerializeField] private int currentAmmo;
    public int CurrentAmmo { set { currentAmmo = value; } }

    private bool isShooting, readyToShoot, reloading;

    [SerializeField] private GameObject bulletHolePrefabs;
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
    [SerializeField] private Transform firePointB;
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
    private void Awake()
    {
        currentAmmo = magazineSize;
        readyToShoot = true;
        //controls = new InputPlayer();
        inputAsset = GetComponentInParent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");


        //controls.Player.Fire.started += ctx => StartShot();
        //controls.Player.Fire.canceled += ctx => EndShot();
        //controls.Player.Reload.performed += ctx => Reload();

    }

    void Update()
    {
        if (isShooting && readyToShoot && currentAmmo > 0)
        {
            bulletsShot = bulletsPerBurst;
            PerfromShot();
        }

    }
    private void Start()
    {
        ammo_Text.text = "Munition: " + currentAmmo;

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
                healthSystem.TakeDamage(bulletDamage);
            }
            TrailRenderer trail = Instantiate(bulletTrail, firePointA.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHitA.point, rayHitA.normal, true));
        }
        else
        {
            TrailRenderer trail = Instantiate(bulletTrail, firePointA.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, firePointA.position + directionA * 100, Vector3.zero, false));
        }

        Vector3 directionB = GetDirection();

        if (Physics.Raycast(firePointB.position, directionB, out rayHitB, bulletRange))
        {
            if (rayHitB.collider.TryGetComponent(out HealthSystem healthSystem))
            {
                healthSystem.TakeDamage(bulletDamage);
            }
            TrailRenderer trail = Instantiate(bulletTrail, firePointB.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHitB.point, rayHitB.normal, true));
        }
        else
        {
            TrailRenderer trail = Instantiate(bulletTrail, firePointB.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, firePointB.position + directionB * 100, Vector3.zero, false));
        }

        muzzleFlashA.Play();
        muzzleFlashB.Play();

        currentAmmo -= 2;
        bulletsShot--;
        ammo_Text.text = "Munition: " + currentAmmo;
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
        ammo_Text.text = "Munition: " + currentAmmo;
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
    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
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
            //Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
            GameObject bulletHole = Instantiate(bulletHolePrefabs, HitPoint + HitNormal * 0.001f, Quaternion.identity);
            bulletHole.transform.LookAt(HitPoint + HitNormal);
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
        ammo_Text.text = "Munition: " + currentAmmo;
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