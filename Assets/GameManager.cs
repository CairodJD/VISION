using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


    public GameObject gameMenu;
    public GameObject menu;

    public Animator mainCameraAnimator;
    public List<Text> hiddenNames;
    private List<string> foundNames;

    //Audio
    AudioSource audioSource;

    public static bool gamestarted = false;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        foundNames = new List<string>();
        GameObject.FindWithTag("Planet").GetComponent<Planet>().PlanetFound += onPlanetFound;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            gamestarted = true;
            StartCoroutine(start());
        }
    }

    public void onPlanetFound(string name) {
        //set a random ??? to the found planet name
        if (hiddenNames != null && hiddenNames.Count > 0 && !foundNames.Contains(name) ) {
            int tomdif = Random.Range(0, hiddenNames.Count - 1);
            hiddenNames[tomdif].text = name;
            StartCoroutine(FadeTextToFullAlpha(1.5f, hiddenNames[tomdif]));
            StartCoroutine(Expand(hiddenNames[tomdif]));
            foundNames.Add(name);
        }
    }

    void PlayClip(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }
    
    IEnumerator start() {
        mainCameraAnimator.SetTrigger("StartGame");

        yield return new WaitForSeconds(mainCameraAnimator.GetCurrentAnimatorStateInfo(0).length);
        gameMenu.SetActive(true);
        menu.SetActive(false);

    }
    //fade in 
    //https://forum.unity.com/threads/fading-in-out-gui-text-with-c-solved.380822/
    IEnumerator FadeTextToFullAlpha(float t, Text i) {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f) {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    // setting up 2 sprites with fill type horizontal but different fill origin left and right
    // creating a effect like we open a curtains form the middle
    IEnumerator Expand(Text t , float decayspeed = 0.1f) {
        Image left = t.transform.GetChild(0).GetComponent<Image>();
        Image right = t.transform.GetChild(1).GetComponent<Image>();

        while (left.fillAmount >= 0 && right.fillAmount >= 0) {
            left.fillAmount -= decayspeed;
            right.fillAmount -= decayspeed;
            yield return null;
        }
    }
}
