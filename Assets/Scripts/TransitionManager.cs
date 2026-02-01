using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance { private set; get; }

    public delegate void TransitionEvent();
    Animator animator;

    public TransitionEvent nextEvent;
    public bool interuptable = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void NextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (sceneIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.Log("Resetting");
            sceneIndex = 0;
        }
        SetScene(sceneIndex);
    }
    public void SetScene(int _index)
    {
        DoInTransition(() => { SceneManager.LoadScene(_index); });
    }
    public void DoInTransition(TransitionEvent _event, bool is_interuptable = false )
    {
        if (nextEvent != null)
        {
            if( interuptable )
            {
                nextEvent = _event;
                interuptable = is_interuptable;
            }
            return;
        }

        interuptable = is_interuptable;
        nextEvent = _event;
        animator.Play("TransitionOut");
    }

    public void PerformEvent()
    {
        if (nextEvent == null)
            return;

        nextEvent.Invoke();
        nextEvent = null;
    }
    public void QuitGame()
    {
        DoInTransition( () => 
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        } );
    }
}
