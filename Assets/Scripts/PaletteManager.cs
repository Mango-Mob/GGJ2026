using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PaletteManager : Utility.SingletonPersistent<PaletteManager>
{
    public Material[] textures;
    public List<MatchPalette> dependancies = new List<MatchPalette>();

    [UnityEngine.Range( 0, 1F)]
    public float h_min = 0;
    [UnityEngine.Range( 0, 1F )]
    public float h_max = 1;
    [UnityEngine.Range( 0, 1F )]
    public float s_min = 0.7F;
    [UnityEngine.Range( 0, 1F )]
    public float s_max = 1F;
    [UnityEngine.Range( 0, 1F )]
    public float v_min = 0.7F;
    [UnityEngine.Range( 0, 1F )]
    public float v_max = 1F;

    [UnityEngine.Range( 4, 15 )]
    public int i_min = 4;
    [UnityEngine.Range( 4, 15 )]
    public int i_max = 15;

    protected Color A;
    protected Color B;
    protected int iterations;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();

        SetPalette();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.R ) )
        {
            SetPalette();
        }
        
        if ( Camera.main )
            Camera.main.backgroundColor = A;
    }

    void SetPalette()
    {
        A = Color.HSVToRGB( Random.Range( h_min, h_max ), Random.Range( s_min, s_max ), Random.Range( v_min, v_max ) );

        float h, s, v;
        Color.RGBToHSV( A, out h, out s, out v );
        B = Color.HSVToRGB( ( ( h + 180.0f / 360.0f ) * 360 ) % 360 / 360, s, v );

        iterations = Random.Range( i_min, i_max );

        foreach ( var texture in textures )
        {

            texture.SetColor( "_Warm", A );
            texture.SetColor( "_Cool", B );
            texture.SetFloat( "_Iterations", iterations );
        }

        if( Camera.main )
            Camera.main.backgroundColor = A;

        foreach ( var other in dependancies )
            other.SetColor( A, B, iterations );
    }

    public void Register( MatchPalette other )
    {
        dependancies.Add( other );
        other.SetColor( A, B, iterations );
    }

    public void Remove( MatchPalette other )
    {
        dependancies.Remove( other );
    }
}
