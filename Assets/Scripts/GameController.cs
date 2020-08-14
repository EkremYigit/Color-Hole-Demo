
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public CameraController CameraController;
    public LevelBarController levelBar;
    public static GameController Instance; //singelton
    private UIController UiController;
    public ParticleSystem ParticleSystem;
    public OnChangePosition holeController;
    public FallController fallController;
    [SerializeField] private int FirstPartObstacles;
    [SerializeField] private int SecondPartObstacles;
    [SerializeField] private Transform gate;
    private bool stageFlag; //this flag shows us user on which stage first or second. if flase user on first stage.

    // Start is called before the first frame update


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    void Start()
    {
        levelBar.numOfObs = FirstPartObstacles; //firstly game will start with first part.
        //levelBar.UpdateCurrentLevel();
        UiController = GetComponent<UIController>();
        UiController.UiAppear = true;
        stageFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelBar.CollectedObstacle == FirstPartObstacles && !stageFlag && !holeController.MoveFlag
        ) //stageFlag  helps to prevent repeating additionally. 
        {
            //half of the game completed successfully.
            //Move to hole center of the first game area and move to second place.
            holeController.IsMoving = false; //stop user finger move.
            gate.DOMove(new Vector3(gate.position.x, -.3f, gate.position.z), 4f);
            holeController.ChangeStage();
        }


        if (levelBar.getSecondProgressBarAmount() >= 1)
        {
            ParticleSystem.gameObject.SetActive(true);
            //level is completed. Text + Particle
            UiController.ShowLevelCompletedText();
            ParticleSystem.Play();
            levelBar.closeLevelBar();
            levelBar.numOfObs = FirstPartObstacles;
        }
    }


    public void StopGame()
    {
        //Block user movement.
        //Show OnPausedScreen.
        //Start Timer.
        UiController.StartOnPausedState(stageFlag);
    }

    public void ResumeGame()
    {
        //allow touch controle.
        UiController.ResumeGame();
        holeController.IsMoving = true;
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1.5f);
        if (SceneManager.GetActiveScene().buildIndex + 1 == 3) //end level. ReStartToBeginning.
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public bool getUIStatement() //return UI is shown or not .
    {
        return UiController.UiAppear;
    }

    public void HideMainMenuUi()
    {
        UiController.UiAppear = false;
        UiController.HideMainScreenEnvironments();
    }

    public void ChangeProgress()
    {
        //Update Level Bar Image. Second half will filled with second area obstacles.
        stageFlag = true; //with this flag changing top of the level progress image will change.
        levelBar.CollectedObstacle = 0;
        levelBar.numOfObs = SecondPartObstacles;
        StartCoroutine(StopCamera()); // Change the number of obstacle that will collect on the second stage.
    }

    IEnumerator StopCamera()
    {
        yield return new WaitForSeconds(3f);
        holeController.IsMoving = true;
        CameraController.FollowFlag = false;
    }

    public void IncreaseProgress()
    {
        levelBar.IncreaseProgress(stageFlag);
    }


    public void MoveCamera()
    {
        CameraController.FollowFlag = true;
    }

    public void StopHole()
    {
        holeController.IsMoving = false;
    }

    public void ShakeCamera()
    {
        CameraController.ShakeCamera();
    }

    public void Vibration(bool vibrate)
    {
        fallController.IsVibrate = vibrate;
    }
}