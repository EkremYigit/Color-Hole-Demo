using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    //this script helps to manage colors for each individual levels.

    [Header("--Materials--")] [SerializeField]
    private Camera _cameraBackgroundMaterial;

    [SerializeField] private Material gameAreaMaterial;
    [SerializeField] private List<SpriteRenderer> frameUISpriteRen; // To change limit frames together.
    [SerializeField] private Material gateMaterial;
    [SerializeField] private Material obstaclesMaterial;
    [SerializeField] private Material enemyObstaclesMaterial;
    [SerializeField] private Material holeUpperBorder;
    [SerializeField] private Material _3Dillision;


    [Space] [Header("--Level Colors--")] [SerializeField]
    private Color backgroundColor;

    [SerializeField] private Color gameAreaColor;
    [SerializeField] private Color frameUIColor;
    [SerializeField] private Color gateColor;
    [SerializeField] private Color _3DillisionColor;

    [Space] [Header("--Object Colors--")] [SerializeField]
    private Color obstaclesColor;

    [SerializeField] private Color enemyObstaclesColor;


    private void Start()
    {
        UpdateColors();
    }

    void UpdateColors() //Update all colors beginning of the game.
    {
        for (int i = 0; i < frameUISpriteRen.Count; i++)
        {
            frameUISpriteRen[i].color = frameUIColor;
        }

        //Gate 428482  //frame E294DF
        //Basic Shader Color Property = Color_C9E7684C  // HoleShader UpperBorder Color Property = Color_97082668
        _cameraBackgroundMaterial.backgroundColor = backgroundColor;
        gameAreaMaterial.SetColor("Color_C9E7684C", gameAreaColor);
        gateMaterial.SetColor("Color_C9E7684C", gateColor);
        obstaclesMaterial.SetColor("Color_C9E7684C", obstaclesColor);
        enemyObstaclesMaterial.SetColor("Color_C9E7684C", enemyObstaclesColor);
        _3Dillision.color = _3DillisionColor;
        holeUpperBorder.SetColor("Color_97082668", gameAreaColor);
    }
}