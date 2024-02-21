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
    private splineMove SplineMove;
    public GameObject TrashCan;
    public LevelCompletion LevelCompletionS;
    public static int counter;
    public GameObject DummyMonster;
    public GameObject Drop;
    public Button button1;
    public Button button2;
    public Button button3;


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
        counter++;
        SplineMove.StartMove();
        SplineMove.pathContainer = Paths[Type];
        Animationplay(AnimationStateName[4]);
        if (SplineMove.reverse == false)
        {
            Debug.LogError("forward");
            this.transform.GetChild(0).gameObject.SetActive(true);
            DummyMonster.transform.GetChild(0).gameObject.SetActive(false);
            SplineMove.reverse = true;
            TrashCan.SetActive(false);

        }
        else
        {
            Debug.LogError("back");
            SplineMove.reverse = false;
            TrashCan.SetActive(true);
            StartCoroutine(LevelCompletionS.Complete());


        }
    }
    public bool StartAnim;
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
            if (SplineMove.currentPoint > 6)
            {
                Animationplay(AnimationStateName[0]);

                TrashCan.SetActive(false);
            }
            else if (SplineMove.currentPoint <= 1)
            {
                Animationplay(AnimationStateName[0]);
            }
            else
            {
                Animationplay(AnimationStateName[4]);
            }
        }

        if (Drop!)
        {
            Drop = GameObject.FindGameObjectWithTag("drop");
        }
    }
    public void OnPointerClick()
    {
        StartCoroutine(EnableButtonAfterDelay(8f));

    }

    private IEnumerator EnableButtonAfterDelay(float delay)
    {
        if (button1.interactable || button2.interactable || button3.interactable)
        {
            button1.interactable = false;
            button2.interactable = false;
            button3.interactable = false;
            yield return new WaitForSeconds(delay);
            button1.interactable = true;
            button2.interactable = true;
            button3.interactable = true;

        }

        else if(!button1.interactable || !button2.interactable || !button3.interactable)
        {
            button1.interactable = true;
            button2.interactable = true;
            button3.interactable = true;
            yield return new WaitForSeconds(delay);
            button1.interactable = false;
            button2.interactable = false;
            button3.interactable = false;

        }






    }


}
