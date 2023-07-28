using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (!Application.isMobilePlatform)
        {
           Destroy(gameObject);
        }
    }

}
