using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    public float min_x;
    public float min_y;
    public float max_x;
    public float max_y;

    private void Start()
    {
        transform.position = new Vector3( Random.Range( min_x, max_x ), Random.Range( min_y, max_y ), gameObject.transform.position.z );

        if ( transform.position.x > 0 )
            transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.R ) )
        {
            transform.position = new Vector3( Random.Range( min_x, max_x ), Random.Range( min_y, max_y ), gameObject.transform.position.z );

            if ( transform.position.x > 0 )
                transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
        }
    }
}
