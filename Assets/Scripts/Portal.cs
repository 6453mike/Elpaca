using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
    [SerializeField]
    private Vector3 destination;

    [SerializeField]
    private Vector3 enterDirection;

    [SerializeField]
    private Vector3 exitDirection;

    protected void OnTriggerEnter(Collider collider) {
        if ((collider.tag == "Player1" || collider.tag == "Player2" ) && collider.transform.forward == enterDirection) {
            collider.transform.position = destination;
            collider.gameObject.GetComponent<PacManMovement>().Destination = destination + ((Config.TileWidth / 2) * exitDirection);
        } else if (collider.tag == "Phantom" && collider.gameObject.GetComponent<PhantomMovement>().CurrentDirection == enterDirection) {
            collider.transform.position = destination;
            collider.gameObject.GetComponent<PhantomMovement>().Destination = destination + ((Config.TileWidth / 2) * exitDirection);
        }
    }
}
