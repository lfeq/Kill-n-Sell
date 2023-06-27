using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishZoneBehaviour : MonoBehaviour {

    private void OnTriggerEnter(Collider t_collision) {
        if (t_collision.CompareTag("Player")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}