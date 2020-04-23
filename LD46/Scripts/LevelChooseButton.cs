using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelChooseButton : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private string des;

    private Button btn;
    void Start()
    {
        if (string.IsNullOrWhiteSpace(name)) name = gameObject.name;
        if (string.IsNullOrWhiteSpace(des)) des = name;

        btn = GetComponent<Button>();
        var levelChooseMana = GameObject.Find("LevelChooseMana").GetComponent<LevelChooseMana>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(()=> levelChooseMana.Load(name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
