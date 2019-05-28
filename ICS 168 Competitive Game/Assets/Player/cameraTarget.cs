using UnityEngine;
using System.Collections;

public class cameraTarget : MonoBehaviour {
	
	public int sensitivity;
	public float lockAngle;
	float deltaY;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		deltaY = Input.GetAxis("Mouse Y")/sensitivity;
		if ((Mathf.Abs(this.transform.parent.transform.eulerAngles.x-90) < lockAngle && deltaY < 0) || (Mathf.Abs(this.transform.parent.transform.eulerAngles.x-270) < lockAngle && deltaY > 0)) {
			transform.localPosition = new Vector3(Input.GetAxis("Mouse X")/sensitivity/1.25f, 0, 1);
		}
		else transform.localPosition = new Vector3(Input.GetAxis("Mouse X")/sensitivity/1.25f, Input.GetAxis("Mouse Y")/sensitivity, 1);
	}
}
