using UnityEngine;

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
        LevelManager.Instance.LoadNextLevel();
    }
}
