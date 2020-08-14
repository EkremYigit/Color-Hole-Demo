
using DG.Tweening;
using UnityEngine;

public class SlideUIObject : MonoBehaviour
{
    [Header("This script slides items has been attached.")] 
    [SerializeField]
    private RectTransform itemToSlide;
    [SerializeField] private Vector2 firstPos;
    [SerializeField] private Vector2 destinationPos;
    
    [Space]
    [SerializeField]private Ease expandEase;
    [SerializeField]private Ease collapseEase;
    [SerializeField]private float easeTime;


    private bool _isSlided;
    private void Start()
    {
        _isSlided = false;
    }
    
    
    public void Slide()// If object is slided this method slides back if button is clicked.
    {
        itemToSlide.DOAnchorPos(!_isSlided ? destinationPos : firstPos, easeTime)
                       .SetEase(!_isSlided ? expandEase: collapseEase);
        _isSlided = !_isSlided;
    }

 
}
