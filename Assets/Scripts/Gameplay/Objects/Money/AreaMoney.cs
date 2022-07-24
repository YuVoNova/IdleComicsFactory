using UnityEngine;

public class AreaMoney : MonoBehaviour
{
    [SerializeField]
    private int Amount;

    [SerializeField]
    private float Speed;

    private Vector3 originPosition;
    private Quaternion originRotation;

    private Vector3 targetPosition;

    private bool isMagnetized;

    private void Awake()
    {
        transform.eulerAngles = new Vector3(0f, Random.Range(-10f, 10f), 0f);

        originPosition = transform.localPosition;
        originRotation = transform.localRotation;

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
                GameManager.Instance.MoneyEarned(Amount);

                isMagnetized = false;

                transform.localPosition = originPosition;
                transform.localRotation = originRotation;

                gameObject.SetActive(false);
            }
        }
    }

    public void Magnetize()
    {
        isMagnetized = true;
    }
}
