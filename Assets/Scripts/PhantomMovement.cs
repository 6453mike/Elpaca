using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhantomMovement : MonoBehaviour {
    [SerializeField]
    private float defaultSpeed;

    [SerializeField]
    private List<Transform> targets;

    [SerializeField]
    private Vector3 exitWaypoint;

    private float CurrentSpeed { get; set; }

    private Vector3 RelativeLeft {
        get {
            return transform.position + Config.TileWidth * new Vector3(-CurrentDirection.z, 0.0f, CurrentDirection.x);
        }
    }

    private Vector3 RelativeRight {
        get {
            return transform.position + Config.TileWidth * new Vector3(CurrentDirection.z, 0.0f, -CurrentDirection.x);
        }
    }

    private Vector3 RelativeForward {
        get {
            return transform.position + Config.TileWidth * CurrentDirection;
        }
    }

    public Vector3 Destination { get; set; }
    public Vector3 CurrentDirection { get; set; }

    protected void Start() {
        CurrentSpeed = defaultSpeed;
        CurrentDirection = Vector3.forward;
        Destination = exitWaypoint;
    }

    protected void FixedUpdate() {
        Vector3 p = Vector3.MoveTowards(transform.position, Destination, CurrentSpeed);
        rigidbody.MovePosition(p);

        if (transform.position == Destination) {
            Destination = GetNewDestination();
            CurrentDirection = (Destination - transform.position).normalized;
        }
    }

    protected void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player1" || collider.tag == "Player2") {
            collider.GetComponent<PacManDeath>().Die();
        }
    }

    private Vector3 GetNewDestination() {
        Vector3 newDestination = transform.position;
        float shortestDistanceToTarget = 100.0f;
        foreach (Vector3 p in GetNextValidPositions()) {
            float tempDistance = Vector3.Distance(p, GetNearestTarget().position);
            if (tempDistance < shortestDistanceToTarget) {
                newDestination = p;
                shortestDistanceToTarget = tempDistance;
            }
        }

        return newDestination;
    }

    private List<Vector3> GetNextValidPositions() {
        List<Vector3> positions = new List<Vector3>();

        if (IsValidPosition(RelativeLeft)) positions.Add(RelativeLeft);
        if (IsValidPosition(RelativeRight)) positions.Add(RelativeRight);
        if (IsValidPosition(RelativeForward)) positions.Add(RelativeForward);

        return positions;
    }

    private Transform GetNearestTarget() {
        Transform nearestTarget = transform;
        float shortestDistanceToTarget = 100.0f;
        foreach (Transform t in targets) {
            float tempDistance = Vector3.Distance(transform.position, t.position);
            if (tempDistance < shortestDistanceToTarget) {
                nearestTarget = t;
                shortestDistanceToTarget = tempDistance;
            }
        }

        return nearestTarget;
    }

    private bool IsValidPosition(Vector3 position) {
        // Cast Line from PacMan to next position in the given direction
        return !Physics.Linecast(transform.position, position, LayerMask.GetMask("Walls"));
    }

    public void AddTarget(Transform target) {
        targets.Add(target);
    }
}
