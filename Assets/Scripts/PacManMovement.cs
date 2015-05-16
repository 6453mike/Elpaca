using UnityEngine;
using System.Collections;

public class PacManMovement : MonoBehaviour {
    [SerializeField]
    private float defaultSpeed;

    [SerializeField]
    private float powerSpeed;

    private float CurrentSpeed { get; set; }

    private Vector3 Up {
        get {
            return transform.position + Config.TileWidth * Vector3.forward;
        }
    }

    private Vector3 Down {
        get {
            return transform.position - Config.TileWidth * Vector3.forward;
        }
    }

    private Vector3 Left {
        get {
            return transform.position - Config.TileWidth * Vector3.right;
        }
    }

    private Vector3 Right {
        get {
            return transform.position + Config.TileWidth * Vector3.right;
        }
    }

    public Vector3 Destination { get; set; }

    protected void Start() {
        CurrentSpeed = defaultSpeed;
        Destination = transform.position;
    }

    protected void FixedUpdate() {
        Vector3 p = Vector3.MoveTowards(transform.position, Destination, CurrentSpeed);
        rigidbody.MovePosition(p);

        if (transform.position == Destination) {
            if (Input.GetKey(KeyCode.W) && IsValidPosition(Up))
                Destination = Up;
            if (Input.GetKey(KeyCode.D) && IsValidPosition(Right))
                Destination = Right;
            if (Input.GetKey(KeyCode.S) && IsValidPosition(Down))
                Destination = Down;
            if (Input.GetKey(KeyCode.A) && IsValidPosition(Left))
                Destination = Left;
        }

        // Update the orientation of pacman to the direction of travel
        Vector3 direction = (Destination - transform.position).normalized;
        transform.forward = (direction != Vector3.zero) ? direction : transform.forward;
    }

    private bool IsValidPosition(Vector3 position) {
        // Cast Line from PacMan to next position in the given direction
        return !Physics.Linecast(transform.position, position, LayerMask.GetMask("Walls"));
    }

    private IEnumerator IncreaseSpeed() {
        CurrentSpeed = powerSpeed;
        yield return new WaitForSeconds(3.0f);
        CurrentSpeed = defaultSpeed;
    }

    public void PowerUp() {
        StartCoroutine(IncreaseSpeed());
    }
}
