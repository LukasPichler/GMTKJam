
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerLoadScenes : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Play");
    }


    public void TryAgain()
    {
        Score.Count = 0;
        SceneManager.LoadScene("Play");
        
    }
    
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    
    
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
