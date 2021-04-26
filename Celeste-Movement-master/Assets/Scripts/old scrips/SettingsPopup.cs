using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopup : MonoBehaviour
{
    //When opened, display the GUI
    public void Open()
    {
        gameObject.SetActive(true);
    }

    //When closed, remove GUI
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
