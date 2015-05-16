using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {
    protected void Start() {
        if (photonView.isMine) {
            GetComponent<PacManMovement>().enabled = true;
            GetComponent<PacManDeath>().enabled = true;
            GetComponent<AudioSource>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
        } else {
        }
    }
}
