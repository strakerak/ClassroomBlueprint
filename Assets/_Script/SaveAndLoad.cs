using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class SaveAndLoad : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private StructurePlacer structurePlacer;

    [SerializeField]
    private PlacementManager pm;

    [SerializeField]
    private ItemDataBaseSO structuresData;

    [SerializeField]
    public PlacementGridData pgd;

    [SerializeField]
    public GameObject saveCanvas;

    [SerializeField]
    public GameObject helpCanvas;

    [SerializeField]
    public GameObject saveNameCanvas;

    [SerializeField]
    public GameObject loadNameCanvas;

    [SerializeField]
    public TMP_InputField saveNameText;

    [SerializeField]
    public TMP_InputField loadNameText;

    [SerializeField]
    public GameObject welcomeCanvas;

    BuildingState buildingState;

    public string saveName = "";

    public string loadName = "";


    public void saveMap()
    {
        string saveList = structurePlacer.printList();
        Debug.Log("save list is");
        Debug.Log(saveList);


        if (WriteToFile(saveName, saveList))
        {
            saveCanvas.SetActive(true);
            Debug.Log("Save success! Saved as " + saveName);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && GameObject.Find("welcome"))
        {
            killWelcome();
        }
    }

    public void closeLoader()
    {
        loadNameCanvas.SetActive(false);
    }

    public void closeSaver()
    {
        saveNameCanvas.SetActive(false);
    }

    private bool WriteToFile(string name, string content)
    {
        string path = "SaveFiles/";

        try
        {
            File.WriteAllText(path + name, content);
            Debug.Log(pgd.gridCellsDictionary);
            string json = JsonUtility.ToJson(pgd.gridCellsDictionary);
            PlayerPrefs.SetString("name", json);
            PlayerPrefs.Save();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving to a file " + e.Message);
        }
        return false;
    }

    public void startSave()
    {
        saveNameCanvas.SetActive(true);
    }

    public void nameSaver()
    {
        saveName = saveNameText.text;
        setSaveName();
    }

    public void killWelcome()
    {
        welcomeCanvas.SetActive(false);
    }

    public void setSaveName()
    {
        saveNameCanvas.SetActive(false);
        saveMap();
    }

    public void okaydone()
    {
        saveCanvas.SetActive(false);
    }
    
    public void noHelp()
    {
        helpCanvas.SetActive(false);
    }

    public void helpMe()
    {
        helpCanvas.SetActive(true);
    }

    public void selectName()
    {
        loadNameCanvas.SetActive(true);
    }

    public void setLoadName()
    {
        loadName = loadNameText.text;
        StaticData.dataToPass = loadName;
        SceneManager.LoadScene("ViewerSystem");

    }

    public void loadMap()
    {
        Debug.Log("Clearing Map");
        Debug.Log("Starting Load");
        List<string> lines = new List<string>();
        lines = loadData("defaulto");
        Debug.Log("Load complete? Now trying to place");
        for (int i = 0; i < lines.Count; i++)
        {
            //Debug.Log(lines[i]);
            string[] listSplit = lines[i].Split(char.Parse(":"));
            if (listSplit.Length > 1)
            {
                //Debug.Log(listSplit[0] + " " + listSplit[1] + " " + listSplit[2]);

                ItemData x = structuresData.GetItemWithID(int.Parse(listSplit[0]));

                //Debug.Log(x.ID);

                listSplit[1] = listSplit[1].Replace("(", "");
                listSplit[1] = listSplit[1].Replace(")", "");

                //Debug.Log(listSplit[1]);

                string[] vec3s = listSplit[1].Split(char.Parse(","));

                //Debug.Log(vec3s[0]+ "," + vec3s[1] + "," + vec3s[2]);

                listSplit[2] = listSplit[2].Replace("(", "");
                listSplit[2] = listSplit[2].Replace(")", "");

                //Debug.Log(listSplit[2]);

                string[] qs = listSplit[2].Split(char.Parse(","));

                //Debug.Log(qs[0] + "," + qs[1] + "," + qs[2] + "," + qs[3]);

                Vector3 position = new Vector3(float.Parse(vec3s[0]), float.Parse(vec3s[1]), float.Parse(vec3s[2]));

                //Debug.Log(position);

                Vector3Int positionint = new Vector3Int((int)float.Parse(vec3s[0]), (int)float.Parse(vec3s[1]), (int)float.Parse(vec3s[2]));

                //Debug.Log(positionint);


                Quaternion rotation = new Quaternion(float.Parse(qs[0]), float.Parse(qs[1]), float.Parse(qs[2]), float.Parse(qs[3]));

                //Debug.Log(rotation);




                if (x.objectPlacementType.IsEdgePlacement())
                {
                    Debug.Log("Edge");
                    Debug.Log(x.name);
                    int objectIndex = structurePlacer.PlaceStructure(x.prefab, position, rotation, 0);
                    try
                    {
                        pgd.AddEdgeObject(objectIndex, x.ID, positionint, x.size, 0, rotation);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }

                }
                else
                {
                    Debug.Log("Cell");
                    Debug.Log(x.name);
                    int objectIndex = structurePlacer.PlaceStructure(x.prefab, position, rotation, 0);
                    try
                    {
                        pgd.AddCellObject(objectIndex, x.ID, positionint, x.size, 0, rotation);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }

                }

            }

        }
    }



    public List<string> loadData(string saveName)
    {
        string data = "";
        List<string> lines = new List<string>();
        if (ReadFromFile(saveName, out data))
        {
            Debug.Log("Data loaded!");
            Debug.Log(data);

            //Go line by line
            string txt = "";
            for(int i = 0;i<data.Length;i++)
            {
                if (data[i] == '\n')
                {
                    lines.Add(txt);
                    txt = "";
                }
                else
                {
                    txt += data[i];
                }
            }
            lines.Add(txt);
            txt = "";
        }
        return lines;
    }


    private bool ReadFromFile(string name, out string content)
    {
        string path = "SaveFiles/";

        try
        {
            content = File.ReadAllText(path+name);
            Debug.Log(content);
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError("Error: Unable to load file " + e.Message);
            content = "";
        }
        return false;
    }

    public void clearMap()
    {
        structurePlacer.clearMapper();
        pm.clearMap();
        SceneManager.LoadScene("PlacementSystem");
    }

    public void quitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
