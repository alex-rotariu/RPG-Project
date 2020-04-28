using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject helpPanel;

    public void RestartLevel() {
        SceneManager.LoadScene(0);
    }

    public void ToggleHelp() {
        bool state = helpPanel.active;
        helpPanel.SetActive(!state);
    }
}
