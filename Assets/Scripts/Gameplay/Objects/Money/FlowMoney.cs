using UnityEngine;

public class FlowMoney : MonoBehaviour
{
    [HideInInspector]
    public Transform TargetTransform;

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
            transform.position = Vector3.MoveTowards(transform.position, TargetTransform.position, Speed * Time.deltaTime);
            transform.LookAt(TargetTransform);

            if (Vector3.Distance(transform.position, TargetTransform.position) < 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }
}
