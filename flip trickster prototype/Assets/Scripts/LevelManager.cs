using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Transform player;
    public void RestartButton()
    {
        SceneManager.LoadScene("scene_01");
    }

    public void QuitButton()
	{
		Application.Quit();
	}
}
