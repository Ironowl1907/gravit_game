using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider mySlider;

    public void SaveSliderValue()
    {
        // Saves the float value with the key "SavedSliderValue"
        PlayerPrefs.SetFloat("SavedSliderValue", mySlider.value);
        PlayerPrefs.Save(); 
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Game"); 
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }
}