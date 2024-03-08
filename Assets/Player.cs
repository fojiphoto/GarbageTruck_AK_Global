using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    void Start()
    {
        //plyrstop pos "Trash Stop0"
        //banan --> follows the "Dumster"
        //when banana Hits the Dumpster , banana follows the truck point side 
        //when banana hits the side point slide animation runs
        // truck ready to go to "Trash Stop1"
        //when all the Target index full 
        // go to the "drop"
        // next as plan
 }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trash Stop0" || other.tag == "Trash Stop0") { } ;
    }


}
