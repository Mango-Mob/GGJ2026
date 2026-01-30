using UnityEngine;
using UnityEngine.UIElements;

public class CursorMaskController : MonoBehaviour
{
    Camera cam;
    [SerializeField] private Transform insideCursorMask;
    [SerializeField] private Transform outsideCursorMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(mousePosition);
        transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);

        insideCursorMask.position = transform.position;
        outsideCursorMask.position = transform.position;
    }
}
