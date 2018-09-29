using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour {

	public float speed = 1.0f;
	public float distanceCap = 3.0f;
	public Transform referencedWorldPos;
	public Transform target;
	public Transform forwardTarget;
	public float minThreshold = 0.05f;
	public float maxThreshold = 0.5f;
	private Vector3 oldReferencedWorldPos;
	private Vector3 oldMovementMomentum;
	public float alpha = 0.6f;
	public float beta = 0.4f;
	private States state;
    private Transform actionTarget = null;
    public Transform lightBulb;
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
    public Animator animatorBody;
    public Animator animatorFace;

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
            if (Vector3.Distance(transform.position, actionTarget.position)<distanceCap)
            {
                performAction();
            }
            else
            {
                float step = speed * Time.deltaTime;
                Vector3 desiredMovement;
                desiredMovement = (referencedWorldPos.position - oldReferencedWorldPos) + Vector3.MoveTowards(transform.position, actionTarget.transform.position, step);
                Vector3 smoothenedMovement;
                smoothenedMovement = alpha * (desiredMovement - transform.position) + beta * oldMovementMomentum;
                transform.position += smoothenedMovement;
                oldMovementMomentum = smoothenedMovement;
                oldReferencedWorldPos = referencedWorldPos.position;
            }
            Vector3 targetDir = actionTarget.position - transform.position;

            // The step size is equal to speed times frame time.
            float angularStep = angularSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, angularStep, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        if (state == States.Poked)
        {
            float step = speed * Time.deltaTime;
            Vector3 desiredMovement;
            desiredMovement = (referencedWorldPos.position - oldReferencedWorldPos) + Vector3.MoveTowards(transform.position, target.transform.position, 0);
            Vector3 smoothenedMovement;
            smoothenedMovement = alpha * (desiredMovement - transform.position) + beta * oldMovementMomentum;
            transform.position += smoothenedMovement;
            oldMovementMomentum = smoothenedMovement;
            oldReferencedWorldPos = referencedWorldPos.position;
            Vector3 targetDir = forwardTarget.position - transform.position;

            // The step size is equal to speed times frame time.
            float angularStep = angularSpeed * 3 * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, angularStep, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        else{
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
			if (state == States.Stationary) {
				Vector3 targetDir = forwardTarget.position - transform.position;

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
		Debug.Log ("State is now "+newState.ToString());
		if (newState==States.Action){
            //Set animator trigger
            animatorBody.SetTrigger("action");
            animatorFace.SetTrigger("action");
		}
		else if (newState==States.Moving){
            //Set animator trigger
            animatorBody.SetTrigger("move");
            animatorFace.SetTrigger("normal");
        }
		else if (newState==States.Stationary){
            //Set animator trigger
            animatorBody.SetTrigger("stay");
            animatorFace.SetTrigger("normal");
        }
		else if (newState==States.Poked){
            //Set animator trigger
            animatorBody.SetTrigger("stay");
            animatorFace.SetTrigger("poked");
            //timer?
        }
	}

	public void prepareAction(Action newAction){
        if (newAction == Action.TurnOnLight && lightBulb)
        {
            actionTarget = lightBulb;
            changeState(States.Action);
        }
		
	}

    public void performAction()
    {
        actionTarget = null;
        // animation trigger
    }

    public void endPoked(){
		if (actionTarget != null) {
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

    public void OnMouseDown()
    {
        changeState(States.Poked);
    }
}
