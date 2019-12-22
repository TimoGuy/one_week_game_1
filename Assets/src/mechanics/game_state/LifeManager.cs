using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class LifeManager : MonoBehaviour {
	public int currentLife = 10;
	public int decrementLifeAmount = 1;
	private TextMesh textMesh;
	private string startString;

	void Start () {
		textMesh = GetComponent<TextMesh>();
		startString = textMesh.text;
		RefreshTextMesh();
	}

	public int GetCurrentLife () {
		return currentLife;
	}

	public void DecrementLife(int multiplier=1) {
		currentLife -= decrementLifeAmount * multiplier;
		RefreshTextMesh();
	}

	private void RefreshTextMesh () {
		textMesh.text = startString + currentLife;
	}
}
