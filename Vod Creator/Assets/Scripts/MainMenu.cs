using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{

    public GameObject[] startScreen;
    public GameObject[] creationScreen;

    public List<string> paths;

    public GameObject selection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.SetResolution(800, 800, false);
        this.showCreationScreen(true);
        checkForFiles();
    }

    public void showCreationScreen(bool active = false)
    {
        for (int i = 0; i < startScreen.Length; i++) { 
            startScreen[i].SetActive(active);
        }
        for(int i = 0;i < creationScreen.Length; i++) {
            creationScreen[i].SetActive(!active); 
        }
    }

    public void createVOD()
    {

    }

    public void checkForFiles()
    {
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo("Assets/Vods");
        int count = dir.GetFiles().Length;
        paths = new List<string>();
        for (int i = 0; i < count; i++)
        {
            string name = dir.GetFiles()[i].Name;
            if (!name.Contains(".meta"))
            {
                paths.Add(dir.GetFiles()[i].Name);
            }
        }
        createSelections();
    }
    void createSelections()
    {
        for (int i = 0; i < paths.Count; i++)
        {
            GameObject g = Instantiate(selection, transform);

            TMP_Text[] texts = g.GetComponentsInChildren<TMP_Text>();
            string name = paths[i].Remove(paths[i].Length - 5);
            texts[0].text = name;
            //JsonReader.jsonPath = "Assets/Quizes/" + paths[i];
            //JsonReader.mainMenuData();
            //texts[1].text = "Last Score: " + Questions.corPer + "%";
            g.GetComponent<RectTransform>().localPosition = new Vector2(150, (-i * 110) + 250);
            int index = i;
            //g.GetComponent<Button>().onClick.AddListener(() => this.selectQuiz(index));
        }

    }

}
