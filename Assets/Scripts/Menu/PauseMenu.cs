using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject _frame;

    private void Start()
    {
        _frame.SetActive(false);
        _inputReader.TogglePauseEvent += TogglePause;
    }

    private void OnDestroy()
    {
        _inputReader.TogglePauseEvent -= TogglePause;
    }

    #region Public Methods

    public void OnClick_Resume()
    {
        Resume();
    }

    public void OnClick_Quit()
    {
        Application.Quit();
    }

    #endregion

    #region Private Methods

    private void TogglePause()
    {
        if (Time.timeScale == 0)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Resume()
    {
        _inputReader.SetControllerMode(ControllerMode.Battle);

        Time.timeScale = 1;
        _frame.SetActive(false);
    }

    private void Pause()
    {
        _inputReader.SetControllerMode(ControllerMode.UI);

        Time.timeScale = 0;
        _frame.SetActive(true);
    }

    #endregion
}