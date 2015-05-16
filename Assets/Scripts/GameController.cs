using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [SerializeField]
    private AudioClip eatPacDot;

    [SerializeField]
    private AudioClip eatPowerDot;

    [SerializeField]
    private AudioClip gameEnd;

    private Text player1Score;
    private Text player2Score;

    private const int MinTotalPoints = 298;

    private AudioSource audioSource;

    protected void Awake() {
        DontDestroyOnLoad(gameObject);

        player1Score = GameObject.FindGameObjectWithTag("Player1Score").GetComponent<Text>();
        player2Score = GameObject.FindGameObjectWithTag("Player2Score").GetComponent<Text>();

        audioSource = GetComponent<AudioSource>();
    }

    protected void Start() {
        print(GameObject.FindGameObjectsWithTag("PacDot").Length);
    }

    private void CheckForWin() {
        int s1 = int.Parse(player1Score.text);
        int s2 = int.Parse(player2Score.text);

        if ((s1 + s2) >= MinTotalPoints) {
            if (s1 > s2) {
                // Player one wins
                Application.LoadLevel("PlayerOneWins");
            } else if (s2 > s1) {
                // Player two wins
                Application.LoadLevel("PlayerTwoWins");
            } else {
                // Draw
                Application.LoadLevel("Draw");
            }
        }
    }

    public void IncreasePlayerOneScore() {
        player1Score.text = (int.Parse(player1Score.text) + 1).ToString();
        CheckForWin();
    }

    public void IncreasePlayerTwoScore() {
        player2Score.text = (int.Parse(player2Score.text) + 1).ToString();
        CheckForWin();
    }

    public void PlayEatPacDot() {
        audioSource.clip = eatPacDot;
        audioSource.Play();
    }

    public void PlayEatPowerDot() {
        audioSource.clip = eatPowerDot;
        audioSource.Play();
    }

    public void PlayGameEnd() {
        audioSource.clip = gameEnd;
        audioSource.Play();
    }
}
