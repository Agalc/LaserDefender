using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float Speed = 1f; //speed of the ship
	public GameObject Projectile; //type of projectile
	public float projectileSpeed = 1f; //speed of projectile
	public float firingRate = 1f; //fire rate of projectiles
	public float health = 3f;
	public AudioClip shotSound;
	public AudioClip destroySound;
	float xMin, xMax;//max and min value for borders of game space

	void Start ()
	{
		//getting distance to a middle of the sprite size
		float middle = GetComponent<SpriteRenderer> ().sprite.bounds.size.x / 2;
		//setting gaming borders
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distance));
		xMax = rightmost.x - middle;
		xMin = leftmost.x + middle;

	}

	void Fire ()//shot two lazer beams from canons
	{	
		//setting coordinates for lazer beams
		Vector3 leftPos = transform.position + new Vector3 (-0.45f, 0.84f,0f);
		Vector3 rightPos = transform.position + new Vector3 (0.45f, 0.84f,0f); 
		//create two beams
		GameObject leftBeam = Instantiate (Projectile, leftPos, Quaternion.identity)as GameObject;
		GameObject rightBeam = Instantiate (Projectile, rightPos, Quaternion.identity)as GameObject;
		AudioSource.PlayClipAtPoint (shotSound, transform.position);
		//add initial velocity
		leftBeam.rigidbody2D.velocity = new Vector2 (0, projectileSpeed);
		rightBeam.rigidbody2D.velocity = new Vector2 (0, projectileSpeed);
	}

	void Update ()
	{
		//keys for shooting
		if (Input.GetKeyDown(KeyCode.Space)) InvokeRepeating ("Fire", 0.0001f, firingRate);
		if (Input.GetKeyUp(KeyCode.Space)) CancelInvoke("Fire");
		//keys for moving
		if (Input.GetKey (KeyCode.LeftArrow))//move left
			transform.position += Vector3.left * Speed * Time.deltaTime;
		if (Input.GetKey (KeyCode.RightArrow))//move right
			transform.position += Vector3.right * Speed * Time.deltaTime;
		//restrict player to gamespace
		float newX = Mathf.Clamp (transform.position.x, xMin, xMax);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
	}

	void Die(){
		StartEvent manager = GameObject.Find("LevelManager").GetComponent<StartEvent>();
		manager.StartLevel("Win");
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D col){
		Projectile missile = col.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.GetDamage();
			missile.Hit();
			if (health<=0) {
				AudioSource.PlayClipAtPoint(destroySound,transform.position);
				Die ();
			}
		}
	}
}
