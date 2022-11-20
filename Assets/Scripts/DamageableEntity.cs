using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableEntity:MonoBehaviour
{
	public float Hp = 10f;

	float dirNum;

	private void Update()
	{
		if (Hp <= 0) Destroy(gameObject);
	}
	public void DamageEntity(float damage, float knockback = 0f, Transform hitOrigin = null)
	{
		Hp -= damage;
		if (knockback != 0)
		{
			var _Rb = GetComponent<Rigidbody2D>();
			if (_Rb != null)
			{
				Vector3 heading = hitOrigin.position - transform.position;
				dirNum = AngleDir(transform.forward, heading, transform.up) * -1;
				_Rb.AddForce(transform.up * knockback + (transform.right * knockback) * dirNum, ForceMode2D.Impulse);
			}
		}
	}
	float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);

		if (dir > 0f)
		{
			return 1f;
		}
		else if (dir < 0f)
		{
			return -1f;
		}
		else
		{
			return 0f;
		}
	}
}
