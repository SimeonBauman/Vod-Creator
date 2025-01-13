using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public List<GameObject> startScreen;
    public GameObject[] creationScreen;

    public List<string> paths;

    public GameObject selection;

    public TMP_InputField[] inputFields;

    public TMP_InputField VODName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.SetResolution(800, 800, false);
        this.showCreationScreen(true);
        checkForFiles();
    }

    public void showCreationScreen(bool active = false)
    {
        for (int i = 0; i < startScreen.Count; i++) { 
            startScreen[i].SetActive(active);
        }
        for(int i = 0;i < creationScreen.Length; i++) {
            creationScreen[i].SetActive(!active); 
        }
    }

    public void createVOD()
    {
        int index = 0;
        for(int i = inputFields.Length-1; i >= 0; i--)
        {
            if(inputFields[i].text != "")
            {
                
                index = i;
                break;
            }
        }

        POVdata[] POVs = new POVdata[index+1];

        for (int i = 0; i <= index; i++)
        {
            Debug.Log(i);
            POVs[i] = new POVdata();
            POVs[i].path = inputFields[i].text;
        }

        JSONReader.writeToJSON(VODName.text, POVs);

        SceneManager.LoadScene(1);
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
            startScreen.Add(g);
        }

    }

}
