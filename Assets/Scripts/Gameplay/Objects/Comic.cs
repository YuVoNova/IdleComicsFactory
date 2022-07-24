using UnityEngine;

public class Comic : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originRotation;

    private Vector3 targetPosition;

    [SerializeField]
    private float Speed;

    private bool isMagnetized;

    private void Awake()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;

        isMagnetized = false;
    }

    private void Update()
    {
        if (isMagnetized)
        {
            targetPosition = Player.Instance.transform.position;
            targetPosition.y = 1.5f;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
            transform.LookAt(Player.Instance.transform);

            if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
            {
                Player.Instance.TakeComic();

                isMagnetized = false;

                transform.position = originPosition;
                transform.rotation = originRotation;

                gameObject.SetActive(false);
            }
        }
    }

    public void Magnetize()
    {
        isMagnetized = true;
    }
}
