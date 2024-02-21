using UnityEngine;
using System.Collections;
namespace SWS
{
    public class avoidtrafic : MonoBehaviour
    {
       // public GameObject horn;

        public GameObject car;
        public bool walk;
        // Use this for initialization
        void Start()
        {
            walk = true;
        }
			
        public void stop1()
        {
            walk = false;
            car.GetComponent<splineMove>().Pause();

 
        }
        public void play1()
        {
           
            car.GetComponent<splineMove>().Resume();


        }

        void OnTriggerEnter(Collider other)
        {
            if(other.tag=="Player")
            {
				stop1();
				this.gameObject.GetComponent<AudioSource>().Play();
				if (this.gameObject.GetComponent<Animator> ()) 
			{
					this.gameObject.GetComponent<Animator> ().enabled = false;
			}

            }
        }
        void OnTriggerExit(Collider other)
        {
           if(other.tag == "Player")
           {
               walk = true;
               play1();

				this.gameObject.GetComponent<Animator> ().enabled = true;		
           }

        }

    }
}
