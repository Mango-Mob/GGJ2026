using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour
{
    bool isHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit)
            return;

        Character character = collision.gameObject.GetComponent<Character>();
        if (character)
        {
            isHit = true;
            StartCoroutine(Kill());
            IEnumerator Kill()
            {
                character.animator.SetTrigger("Death");
                yield return new WaitForSeconds(1.0f);
                TransitionManager.instance.SetScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
