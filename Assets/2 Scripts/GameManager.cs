using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using static UnityEngine.Rendering.SplashScreen;

public class GameManager : MonoBehaviour
{
    [Header ("Stats")]
    [SerializeField] private GameState currentGameState;
    [SerializeField] private MiniGameSettings gameSettings;

    [Header("UI - General")]
    [SerializeField] private GameObject transitionscreen;
    [SerializeField] private GameObject gameOverScreenSkill;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI gameDescription;

    [Header("References - Pfand Game")]
    [SerializeField] private GameObject pfandEnvironment;
    [SerializeField] private Animation pfandBottleCorrect;
    [SerializeField] private Animation pfandBottleWrong;
    [SerializeField] private Animation pfandArm;


    [SerializeField] private MarkerMovement marker;
    [SerializeField] private GameObject successIcon;
    [SerializeField] private GameObject balanceMetre;
    [SerializeField] private GameObject failureIcon;
    [SerializeField] private TextMeshProUGUI bottleAmountText;

    [Header("References - Ship Game")]
    [SerializeField] private GameObject shipEnvironment;
    [SerializeField] private GameObject shipIcon;
    [SerializeField] private TrashGrid trashGrid;
    [SerializeField] private ShipBehavior shipBehavior;

    [Header("References - Trash Game")]
    [SerializeField] private GameObject trashEnvironment;

    private int currentTimer;

    private int difficultyPfand = -1;
    private int difficultyShip = -1;
    private int difficultyTrash = -1;

    private int remainingBottleAmount;
    private int remainingContainerAmount;

    private enum GameState { Pfand, Ship, Trash}

    private bool isGameOver = false;

    private void Start()
    {
        difficultyPfand = -1;
        difficultyShip = -1;
        difficultyTrash = -1;

        InitializeNewGameState(currentGameState);
        timer.gameObject.SetActive(true);
    }

    private void InitializeNewGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        int newTimer = 1000;

        switch (currentGameState)
        {
            case GameState.Pfand:
                
                if(gameSettings.PfandGameDifficultySettings.Length -1 > difficultyPfand)
                    difficultyPfand++;

                // Initialize Difficulty Settings
                newTimer = gameSettings.PfandGameDifficultySettings[difficultyPfand].LevelDuration;

                // Turn Objects on, initialize text
                marker.IsCraneGame = false;
                marker.SetSpeed(gameSettings.PfandGameDifficultySettings[difficultyPfand].MarkerSpeed);
                remainingBottleAmount = gameSettings.PfandGameDifficultySettings[difficultyPfand].BottleAmount;
                bottleAmountText.text = remainingBottleAmount.ToString();
                gameDescription.text = gameSettings.PfandGameDescription;

                pfandEnvironment.SetActive(true);
                marker.gameObject.SetActive(true);
                balanceMetre.SetActive(true);
                bottleAmountText.gameObject.SetActive(true);

                break;


            case GameState.Ship:

                if (gameSettings.ShipGameDifficultySettings.Length -1 > difficultyShip)
                    difficultyShip++;

                // Initialize Difficulty Settings
                remainingContainerAmount = gameSettings.ShipGameDifficultySettings[difficultyShip].Containers;
                trashGrid.TrashRowAmount = gameSettings.ShipGameDifficultySettings[difficultyShip].TrashRows;
                trashGrid.TrashPerRow = gameSettings.ShipGameDifficultySettings[difficultyShip].TrashPerRow;
                trashGrid.SpawnEnvironment();
                shipBehavior.UpdateContainers(remainingContainerAmount);
                newTimer = gameSettings.ShipGameDifficultySettings[difficultyShip].LevelDuration;

                // Turn Objects on, initialize text
                gameDescription.text = gameSettings.ShipGameDescription;
                shipEnvironment.SetActive(true);
                break;


            case GameState.Trash:
                if (gameSettings.TrashGameDifficultySettings.Length - 1 > difficultyTrash)
                    difficultyTrash++;

                // Initialize Difficulty Settings
                newTimer = gameSettings.TrashGameDifficultySettings[difficultyTrash].LevelDuration;
                //...............

                // Turn Objects on, initialize text
                gameDescription.text = gameSettings.TrashGameDescription;
                marker.SetSpeed(gameSettings.TrashGameDifficultySettings[difficultyTrash].MarkerSpeed);
                remainingBottleAmount = gameSettings.TrashGameDifficultySettings[difficultyTrash].BottleAmount;
                bottleAmountText.text = remainingBottleAmount.ToString();

                marker.IsCraneGame = true;
                trashEnvironment.SetActive(true);
                marker.gameObject.SetActive(true);
                balanceMetre.SetActive(true);
                bottleAmountText.gameObject.SetActive(true);

                break;
        }

        StartCoroutine(StartTimer(newTimer));
    }

    public void OnInteract(InputValue value)
    {
        switch(currentGameState)
        {
            case GameState.Pfand:
                EvaluateResult(marker.HitMarker());
                break;
            case GameState.Ship:
                trashGrid.SetShipSpeed(100);
                break;
            case GameState.Trash:
                EvaluateResult(marker.HitMarker());
                break;
        }
    }

    private void EvaluateResult(float result)
    {
        switch (result)
        {
            case 0:
                failureIcon.SetActive(true);
                pfandArm.Stop();
                pfandArm.Play();
                pfandBottleWrong.Stop();
                pfandBottleWrong.Play();
                break;
            case 1:
                successIcon.SetActive(true);
                pfandBottleCorrect.Stop();
                pfandBottleCorrect.Play();
                pfandArm.Stop();
                pfandArm.Play();
                remainingBottleAmount--;
                
                break;
            case 2:
                successIcon.SetActive(true);
                pfandBottleCorrect.Stop();
                pfandBottleCorrect.Play();
                pfandArm.Stop();
                pfandArm.Play();
                remainingBottleAmount--;
                break;
        }

        bottleAmountText.text = remainingBottleAmount.ToString();

        //if all bottles have been put in, end game earlier
        if(remainingBottleAmount == 0)
            currentTimer = 0;
    }

    private IEnumerator StartTimer(int startValue)
    {
        currentTimer = startValue;
        timer.text = startValue.ToString();

        while (currentTimer != 0)
        {
            currentTimer -= 1;
            timer.text = currentTimer.ToString();
            yield return new WaitForSeconds(1f);
        }

        if (remainingBottleAmount > 0)
            isGameOver = true;

        StartLevelTransition();
    }

    public void StartLevelTransition()
    {
        transitionscreen.SetActive(true);
    }
    public void QueueNextGameState()
    {
        switch (currentGameState)
        {
            case GameState.Pfand:

                pfandEnvironment.SetActive(false);
                marker.gameObject.SetActive(false);
                balanceMetre.SetActive(false);
                bottleAmountText.gameObject.SetActive(false);

                if (!isGameOver)
                    InitializeNewGameState(GameState.Ship);
                else
                    SetGameOver();

                break;
            case GameState.Ship:
                shipEnvironment.SetActive(false);

                if (!isGameOver)
                    InitializeNewGameState(GameState.Trash);
                else
                    SetGameOver();

                break;
            case GameState.Trash:
                trashEnvironment.SetActive(false);
                marker.gameObject.SetActive(false);
                balanceMetre.SetActive(false);
                bottleAmountText.gameObject.SetActive(false);


                if (!isGameOver)
                    InitializeNewGameState(GameState.Pfand);
                else
                    SetGameOver();

                break;
        }

    }
    public void LooseContainer()
    {
        remainingContainerAmount--;
        shipBehavior.UpdateContainers(remainingContainerAmount, true);

        // if no ship containers remain, set game over screen
        if (remainingContainerAmount == 0)
        {
            currentTimer = 0;
            isGameOver = true;
        }
    }

    private void SetGameOver()
    {
        gameOverScreenSkill.SetActive(true);
    }
}
