using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour //this script will run when timer has been activated.
{
    [SerializeField] private int countDownTimerCount;
    [SerializeField] private TMP_Text countDownTimerText;
    [SerializeField] private Image countDownTimerImage;
    private WaitForSeconds _countDown;
    private int _totalTime;

    private void Start()
    {
        _countDown = new WaitForSeconds(1f); //reducing redundant new WaitForSeconds.
        _totalTime = countDownTimerCount;
        countDownTimerText.text = countDownTimerCount.ToString();
    }

    public void StartCountDown()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while (countDownTimerCount > 0)
        {
            yield return _countDown;
            countDownTimerCount--;
            UpdateImage();
        }

        //Orient to user tap to restart UI screen.
    }

    private void UpdateImage()
    {
        countDownTimerImage.fillAmount = (float) countDownTimerCount / _totalTime;
        countDownTimerText.text = countDownTimerCount.ToString();
    }
}