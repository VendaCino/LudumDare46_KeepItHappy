using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Cinemachine;
using UnityEngine;

public class MapScaleMana : MonoBehaviour
{
    [SerializeField] private Sprite scaleSprite;
    [SerializeField] private SpriteRenderer backGround;
    [SerializeField] private Transform Group;
    [SerializeField] private SmoothOrthographicSize playerCam;

    [SerializeField] private float pixPerUint = 4;
    [Range(0, 10)]
    [SerializeField] private float vcamScale = 1;
    [Range(0, 1)]
    [SerializeField] private float speedScale = 1;


    [Range(1, 10)]
    [SerializeField]
    private float scaleK = 1;

    [Range(0, 10)]
    [SerializeField]
    private float scaleOffset = 0;



    void Start()
    {
    }

    private float GetScale(Vector2 pos)
    {
        Vector2 backCenter = backGround.transform.position;
        var texture = scaleSprite.texture;
        pos = pos - backCenter;

        pos = pos / backGround.transform.lossyScale;
        float x = pos.x;
        float y = pos.y;
        x =( x / backGround.size.x )* pixPerUint + 0.5f;
        y = (y / backGround.size.y )* pixPerUint + 0.5f;

        
        int ix = (int) (x * texture.width);
        int iy = (int)(y * texture.height);
        if (ix < 0) ix = 0;
        if (ix >= texture.width) x = texture.width-1;
        if (iy < 0) iy = 0;
        if (iy >= texture.height) y = texture.height - 1;
        var pixel = texture.GetPixel(ix, iy);
        return pixel.r;

        return 1;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform ts in Group)
        {
            var scale = GetScale(ts.position);
            scale = scale * scaleK + scaleOffset;
            ts.localScale = Vector3.one * scale;

            if (ts.tag == "Player")
            {
                playerCam.SetOrthographicSize(playerCam.OriginSize * vcamScale * scale);
                ts.GetComponent<PlayerController>().SetSpeedScale(speedScale * scale + (1 - speedScale));
            }
        }
    }
}
