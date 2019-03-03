using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

	public float speed;

	private GameObject ovrCameraRig;

	private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        ovrCameraRig = GameObject.Find("CenterEyeAnchor");
    }

    // Update is called once per frame
    void Update()
    {
    	if(GameManager.instance.GetStage() == 2)
    		target = ovrCameraRig.transform.position + ovrCameraRig.transform.forward;
    	else
    		target = ovrCameraRig.transform.position;
        Vector3 targetDir = target - transform.position;
        Quaternion newRotation = Quaternion.LookRotation(targetDir) * Quaternion.Euler(0, 90, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 1.0f);
    }
}
