using Unity.Collections;
using UnityEngine;
using UnityEditor;
using System.IO;

[CreateAssetMenu(fileName = "Palette", menuName = "Scriptable Objects/Palette")]
public class Palette : ScriptableObject
{
    public Color A;
    public Color B;
    public int iterations;

    [ReadOnly] public Color[] colours;

    private void Reset()
    {
        A = new Color( Random.Range( 0F, 1F ), Random.Range( 0, 1F ), Random.Range( 0, 1F ), 1F );
        
        float h, s, v;
        Color.RGBToHSV( A, out h, out s, out v );
        B = Color.HSVToRGB( ( ( h + 180.0f / 360.0f ) * 360 ) % 360 / 360, s, v );

        iterations = Random.Range( 3, 10 );
        OnValidate();
    }

    private void OnValidate()
    {
        if ( iterations <= 0 )
            return;

        colours = new Color[iterations];
        if ( iterations <= 1 )
        {
            colours[ 0 ] = Color.black;
            return;
        }

        for ( int i = 0; i < iterations; i++ )
        {
            colours[ i ] = Color.Lerp( A, B, ( float )i / ( iterations - 1 ) );
        } 
    }
}
