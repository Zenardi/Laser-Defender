using UnityEngine;
using System.Collections;

public class EnemyBahaviour : MonoBehaviour {
	public float health = 150f;
	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float shotsFrequency = 0.5f;
	private ScoreKeeper scoreKeeper;
	public int scoreValue = 150;
	public AudioClip fireSound;
	public AudioClip deathSoud;

	// Use this for initialization
	void Start () {
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();
	}
	
	// Update is called once per frame
	void Update () {
		float probability = shotsFrequency * Time.deltaTime;
		if (Random.value < probability) {
			Fire ();
		}
	}


	void Fire(){
		//Vector3 startPosition = transform.position + new Vector3 (0, -1, 0);
		GameObject missile = Instantiate (projectile, this.transform.position, Quaternion.identity) as GameObject;
		missile.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, -projectileSpeed, 0);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}
	void OnTriggerEnter2D(Collider2D collider)
	{
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();

		if (missile) {
			health -= missile.getDamage ();
			missile.Hit ();
			if (health <= 0) {
				Die ();
			}

		}
	}

	private void Die(){
		AudioSource.PlayClipAtPoint (deathSoud, this.transform.position);
		Destroy (this.gameObject);
		scoreKeeper.Score (scoreValue);
	}
}
