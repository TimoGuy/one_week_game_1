using UnityEngine;
using System.Collections;

public class ScrollingTextures : MonoBehaviour {
    public float horizontalScrollSpeed = 0.25f;
    public float verticalScrollSpeed = 0.25f;
    public bool scroll = true;
	public bool hurt = false;
	private int hurtRedRenderCounter = 0;
	private Renderer myRenderer;

	void Start () {
		myRenderer = GetComponent<Renderer>();
	}

    void FixedUpdate () {
		myRenderer.material.SetColor("_Color", Color.white);
        if (scroll) {
            float verticalOffset = Time.time * verticalScrollSpeed;
            float horizontalOffset = Time.time * horizontalScrollSpeed;
            myRenderer.material.mainTextureOffset = new Vector2(horizontalOffset, verticalOffset);
        }
		if (hurt) {
			if (hurtRedRenderCounter % 4 == 0) {
				myRenderer.material.SetColor("_Color", new Color(100, 0, 0, 1));
			}
			hurtRedRenderCounter++;
		}
    }

    public void ToggleScroll () {
        scroll = !scroll;
    }
}
 