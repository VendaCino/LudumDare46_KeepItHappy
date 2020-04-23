using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody2D),typeof(CircleCollider2D))]
public class Character : MonoBehaviour
{
    //-----------------UI or GameObj----------------
    [SerializeField] private TextMesh CountText;
    [SerializeField] private SpriteRenderer Sprite;
    [SerializeField] private CircleCollider2D CheckDistanceCollider;
    [SerializeField] private SpriteRenderer CheckDistanceIndicator;
    [SerializeField] private TextMesh SayingText;
    [SerializeField] private Light2D pointLight2D;
    [SerializeField] private CharacterAudioManager characterAudio;


    //----------------Config Param----------------
    [SerializeField] private float _CheckDistance = 2;
    public int NeedPeopleNum = 2;
    public int WeightNum = 1;
    [SerializeField] private TextConfig textConfig;
    private float sayWaitTime = 2f;
    public bool needToCount = true;

    //-----------------Logical-------------------
    private int NowPeopleNum = 0;
    private bool _enough = false;
    private float sayColdTimer = 0f;

    private bool stopCountPeopleNum = false;

    private CircleCollider2D baseCircleCollider2D;
    private float baseCircleCollider2DOriginRadius;


    //-----------CONSTANT----------
    private static Color sadColor = new Color(0.4913726f, 0.09411765f, 0.007843138f);
    private static Color happyColor = new Color(1, 0.40f, 0);// new Color(0.4260892f, 1, 0.3215686f);
    

    void Start()
    {
        SetCheckDistanceObject(_CheckDistance);
        textConfig.Init();
        baseCircleCollider2D = GetComponent<CircleCollider2D>();
        baseCircleCollider2DOriginRadius = baseCircleCollider2D.radius;
        _enough = NowPeopleNum >= NeedPeopleNum;
    }

    //---------------Getter Setter-------------------

    public CircleCollider2D BaseCircleCollider2D => baseCircleCollider2D;
    public CircleCollider2D CheckDistanceCollider2D => CheckDistanceCollider;
    public float CheckDistance => _CheckDistance;
    public Animator SpriteAnimator => Sprite.GetComponent<Animator>();
    public CharacterAudioManager CharacterAudio => characterAudio;

    public void SetCheckDistanceObject(float distance)
    {
        if (_enough) distance = distance * 2;

        CheckDistanceCollider.radius = distance;
        CheckDistanceIndicator.transform.localScale = Vector3.one * distance * 2;
        pointLight2D.pointLightOuterRadius = distance;
    }

    // Update is called once per frame
    void Update()
    {
        if(!stopCountPeopleNum) UpdateNowPeopleNum();
        UpdateCountText();
        UpdateTimer();
        UpdateEnough();
    }

    void UpdateNowPeopleNum()
    {
        int count = 0;
        foreach (Transform t in transform.parent)
        {
            var other = t.GetComponent<Character>();
            if (other == null) continue;
            if (other == this) continue;
            if (!CheckDistanceCollider.IsTouching(other.CheckDistanceCollider)) continue;
            count += other.WeightNum;
        }
        NowPeopleNum = count;
    }

    void UpdateCountText()
    {
        CountText.text = $"{NowPeopleNum}/{NeedPeopleNum}";
    }

    void UpdateEnough()
    {
        bool enough = NowPeopleNum >= NeedPeopleNum;
        // 1. On Enter Enough
        if (enough == true && _enough == false)
        {
            Say(textConfig.OnEnter,happyColor);
            GameMana.Single.AddToList(this);
            CharacterAudio.SetRandomPitchScale(0.5f).AtRandomHighPitch().SayHappy();

            SetCheckDistanceObject(CheckDistance);
        }
        // 2. On Exit Enough
        if (enough == false && _enough == true)
        {
            Say(textConfig.OnExit,sadColor);
            GameMana.Single.RemoveFromList(this);
            CharacterAudio.SetRandomPitchScale(0.5f).AtRandomLowPitch().SayHappy();

            SetCheckDistanceObject(CheckDistance);
        }
        if (enough == false) SayWait(textConfig.OnNot,sadColor);
        if (enough == true) SayWait(textConfig.OnYes,happyColor);
        _enough = enough;
        pointLight2D.gameObject.SetActive(enough);

        
    }

   

    private static Random random = new Random();

    void UpdateTimer()
    {
        sayColdTimer = Math.Max(0, sayColdTimer - Time.deltaTime);
    }
    private void Say(string[] list,Color color)
    {
        if (list == null || list.Length == 0) return;
        var sentence = list[random.Next(list.Length)];
        SayingText.text = sentence;
        SayingText.gameObject.SetActive(true);
        SayingText.color = color;
    }

    private void SayWait(string[] list, Color color)
    {
        if (sayColdTimer > 0) return;
        else sayColdTimer = sayWaitTime +random.Next(100) / 100f;
        Say(list, color);
    }

    [Serializable]
    struct TextConfig
    {
        [SerializeField]private string OnNotEnough;
        [SerializeField] private string OnEnough;
        [SerializeField] private string OnEnterEnough;
        [SerializeField] private string OnExitEnough;
        [NonSerialized] public string[] OnNot;
        [NonSerialized] public string[] OnYes;
        [NonSerialized] public string[] OnEnter;
        [NonSerialized] public string[] OnExit;
        public void Init()
        {
            OnNot = OnNotEnough.Split('|');
            OnYes = OnEnough.Split('|');
            OnEnter = OnEnterEnough.Split('|');
            OnExit = OnExitEnough.Split('|');
        }
    }

    public void StopCountPeopleNumAndFillAll()
    {
        NowPeopleNum = NeedPeopleNum;
        stopCountPeopleNum = true;
    }

    public void ScaleBody(float scale)
    {
        SpriteAnimator.transform.localScale = Vector3.one*scale;
        baseCircleCollider2D.radius = baseCircleCollider2DOriginRadius * scale;
    }
}
