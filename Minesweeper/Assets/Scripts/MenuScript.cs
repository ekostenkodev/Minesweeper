using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayMenuSound()
    {
        FindObjectOfType<AudioManager>().Play("flag");
    }
    
    public void LoadClassicGame(int difficulty)
    {
        int size, mines;
        switch (difficulty)
        {
            case 0:
                size = 9;
                mines = 10;
            break;
            
            case 1:
                size = 16;
                mines = 40;
            break;
            
            case 2:
                size = 25;
                mines = 99;
            break;

            default:
                size = 9;
                mines = 10;
            break;
        }
        PlayerPrefs.SetInt("Size",size);
        PlayerPrefs.SetInt("Mines",mines);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    
}