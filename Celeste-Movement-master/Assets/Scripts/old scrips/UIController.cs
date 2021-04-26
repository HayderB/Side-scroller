using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text scoreLabel;
    [SerializeField] private SettingsPopup settingsPopup;

    //Always start with GUI closed
    void Start()
    {
        settingsPopup.Close();
    }

    //Update the Time since start of the program
    void Update()
    {
        scoreLabel.text = Time.realtimeSinceStartup.ToString();
    }

    //If button is pressed, open the GUI
    public void OnOpenSettings()
    {
        settingsPopup.Open();
    }
}