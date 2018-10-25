/*
 * Copyright (c) Julian McNeill
 * http://julianmcneill.com
*/
using System.Collections;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    public Transform[] backgrounds;     // list of all back- and foregrounds to be parallaxed
    public float smoothing = 1f;        // how smooth parallax effect is going to be. Make sure to set this above 0.

    float[] parallaxScales;             // proportion of camera's movement to move backgrounds by
    Transform cam;                      // reference to main camera's transform
    Vector3 previousCamPos;             // position of camera in previous frame

    // Great for references
    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
	{
        previousCamPos = cam.position;
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	void Update()
	{
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // parallax is opposite of camera movement because previous frame is multiplied by scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // set a target x position which is current position plus parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // create a target position which is background's current position with it's target position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX,
                backgrounds[i].position.y, backgrounds[i].position.z);

            // fade between current position and target position
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position,
                backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
	}
}