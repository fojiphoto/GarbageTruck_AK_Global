using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setactive : MonoBehaviour
{
    public static bool Andrew;
    
    void Start()
    {
       if(Andrew)
        {
            this.gameObject.SetActive(false);
        }
       else
        {
            Andrew = true;
        }
    }
    public void justonetime()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
