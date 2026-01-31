using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();
        if (character)
        {
            TransitionManager.instance.SetScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
