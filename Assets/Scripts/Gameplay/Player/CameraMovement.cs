using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float FollowSpeed;

    private Vector3 offset;
    private Vector3 target;

    private bool isPlayer;

    private void Awake()
    {
        offset = transform.position;

        isPlayer = true;
    }

    private void FixedUpdate()
    {
        if (isPlayer)
        {
            target = Player.Instance.transform.position + offset;
        }
        transform.position = Vector3.Lerp(transform.position, target, FollowSpeed * Time.deltaTime);
    }

    public void ChangeTarget(Vector3 newTarget)
    {
        isPlayer = false;
        target = newTarget + offset;

        GameManager.Instance.IsGameOn = false;
    }

    public void DefaultTarget()
    {
        isPlayer = true;

        GameManager.Instance.IsGameOn = true;
    }
}
