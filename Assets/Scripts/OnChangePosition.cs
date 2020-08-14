
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D holePolygonCollider;
    public PolygonCollider2D groundPolygonCollider;
    public Collider gameAreaCollider; // all map mesh collider that contains in GameArea.
    public MeshCollider GeneratedMeshCollider;
    private Mesh GeneratedMesh;
    public List<GameObject> allObstacles;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseMoveSpeed;
    private Transform[] _objects;
    private bool _progressFlag, moveFlag;
    private bool _isMoving;
    private Touch touch;
    private int TouchCounter = 0;
    private SphereCollider _sphereCollider;

    [Space] [Header("Hole Movement Ranges")] [SerializeField]
    private float xRight;

    [SerializeField] private float xLeft;
    [SerializeField] private float zUp;
    [SerializeField] private float zDown;
    [SerializeField] private float stage2Range;

    //Mouse in editor
    private float x, y;

    private Vector3 mouseTouch;

    //
    private void Start()
    {
        foreach (var obs in allObstacles)
        {
            Physics.IgnoreCollision(obs.GetComponent<Collider>(), GeneratedMeshCollider, true);
        }

        moveFlag = false;
        _progressFlag = false;
        _sphereCollider = GetComponent<SphereCollider>();
        _isMoving = true;
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (_isMoving
            ) // when hole move with DOTween while stage changing this flag will false to avoid user movement.
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    //Close all UI object cause player play game
                    if (GameController.Instance.getUIStatement())
                    {
                        GameController.Instance.HideMainMenuUi();
                    }

                    MoveHoleInEditor();
                }
                else if (!GameController.Instance.getUIStatement())
                {
                    MoveHoleInEditor();
                }
            }
        }

#else
            if (Input.touchCount>0)
        {
            touch = Input.GetTouch(0);
            if (_isMoving) // when hole move with DOTween while stage changing this flag will false to avoid user movement.
            {
                if (touch.phase == TouchPhase.Moved &&
                    (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
                {
                    //Close all UI object cause player play game
                    if (GameController.Instance.getUIStatement())
                    {
                        GameController.Instance.HideMainMenuUi();
                    }

                    MoveHole();

                }
                else if (touch.phase == TouchPhase.Moved && !GameController.Instance.getUIStatement())
                {
                    MoveHole();

                }
                
            }
        }

#endif


        if (transform.hasChanged)
        {
            holePolygonCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            //This function will call when user click the game area to move.GameController.Instance.HideMainMenuUi();
        }

        MakeHole2D(); // attach hole to play area.
        Make3DMeshCollider(); //attach to combined area to game play surface.

        if (transform.position.z >= -3.35 && !_sphereCollider.enabled)
        {
            _sphereCollider.enabled = true;
        }

        if (transform.position.z >= 4.1f && !_progressFlag)
        {
            _progressFlag = true;
            GameController.Instance.ChangeProgress();
        }
    }

    private void MoveHoleInEditor()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        mouseTouch = Vector3.Lerp(transform.position, transform.position + new Vector3(x, 0, y),
            Time.deltaTime * mouseMoveSpeed);

        if(CheckRange(mouseTouch))transform.position = mouseTouch;
    }

    private void MoveHole()
    {
        //hole move along X and Z axis not Y!
        Vector3 MoveTo = new Vector3(
            transform.position.x + (touch.deltaPosition.x / moveSpeed) * 1.05f +
            (touch.deltaPosition.x <= 0 ? -0.014f : +0.014f),
            transform.position.y,
            transform.position.z + ((touch.deltaPosition.y / moveSpeed)) * 1.05f +
            (touch.deltaPosition.y <= 0 ? -0.014f : +0.014f));


        // Debug.LogWarning("New Transform = " + MoveTo);
        if (CheckRange(MoveTo))
        {
            //Debug.LogWarning("New Transform = " + MoveTo);
            transform.position = MoveTo;
        }
    }

    private bool CheckRange(Vector3 MovePos) //Game range borders are changed according to stage.
    {
        if (MovePos.x <= xLeft || MovePos.x >= xRight)
        {
            //Debug.Log("Move Range X Returns False!!");
            return false;
        }

        if (MovePos.z <= zDown || MovePos.z >= zUp)
        {
           // Debug.Log("Move Range Z Returns False!!");
            return false;
        }


        return true;
    }

    private void UpdateMoveRange()
    {
        zDown += stage2Range;
        zUp += stage2Range;
    }

    private void OnTriggerEnter(Collider other)
    {
        //ignore trigger with hole and play area.
        Physics.IgnoreCollision(other, gameAreaCollider, true); //ignore trigger with hole and play area.

        Physics.IgnoreCollision(other, GeneratedMeshCollider, false);
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, gameAreaCollider, false);
        Physics.IgnoreCollision(other, GeneratedMeshCollider, true);
    }

    private void MakeHole2D()
    {
        Vector2[] PointPositions = holePolygonCollider.GetPath(0); // get the hole collider vertices points. 
        for (int i = 0; i < PointPositions.Length; i++)
        {
            PointPositions[i] += (Vector2) holePolygonCollider.transform.position;
        }

        groundPolygonCollider.pathCount = 2;
        groundPolygonCollider.SetPath(1, PointPositions); //attach to provided play area collider vertices.
    }

    private void Make3DMeshCollider()
    {
        if (GeneratedMesh != null) Destroy(GeneratedMesh);
        GeneratedMesh = groundPolygonCollider.CreateMesh(true, true);
        GeneratedMeshCollider.sharedMesh = GeneratedMesh;
    }

    public void ChangeStage()
    {
        //if user collects all obstacles in the first stage this method will call from GameController.
        //Move to hole X center. And move to second place with camera.
        //if user on this stage cant move!

        _sphereCollider.enabled = false;
        var position = transform.position;
        transform.DOMove(new Vector3(0, position.y, position.z), 5)
            .OnComplete(() =>
            {
                transform.DOMove(new Vector3(0, transform.position.y, 4.2f), 6.6f);
                UpdateMoveRange(); //when player moves thorough z axis Move borders will updated.
            });
        GameController.Instance.MoveCamera();
        moveFlag = true;
    }


    public bool MoveFlag
    {
        get => moveFlag;
        set => moveFlag = value;
    }

    public bool IsMoving
    {
        get => _isMoving;
        set => _isMoving = value;
    }
}