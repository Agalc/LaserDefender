using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

	public GameObject enemyPrefab;
	public float width;
	public float height;
	public float speed;
	public float spawnDelay = 0.5f;
	bool movingRight;
	float xMax, xMin;

	void SpawnUntillFull ()//spawn enemy formation
	{
		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.Rotate (0, 0, 180);
			enemy.transform.parent = freePosition;
		}
		if (NextFreePosition ())
			Invoke ("SpawnUntillFull", spawnDelay);
	}

	void Start ()// Use this for initialization
	{
		//getting distance to a middle of the sprite size
		//setting gaming borders
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distance));
		xMax = rightmost.x;
		xMin = leftmost.x;

		SpawnUntillFull ();
	}

	public void OnDrawGizmos ()
	{
		Gizmos.DrawWireCube (transform.position, new Vector3 (width, height));
	}

	bool AllMembersDead ()//checks if every enemy is dead
	{
		foreach (Transform childPositionGameObject in transform) 
			if (childPositionGameObject.childCount > 0)
				return false;
		return true;
	}

	Transform NextFreePosition ()
	{
		foreach (Transform childPositionGameObject in transform) 
			if (childPositionGameObject.childCount == 0)
				return childPositionGameObject;
		return null;
	}

	void Update ()// Update is called once per frame
	{
		if (AllMembersDead ())
			SpawnUntillFull ();
		//moves formation left to right
		if (movingRight) 
			transform.position += Vector3.right * speed * Time.deltaTime;
		else 
			transform.position += Vector3.left * speed * Time.deltaTime;
		//check that fromation doesn't cross the borders
		float rightEdge = transform.position.x + (width / 2);
		float leftEdge = transform.position.x - (width / 2);
		if (leftEdge <= xMin)
			movingRight = true;
		else if (rightEdge >= xMax)
			movingRight = false;
	}
}
