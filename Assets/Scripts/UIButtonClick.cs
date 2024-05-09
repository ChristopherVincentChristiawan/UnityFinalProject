using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonClick : MonoBehaviour
{
    [SerializeField] AudioSource clickSound;
	[SerializeField] GameObject loadingImage;

    public void LoadGameLevel()
    {
    	loadingImage.SetActive(true);
		Invoke("loadLevel", 1.0f);
    }

    void loadLevel()
	{
		SceneManager.LoadScene("RPG_scene");
	}

    public void  PlaySound ()
	{
		clickSound.Play();
	}

	public void QuitGame ()
	{
		Debug.Log("QUIT");
		Application.Quit();
	}
}
