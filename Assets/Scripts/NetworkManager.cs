using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : Photon.MonoBehaviour {
    private const string Version = "v0.0.1";

    private bool isRoomCreator;
    private bool player1Ready;
    private bool player2Ready;

    private GameObject player1;
    private GameObject player2;

    private GameObject phantom1;
    private GameObject phantom2;

    protected void Awake() {
        DontDestroyOnLoad(gameObject);

        isRoomCreator = false;
        player1Ready = false;
        player2Ready = false;
    }

    protected void Start() {
        PhotonNetwork.ConnectUsingSettings(Version);
    }

    protected void OnJoinedRoom() {
        if (PhotonNetwork.room.playerCount == 1) {
            isRoomCreator = true;
        } else {
            photonView.RPC("OnPlayerTwoConnected", PhotonTargets.All);            
        }
    }


    protected void OnLevelWasLoaded(int level) {
        if (level == 2) {
            if (isRoomCreator) {
                player1 = PhotonNetwork.Instantiate("Player1", new Vector3(-1.5f, 0.0f, -2.0f), Quaternion.identity, 0);
                photonView.RPC("SetPlayer1To", PhotonTargets.All, player1.GetPhotonView().viewID); 
                photonView.RPC("SetPlayer1Ready", PhotonTargets.All);
            } else {
                player2 = PhotonNetwork.Instantiate("Player2", new Vector3(1.5f, 0.0f, -2.0f), Quaternion.identity, 0);
                photonView.RPC("SetPlayer2Ready", PhotonTargets.All);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        // Stub
    }

    private IEnumerator Loading() {
        while (true) {
            if (player1Ready && player2Ready) {
                if (!isRoomCreator) {
                    phantom1 = PhotonNetwork.Instantiate("Phantom1", new Vector3(-0.5f, 0.0f, 2.0f), Quaternion.identity, 0);
                    phantom2 = PhotonNetwork.Instantiate("Phantom2", new Vector3(0.5f, 0.0f, 2.0f), Quaternion.identity, 0);

                    phantom1.GetComponent<PhantomMovement>().AddTarget(player1.transform);
                    phantom1.GetComponent<PhantomMovement>().AddTarget(player2.transform);
                    phantom2.GetComponent<PhantomMovement>().AddTarget(player1.transform);
                    phantom2.GetComponent<PhantomMovement>().AddTarget(player2.transform);
                }
                break;
            }

            yield return 0.0f;
        }
    }

    [RPC]
    public void OnPlayerTwoConnected() {
        PhotonNetwork.LoadLevel("Main");
        StartCoroutine(Loading());
    }

    [RPC]
    public void SetPlayer1Ready() {
        player1Ready = true;
    }

    [RPC]
    public void SetPlayer2Ready() {
        player2Ready = true;
    }

    [RPC]
    public void SetPlayer1To(int id) {
        player1 = PhotonView.Find(id).gameObject;
    }

    public void CreateOrJoinRoom() {
        string roomName = GameObject.FindGameObjectWithTag("RoomNameField").GetComponent<InputField>().text;
        RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 2 };

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        PhotonNetwork.LoadLevel("Wait");
    }
}