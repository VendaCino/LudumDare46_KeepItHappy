using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    [SerializeField] private TextMesh hintText;


    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D o)
    {
        if (o.tag != "Player") return;
        
        hintText.gameObject.SetActive(true);

        var player = o.GetComponent<PlayerController>();
        if (player == null) return;
        player.SetCanPickItem(this);
    }
    void OnTriggerExit2D(Collider2D o)
    {
        if (o.tag != "Player") return;
        hintText.gameObject.SetActive(false);

        var player = o.GetComponent<PlayerController>();
        if (player == null) return;
        player.UnSetCanPickItem(this);
    }
}
