using UnityEngine;

namespace Utility
{
    public static class Geometric
    {
        public static Vector2 OnUnitSquare( Vector2 unit_vector )
        {
            if ( unit_vector == Vector2.zero )
                unit_vector = Random.insideUnitCircle;

            unit_vector = unit_vector.normalized;

            if ( 1.0f - Mathf.Abs( unit_vector.x ) < 1.0f - Mathf.Abs( unit_vector.y ) )
                unit_vector.x = ( unit_vector.x < 0 ) ? -1f : 1f;
            else
                unit_vector.y = ( unit_vector.y < 0 ) ? -1f : 1f;

            return unit_vector;
        }

        public static Vector3 MidPoint( Vector3 a, Vector3 b ) { return ( a + b ) / 2.0f; }
        public static Vector2 FromAngle( float radians, float distance ) { return new Vector2( Mathf.Cos( radians ), Mathf.Sin( radians ) ) * distance; }
        public static bool CircleVsCircle( Vector2 a, Vector2 b, float rA, float rB ) { return rA + rB > Vector2.Distance( a, b ); }
    }
}