using UnityEngine;

public class Character : MonoBehaviour
{
    public AnimationCurve jump_curv;
    public AnimationCurve forward_vel;
    public float time_to_top_speed = 1.0f;
    private float acceleration = 0.0f;
    public float grav_const = 9.81f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int forward_vector = 0;
        bool left = Input.GetKeyDown( KeyCode.A ) || Input.GetKeyDown( KeyCode.LeftArrow );
        bool right = Input.GetKeyDown( KeyCode.D ) || Input.GetKeyDown( KeyCode.RightArrow );

        if ( left )
            forward_vector += 1;
        if ( right ) 
            forward_vector -= 1;

        acceleration += forward_vector * Time.deltaTime;
        if ( acceleration == 0.0f )
            return;

        float direct = Mathf.Abs( acceleration ) / acceleration;
        Vector2 position = transform.position;
        position.x += direct * forward_vel.Evaluate( Mathf.Abs( acceleration ) ) * Time.deltaTime;

        transform.position = position;
    }
}
