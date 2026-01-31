using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CursorMaskController : MonoBehaviour
{
    Camera cam;
    [SerializeField] private Transform insideCursorMask;
    [SerializeField] private Transform outsideCursorMask;
    [SerializeField] private float rotateSpeed = 45.0f;
    private Animator animator;
    private string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();

        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Character.instance)
        {
            animator.enabled = Character.instance.mode == MaskMode.Manual && (sceneName == "1" || sceneName == "2");
            switch (Character.instance.mode)
            {
                case MaskMode.Manual:
                    break;
                case MaskMode.Centred:
                    transform.position = Character.instance.transform.position + Vector3.up * 0.25f;
                    break;
                case MaskMode.Cursor:
                    Vector3 mousePosition = Input.mousePosition;
                    Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(mousePosition);
                    transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
                    break;
            }
        }
        else // Main menu
        {
            Debug.Log("Moving cursor mask in menu");
            Vector3 mousePosition = Input.mousePosition;
            Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
        }

        insideCursorMask.position = transform.position;
        outsideCursorMask.position = transform.position;
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
