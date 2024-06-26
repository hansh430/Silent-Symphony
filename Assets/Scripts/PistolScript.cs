using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PistolScript : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private int maxAmmoInMag = 10;
    [SerializeField] private int maxAmmoInStorage = 30;
    [SerializeField] private int currentAmmoInMag;
    [SerializeField] private int currentAmmoInStorage;

    [Header("CoolDown and Forces")]
    [SerializeField] private float shootCooldown = 0.5f;
    [SerializeField] private float reloadCooldown = 0.5f;
    [SerializeField] private float shootRange = 100f;
    [SerializeField] private float cartridgeEjectionForce = 5f;
    private float switchCooldown = 0.5f;
    [SerializeField] private int damager;
    private float shootTimer;

    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject muzzleFlashLight;
    [SerializeField] private ParticleSystem impactEffect;

    [Header("Cartridge")]
    [SerializeField] private Transform cartridgeEjectionPoint;
    [SerializeField] private GameObject cartridgePrefab;

    [Header("Animation and Audio")]
    [SerializeField] private Animator gun;
    public AudioSource shoot;

    [Header("Conditions")]
    [SerializeField] private bool canShoot = true;
    [SerializeField] private bool canSwitch = true;
    private bool isReloading = false;

    private void Start()
    {
        currentAmmoInMag = maxAmmoInMag;
        currentAmmoInStorage = maxAmmoInStorage;
        canSwitch = true;
        muzzleFlashLight.SetActive(false);
    }
    private void Update()
    {
        currentAmmoInMag = Mathf.Clamp(currentAmmoInMag,0,maxAmmoInMag);
        currentAmmoInStorage= Mathf.Clamp(currentAmmoInStorage,0,maxAmmoInStorage);
        if(Input.GetButtonDown("Fire1")&& canShoot && !isReloading)
        {
            switchCooldown = shootCooldown;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            switchCooldown = reloadCooldown;
            Reload();
        }

        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        canSwitch = false;
        shoot.Play();
        muzzleFlash.Play();
        muzzleFlashLight.SetActive(true);
        gun.SetBool("shoot", true);
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit, shootRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                if(enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damager);
                }
            }
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject cartridge = Instantiate(cartridgePrefab, cartridgeEjectionPoint.position, cartridgeEjectionPoint.rotation);
            Rigidbody cartridgeRigidbody = cartridge.GetComponent<Rigidbody>();
            cartridgeRigidbody.AddForce(cartridgeEjectionPoint.right * cartridgeEjectionForce, ForceMode.Impulse);
            StartCoroutine(EndAnimations());
            StartCoroutine(EndLight());
            StartCoroutine(CanSwitchShoot());
            switchCooldown -= Time.deltaTime;
            currentAmmoInMag--;
            shootTimer = shootCooldown;
        }
        else
        {
            // Out of ammo in the magazine or shoot on cooldown
            Debug.Log("Cannot shoot");
        }

    }
    IEnumerator EndAnimations()
    {
        yield return new WaitForSeconds(.1f);
        gun.SetBool("shoot", false);
        gun.SetBool("reload", false);
    }
    IEnumerator EndLight()
    {
        yield return new WaitForSeconds(.1f);
        muzzleFlashLight.SetActive(false);
    }

    IEnumerator CanSwitchShoot()
    {
        yield return new WaitForSeconds(shootCooldown);
        canSwitch = true;
    }
    private void Reload()
    {
        switchCooldown -= Time.deltaTime;
        // Check if already reloading or out of ammo in the storage
        if (isReloading || currentAmmoInStorage <= 0)
            return;

        // Calculate the number of bullets to reload
        int bulletsToReload = maxAmmoInMag - currentAmmoInMag;

        // Check if there is enough ammo in the storage for reloading
        if (bulletsToReload > 0)
        {

            gun.SetBool("reload", true);
            StartCoroutine(EndAnimations());


            // Determine the actual number of bullets to reload based on available ammo
            int bulletsAvailable = Mathf.Min(bulletsToReload, currentAmmoInStorage);

            // Update ammo counts
            currentAmmoInMag += bulletsAvailable;
            currentAmmoInStorage -= bulletsAvailable;

            Debug.Log("Reloaded " + bulletsAvailable + " bullets");

            // Start the reload cooldown
            StartCoroutine(ReloadCooldown());
        }
        else
        {
            Debug.Log("Cannot reload");
        }
    }
    IEnumerator ReloadCooldown()
    {
        isReloading = true;
        canShoot = false;
        canSwitch = false;

        yield return new WaitForSeconds(reloadCooldown);

        isReloading = false;
        canShoot = true;
        canSwitch = true;
    }

}
