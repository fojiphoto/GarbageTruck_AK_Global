using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public string AnimationName;
    public Animator Player;
    public bool play;
    public PathManager[] Paths; //Left , Right , Back
    public bool ReverseSpline;
    public string[] AnimationStateName;
    public int TestSpline, AnimationState;
    public splineMove SplineMove;
    public GameObject TrashCan;
    public LevelCompletion LevelCompletionS;
    //public static int counter;
    public GameObject DummyMonster;
    public GameObject Drop;
    //public Button button1;
    //public Button button2;
    public GameObject button1;
    public bool StartAnim;
   

    public static PlayerController instance;

    public void Awake()
    {
       if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        Player = this.GetComponent<Animator>();
        SplineMove = this.GetComponent<splineMove>();
        // Drop.SetActive(false);
    }
    public void Animationplay(string AnimationName)
    {
        Player.Play(AnimationName);
    }
    public void PlaySpline(int PathNumber)
    {
        SplineMove.Stop();
        SplineMove.pathContainer = Paths[PathNumber];
        SplineMove.reverse = ReverseSpline;
        SplineMove.StartMove();
        StartAnim = true;
    }
    public void PickTrash(int Type)
    {
        //counter++;
        SplineMove.StartMove();
        SplineMove.pathContainer = Paths[Type];
        StartAnim = true;
        Animationplay(AnimationStateName[4]);
        if(SplineMove.reverse == false)
        {
            //Debug.LogError("forward");
            this.transform.GetChild(0).gameObject.SetActive(true);
           // DummyMonster.transform.GetChild(0).gameObject.SetActive(false);
            DummyMonster.SetActive(false);
            SplineMove.reverse = true;
            TrashCan.SetActive(false);
        }
        else
        {
            StartCoroutine(LevelCompletionS.Complete());
            //Debug.LogError("back");
            SplineMove.reverse = false;
            TrashCan.SetActive(true);
            
        }
    }
    
    void Update()
    {
        
        if (play)
        {
            Animationplay(AnimationStateName[AnimationState]);
            PlaySpline(TestSpline);
            play = false;
        }
        
        if (StartAnim)
        {
            Debug.Log("CurrentSplineMove: " + SplineMove.currentPoint);
            Debug.Log("AnimationStateName: " + AnimationStateName[SplineMove.currentPoint]);
            if (SplineMove.currentPoint == 6 )
            {
                button1.SetActive(true);
                TrashCan.SetActive(false);
                Animationplay(AnimationStateName[0]);
            }
            if (SplineMove.currentPoint == 6 && AnimationName == AnimationStateName[1])
            {   
                button1.SetActive(false);             
               
            }
            
            else if (SplineMove.currentPoint == 0)
            {
                Animationplay(AnimationStateName[0]);
                button1.SetActive(true);
                
            }
            else if(SplineMove.currentPoint > 0 && SplineMove.currentPoint < 6)
            {
                Animationplay(AnimationStateName[4]);
              button1.SetActive(false);
            }
            else
            {
                //button1.SetActive(false);
            }
        }

        if (Drop!)
        {
            Drop = GameObject.FindGameObjectWithTag("drop");
        }
    }
    //public void OnPointerClick()
    //{
    //    StartCoroutine(EnableButtonAfterDelay(3f));

    //}

    //private IEnumerator EnableButtonAfterDelay(float delay)
    //{
    //    if (button1.interactable)
    //    {
    //        button1.interactable = false;

    //        yield return new WaitForSeconds(delay);
    //        button1.interactable = true;


    //    }

    //    else if(!button1.interactable)
    //    {
    //        button1.interactable = true;

    //        yield return new WaitForSeconds(delay);
    //        button1.interactable = false;


    //    } }


}
