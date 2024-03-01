using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    public Image Fill;
    void Start()
    {
        StartCoroutine("SceneLoadingFromSplash");
    }
    IEnumerator SceneLoadingFromSplash()
    {
        Fill.DOFillAmount(1, 12f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(12f);
        SceneManager.LoadScene("Main Menu");

    }
}
