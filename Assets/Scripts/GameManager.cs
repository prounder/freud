using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
	public static GameManager instance;

	private int stage;
	private int index;

	private Vector3 startLocation;

	private GameObject dummy;
	private GameObject ovrCameraRig;
	private GameObject localAvatar;

	private GameObject therapist;
	private GameObject fBody;
	private GameObject fHead;

	private GameObject headObject;
	private GameObject leftObject;
	private GameObject rightObject;

	private GameObject dummyHead;
	private GameObject dummyLeft;
	private GameObject dummyRight;

	private List<Vector3> leftPosHistory;
	private List<Vector3> rightPosHistory;
	private List<Vector3> headPosHistory;

	private List<Quaternion> leftRotHistory;
	private List<Quaternion> rightRotHistory;
	private List<Quaternion> headRotHistory;

	private AudioSource confession;

	private bool allPresent;

    
	void Awake(){
		if(GameManager.instance == null){
			GameManager.instance = this;
		}
		else if(GameManager.instance != this){
			Destroy(this);
		}

		allPresent = false;
		leftPosHistory = new List<Vector3>();
		rightPosHistory = new List<Vector3>();
		headPosHistory = new List<Vector3>();
		leftRotHistory = new List<Quaternion>();
		rightRotHistory = new List<Quaternion>();
		headRotHistory = new List<Quaternion>();
		confession = GetComponent<AudioSource>();
		print("はじめ！");
	}

    void Start(){
        stage = 0;
 		index = 0;
 		CheckInputs();

 		fHead = GameObject.Find("Freud-Head");
 		fBody = GameObject.Find("Freud");
 		dummy = GameObject.Find("Dummy");
 		dummyHead = GameObject.Find("dHead");
 		dummyLeft = GameObject.Find("dLeft");
 		dummyRight = GameObject.Find("dRight");
 		dummy.SetActive(false);

 		ovrCameraRig = GameObject.Find("OVRCameraRig");
 		therapist = GameObject.Find("Therapist");
 		localAvatar = GameObject.Find("LocalAvatar");

 		startLocation = ovrCameraRig.transform.position;
    }

    void Update(){
    	if(!allPresent)
    		CheckInputs();
    	if(!allPresent)
    		return;
        CheckTransition();

        switch(stage){
        	case 1:
        		leftPosHistory.Add(LeftPosition());
        		rightPosHistory.Add(RightPosition());
        		headPosHistory.Add(HeadPosition());

        		leftRotHistory.Add(LeftRotation());
        		rightRotHistory.Add(RightRotation());
        		headRotHistory.Add(HeadRotation());
        		break;
        	case 3:
        		dummyHead.transform.position = headPosHistory[index];
        		dummyLeft.transform.position = leftPosHistory[index];
        		dummyRight.transform.position = rightPosHistory[index];

        		dummyHead.transform.rotation = headRotHistory[index] * Quaternion.Euler(-90f, 0f, 0f);
        		dummyLeft.transform.rotation = leftRotHistory[index] * Quaternion.Euler(-90f, 0f, 0f);
        		dummyRight.transform.rotation = rightRotHistory[index] * Quaternion.Euler(-90f, 0f, 0f);

        		if(index < headPosHistory.Count - 1)
        			index++;
        		break;

        	default:
        		break;
        }


    }

    void CheckInputs(){
		if(headObject == null)
			headObject = GameObject.Find("CenterEyeAnchor");
		if(leftObject == null)
			leftObject = GameObject.Find("LeftHandAnchor");
		if(rightObject == null)
			rightObject = GameObject.Find("RightHandAnchor");

		if(headObject != null && leftObject != null && rightObject != null)
			allPresent = true;
		else
			return;
    }

    void CheckTransition(){
    	if(OVRInput.GetDown(OVRInput.Button.One)){
    		stage++;

    		print("Stage " + stage);

	    	switch(stage){
	    		case 1:
	    			confession.clip = Microphone.Start("", true, 300, 44100);
	    			break;
	    		case 2:
	    			Microphone.End("");

	    			fHead.SetActive(false);
	    			fBody.SetActive(false);
	    			
	    			ovrCameraRig.transform.position = therapist.transform.position;
	    			localAvatar.transform.position = therapist.transform.position;

	    			dummy.SetActive(true);
	    			dummyHead.transform.position = headPosHistory[index];
					dummyLeft.transform.position = leftPosHistory[index];
					dummyRight.transform.position = rightPosHistory[index];

					dummyHead.transform.rotation = headRotHistory[index];
					dummyLeft.transform.rotation = leftRotHistory[index];
					dummyRight.transform.rotation = rightRotHistory[index];
	    			break;
	    		case 3:
	    			confession.Play();
	    			break;
	    		case 4:
	    			ovrCameraRig.transform.position = startLocation;
	    			localAvatar.transform.position = startLocation;
	    			fHead.SetActive(true);
	    			fBody.SetActive(true);
	    			stage = 0;
	    			index = 0;
	    			ClearHistory();
	    			confession.clip = null;
	    			dummy.SetActive(false);
	    			break;

	    		default:
	    			break;
	    	}
    	}
    }

    void ClearHistory(){
    	leftPosHistory.Clear();
    	leftRotHistory.Clear();
    	rightPosHistory.Clear();
    	rightRotHistory.Clear();
    	headPosHistory.Clear();
    	headRotHistory.Clear();
    }

    Vector3 LeftPosition(){
    	return leftObject.transform.position;
    	// return OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
    }

    Vector3 RightPosition(){
    	return rightObject.transform.position;
    	// return OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
    }

    Vector3 HeadPosition(){
    	return headObject.transform.position;
    }

    Quaternion LeftRotation(){
    	return leftObject.transform.rotation;
    	// return OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
    }

    Quaternion RightRotation(){
    	return rightObject.transform.rotation;
    	// return OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    }

    Quaternion HeadRotation(){
    	return headObject.transform.rotation;
    }
}
