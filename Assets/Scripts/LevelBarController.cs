
using UnityEngine;
using UnityEngine.UI;

public class LevelBarController : MonoBehaviour
{
    // This class modify level bar components.
    [SerializeField] private Image firstProgressBar;
    [SerializeField] private Image SecondProgressBar;

    private float TotalObject;
    private int _collectedObstacle;

    void Start()
    {
        _collectedObstacle = 0;
    }


    public void IncreaseProgress(bool flag) // this function fill the bar when user collect obstacles.
    {
        //if flag = false its represent user on the first part of game. else flag = true second part

        if (!flag)
        {
            _collectedObstacle++;
            firstProgressBar.fillAmount = _collectedObstacle / TotalObject;
        }
        else
        {
            _collectedObstacle++;
            SecondProgressBar.fillAmount = _collectedObstacle / TotalObject;
        }
    }


    public float numOfObs
    {
        set => TotalObject = value;
    }

    public float getSecondProgressBarAmount()
    {
        return SecondProgressBar.fillAmount;
    }


    public int CollectedObstacle
    {
        get => _collectedObstacle;
        set => _collectedObstacle = value;
    }


    public void closeLevelBar()
    {
        gameObject.SetActive(false);
        SecondProgressBar.fillAmount = 0;
    }
}