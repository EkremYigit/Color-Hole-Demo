using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;


public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text levelCompletedText;
    [SerializeField] private Transform onPausedScreen;
    [SerializeField] private List<Transform> mainScreenEnvironements;
    [SerializeField] private Transform swipeIcons; //Swipe icons fading to 0 when game is playing.
    [SerializeField] private TimerController timerController;
    [SerializeField] private TMP_Text stageText;
    private bool _uiAppear;


    public void ShowLevelCompletedText()
    {
        levelCompletedText.DOFade(1f, 0.3f).From(0f);
        GameController.Instance.NextLevel();
    }


    public void HideMainScreenEnvironments()
    {
        for (int i = 0; i < mainScreenEnvironements.Count; i++)
        {
            mainScreenEnvironements[i].gameObject.SetActive(false);
        }

        swipeIcons.gameObject.SetActive(false);


// FadeOutSwipeIcons();
    }

    /*  private void FadeOutSwipeIcons()
      {
          for (int i = 0; i < swipeIcons.childCount-1; i++)
          {
              swipeIcons.GetChild(i).GetComponent<Image>().DOFade(0, 0.5f);
          }
  
          swipeIcons.GetChild(swipeIcons.childCount - 1).GetComponent<TMP_Text>().DOFade(0, 0.5f);
      }
  */
    public void StartOnPausedState(bool stageFlag)
    {
        SetStageText(stageFlag);
        onPausedScreen.gameObject.SetActive(true);
        timerController.StartCountDown();
    }

    private void SetStageText(bool stageFlag)
    {
        //if user collect an enemy obstacle, stage text updated according to user current stage 1 or 2 .

        stageText.text = !stageFlag ? "STAGE1" : "STAGE2";
    }

    public void ResumeGame()
    {
        onPausedScreen.gameObject.SetActive(false);
        _uiAppear = false;
    }


    public bool UiAppear
    {
        get => _uiAppear;
        set => _uiAppear = value;
    }
}