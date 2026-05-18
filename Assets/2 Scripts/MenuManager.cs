using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject Credits;

     public void BTN_StartGame()
     {
        SceneManager.LoadScene(1);
     }

    public void BTN_ToggleCredits()
    {
        Credits.SetActive(!Credits.activeSelf);
    }

    public void BTN_QuitGame()
    {
        Application.Quit();
    }

    public void BTN_StartMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
