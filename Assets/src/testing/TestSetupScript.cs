using UnityEngine;
using System.Collections;

public class TestSetupScript : MonoBehaviour {
	public GameObject PlayerWrapper;
	public bool ActivateTestSetup;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (ActivateTestSetup) {
			ActivateTestSetup = false;

			PlayerWrapper.transform.position = transform.position;
			PlayerWrapper.GetComponentInChildren<ThirdPersonControllerInput>().ResetTransform();
		}
	}
}
