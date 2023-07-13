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

    private RaycastHit rayHit;

    [SerializeField] private float bulletRange;
    [SerializeField] private float fireRate, reloadTime;
    [SerializeField] private bool isAutomatic;
    [SerializeField] private int magazineSize;
    [SerializeField] private int currentAmmo;
    public int CurrentAmmo { set { currentAmmo = value; } }

    private bool isShooting, readyToShoot;

    [SerializeField] private GameObject bulletHolePrefabs;
    [SerializeField] private float bulletHoleLifeSpan;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private string EnemyTag;

    [SerializeField] private bool AddBulletSpread = true;
    [SerializeField] private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private float burstDealy;
    [SerializeField] private int bulletsPerBurst;
    private int bulletsShot;
    //[SerializeField] private float proyectileForce;
    //[SerializeField] private Rigidbody bullet_Rb;
    [SerializeField] private Transform firePoint;
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

        Vector3 direction = GetDirection();

        if (Physics.Raycast(firePoint.position, direction, out rayHit, bulletRange))
        {
            if (rayHit.collider.TryGetComponent(out HealthSystem healthSystem))
            {
                healthSystem.TakeDamage(bulletDamage);
            }
            TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHit.point, rayHit.normal, true));
        }
        else
        {
            TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, firePoint.position + direction * 100, Vector3.zero, false));
        }

        muzzleFlash.Play();

        currentAmmo--;
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
        Vector3 direction = firePoint.forward;

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
            GameObject bulletHole = Instantiate(bulletHolePrefabs, rayHit.point + rayHit.normal * 0.001f, Quaternion.identity);
            bulletHole.transform.LookAt(rayHit.point + rayHit.normal);
            Destroy(bulletHole, bulletHoleLifeSpan);
        }

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