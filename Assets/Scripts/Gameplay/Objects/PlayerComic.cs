using UnityEngine;

public class PlayerComic : MonoBehaviour
{
    [SerializeField]
    private Collider Collider;

    [SerializeField]
    private float Speed;

    private bool isMagnetized;

    private Vector3 originPosition;
    private Quaternion originRotation;

    private Vector3 targetPosition;

    private void Awake()
    {
        originPosition = transform.localPosition;
        originRotation = transform.localRotation;

        Collider.enabled = false;

        isMagnetized = false;
    }

    private void Update()
    {
        if (isMagnetized)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isMagnetized)
        {
            if (other.transform.tag == "Shelf")
            {
                Disable();

                other.transform.parent.Find("Interactable_Shelf").GetComponent<Interactable_Shelf>().TakeComic();
            }
            else if (other.transform.tag == "Truck")
            {
                Disable();

                GameManager.Instance.Truck.TakeComic();
            }
        }
    }

    public void Magnetize(Vector3 position)
    {
        targetPosition = position;

        Collider.enabled = true;

        isMagnetized = true;
    }

    private void Disable()
    {
        transform.localPosition = originPosition;
        transform.localRotation = originRotation;

        Collider.enabled = false;

        targetPosition = Vector3.zero;

        gameObject.SetActive(false);

        isMagnetized = false;
    }


}
