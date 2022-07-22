using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMoney : MonoBehaviour
{
    private void Awake()
    {
        transform.eulerAngles = new Vector3(0f, Random.Range(-10f, 10f), 0f);
    }


}
