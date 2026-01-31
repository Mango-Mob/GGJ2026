using UnityEngine;

public class SettingsToggle : MonoBehaviour
{
    public GameObject[] settings_objects;

    public bool showing_settings = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach ( var item in settings_objects )
        {
            item.SetActive( showing_settings );
        }
    }

    public void Toggle()
    {
        showing_settings = !showing_settings;
        foreach ( var item in settings_objects )
        {
            item.SetActive( showing_settings );
        }
    }
}
