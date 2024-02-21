using UnityEngine;
using System.Collections;
namespace SWS
{
    public class rotatetyre : MonoBehaviour
    {
        public GameObject mycar;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (mycar.GetComponent<avoidtrafic>().walk == true)
            {
                transform.Rotate(Vector3.right * 10f);
 
            }
               

        }
    }
}