using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // configuration parameters
    [Header("Enemy")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 150;

    [Header("Projectile")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float projectileSpeed = 15f;
    [SerializeField] GameObject laserPrefab;

    [Header("VFX")]
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float durationOfexplosion = 1f;

    [Header("SFX")]
    [SerializeField] AudioClip deathAudio;
    [SerializeField] [Range(0, 1)] float deathPlaybackVolume = 1f;
    [SerializeField] AudioClip shootAudio;
    [SerializeField] [Range(0, 1)] float shootPlaybackVolume = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
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
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, durationOfexplosion);
        AudioSource.PlayClipAtPoint(deathAudio, Camera.main.transform.position, deathPlaybackVolume);
        Destroy(gameObject);
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1 * projectileSpeed);
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        AudioSource.PlayClipAtPoint(shootAudio, Camera.main.transform.position, shootPlaybackVolume);
    }
}
