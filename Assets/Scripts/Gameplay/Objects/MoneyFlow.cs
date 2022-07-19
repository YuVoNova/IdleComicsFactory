using UnityEngine;

public class MoneyFlow : MonoBehaviour
{
    [SerializeField]
    private GameObject FlowMoneyPrefab;

    [SerializeField]
    private float SpawnDuration;

    private float timer;

    private Transform OriginTransform;
    [HideInInspector]
    public Transform TargetTransform;

    private bool isFlowOn;

    private FlowMoney spawnedMoney;

    private void Update()
    {
        if (isFlowOn)
        {
            if (timer <= 0f)
            {
                SpawnMoney();

                timer = SpawnDuration;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    public void StartFlow(Transform origin, Transform target)
    {
        OriginTransform = origin;
        TargetTransform = target;

        timer = 0f;

        isFlowOn = true;
    }

    public void EndFlow()
    {
        isFlowOn = false;
    }

    private void SpawnMoney()
    {
        spawnedMoney = Instantiate(FlowMoneyPrefab, OriginTransform.position, Quaternion.identity).GetComponent<FlowMoney>();
        spawnedMoney.TargetTransform = TargetTransform;
        spawnedMoney.IsOn = true;
    }
}
