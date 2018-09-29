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
	private Vector3 oldMovementMomentum;
	public float alpha = 0.6f;
	public float beta = 0.4f;
	private States state;
	public Action action;
	public float angularSpeed = 2.0f;
	public float xOffset = 270.0f;
	public float yOffset = 180.0f;
	private enum States{
		Stationary,
		Moving,
		Action,
		Poked
	}
	public enum Action{
		TurnOnLight,
		TurnOffLight
	}

	// Use this for initialization
	void Start () {
		if (referencedWorldPos)
			oldReferencedWorldPos = referencedWorldPos.position;
		oldMovementMomentum = Vector3.zero;
		state = States.Stationary;
	}

	// Update is called once per frame
	void Update () {
		if (state == States.Action) {
			//perform action by moving
		} else if (state == States.Poked)
			return;
		else {
			float step = speed * Time.deltaTime;
			Vector3 desiredMovement;
			desiredMovement = (referencedWorldPos.position - oldReferencedWorldPos) + Vector3.MoveTowards (transform.position, target.transform.position, step);
			Vector3 smoothenedMovement;
			/*if (referencedWorldPos && Vector3.Distance (referencedWorldPos.position, oldReferencedWorldPos) >= minThreshold &&
			    Vector3.Distance (referencedWorldPos.position, oldReferencedWorldPos) <= maxThreshold) {
				smoothenedMovement = alpha * (desiredMovement - transform.position) + beta * oldMovementMomentum;
			} else
				smoothenedMovement = Vector3.zero;*/
			smoothenedMovement = alpha * (desiredMovement - transform.position) + beta * oldMovementMomentum;
			transform.position += smoothenedMovement;
			/*if (Vector3.Distance (target.transform.position, transform.position) > distanceCap) {
				float step = speed * Time.deltaTime;
				// Move our position a step closer to the target.
				transform.position = Vector3.MoveTowards (transform.position, target.transform.position, step);
			}*/
			oldMovementMomentum = smoothenedMovement;
			oldReferencedWorldPos = referencedWorldPos.position;
			if (Vector3.Distance (target.transform.position, transform.position) <= distanceCap) {
				if (state != States.Stationary)
					changeState (States.Stationary);
			} else {
				if (state != States.Moving)
					changeState (States.Moving);
			}

			if (state == States.Moving) {
				Vector3 targetDir = target.position - transform.position;

				// The step size is equal to speed times frame time.
				float angularStep = angularSpeed * Time.deltaTime;
				Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, angularStep, 0.0f);
				Debug.DrawRay (transform.position, newDir, Color.red);

				// Move our position a step closer to the target.
				transform.rotation = Quaternion.LookRotation (newDir);
			}
		}
	}

	void changeState(States newState){
		state = newState;
		if (newState==States.Action){
			//Set animator trigger
		}
		else if (newState==States.Moving){
			//Set animator trigger
		}
		else if (newState==States.Stationary){
			//Set animator trigger
		}
		else if (newState==States.Poked){
			//Set animator trigger
		}
	}

	public void performAction(Action newAction){
		action = newAction;
		changeState (States.Action);
	}

	public void endPoked(){
		if (action != null) {
			changeState (States.Action);
		} else if (Vector3.Distance (target.transform.position, transform.position) <= distanceCap) {
			changeState (States.Moving);
		} else {
			changeState (States.Stationary);
		}
	}

	public void endAction(){
		if (Vector3.Distance (target.transform.position, transform.position) <= distanceCap) {
			changeState (States.Moving);
		} else {
			changeState (States.Stationary);
		}
	}
}
