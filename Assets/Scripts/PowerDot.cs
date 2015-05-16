using UnityEngine;
using System.Collections;

public class PowerDot : Photon.MonoBehaviour {
    private Renderer r;

    private GameController gameController;

    protected void Awake() {
        r = gameObject.GetComponentInChildren<Renderer>();

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    protected void Update() {
        if (r == null) return;
        r.material.color = new Color(Random.value, 0.0f, Random.value);
    }

    protected void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player1" || collider.tag == "Player2") {
            if (GetComponent<BoxCollider>() != null)  GetComponent<BoxCollider>().enabled = false;

            gameController.PlayEatPowerDot();

            // Increase pacman's speed
            collider.GetComponent<PacManMovement>().PowerUp();

            photonView.RPC("IncreasePlayerScore", PhotonTargets.All, collider.tag);
            photonView.RPC("DestroyPowerDot", PhotonTargets.Others);

            renderer.enabled = false;

            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        // Stub
    }

    [RPC]
    public void DestroyPowerDot() {
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
