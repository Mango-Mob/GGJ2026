using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
public enum MaskMode
{
    Manual,
    Centred,
    Cursor,
}

[RequireComponent( typeof( Rigidbody2D ) )]
[RequireComponent( typeof( SpriteRenderer ) )]
[RequireComponent( typeof( Animator ) )]
public class Character : MonoBehaviour
{
    public MaskMode mode;

    [SerializeField] private Transform groundCheckBox;

    [Header( "Constants" )]
    [SerializeField] private float ground_check = 1.0f;

    [Header("Forward")]
    [SerializeField] protected AnimationCurve movement_curve;
    [SerializeField] protected float movement_duration = 1.0f;
    [SerializeField] protected float movement_amplitude = 1.0f;
    [SerializeField] private float movement_drag = 2.0f;
    private bool is_moving = false;
    private float movement_timer = 0.0f;
    private int movement_vector = 0;

    [Header( "Upwards" )]
    [SerializeField] protected AnimationCurve jump_curve;
    [SerializeField] protected float jump_duration = 1.0f;
    [SerializeField] protected float jump_amplitude = 1.0f;
    private bool is_jumping = false;
    private bool is_grounded = false;
    private float jump_timer = 0.0f;

    private Animator animator { get { return GetComponent<Animator>(); } }
    private SpriteRenderer visual { get { return GetComponent<SpriteRenderer>(); } }
    private Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void ProcessInput()
    {
        bool left = Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow );
        bool right = Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow );
        bool jump = Input.GetKey( KeyCode.Space ) || Input.GetKey( KeyCode.Keypad0 );

        movement_vector = 0;

        if ( left )
            movement_vector -= 1;

        if ( right )
            movement_vector += 1;

        if ( jump && is_grounded)
        {
            is_jumping = true;
            //movement_vector = 0;
        }
        else
        {
            is_jumping = false;
        }

        bool will_move = movement_vector != 0;
        if ( is_moving != will_move && !is_jumping)
        {
            movement_timer = 0.0f;
        }
        is_moving = will_move;
    }

    void ProcessAnimation()
    {
        animator.SetFloat( "XVelocity", body.linearVelocityX );
        animator.SetFloat( "YVelocity", body.linearVelocityY );
        animator.SetBool( "IsJumping", is_jumping );
        animator.SetBool( "IsGrounded", is_grounded );
    }

    void Update()
    {
        ProcessInput();

        

        ProcessAnimation();
    }

    private void FixedUpdate()
    {
        var hits = Physics2D.OverlapBoxAll(groundCheckBox.position, groundCheckBox.lossyScale, 0.0f);

        //RaycastHit2D valid_hit = new RaycastHit2D();
        bool has_hit = false;
        foreach ( var hit in hits )
        {
            if (hit.gameObject == this.gameObject )
                continue;
        
            //valid_hit = hit;
            has_hit = true;
            break;
        }

        is_grounded = has_hit;



        // Movement
        if (is_jumping)
        {
            body.linearVelocityX = 0;
            jump_timer += Time.deltaTime / jump_duration;
            if (jump_timer > 1.0f)
            {
                jump_timer = 1.0f;
                //is_jumping = false;
            }
        }

        if ((!is_jumping || !is_grounded) && jump_timer > 0)
        {
            body.linearVelocityY += jump_curve.Evaluate(jump_timer) * jump_amplitude;
            movement_timer = jump_timer;
            body.linearVelocityX = movement_curve.Evaluate(0.5f) * movement_vector;
            jump_timer = 0.0f;
        }
        else if ((!is_jumping) && is_moving)
        {
            movement_timer = Mathf.Clamp(movement_timer + Time.fixedDeltaTime / movement_duration, 0.0f, 1.0f);

            float speed_multiplier = movement_curve.Evaluate(movement_timer);
            float current_speed = speed_multiplier * movement_amplitude;

            body.linearVelocityX = movement_vector * current_speed;
        }
        else if (is_grounded && !is_moving) // Slowing
        {
            float oldVel = body.linearVelocityX;
            body.linearVelocityX -= Mathf.Sign(body.linearVelocityX) * Time.fixedDeltaTime * movement_drag;
            if (Mathf.Sign(body.linearVelocityX) != Mathf.Sign(oldVel))
            {
                body.linearVelocityX = 0.0f;
            }
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine( transform.position, transform.position + Vector3.down * ground_check );

        Gizmos.DrawWireCube(groundCheckBox.position, groundCheckBox.lossyScale);
    }
}
