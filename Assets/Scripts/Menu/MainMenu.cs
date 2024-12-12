using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainFrame;
    [SerializeField] private GameObject _loadingFrame;

    private void Start()
    {
        _mainFrame.SetActive(true);
        _loadingFrame.SetActive(false);
    }

    #region Public Methods

    public void OnClick_Play()
    {
        StartCoroutine(LoadSceneRoutine("SCN_Game"));
        _mainFrame.SetActive(false);
        _loadingFrame.SetActive(true);
    }

    public void OnClick_Quit()
    {
        Application.Quit();
    }

    #endregion

    #region Private Methods

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    #endregion
}