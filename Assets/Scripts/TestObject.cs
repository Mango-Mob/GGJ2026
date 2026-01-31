using UnityEngine;

public class TestObject : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10.0f)
        {
            rb.linearVelocity = Vector2.zero;
            transform.position = startPos;
        }
    }
}
