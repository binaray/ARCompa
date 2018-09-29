using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndRotateTowards : MonoBehaviour {

    public float angularSpeed = 2.0f;
    public Transform target;
    public float xOffset = 270.0f;
    public float yOffset = 180.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetDir = target.position - transform.position;

        // The step size is equal to speed times frame time.
        float step = angularSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        Debug.DrawRay(transform.position, newDir, Color.red);

        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
