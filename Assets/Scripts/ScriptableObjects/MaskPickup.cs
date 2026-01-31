using UnityEngine;
using UnityEngine.TextCore.Text;

public class MaskPickup : MonoBehaviour
{
    public Mask mask;

    public void OnTriggerEnter2D( Collider2D collision )
    {
        var character = collision.gameObject.GetComponent<Character>();
        if( character != null )
        {
            character.EquipMask( mask );
            this.gameObject.SetActive( false );
        }
    }
}
