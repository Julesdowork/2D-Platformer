using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate = 0;
    public int damage = 10;
    public LayerMask hitLayer;
    public Transform bulletTrailPrefab;
    public float effectSpawnRate = 10f;
    public Transform muzzleFlashPrefab;
    public Transform hitPrefab;
    public float camShakeAmount = 0.05f;     // Handle camera shaking
    public float camShakeLength = 0.1f;
    public string shootSound = "DefaultShot";

    float timeToFire = 0;
    Transform firePoint;
    float timeToSpawnEffect = 0;
    CameraShake camShake;
    AudioManager audioManager;

    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
            Debug.LogError("No FirePoint? WHAAAAAAT?!");
    }

    void Start()
    {
        camShake = GameManager.instance.GetComponent<CameraShake>();
        if (camShake == null)
        {
            Debug.LogError("No CameraShake component found on GameManager instance!");
        }
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No Audio Manager found in scene.");
        }
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
        
        //Debug.DrawLine(firePointPos, (mousePos - firePointPos) * 100f, Color.cyan);
        if (hit.collider != null)
        {
            //Debug.DrawLine(firePointPos, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
                Debug.Log("Fired at " + hit.collider.name + " and did " + damage + " damage.");
            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPos = (mousePos - firePointPos) * 30f;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1f / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null)
        {
            // SET POSITIONS
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy(trail.gameObject, 0.04f);

        if (hitNormal != new Vector3(9999, 9999, 9999))
        {
            Transform hitParticle = Instantiate(hitPrefab, hitPos,
                Quaternion.FromToRotation(Vector3.right, hitNormal));
            Destroy(hitParticle.gameObject, 1f);
        }

        Transform clone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);

        // Shake the camera
        camShake.Shake(camShakeAmount, camShakeLength);

        // Play shoot sound
        audioManager.PlaySound(shootSound);
    }
}
