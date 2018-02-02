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

	Color color = new Color(0.25f, 0.25f, 0.25f);
	Color colorEmissionOn = new Color(0f, 0.7f, 0.85f);
	Color colorEmissionOff = new Color(0.5f, 0.5f, 0.5f);

	public int value = Trigger.ONE;
	public bool active = false;

	public bool SetActive(bool active) {
		if (this.active != active) {
			this.active = active;
			var go1 = transform.GetChild(0).gameObject;
			var	go2 = transform.GetChild(1).gameObject;
			if (this.active) {
				go2.GetComponent<Renderer>().material.SetColor("Color", color);
				go2.GetComponent<Renderer>().material.SetColor("_EmissionColor", colorEmissionOn);
			} else {
				go2.GetComponent<Renderer>().material.SetColor("Color", color);
				go2.GetComponent<Renderer>().material.SetColor("_EmissionColor", colorEmissionOff);
			}
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
