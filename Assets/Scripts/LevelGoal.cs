using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>())
        {
            NextLevel();
        }
    }

    public void NextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}
