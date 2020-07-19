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

    public static bool gamestarted = false;
    private void Awake() {
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
            foundNames.Add(name);
        }
    }


    IEnumerator start() {
        mainCameraAnimator.SetTrigger("StartGame");

        yield return new WaitForSeconds(mainCameraAnimator.GetCurrentAnimatorStateInfo(0).length);
        gameMenu.SetActive(true);
        menu.SetActive(false);

    }

}
