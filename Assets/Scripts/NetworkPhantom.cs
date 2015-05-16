using UnityEngine;
using System.Collections;

public class NetworkPhantom : Photon.MonoBehaviour {
    protected void Start() {
        if (photonView.isMine) {
            PhantomMovement pm = GetComponent<PhantomMovement>();
            pm.enabled = true;

            GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
            GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
            pm.AddTarget(player1.transform);
            pm.AddTarget(player2.transform);
        } else {
        }
    }
}
