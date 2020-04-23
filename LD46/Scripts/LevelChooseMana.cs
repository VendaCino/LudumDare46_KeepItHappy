using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChooseMana : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.F5)) Screen.fullScreen = !Screen.fullScreen;
        if (Input.GetKeyDown("r"))
        {
            Destroy(this.gameObject);
            Load("zEnd");
        }
    }

    public void Load(string name)
    {
        GameObject.DontDestroyOnLoad(this);
        SceneManager.LoadScene(name);
    }
}
