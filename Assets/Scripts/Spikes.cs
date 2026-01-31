using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Character>())
        {
            TransitionManager.instance.SetScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
