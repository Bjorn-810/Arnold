using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject _PauseAssets;
    [SerializeField] private GameObject _UIElements;

    [Header("TimerSystem")]
    [SerializeField] private TextMeshProUGUI _TimerText;
    public float _countdownTimer = 300;
    public bool ReachedTimer;
    
    void Update()
    {
        PauseMenu();
        HandleTimer();
    }

    private void HandleTimer()
    {
        //Counts down the time with time,deltaTime
        if (_countdownTimer > 0)
            _countdownTimer -= Time.deltaTime;
        

        if (_countdownTimer < 0)
        {
            ReachedTimer = true;
            SceneManager.LoadScene("Start Screen");
            _countdownTimer = 0;
        }

        //Rounds the countdowntimer to a max of 2 decimals
        float roundedNumber = Mathf.Round(_countdownTimer * 100) / 100;

        //Changes the text on screen to be rounded number
        _TimerText.text ="Time left: " + roundedNumber;
    }

    private void PauseMenu()
    {
        //If you press escape and the pauseAssets are not active in the hierarchy
        if (Input.GetKeyDown(KeyCode.Escape) && !_PauseAssets.activeInHierarchy)
        {
            //Enables pauseAssets
            _PauseAssets.SetActive(true);
            //disables the UIElements
            _UIElements.SetActive(false);
            //Sets the timeScale to 0
            Time.timeScale = 0;
        }                               //Same function as above
        else if (Input.GetKeyDown(KeyCode.Escape) && _PauseAssets.activeInHierarchy)
        {
            _PauseAssets.SetActive(false);
            _UIElements.SetActive(true);
            Time.timeScale = 1.0f;
        }
    }

    //Continues the game when pressing a button in the pause screen
    public void ContinueGame()
    {
        //Puts the pause assets to false
        _PauseAssets.SetActive(false);
        //Sets the UI elements back on
        _UIElements.SetActive(true);
        //Sets the time scale back to normal
        Time.timeScale = 1.0f;
    }
}
