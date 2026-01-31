using UnityEngine;
using Audio;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour
{
    public VolumeChannel channel;
    public bool is_master = false;
    
    private Slider volumeSlider;

    public void Awake()
    {
        volumeSlider = GetComponentInChildren<Slider>();
        if( is_master )
            volumeSlider.value = AudioManager.Instance.master_volume;
        else
            volumeSlider.value = AudioManager.Instance.GetVolume( channel );
    }

    public void OnVolumeChanged()
    {
        if ( is_master )
            AudioManager.Instance.master_volume = volumeSlider.value;
        else
            AudioManager.Instance.SetVolumeValue( channel, volumeSlider.value );
    }
}
