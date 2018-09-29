using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour {

	public float speed = 1.0f;
	public float distanceCap = 3.0f;
	public Transform referencedWorldPos;
	public Transform target;
	public float minThreshold = 0.05f;
	public float maxThreshold = 0.5f;
	private Vector3 oldReferencedWorldPos;

	// Use this for initialization
	void Start () {
		if (referencedWorldPos)
			oldReferencedWorldPos = referencedWorldPos.position;
	}

	// Update is called once per frame
	void Update () {
		if (referencedWorldPos && Vector3.Distance(referencedWorldPos.position, oldReferencedWorldPos)>=minThreshold &&
			Vector3.Distance(referencedWorldPos.position, oldReferencedWorldPos)<=maxThreshold)
			transform.position += referencedWorldPos.position - oldReferencedWorldPos;
		else if (Vector3.Distance (target.transform.position, transform.position) > distanceCap) {
			float step = speed * Time.deltaTime;
			// Move our position a step closer to the target.
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, step);
		}
		oldReferencedWorldPos = referencedWorldPos.position;
	}
}
