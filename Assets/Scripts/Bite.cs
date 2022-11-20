using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bite : MonoBehaviour
{
	public float baseDamage = 10f;

	public GameManager gm;

	public GameObject bloodParticles;
	public GameObject damageParticles;
	public CameraShake camShake;

	public AudioSource audioSource;
	public AudioClip dealDamage;

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider != null)
		{
			string tag = col.collider.tag;
			if (tag.Contains("Enemy"))
			{
				var dmgScript = col.collider.GetComponent<DamageableEntity>();
				dmgScript.DamageEntity(baseDamage);
				if (tag.Contains("F"))
				{
					camShake.startShake(3f, 0.3f);
					var blood = Instantiate(bloodParticles, transform.position, transform.rotation);
					Destroy(blood, 1f);
					gm.AddScore();
					audioSource.PlayOneShot(dealDamage);
					Destroy(gameObject);
				}
				else if (tag.Contains("K"))
				{
					camShake.startShake(3f, 0.3f);
					var blood = Instantiate(bloodParticles, transform.position, transform.rotation);
					Destroy(blood, 1f);
					var dmg = Instantiate(damageParticles, transform.position, transform.rotation);
					Destroy(dmg, 1f);
					gm.AddScore(200);
					audioSource.PlayOneShot(dealDamage);
					Destroy(gameObject);
				}
				else if (tag.Contains("C"))
				{
					camShake.startShake(3f, 0.3f);
					var dmg = Instantiate(damageParticles, transform.position, transform.rotation);
					Destroy(dmg, 1f);
					audioSource.PlayOneShot(dealDamage);
					Destroy(gameObject);
				}
			}
		}
	}
}
