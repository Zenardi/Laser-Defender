using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width;
	public float height;
	private bool movingRight = false;
	public float speed = 5f;
	private float xmax;
	private float xmin;
	public float spawnDelay = 0.5f;

	// Use this for initialization
	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanceToCamera));
		xmax = rightBoundary.x;
		xmin = leftBoundary.x;
		SpawnUntilFull ();
	}

	public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube (transform.position, new Vector3 (width, height));
	}

	// Update is called once per frame
	void Update () {
		if (movingRight) {
			//this.transform.position += new Vector3 (speed * Time.deltaTime, 0, 0);
			this.transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			//this.transform.position += new Vector3 (-speed * Time.deltaTime, 0, 0);
			this.transform.position += Vector3.left * speed * Time.deltaTime;
		}

		float rightEdgeFormation = transform.position.x + (0.5f * width);
		float leftEdgeFormation = transform.position.x -(0.5f * width);
		if (leftEdgeFormation < xmin) {
			movingRight = true;	
		} else if (rightEdgeFormation > xmax) {
			movingRight = false;
		}
		if(AllMembersDead())
			SpawnUntilFull ();

		}


	private void SpawnUntilFull(){
		Transform freePosition = NextFreePosition ();
		if (freePosition != null) {
			GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}

		//If there is a free position
		if(NextFreePosition())
			Invoke ("SpawnUntilFull", spawnDelay);
	}

	private void SpawnEnemies(){
		foreach (Transform child in transform)
		{
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}

	private Transform NextFreePosition()
	{
		foreach (Transform child in this.transform) {
			if (child.childCount == 0)
				return child;
		}

		return null;
	}
	private bool AllMembersDead(){
		
		foreach (Transform child in this.transform) {
			if (child.childCount > 0)
				return false;
		}

		return true;

	}
}
