using UnityEngine;
using System.Collections;

public class SendMessageOnEnable : MonoBehaviour {
	public string message = "FunctionName";

	void Start () {
		SendMessage(message);
	}
}
