using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class SmoothOrthographicSize : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera target;
    [SerializeField] private float speed;
    [SerializeField] private float precision = 0.01f;

    private float targetSize;
    private float originSize;

    public float NowSize => target.m_Lens.OrthographicSize;
    public float OriginSize => originSize;
    void Start()
    {
        target = GetComponent<CinemachineVirtualCamera>();
        targetSize = NowSize;
        originSize = targetSize;
    }


    public void SetOrthographicSize(float size)
    {
        targetSize = size;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(NowSize - targetSize) < precision) return;

        float dValue = speed * Time.deltaTime;


        if (NowSize < targetSize && NowSize + dValue > targetSize)
        {
            target.m_Lens.OrthographicSize = targetSize;
        }
        else if (NowSize > targetSize && NowSize - dValue < targetSize)
        {
            target.m_Lens.OrthographicSize = targetSize;
        }else if (NowSize < targetSize)
        {
            target.m_Lens.OrthographicSize += dValue;
        }
        else
        {
            target.m_Lens.OrthographicSize -= dValue;
        }

    }
}
