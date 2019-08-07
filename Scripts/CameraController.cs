using UnityEngine;
using System.Collections;
 
public class CameraController : MonoBehaviour 
{
	
	private const int LevelArea = 10;

	private const int ScrollArea = 1;
	private const int ScrollSpeed = 10;
	private const int DragSpeed = 100;


	private const int PanSpeed = 50;
	private const int PanAngleMin = 50;
	private const int PanAngleMax = 80;

	// Update is called once per frame
	void Update()
	{
		// Init camera translation for this frame.
		var translation = Vector3.zero;



		// Move camera with arrow keys
		translation += new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);

		// Move camera if mouse pointer reaches screen borders
		if (Input.mousePosition.x < ScrollArea)
		{
			translation += Vector3.right * -ScrollSpeed * Time.deltaTime;
		}

		if (Input.mousePosition.x >= Screen.width - ScrollArea)
		{
			translation += Vector3.right * ScrollSpeed * Time.deltaTime;
		}

		if (Input.mousePosition.y < ScrollArea)
		{
			translation += Vector3.forward * -ScrollSpeed * Time.deltaTime;
		}

		if (Input.mousePosition.y > Screen.height - ScrollArea)
		{
			translation += Vector3.forward * ScrollSpeed * Time.deltaTime;
		}


		// Keep camera within level and zoom area
		var desiredPosition = transform.position + translation;
		if (desiredPosition.x < -LevelArea || LevelArea < desiredPosition.x)
		{
			translation.x = 0;
		}
		if (desiredPosition.z < -LevelArea || LevelArea < desiredPosition.z)
		{
			translation.z = 0;
		}

		
		transform.localPosition += translation;
	}

}