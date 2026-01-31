using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchPalette : MonoBehaviour
{
    [SerializeField] private Image[] images;
    [SerializeField] private TMP_Text[] texts;

    [Range(0, 1F)]
    [SerializeField] private float tint = 0.0f;
    [SerializeField] private bool invert = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        PaletteManager.Instance.Register( this );
    }

    // Update is called once per frame
    void OnDestroy()
    {
        if( PaletteManager.HasInstance() )
            PaletteManager.Instance.Remove( this );
    }

    public void SetColor( Color A, Color B, int iterations )
    {
        var normalised_tint = Mathf.Floor( tint * ( iterations - 1 ) ) / ( iterations - 1 );
        Color lerped = Color.Lerp( A, B, normalised_tint );

        foreach ( var text in texts )
            text.color = lerped;
        foreach ( var image in images )
            image.color = lerped;
    }

    public void OnValidate()
    {
        foreach ( var text in texts )
            text.color = Color.Lerp( Color.black, Color.white, tint );
    }
}
