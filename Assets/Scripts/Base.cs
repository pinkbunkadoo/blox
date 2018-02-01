using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour {

	public GameObject pivot;
	public GameObject token;
	public GameObject glowPrefab;

	int mapWidth = 5;
	int mapHeight = 5;
	static float cubeSize = 1f;
	static float cubeHalfSize = cubeSize * 0.5f;

	int[] map = {
		1, 1, 1, 1, 1,
		1, 0, 0, 1, 1,
		1, 1, 0, 1, 1,
		1, 1, 1, 1, 1,
		1, 1, 1, 1, 1
		};

	float posX = 0;
	float posY = 0;

	float lastX = 0;
	float lastY = 0;
	float dx = 0;
	float dy = 0;

	float baseRotation = 0;

	float xRotation = 0;
	float zRotation = 0;

	float f = 0;

	float rotX = 0;
	float rotZ = 0;

	Vector3 lastPos;
	Vector3 mouseDownPos;

	float dragX = 0;
	float dragZ = 0;

	GameObject target;
	Vector3 targetPos;
	Quaternion targetRotation;
	Quaternion rotSave;

	UnityEngine.Plane targetPlane;

	// GameObject token;
	Vector3 dragDirection;

	GameObject cursor;
	GameObject cursorEnd;

	Transform parent;
	// Transform pivot;

	Vector3 rotPoint;
	Vector3 pivotOffset;
	
	Vector3 focus;

	GameObject cameraPivot;
	float cameraRotation = 0;


	void RotateCameraBy(float deg) {
		// Camera.main.transform.RotateAround(focus, Vector3.up, deg);
		cameraPivot.transform.Rotate(Vector3.up, deg);
		Camera.main.transform.LookAt(focus);
	}

	void SetPosition(int x, int y) {
		posX = x;
		posY = y;
		pivot.transform.position = parent.TransformVector(new Vector3(posX, 0, posY));
	}
	

	// Use this for initialization
	void Start () {
		token = GameObject.FindWithTag("Token");
		cursor = GameObject.Find("Cursor");
		cursorEnd = GameObject.Find("CursorEnd");
		cameraPivot = GameObject.Find("CameraPivot");

		// targetPos = token.transform.position;
		// pivotGO = new GameObject();
		// pivot = pivotObject.transform;

		// pivot.transform.position = new Vector3(0, token.transform.position.y - cubeHalfSize, 0);
		parent = pivot.transform.parent;
		posX = pivot.transform.localPosition.x;
		posY = pivot.transform.localPosition.z;

		focus = new Vector3(0, -0.6f, 0);
		// Camera.main.transform.LookAt(focus);

		cameraRotation = -45;
		RotateCameraBy(-cameraRotation);
		
	}

	// void SetPivot(Vector3 position) {
	// 	pivot.transform.position = position;

	// }

	void TestPosition() {
		RaycastHit hit;
		if (Physics.Raycast(pivot.transform.position, -Vector3.up, out hit, 10f)) {
			if (hit.transform.CompareTag("Trigger")) {
				GameObject go = hit.transform.gameObject;
				
				int value = go.GetComponent<Trigger>().value;
				int side = 0;

				if (Mathf.Round(token.transform.up.y) == -1) { // 1
					side = 1;
				}
				else if (Mathf.Round(token.transform.up.y) == 1) { // 6
					side = 6;
				}
				else if (Mathf.Round(token.transform.forward.y) == 1) { // 4
					side = 4;
				}
				else if (Mathf.Round(token.transform.forward.y) == -1) { // 3
					side = 3;
				}
				else if (Mathf.Round(token.transform.right.y) == -1) { // 5
					side = 5;
				}
				else if (Mathf.Round(token.transform.right.y) == 1) { // 2
					side = 2;
				}
				
				// print(value);
				// print(side);

				if (value == side) {
					print(side);
					if (go.GetComponent<Trigger>().SetActive(true)) {
						// var glow = Instantiate(glowPrefab, parent);
						// glow.transform.position = go.transform.position;

 						// GameObject light = new GameObject("Light");
        				// Light lightComp = light.AddComponent<Light>();
						// lightComp.range = 3;
						// lightComp.intensity = 0;
        				// lightComp.color = Color.red;
        				// light.transform.position = go.transform.position;
						// light.transform.parent = parent;

						// Renderer renderer = token.GetComponent<Renderer>();
						// renderer.material.SetColor("_Color", Color.cyan);
						// renderer.material.SetTextureOffset("_MainTex", new Vector2(0f, 0.5f));

					}
				}
			}
		}

		// var triggers = GameObject.FindGameObjectsWithTag("Trigger");
		// foreach (GameObject trigger in triggers) {
		// 	if (trigger.transform.position.x == posX && trigger.transform.position.z == posY) {

				// if (trigger.GetComponent<Trigger>().value > 0) {
				// 	if (Mathf.Round(token.transform.up.y) == -1) {
				// 		print("trigger1");
				// 	}
				// 	else if (Mathf.Round(token.transform.forward.y) == -1) {
				// 		print("trigger4");
				// 	}
				// }
			// }
            // Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
        // }

	}
	
	// Update is called once per frame
	void Update () {
		float mx = Input.GetAxis("Mouse X");
		float my = Input.GetAxis("Mouse Y");

		// GameObject.Find("Canvas/Text2").GetComponent<Text>().text = mx.ToString("n2");
		// GameObject.Find("Canvas/Text3").GetComponent<Text>().text = my.ToString("n2");

		// float fx = 0;
		// float fy = 0;
		// float fz = 0;

		if (Input.GetMouseButtonDown(0)) {
			baseRotation = 0;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

        	if (Physics.Raycast(ray, out hit, 100)) {				
				if (hit.transform.tag == "TokenPivot" && hit.normal == Vector3.up) {
					dx = 0;
					dy = 0;
					xRotation = 0;
					zRotation = 0;
					dragX = 0;
					dragZ = 0;

					pivotOffset = new Vector3(0, 0, 0);
					rotSave = pivot.transform.rotation;

					target = token; //hit.transform.gameObject;

					// targetPos = target.transform.position;
					// targetRotation = target.transform.rotation;

					// targetPlane = new UnityEngine.Plane(Vector3.up, new Vector3(0, hit.point.y, 0));

					// cursor.transform.position = hit.point;

					// float rayDistance;

					// if (targetPlane.Raycast(ray, out rayDistance)) {
					// 	Vector3 pos = ray.GetPoint(rayDistance);
					// 	mouseDownPos = pos;
					// }
					
				} else {
					target = null;
					
				}
			}
			lastX = Input.mousePosition.x;
			lastY = Input.mousePosition.y;
		}

		if (Input.GetMouseButton(0)) {
			// Vector3 v1 = Camera.main.transform.position - new Vector3(0, Camera.main.transform.position.y, 0) - focus;

			Vector3 v1 = (Camera.main.transform.position - new Vector3(0, Camera.main.transform.position.y, 0)).normalized;
			GameObject.Find("Canvas/Text1").GetComponent<Text>().text = "Cam:" + v1.ToString();

			// v1 = v1.normalized;
			// Vector3 p2 = focus;

			// float angle = parent.localEulerAngles.y;
			// float angle = (v1.x / v1.z) * Mathf.Rad2Deg;

			// float angle = Vector3.Angle(parent.right, v1);
			float angle = cameraRotation;
			GameObject.Find("Canvas/Text2").GetComponent<Text>().text = "Deg:" + angle.ToString("n2");

			float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
			float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

			dx = dx + Input.mousePosition.x - lastX;
			dy = dy + Input.mousePosition.y - lastY;

			if (target) {
				// cursorEnd.transform.position = cursor.transform.position + dir * 2;

				float cx = dx * cos - dy * sin;
				float cy = dx * sin + dy * cos;

				Vector3 dir = new Vector3(cx, 0, cy).normalized;
				GameObject.Find("Canvas/Text3").GetComponent<Text>().text = "Dir:" + dir.ToString();

				float d = Mathf.Sqrt(dx * dx + dy * dy);

				// GameObject img1 = GameObject.Find("Canvas/Image1");
				// GameObject img2 = GameObject.Find("Canvas/Image2");

				// Vector3 p1 = new Vector3(dx, dy, 0);
				// Vector3 p2 = new Vector3(cx, cy, 0);
				// img1.transform.position = new Vector3(320, 240, 0) + p1.normalized * 100;
				// img2.transform.position = new Vector3(320, 240, 0) + p2.normalized * 100;

				if (dragX == 0 && dragZ == 0 && Mathf.Abs(d) > 5) {
					pivotOffset = new Vector3(0, 0, 0);

					if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z)) {
						dragX = dir.x > 0 ? 1 : -1;
						if (dir.x > 0) {
							pivotOffset = parent.transform.right * cubeHalfSize;
						}
						else {
							pivotOffset = parent.transform.right * -cubeHalfSize;
						}
					}
					else {
						dragZ = dir.z > 0 ? 1 : -1;
						if (dir.z > 0) {
							pivotOffset = parent.transform.forward * cubeHalfSize;
						}
						else {
							pivotOffset = -parent.transform.forward * cubeHalfSize;
						}
					}

					pivotOffset += -parent.transform.up * cubeHalfSize;
					pivot.transform.position += pivotOffset;
					target.transform.position -= pivotOffset;
					// target.transform.position -= Vector3.up * 0.1f;

				} else {
					float rx = mx * cos - my * sin;
					float ry = mx * sin + my * cos;
					// float rx = dx * cos - dy * sin;
					// float ry = dx * sin + dy * cos;

					if (mx != 0 || my != 0) {
						if (dragX != 0) {
							zRotation += rx * 10f;
							zRotation = dragX > 0 ? Mathf.Clamp(zRotation, 0, 90) : Mathf.Clamp(zRotation, -90, 0);
							pivot.transform.localEulerAngles = new Vector3(0, 0, -zRotation);
						}
						else if (dragZ != 0) {
							xRotation += ry * 10f;
							xRotation = dragZ > 0 ? Mathf.Clamp(xRotation, 0, 90) : Mathf.Clamp(xRotation, -90, 0);
							pivot.transform.localEulerAngles = new Vector3(xRotation, 0, 0);
						}
					}
					
				}

				// GameObject.Find("Canvas/Text2").GetComponent<Text>().text = x.ToString();
				// GameObject.Find("Canvas/Text3").GetComponent<Text>().text = xRotation.ToString() + ", " + zRotation.ToString();

				// GameObject.Find("Canvas/Text2").GetComponent<Text>().text = pivot.transform.localEulerAngles.ToString();
			} else {
				// baseRotation = -mx;
				// parent.Rotate(parent.up, -mx * 8, Space.World);
				cameraRotation += -mx * 8;

				RotateCameraBy(mx * 8);
			}

			lastX = Input.mousePosition.x;
			lastY = Input.mousePosition.y;
		}

		if (Input.GetMouseButtonUp(0)) {

			if (target) {
				bool moved = false;
				// target.transform.rotation = targetRotation;
				// pivot.transform.rotation = Quaternion.identity;

				// target.transform.parent = null;
				Vector3 offset;

				pivot.transform.rotation = rotSave;

				if (zRotation > 45) {
					// rotZ -= 90;
					posX++;
					pivot.transform.position += parent.right;
					// target.transform.rotation = Quaternion.RotateTowards(target.transform.rotation, Quaternion.AngleAxis(90, pivot.transform.forward), 90);
					// target.transform.rotation = Quaternion.FromToRotation(Vector3.up, pivot.transform.right);
					target.transform.Rotate(-parent.forward, 90, Space.World);
					moved = true;
				}
				else if (zRotation < -45) {
					posX--;
					pivot.transform.position -= parent.right;
					target.transform.Rotate(parent.forward, 90, Space.World);
					moved = true;
					// target.transform.Rotate(0, 0, 90);
				}
				else if (xRotation > 45) {
					// rotX -= 90;
					posY++;
					pivot.transform.position += parent.forward;
					target.transform.Rotate(parent.right, 90, Space.World);
					moved = true;
					// target.transform.Rotate(90, 0, 0);
				}
				else if (xRotation < -45) {
					// rotX += 90;
					posY--;
					pivot.transform.position -= parent.forward;
					target.transform.Rotate(-parent.right, 90, Space.World);
					moved = true;
					// target.transform.Rotate(-90, 0, 0);
				}
				// target.transform.localEulerAngles = new Vector3(rotX, 0, rotZ);

				pivot.transform.position -= pivotOffset;
				target.transform.position += pivotOffset;
				pivot.transform.localPosition = new Vector3(posX, 0, posY);
				target.transform.localPosition = new Vector3(0, 0, 0);

				// pivot.transform.position = new Vector3(posX, 1, posY);

				// GameObject.Find("Canvas/Text3").GetComponent<Text>().text = target.transform.up.ToString();
				// GameObject.Find("Canvas/Text3").GetComponent<Text>().text = posX.ToString() + "," + posY.ToString();

				if (moved) {
					TestPosition();
					// Camera.main.GetComponent<MainCamera>().Refocus();
				}

			} else {
				// baseRotation = mx * 4;
			}

			target = null;
			
		}


		// baseRotation -= 0.1;
		// baseRotation = Mathf.Clamp(baseRotation, 0, )

		// token.transform.position = targetPos + new Vector3(fx, 0, fz);


		// token.transform.localEulerAngles = new Vector3(xRotation, 0, zRotation);
		// if (target) {
		// 	token.transform.position = targetPos + new Vector3(1 * fz, 0, -1 * fx);
		// }

		// GameObject.Find("Canvas/Text1").GetComponent<Text>().text = fx.ToString("n2");

		// text = GameObject.Find("Canvas/Text2").GetComponent<Text>();
		// text.text = zRotation.ToString("n2");

		// token.transform.Translate(new Vector3(xRotation, 0, zRotation));
		// token.transform.localEulerAngles = new Vector3(xRotation, 0, zRotation);
	}

	void LateUpdate() {
		// parent.Rotate(parent.up, baseRotation, Space.World);
		// baseRotation = Mathf.MoveTowards(baseRotation, 0, 10 * Time.deltaTime);

		// Camera.main.transform.RotateAround(token.transform.position, Vector3.up, baseRotation);
		// Camera.main.transform.LookAt(token.transform.position);

		// baseRotation = Mathf.MoveTowards(baseRotation, 0, 10 * Time.deltaTime);

	}
}
