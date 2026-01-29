using System.Drawing;
using UnityEngine;

namespace Utility
{
    public static class Extentions
    {
        public static Vector3 DirectionTo( this Vector3 from, Vector3 to ) { return ( to - from ).normalized; }
        public static void SetEnabled( this MonoBehaviour component, bool status ) { component.enabled = status; }
        
        //Flip animation curve axis to find the time with a known value (only for 1-1 curves, not curves that return to a value).
        public static float EvaluateInverse( this AnimationCurve curve, float value )
        {
            var inverse = new AnimationCurve();
            for ( int i = 0; i < curve.length; i++ )
                inverse.AddKey( new Keyframe( curve.keys[ i ].value, curve.keys[ i ].time ) );

            return inverse.Evaluate( value );
        }

        public static void DrawCircle( this Gizmos gizmos, Vector3 point, float radius )
        {
            var flat_matrix = Matrix4x4.TRS( point, Quaternion.identity, new Vector3( 1f, 0f, 1f ) );
            Gizmos.matrix = Gizmos.matrix * flat_matrix;
            Gizmos.DrawWireSphere( Vector3.zero, radius );
            Gizmos.matrix = Gizmos.matrix * flat_matrix.inverse;
        }

        public static void DrawSquare( this Gizmos gizmos, Vector3 point, Quaternion rotation, Vector2 size )
        {
            var flat_matrix = Matrix4x4.TRS( point, rotation, new Vector3( 1f, 0f, 1f ) );
            Gizmos.matrix = Gizmos.matrix * flat_matrix;
            Gizmos.DrawWireCube( Vector3.zero, new Vector3( size.x, 0, size.y ) );
            Gizmos.matrix = Gizmos.matrix * flat_matrix.inverse;
        }

        public static float RandomGaussian( this System.Random random, float min_value = 0.0f, float max_value = 1.0f )
        {
            float u, v, S;
            do
            {
                u = 2.0f * ( float )random.NextDouble() - 1.0f;
                v = 2.0f * ( float )random.NextDouble() - 1.0f;
                S = u * u + v * v;
            } while ( S >= 1.0f );

            // Standard Normal Distribution
            float std = u * Mathf.Sqrt( -2.0f * Mathf.Log( S ) / S );

            // Normal Distribution centered between the min and max value
            // and clamped following the "three-sigma rule"
            float mean = ( min_value + max_value ) / 2.0f;
            float sigma = ( max_value - mean ) / 3.0f;
            return Mathf.Clamp( std * sigma + mean, min_value, max_value );
        }

        public static float RandomNormalDistribution( this System.Random random, float mean = 0.0f, float std = 1.0f )
        {
            float x1, x2, w, y1;
            do
            {
                x1 = 2f * ( float )random.NextDouble() - 1f;
                x2 = 2f * ( float )random.NextDouble() - 1f;
                w = x1 * x1 + x2 * x2;
            } while ( w >= 1f );

            w = Mathf.Sqrt( ( -2f * Mathf.Log( w ) ) / w );
            y1 = x1 * w;

            return ( y1 * std ) + mean;
        }

        public static bool Roll( this Mathf math, int chance ) { return chance > 100 || Random.Range( 0, 100 ) > chance; }
    }
}