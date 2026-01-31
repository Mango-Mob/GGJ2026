using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Character>())
        {
            NextLevel();
        }
    }
    public void NextLevel()
    {
        Character.instance.PlayAudio(4);
        TransitionManager.instance.NextScene();
    }
    public void ReloadScene()
    {
        TransitionManager.instance.SetScene(SceneManager.GetActiveScene().buildIndex);
    }
}
