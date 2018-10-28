/*
 * Copyright (c) Julian McNeill
 * http://julianmcneill.com
*/
using System.Collections;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{	
	void Update()
	{
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
	}
}