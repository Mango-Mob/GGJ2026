using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CursorMaskController : MonoBehaviour
{
    public static CursorMaskController instance { get; private set;}

    Camera cam;
    [SerializeField] private Transform insideCursorMask;
    [SerializeField] private Transform outsideCursorMask;
    [SerializeField] private float rotateSpeed = 45.0f;
    [SerializeField] private float maxMoveSpeed = 5.0f;
    private Animator animator;
    private string sceneName;
    public Vector2 velocity = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;

        cam = Camera.main;
        animator = GetComponent<Animator>();
        animator.enabled = false;

        sceneName = SceneManager.GetActiveScene().name;

        if (Character.instance && Character.instance.mode == MaskMode.Cursor) // Start on player
        {
            transform.position = Character.instance.transform.position + Vector3.up * 0.25f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 lastPosition = transform.position;
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
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z), maxMoveSpeed * Time.fixedDeltaTime);
                    break;
            }
        }
        else // Main menu
        {
            //Debug.Log("Moving cursor mask in menu");
            Vector3 mousePosition = Input.mousePosition;
            Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
        }

        insideCursorMask.position = transform.position;
        outsideCursorMask.position = transform.position;
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
