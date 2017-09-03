using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 15f;
	private float xmin;
	private float xmax;
	public float padding = 1f;
	public GameObject projectile;
	public float projectileSpeed = 5f;
	public float fireRate = 0.2f;
	public float health = 200f;
	public AudioClip fireSound;

	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost =  Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightMost =  Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xmin = leftMost.x + padding;
		xmax = rightMost.x - padding;
	}

	void Fire(){
		Vector3 offset = new Vector3 (0, 1, 0);
		GameObject beam = Instantiate (projectile, this.transform.position + offset, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.000001f, fireRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("Fire");
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			//this.transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
			this.transform.position += Vector3.left * speed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			//this.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
			this.transform.position += Vector3.right * speed * Time.deltaTime;
		}

		//restrict player to the game space
		float newX = Mathf.Clamp (this.transform.position.x, xmin, xmax);
		transform.position = new Vector3 (newX,transform.position.y, transform.position.z);
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
		LevelManager levelManager = GameObject.Find ("LevelManager").GetComponent<LevelManager> ();
		levelManager.LoadLevel ("Win Screen");
		Destroy (this.gameObject);
	}
}
