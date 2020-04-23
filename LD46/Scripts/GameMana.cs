using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMana : MonoBehaviour
{
    public static GameMana Single => _Single;
    private static GameMana _Single;

    //-------------------GAME OBJ--------------------
    [SerializeField] private TextMesh countText;
    [SerializeField] private Transform allObj;
    [SerializeField] private PlayerController player;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera mapCamera;
    [SerializeField] private GameObject onEndEffect;
    [SerializeField] private PlayableDirector onEndShow;


    private LinkedList<Character> list = new LinkedList<Character>();
    private List<Character> all = new List<Character>();
    private int AllCount;

    private bool gameEnd = false;

    void Start()
    {
        _Single = this;
        foreach (Transform t in allObj)
        {
            var c = t.GetComponent<Character>();
            if (c != null) all.Add(c);
        }

        AllCount = all.Count(e => e.needToCount);
        UpdateCountUI();

        playerCamera.gameObject.SetActive(true);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.F5)) Screen.fullScreen = !Screen.fullScreen;
        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKeyDown(KeyCode.E)) SceneManager.LoadScene("zEnd");
        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKeyDown(KeyCode.S)) GoToNextLevel();
        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKeyDown(KeyCode.C)) {gameEnd=true; OnGameEnd();}
    }

    private void UpdateCountUI()
    {
        var listCount = list.Count;
        countText.text = $"{listCount}/{AllCount}";
        if (listCount == AllCount)
        {
            gameEnd = true;
            OnGameEnd();
        }
    }

    private void OnGameEnd()
    {
        player.SetStop();
        StartCoroutine(ShowEnd());
    }

    public void AddToList(Character c)
    {
        if (gameEnd) return;
        if (!c.needToCount) return;
        list.AddLast(c);
        UpdateCountUI();
    }

    IEnumerator ShowEnd()
    {
        foreach (var character in list) character.StopCountPeopleNumAndFillAll();
        yield return new WaitForSeconds(0.2f);
        playerCamera.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        if (onEndShow != null)
        {
            onEndShow.gameObject.SetActive(true);
            yield return new WaitForSeconds((float) onEndShow.duration); 
        }

        int count = 0;
        if(onEndEffect != null) onEndEffect.SetActive(true);

        float timeScale = 3f / list.Count;

        foreach (var character in list)
        {
            character.SpriteAnimator.SetTrigger("Jump3Times");
            yield return new WaitForSeconds(0.2f* timeScale);
            character.CharacterAudio
                .SetRandomPitchScale(0.25f)
                .SetPitchDelta((count+++0f)/list.Count)
                .SayHi(); ;
            yield return new WaitForSeconds(0.6f* timeScale);
        }
        foreach (var character in list)
        {
            character.SpriteAnimator.SetBool("JumpLoop",true);
        }
        yield return new WaitForSeconds(1f);
        GoToNextLevel();
    }

    public void RemoveFromList(Character c)
    {
        if (gameEnd) return;
        if (!c.needToCount) return;
        list.Remove(c);
        UpdateCountUI();
    }

    public void GoToNextLevel()
    {
        var activeScene = SceneManager.GetActiveScene();
        if (activeScene.buildIndex == SceneManager.sceneCountInBuildSettings - 1) SceneManager.LoadScene("zEnd");
        else
        {
            
            SceneManager.LoadScene(activeScene.buildIndex + 1);
        }
    }
}
