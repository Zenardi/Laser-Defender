using UnityEngine;
using System.Collections;

public class Shreadder : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider){
		Destroy (collider.gameObject);
	}
}
