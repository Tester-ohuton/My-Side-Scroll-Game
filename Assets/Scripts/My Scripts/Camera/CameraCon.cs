using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCon : MonoBehaviour
{
	Transform cameraTrans;
	[SerializeField] Transform playerTrans;

	[SerializeField] Vector3 cameraPos;  //Vector3(0, 1, -1)
	[SerializeField] Vector3 cameraRot;  //Vector3(45, 0, 0)

	void Awake()
	{
		cameraTrans = transform;
		cameraTrans.rotation = Quaternion.Euler(cameraRot);
	}

    void LateUpdate()
	{
		if (cameraTrans != null)
		{
			cameraTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y, 0) + cameraPos;
		}
	}
}