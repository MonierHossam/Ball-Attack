using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    LevelManager levelManager;

    [SerializeField] GameObject endGameScreen;

    [SerializeField] Button endLevelButton;

    [SerializeField] TMP_Text endGameText;
    [SerializeField] TMP_Text sizeValueText;
    [SerializeField] TMP_Text ballsCountText;
    [SerializeField] TMP_Text levelNumberText;

    GameState currentState;

    private void Awake()
    {
        levelManager = LevelManager.GetInstance();
        gameManager = GameManager.GetInstance();
    }

    private void OnEnable()
    {
        gameManager.OnSizeChanged += UpdateSizeText;
        gameManager.OnBallsCountChanged += UpdateBallsCountText;
        gameManager.OnGameEnded += GameEnded;
        gameManager.OnLevelNumberChange += UpdateLevelNumberText;
    }

    private void OnDisable()
    {
        gameManager.OnSizeChanged -= UpdateSizeText;
        gameManager.OnBallsCountChanged -= UpdateBallsCountText;
        gameManager.OnGameEnded -= GameEnded;
        gameManager.OnLevelNumberChange -= UpdateLevelNumberText;
    }

    private void UpdateLevelNumberText(int value)
    {
        levelNumberText.text = value.ToString();
    }

    private void Start()
    {
        endLevelButton.onClick.AddListener(EndLevelButtonClicked);
    }

    private void EndLevelButtonClicked()
    {
        if (currentState == GameState.Won)
        {
            levelManager.LoadNewLevel();
        }
        else
        {
            levelManager.RestartCurrentLevel();
        }
    }

    public void UpdateSizeText(float value)
    {
        sizeValueText.text = value.ToString();
    }

    public void UpdateBallsCountText(int value)
    {
        ballsCountText.text = value.ToString();
    }

    public void GameEnded(GameState state)
    {
        currentState = state;

        if (state == GameState.Won)
        {
            endGameText.text = "Congratulations you won, Play again!";
            endLevelButton.gameObject.SetActive(!levelManager.IsLastLevel());
        }
        else
        {
            endGameText.text = "Opps sorry you lost retry again!";
        }

        endGameScreen.SetActive(true);
    }
}
