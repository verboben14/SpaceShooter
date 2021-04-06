using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [Header("Player")]
    [SerializeField] float padding = 1f;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int health = 200;

    [Header("Projectile")]
    [SerializeField] float projectileSpeed = 30f;
    [SerializeField] float projectileFiringPeriod = 0.2f;
    [SerializeField] GameObject laserPrefab;

    [Header("VFX")]
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float durationOfexplosion = 1f;

    [Header("SFX")]
    [SerializeField] AudioClip deathAudio;
    [SerializeField] [Range(0,1)] float deathPlaybackVolume = 1f;
    [SerializeField] AudioClip shootAudio;
    [SerializeField] [Range(0, 1)] float shootPlaybackVolume = 0.1f;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    Coroutine firingCoroutine;

    public int Health { get { return health; } }

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootAudio, Camera.main.transform.position, shootPlaybackVolume);

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer == null)
        {
            return;
        }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        damageDealer.Hit();
        health -= damageDealer.Damage;
        if (health <= 0)
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, durationOfexplosion);
        AudioSource.PlayClipAtPoint(deathAudio, Camera.main.transform.position, deathPlaybackVolume);
        Destroy(gameObject);
        FindObjectOfType<Level>().LoadGameOver();
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
