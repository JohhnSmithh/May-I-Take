using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // instance
    public static GameManager instance;

    // save data
    [System.Serializable]
    private class SaveData
    {
        public bool breakfastClear;
        public bool lunchClear;
        public bool dinnerClear;
        public bool unlimited;
    }
    private SaveData data;

    // audio variables
    AudioSource audioSource;
    [SerializeField] private AudioClip unlimitedToggleAudio;
    [SerializeField] private AudioClip levelSelectAudio;
    [SerializeField] private AudioClip howToAudio;
    [SerializeField] private AudioClip ambience;

    #region UNITY METHODS

    public void Awake() // called each time a scene is loaded/reloaded
    {
        // set up GameManager as singleton class
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            data = new SaveData();
            string path = Application.persistentDataPath + "/savedata.json";
            if (File.Exists(path))
            {
                // read json file into data object
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<SaveData>(json);
            }
            else // default save file configuration
            {
                data.breakfastClear = false;
                data.lunchClear = false;
                data.dinnerClear = false;
                data.unlimited = false;
            }
        }
        else
        {
            Destroy(gameObject); // destroy duplicate of singleton object
        }
    }

    void Start() // only called once at program boot-up (since duplicate GameManagers are deleted in Awake() as a singleton)
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(ambience);
        }
    }

    private void OnApplicationQuit()
    {
        // save SaveData to json file
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }

    #endregion

    #region DATA GETTERS

    public bool GetBreakfastClear()
    {
        return data.breakfastClear;
    }

    public bool GetLunchClear()
    {
        return data.lunchClear;
    }

    public bool GetDinnerClear()
    {
        return data.dinnerClear;
    }

    public bool GetUnlimited()
    {
        return data.unlimited;
    }

    #endregion

    #region DATA SETTERS

    public void SetBreakfastClear(bool state)
    {
        data.breakfastClear = state;
    }

    public void SetLunchClear(bool state)
    {
        data.lunchClear = state;
    }

    public void SetDinnerClear(bool state)
    {
        data.dinnerClear = state;
    }

    public void ToggleUnlimited()
    {
        data.unlimited = !data.unlimited;
        audioSource.PlayOneShot(unlimitedToggleAudio, 5f);
    }

    #endregion

    #region SCENE MANAGEMENT

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadBreakfast()
    {
        audioSource.PlayOneShot(levelSelectAudio);
        if (data.unlimited)
            LoadScene("BreakfastUnlimited");
        else
            LoadScene("Breakfast");
    }

    public void LoadLunch()
    {
        audioSource.PlayOneShot(levelSelectAudio);
        if (data.unlimited)
            LoadScene("LunchUnlimited");
        else
            LoadScene("Lunch");
    }

    public void LoadDinner()
    {
        audioSource.PlayOneShot(levelSelectAudio);
        if (data.unlimited)
            LoadScene("DinnerUnlimited");
        else
            LoadScene("Dinner");
    }

    public void LoadHowTo()
    {
        audioSource.PlayOneShot(howToAudio);
        LoadScene("HowTo");
    }

    public void LoadMenu()
    {
        audioSource.PlayOneShot(howToAudio);
        LoadScene("Menu");
    }

    #endregion
}
