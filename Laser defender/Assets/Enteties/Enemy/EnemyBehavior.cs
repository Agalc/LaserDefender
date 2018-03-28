using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
	public float health = 2f;
	public GameObject Projectile;
	public float projectileSpeed = 5f;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 100;
	public AudioClip shotSound;
	public AudioClip destroySound;
	ScoreKeeper scoreKeeper;

	void Fire ()//shot two lazer beams from canons
	{	
		//setting coordinates for lazer beams
		Vector3 leftPos = transform.position + new Vector3 (-0.176f, -0.424f, 0); 
		Vector3 rightPos = transform.position + new Vector3 (0.176f, -0.424f, 0);
		//create two beams
		GameObject leftBeam = Instantiate (Projectile, leftPos, Quaternion.identity)as GameObject;
		GameObject rightBeam = Instantiate (Projectile, rightPos, Quaternion.identity)as GameObject;
		AudioSource.PlayClipAtPoint (shotSound, transform.position);
		//add initial velocity
		leftBeam.rigidbody2D.velocity = new Vector2 (0, -projectileSpeed);
		rightBeam.rigidbody2D.velocity = new Vector2 (0, -projectileSpeed);
	}

	void Start ()
	{
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();
	}

	void Update ()
	{
		float probability = Time.deltaTime * shotsPerSecond;
		if (Random.value < probability)
			Fire ();
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		Projectile missile = col.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.GetDamage ();
			missile.Hit ();
			if (health <= 0) {
				scoreKeeper.Score(scoreValue);
				AudioSource.PlayClipAtPoint(destroySound,transform.position);
				Destroy (gameObject);
			}
		}
	}
}
