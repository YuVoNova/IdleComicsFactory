using UnityEngine;

public class Magnet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)    // Money
        {
            other.GetComponent<ThrowMoney>().Magnetize(transform.parent.position);
        }
    }
}
