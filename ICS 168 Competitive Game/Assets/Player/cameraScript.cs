using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {
	
	public Transform target;
	public Transform player;
	public LayerMask raycastTargets;
	public float maxDist;
	public Vector3 cameraDir;
	public float horizontalOffset;
	public bool lockPlayerRotation;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftControl)) Cursor.lockState = CursorLockMode.Locked;
		if (target != null && player != null) {
			transform.LookAt(target);
			cameraDir = -this.transform.forward;
			RaycastHit hit;
			if (Physics.Raycast(player.position + player.right*horizontalOffset, cameraDir, out hit, maxDist)) {
				this.transform.position = hit.point;
			}
			else this.transform.position = player.position + cameraDir*maxDist + player.right*horizontalOffset;
			
			if (lockPlayerRotation) {
				Transform testTransform = player.parent.transform;
				testTransform.localRotation = Quaternion.Euler(testTransform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			}
		}
	}
}
