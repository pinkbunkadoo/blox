using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
	public static int ONE = 1;
	public static int TWO = 2;
	public static int THREE = 3;
	public static int FOUR = 4;
	public static int FIVE = 5;
	public static int SIX = 6;

	public int value = Trigger.ONE;
	public bool active = false;

	public bool SetActive(bool a) {
		if (active != a) {
			active = a;
			// Mesh mesh = GetComponent<MeshFilter>().mesh;
			// var go = transform.Find("Symbol" + value.ToString()).gameObject;
			// var go = transform.GetChild(0).gameObject;
			// Renderer renderer = go.GetComponent<Renderer>();
			// renderer.material.SetTextureOffset("_MainTex", new Vector2(0f, 0.5f));
			// renderer.receiveShadows = false;

			var go = transform.GetChild(0).gameObject;
			go.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);

			go = transform.GetChild(1).gameObject;
			go.GetComponent<Renderer>().material.SetColor("_Emission", Color.cyan);

			return true;
		}
		return false;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
