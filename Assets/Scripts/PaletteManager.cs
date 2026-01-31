using Audio;
using UnityEngine;

public class PaletteManager : Utility.SingletonPersistent<PaletteManager>
{
    public Material main;
    public Material invert;

    [Range(0, 1F)]
    public float h_min = 0;
    [Range( 0, 1F )]
    public float h_max = 1;
    [Range( 0, 1F )]
    public float s_min = 0.7F;
    [Range( 0, 1F )]
    public float s_max = 1F;
    [Range( 0, 1F )]
    public float v_min = 0.7F;
    [Range( 0, 1F )]
    public float v_max = 1F;

    [Range( 4, 15 )]
    public int i_min = 4;
    [Range( 4, 15 )]
    public int i_max = 15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        SetPalette();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.R ) )
        {
            SetPalette();
        }
    }

    void SetPalette()
    {
        Color A = Color.HSVToRGB( Random.Range( h_min, h_max ), Random.Range( s_min, s_max ), Random.Range( v_min, v_max ) );

        float h, s, v;
        Color.RGBToHSV( A, out h, out s, out v );
        Color B = Color.HSVToRGB( ( ( h + 180.0f / 360.0f ) * 360 ) % 360 / 360, s, v );

        float iterations = Random.Range( i_min, i_max );
        
        main.SetColor( "_Warm", A );
        main.SetColor( "_Cool", B );
        main.SetFloat( "_Iterations", iterations );
        invert.SetColor( "_Warm", A );
        invert.SetColor( "_Cool", B );
        invert.SetFloat( "_Iterations", iterations );
    }
}
