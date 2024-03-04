using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Truckprop : MonoBehaviour
{
    public Slider Healthfill;
    public Slider Collectionfill;
    // Start is called before the first frame update
    public void SetMaxHealth(int health)
    {
        Healthfill.maxValue = health;
        Healthfill.value = health;

    }
    public void SetHealth(int health)
    {
        Healthfill.DOValue(health, 1f);
    }

    public void  GarbageFill(int collect)
    {

        Collectionfill.DOValue(collect, 2f);
        Debug.Log("Collect" +collect);
    }

}
