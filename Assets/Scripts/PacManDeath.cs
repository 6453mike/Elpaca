using UnityEngine;
using System.Collections;

public class PacManDeath : MonoBehaviour {
    [SerializeField]
    private Vector3 respawnPosition;

    public void Die() {
        transform.position = respawnPosition;
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource.enabled) audioSource.Play();
        GetComponent<PacManMovement>().Destination = transform.position;
    }
}
