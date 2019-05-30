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
                //Some stuff to get the cursor to lock/unlock from the center of the screen.
		if (Input.GetKeyDown(KeyCode.LeftControl)) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		if (target != null && player != null) {

                        //Constantly rotate towards the attached target object, which moves based on mouse movement.

			transform.LookAt(target);
			cameraDir = -this.transform.forward;

                        //Then raycast backwards to see how far you can go before hitting something.
                        //Set the position to either where the raycast hit or the max distance set for the camera.

			RaycastHit hit;
			if (Physics.Raycast(player.position + player.right*horizontalOffset, cameraDir, out hit, maxDist)) {
				this.transform.position = hit.point;
			}
			else this.transform.position = player.position + cameraDir*maxDist + player.right*horizontalOffset;

			//This assumes the camera "player" target is a child of the actual player object.
                        //This sets the rotation of the player to the rotation of the camera, except it retains its current X rotation.

			if (lockPlayerRotation) {
				if (player.parent != null) {
					Transform testTransform = player.parent.transform;
					testTransform.localRotation = Quaternion.Euler(testTransform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
				}
			}
		}
	}
}
