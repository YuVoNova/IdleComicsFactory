using UnityEngine;

public class FlowMoney : MonoBehaviour
{
    [HideInInspector]
    public Transform TargetTransform;

    private Vector3 targetPosition;

    [HideInInspector]
    public bool IsOn;

    [SerializeField]
    private float Speed;

    private void Awake()
    {
        IsOn = false;
    }

    private void Update()
    {
        if (IsOn)
        {
            targetPosition = TargetTransform.position;
            targetPosition.y = 0.75f;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
            transform.LookAt(TargetTransform);

            if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }
}
