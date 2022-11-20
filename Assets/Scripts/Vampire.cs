using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;
using UnityEngine.UI;

public class Vampire : MonoBehaviour
{
	public bool raging;
	public float baseDamage = 10f;
	public float ragingDamage = 100f;
	public float ragingSpeed = 23f;

	public float rage = 100f;
    public float rageOverTime = 0.2f;
	public float rageDuration = 5f;
	public float rageRecovery = 10f;

	public float knightDamage = 5f;
	public float crusaderDamage = 20f;
	
	public float attackSpeed = 5f;
	public float attackLifeTime = 0.1f;

	public GameManager gm;

	public GameObject biteAttack;
	public GameObject bloodParticles;
	public GameObject damageParticles;
	public CameraShake camShake;

	public AudioSource audioSource;
	public AudioSource globalAudioSource;
	public AudioClip takeDamage;
	public AudioClip dealDamage;
	public AudioClip death;

	public Slider rageBar;

	PlayerController player;
	float playerMovClamp;

    private void Start()
    {
		player = GetComponent<PlayerController>();
		playerMovClamp = player._moveClamp;
	}
    void Update()
    {
		if (Input.GetMouseButtonDown(0)){
			Vector3 shootDirection;
			shootDirection = Input.mousePosition;
			shootDirection.z = 0.0f;
			shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
			shootDirection = shootDirection - transform.position;
			var bulletInstance = Instantiate(biteAttack, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
			var biteScript = bulletInstance.GetComponent<Bite>();
			biteScript.gm = gm;
			biteScript.camShake = camShake;
			biteScript.audioSource = audioSource;
			var bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
			bulletRb.velocity = new Vector2(shootDirection.x * attackSpeed, shootDirection.y * attackSpeed);
			Destroy(bulletInstance, attackLifeTime);
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider != null)
		{
			string tag = col.collider.tag;
			if (tag.Contains("Enemy"))
			{
				var dmgScript = col.collider.GetComponent<DamageableEntity>();
				//dmgScript.DamageEntity(baseDamage);
                if (tag.Contains("F"))
                {
					camShake.startShake(3f, 0.3f);
					var blood = Instantiate(bloodParticles, transform.position, transform.rotation);
					Destroy(blood, 1f);
					gm.AddScore();
					audioSource.PlayOneShot(dealDamage);
				} else if (tag.Contains("K"))
				{
					camShake.startShake(3f, 0.3f);
					var blood = Instantiate(bloodParticles, transform.position, transform.rotation);
					Destroy(blood, 1f);
					var dmg = Instantiate(damageParticles, transform.position, transform.rotation);
					Destroy(dmg, 1f);
					gm.AddScore(200);
					audioSource.PlayOneShot(dealDamage);
				} else if (tag.Contains("C"))
				{
					camShake.startShake(3f, 0.3f);
					var dmg = Instantiate(damageParticles, transform.position, transform.rotation);
					Destroy(dmg, 1f);
					audioSource.PlayOneShot(takeDamage);
				}
			}
		}
	}
}
