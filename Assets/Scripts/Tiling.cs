/*
 * Copyright (c) Julian McNeill
 * http://julianmcneill.com
*/
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour
{
    public int offsetX = 2;     // the offset so we don't get any weird errors
    // for checking if we need to instantiate stuff
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;   // used if object isn't tilable

    float spriteWidth = 0;              // width of our element
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Start()
	{
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x;
	}
	
	void Update()
	{
		if (!hasALeftBuddy || !hasARightBuddy)
        {
            // calculate camera's extents (half the width) of what camera can see in world coordinates
            float camHorizontalExtent = cam.orthographicSize * Screen.width / Screen.height;

            // calculate x position where camera can see edge of sprite (element)
            float edgeVisiblePositionRight = (transform.position.x + spriteWidth / 2) - camHorizontalExtent;
            float edgeVisiblePositionLeft = (transform.position.x - spriteWidth / 2) + camHorizontalExtent;

            // check to see if we can see edge of element
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && !hasARightBuddy)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && !hasALeftBuddy)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
	}


    void MakeNewBuddy(int direction)
    {
        // -1 for left side, 1 for right side
        Vector3 newPosition = new Vector3(transform.position.x + spriteWidth * direction,
            transform.position.y, transform.position.z);
        Transform newBuddy = Instantiate(transform, newPosition, transform.rotation);

        // if not tilable let's reverse x size of our object to get rid of ugly seams
        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1,
                newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = transform.parent;
        if (direction > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}