using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallController : MonoBehaviour
{
    private bool isVibrate;

    private void Start()
    {
        isVibrate = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            //Debug.Log("This is obstacle");
            other.gameObject.SetActive(false);
            if (isVibrate) Vibration.Vibrate(13);
            GameController.Instance.IncreaseProgress();
        }
        else if (other.CompareTag("Enemy"))
        {
            //Debug.Log("This is enemy object");
            GameController.Instance.StopHole();
            GameController.Instance.ShakeCamera();
            //Game stopped.
        }
    }


    public bool IsVibrate
    {
        get => isVibrate;
        set => isVibrate = value;
    }
}