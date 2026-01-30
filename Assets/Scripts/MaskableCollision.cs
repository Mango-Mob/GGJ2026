using UnityEngine;

public class MaskableCollision : MonoBehaviour
{
    // JUST KIDDING MY JOB HAS BEEN TAKEN BY A UNITY COMPONENT ASDHAYDGYUAGSDGASKLDGLJASD

    //[SerializeField] private Collider2D maskCollider;

    //[SerializeField] private SpriteRenderer spriteRenderer;
    //private PolygonCollider2D polyCollider;
    //private bool visibleInside = false;

    //Vector2[] defaultPoints;

    //private void Awake()
    //{
    //    polyCollider = GetComponent<PolygonCollider2D>();

    //    switch (spriteRenderer.maskInteraction)
    //    {
    //        case SpriteMaskInteraction.VisibleInsideMask:
    //            visibleInside = true;
    //            break;
    //        case SpriteMaskInteraction.VisibleOutsideMask:
    //            visibleInside = false;
    //            break;
    //        default:
    //            Debug.LogError("Set mask interaction to something, like literally anything.");
    //            break;
    //    }
    //}

    //void Start()
    //{
    //    defaultPoints = polyCollider.points;
    //}
    
    //void Update()
    //{
        
    //}
    //public void CalculatePolygonCollider()
    //{
    //    // Check if mask is intersecting with mask
    //    polyCollider.enabled = true;
    //    bool isIntersecting = polyCollider.IsTouching(maskCollider);

    //    if (!visibleInside) 
    //    {
    //        if (!isIntersecting) // It is visible outside mask and is outside fully
    //        {
    //            polyCollider.points = defaultPoints; // Use default state of collider
    //            return;
    //        }

    //        // There is an intersection. We need to find point along the circle that collide with the base collider.
    //        // Help me


    //    }
    //    else
    //    {
    //        if (!isIntersecting) // If it is only visible inside mask and is outside fully
    //        {
    //            polyCollider.enabled = false;
    //            return;
    //        }
    //    }
    //}
}
