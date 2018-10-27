﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate = 0;
    public float damage = 10f;
    public LayerMask hitLayer;

    float timeToFire = 0;
    Transform firePoint;

    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
            Debug.LogError("No FirePoint? WHAAAAAAT?!");
    }
	
	// Update is called once per frame
	void Update()
    {
		if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }
	}

    void Shoot()
    {
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPos, mousePos - firePointPos, 100f, hitLayer);
        Debug.DrawLine(firePointPos, (mousePos - firePointPos) * 100f, Color.cyan);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPos, hit.point, Color.red);
            Debug.Log("Fired at " + hit.collider.name + " and did " + damage + " damage.");
        }
    }
}