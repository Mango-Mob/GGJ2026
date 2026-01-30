using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent( typeof( Rigidbody2D ) )]
public class Character : MonoBehaviour
{
    [Header( "Constants" )]
    [SerializeField] private float ground_check = 1.0f;

    [Header("Forward")]
    [SerializeField] protected AnimationCurve movement_curve;
    [SerializeField] protected float movement_duration = 1.0f;
    [SerializeField] protected float movement_amplitude = 1.0f;
    private bool is_moving = false;
    private float movement_timer = 0.0f;
    private int movement_vector = 0;

    [Header( "Upwards" )]
    [SerializeField] protected AnimationCurve jump_curve;
    [SerializeField] protected float jump_duration = 1.0f;
    [SerializeField] protected float jump_amplitude = 1.0f;
    private bool is_jumping = false;
    private float jump_timer = 0.0f;

    private Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void ProcessInput()
    {
        bool left = Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow );
        bool right = Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow );
        bool jump = Input.GetKeyDown( KeyCode.Space ) || Input.GetKeyDown( KeyCode.Keypad0 );

        if ( jump )
        {
            StartJump();
        }

        movement_vector = 0;

        if ( left )
            movement_vector -= 1;

        if ( right )
            movement_vector += 1;

        bool will_move = left ^ right;
        if ( is_moving != will_move )
        {
            movement_timer = 0.0f;
        }
        is_moving = will_move;
    }
    
    void Update()
    {
        ProcessInput();

        if ( is_jumping )
        {
            jump_timer += Time.deltaTime / jump_duration;
            if ( jump_timer > 1.0f )
            {
                jump_timer = 1.0f;
                is_jumping = false;
            }

            float speed_multiplier = jump_curve.Evaluate( jump_timer );
            float current_speed = speed_multiplier * jump_amplitude;

            body.linearVelocityY = current_speed * Time.deltaTime;
        }

        if ( is_moving )
        {
            movement_timer = Mathf.Clamp( movement_timer + Time.deltaTime / movement_duration, 0.0f, 1.0f );

            float speed_multiplier = movement_curve.Evaluate( movement_timer );
            float current_speed = speed_multiplier * movement_amplitude;

            body.linearVelocityX = movement_vector * current_speed * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //var hits = Physics2D.LinecastAll( transform.position, transform.position + Vector3.down * ground_check );
        //RaycastHit2D valid_hit = new RaycastHit2D();
        //bool has_hit = false;
        //foreach ( var hit in hits )
        //{
        //    if ( hit.collider.gameObject == this.gameObject )
        //        continue;
        //
        //    valid_hit = hit;
        //    has_hit = true;
        //    break;
        //}
        //
        //if ( !has_hit )
        //{
        //    transform.Translate( Vector3.down * gravity * Time.fixedDeltaTime );
        //}
        //else if ( !is_jumping ) 
        //{
        //    transform.position = new Vector2( transform.position.x, valid_hit.point.y );
        //}
    }

    void StartJump()
    {
        jump_timer = 0.0f;
        is_jumping = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine( transform.position, transform.position + Vector3.down * ground_check );
    }
}
