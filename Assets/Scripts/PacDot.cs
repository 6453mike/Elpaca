using UnityEngine;
using System.Collections;

public class PacDot : Photon.MonoBehaviour {
    private GameController gameController;

    protected void Awake() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    protected void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player1" || collider.tag == "Player2") {
            if (GetComponent<BoxCollider>() != null) GetComponent<BoxCollider>().enabled = false;

            gameController.PlayEatPacDot();

            photonView.RPC("IncreasePlayerScore", PhotonTargets.All, collider.tag);
            photonView.RPC("DestroyPacDot", PhotonTargets.Others);

            renderer.enabled = false;
        
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        // Stub
    }

    [RPC]
    public void DestroyPacDot() {
        renderer.enabled = false;
        PhotonNetwork.Destroy(gameObject);
    }

    [RPC]
    public void IncreasePlayerScore(string player) {
        switch (player) {
            case "Player1": gameController.IncreasePlayerOneScore(); break;
            case "Player2": gameController.IncreasePlayerTwoScore(); break;
        }
    }
}
