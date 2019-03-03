using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

	public float speed;

	private GameObject ovrCameraRig;

    // Start is called before the first frame update
    void Start()
    {
        ovrCameraRig = GameObject.Find("CenterEyeAnchor");
    }

    // Update is called once per frame
    void Update()
    {
    	// print("Following!");
        Vector3 targetDir = ovrCameraRig.transform.position - transform.position;
        // targetDir.Normalize();

        // // Move our position a step closer to the target.
        // transform.rotation = Quaternion.Euler(targetDir.x, targetDir.y, targetDir.z);
        Quaternion newRotation = Quaternion.LookRotation(targetDir) * Quaternion.Euler(0, 90, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 1.0f);
    }
}
