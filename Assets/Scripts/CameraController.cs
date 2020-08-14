
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private Vector3 offset;
    private bool followFlag;


    private void Start()
    {
        followFlag = false;
    }

    private void FixedUpdate()
    {
        if (followFlag)
        {
            var position = transform.position;
            Vector3 pos = new Vector3(position.x, position.y, objectToFollow.position.z) + offset;
            transform.position = Vector3.Lerp(position, pos, Time.fixedDeltaTime);
        }
    }


    public bool FollowFlag
    {
        get => followFlag;
        set => followFlag = value;
    }

    public void ShakeCamera()
    {
        // float strength = 90f, int vibrato = 10, float randomness = 90f, float delay = 0
        transform.DOShakePosition(1.2f, 0.5f, 25, 50f).OnComplete((() => { GameController.Instance.StopGame(); }));
    }
}