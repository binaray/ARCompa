using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class componentAnimationController : MonoBehaviour {

    public PetController parentController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void endAction()
    {
        parentController.endAction();
    }

	public void endPoke()
	{
		parentController.endPoked();
	}
}
