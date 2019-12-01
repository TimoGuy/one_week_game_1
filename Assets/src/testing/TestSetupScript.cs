using UnityEngine;
using System.Collections;

public class TestSetupScript : MonoBehaviour {
	public GameObject PlayerWrapper;
	public bool ActivateTestSetup;
	public string InputString = "1";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (ActivateTestSetup || Input.GetKeyDown(InputString)) {
			ActivateTestSetup = false;

			PlayerWrapper.GetComponentInChildren<ThirdPersonControllerInput>().ResetTransform();
			PlayerWrapper.transform.position = transform.position;
		}
	}
}
