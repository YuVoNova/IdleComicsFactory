using UnityEngine;

public class ThrowMoney : MonoBehaviour
{
    [HideInInspector]
    public int Amount;

    [SerializeField]
    private float Duration;

    private bool isMagnetized;
    private Vector3 target;

    private float timer;

    private void Awake()
    {
        isMagnetized = false;

        timer = Duration;
    }

    private void Update()
    {
        if (timer <= 0f)
        {
            if (isMagnetized)
            {
                target = Player.Instance.transform.position;

                transform.position = Vector3.MoveTowards(transform.position, target, 10f * Time.deltaTime);

                if (Vector3.Distance(transform.position, target) < 1f)
                {
                    GameManager.Instance.MoneyEarned(Amount);

                    Destroy(gameObject);
                }
            }
        }
        else
        {
            timer -= Time.deltaTime;

            if (timer <= 0f && isMagnetized)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().isTrigger = true;
            }
        }
    }

    public void Magnetize(Vector3 position)
    {
        isMagnetized = true;
        target = position;
    }
}
