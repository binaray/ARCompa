using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour {

	public float speed = 1.0f;
	public float distanceCap = 3.0f;
	public GameObject target;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

		if (Vector3.Distance (target.transform.position, transform.position) > distanceCap) {
			float step = speed * Time.deltaTime;
			// Move our position a step closer to the target.
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, step);
		}
	}
}
