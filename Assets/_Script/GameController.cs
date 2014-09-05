using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public string ipAddress = "128.2.239.44";
	public string port = "7899";
	
	public GameObject gem, handle;
	
	public bool isMirror = true;
	
	public float zOffset = 20;
	Quaternion temp = new Quaternion(0,0,0,0);
	
	
	#region GUI Variables
	string cameraStr = "Camera Switch On";
	string rStr = "0", gStr = "0", bStr = "0";
	string rumbleStr = "0";
	#endregion
	
	public GameObject sphere;
	
	// Use this for initialization
	void Start () {
		
	}
	
	
	void Update() {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if(PSMoveInput.IsConnected && PSMoveInput.MoveControllers[0].Connected) {
			
			Vector3 gemPos, handlePos;
			MoveData moveData = PSMoveInput.MoveControllers[0].Data;
			gemPos = moveData.Position;
			handlePos = moveData.HandlePosition;
			if(isMirror) {
				gem.transform.localPosition = gemPos;
				handle.transform.localPosition = handlePos;
				handle.transform.localRotation = Quaternion.Euler(moveData.Orientation);
			}
			else {
				gemPos.z = -gemPos.z + zOffset;
				handlePos.z = -handlePos.z + zOffset;
				gem.transform.localPosition = gemPos;
				handle.transform.localPosition = handlePos;
				handle.transform.localRotation = Quaternion.LookRotation(gemPos - handlePos);
				handle.transform.Rotate(new Vector3(0,0,moveData.Orientation.z));
				
				/* using quaternion rotation directly
			 * the rotations on the x and y axes are inverted - i.e. left shows up as right, and right shows up as left. This code fixes this in case 
			 * the object you are using is facing away from the screen. Comment out this code if you do want an inversion along these axes
			 * 
			 * Add by Karthik Krishnamurthy*/
				
				temp = moveData.QOrientation;
				temp.x = -moveData.QOrientation.x;
				temp.y = -moveData.QOrientation.y;
				handle.transform.localRotation = temp;
				
			}
			RaycastHit hit;
			if(Physics.Raycast(gemPos, gemPos - handlePos, out hit, 500.0f)){
				sphere.transform.position = hit.point;
			}
		}
	}
	
	void OnGUI() {
		
		if(!PSMoveInput.IsConnected) {
			
			GUI.Label(new Rect(20, 45, 30, 35), "IP:");
			ipAddress = GUI.TextField(new Rect(60, 45, 120, 25), ipAddress);
			
			GUI.Label(new Rect(190, 45, 30, 35), "port:");
			port = GUI.TextField(new Rect(230, 45, 50, 25), port);
			
			if(GUI.Button(new Rect(300, 40, 100, 35), "Connect")) {
				PSMoveInput.Connect(ipAddress, int.Parse(port));
			}
			
		}	
		
	}
	
	private void Reset() {
		cameraStr = "Camera Switch On";
		rStr = "0"; 
		gStr = "0"; 
		bStr = "0";
		rumbleStr = "0";
	}
}
