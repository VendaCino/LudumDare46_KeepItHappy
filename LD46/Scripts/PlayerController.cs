using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //-----------UI And GameObj--------------

    //------Config-----------
    [SerializeField] private float maxSpeed = 1;

    //----------------Logical---------------
    private Rigidbody2D rb;
    private Item canPickedItem;
    private Item pickingItem;

    private bool stop = false;
    private float zSleepTime = 0.05f;
    private float zColdTimer = 0f;

    private bool zPress = false;

    private float originSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originSpeed = maxSpeed;
    }

    void Update()
    {
        zColdTimer = Math.Max(0, zColdTimer - Time.deltaTime);
        if (stop) return;
        InputSpeed();
        InputItemPick();
        UpdatePickingItemPosition();
    }


    void InputSpeed()
    {
        var vx = Input.GetAxis("Horizontal"); 
        var vy = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(vx, vy) * maxSpeed;
    }

    void InputItemPick()
    {
        if (!Input.GetKeyDown("z")) zPress = false;
        if (zPress) return;
        if (!Input.GetKeyDown("z")) return;

        zPress = true;

        if (zColdTimer > 0) return;
        else zColdTimer = zSleepTime;

        if (canPickedItem == null && pickingItem == null) return;
        else if (canPickedItem != null && pickingItem == null) PickItem(canPickedItem);
        else if (canPickedItem == null && pickingItem != null )
        {
            canPickedItem = pickingItem;
            PutItem(pickingItem);
        }else if(pickingItem == canPickedItem && pickingItem != null)
        {
            PutItem(pickingItem);
        }
        else 
        {
            PutItem(pickingItem);
            PickItem(canPickedItem);
        }
    }

    void UpdatePickingItemPosition()
    {
        if (pickingItem == null) return;
        pickingItem.GetComponent<Rigidbody2D>().position = rb.position+Vector2.up*transform.lossyScale.y;
    }

    private void PickItem(Item item)
    {
        var c = item.GetComponent<Character>();
        c.BaseCircleCollider2D.isTrigger = true;
        pickingItem = item;
        canPickedItem = null;

        GetComponent<Character>().SpriteAnimator.SetTrigger("JumpOnce");
    }

    private void PutItem(Item item)
    {
        var c = item.GetComponent<Character>();
        c.BaseCircleCollider2D.isTrigger = false;

        GetComponent<Character>().SpriteAnimator.SetTrigger("JumpOnce");
        pickingItem = null;
    }

    public void SetCanPickItem(Item item)
    {
        canPickedItem = item;
    }
    public void UnSetCanPickItem(Item item)
    {
        // check in case of unset of other item
        if (item == canPickedItem) canPickedItem = null;
    }

    public void SetStop()
    {
        stop = true;
    }

    public void SetSpeedScale(float scale)
    {
        maxSpeed = originSpeed * scale;
    }
}
