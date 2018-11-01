using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {

    public Transform target;        // Thing we're chasing
    public float updateRate = 2f;   // How many times each second we will update our path
    public Path path;               // The calculated path
    public float speed = 300f;      // The AI's speed per second
    public ForceMode2D fMode;
    [HideInInspector] public bool pathIsEnded = false;
    // The max distance from AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3f;

    Seeker seeker;
    Rigidbody2D rb;
    int currentWaypoint = 0;        // The waypoint we are currently moving towards
    bool searchForPlayer = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            if (!searchForPlayer)
            {
                searchForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        // Start a new path to target position, return result to OnPathComplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            if (!searchForPlayer)
            {
                searchForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        // TODO always look at player?
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            //Debug.Log("End of path reached.");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        // Direction to next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        // Move the AI
        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    IEnumerator SearchForPlayer()
    {
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        if (searchResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            searchForPlayer = false;
            target = searchResult.transform;
            StartCoroutine(UpdatePath());
            yield return false;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!searchForPlayer)
            {
                searchForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield return false;
        }

        // Start a new path to target position, return result to OnPathComplete method
        if (target != null)
            seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1 / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        //Debug.Log("We got a path. Did it have an error? " + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
