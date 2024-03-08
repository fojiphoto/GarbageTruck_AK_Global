using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleanimation : MonoBehaviour
{
    public Animator Player;
    // Start is called before the first frame update
    void OnEnable()
    {
        Player.Play("idle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
