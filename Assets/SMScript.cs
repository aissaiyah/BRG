using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SMScript : MonoBehaviour
{
    public static SMScript Instance;
    public GameObject pause;
    public bool paused;
    [Header("UI Text")]
    public TMP_Text playerText;
    public TMP_Text inkyText;
    public TMP_Text blinkyText;
    public TMP_Text pinkyText;
    public TMP_Text clydeText;

    [Header("Sliders")]
    public Slider playerSlider;
    public Slider inkySlider;
    public Slider blinkySlider;
    public Slider pinkySlider;
    public Slider clydeSlider;

    // Static speeds used by other scripts
    public static float PlayerSpeed = 0f;
    public static float InkySpeed = 0f;
    public static float BlinkySpeed = 0f;
    public static float PinkySpeed = 0f;
    public static float ClydeSpeed = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize values from sliders
        UpdatePlayerSpeed(playerSlider.value);
        UpdateInkySpeed(inkySlider.value);
        UpdateBlinkySpeed(blinkySlider.value);
        UpdatePinkySpeed(pinkySlider.value);
        UpdateClydeSpeed(clydeSlider.value);

        // Add listeners
        playerSlider.onValueChanged.AddListener(UpdatePlayerSpeed);
        inkySlider.onValueChanged.AddListener(UpdateInkySpeed);
        blinkySlider.onValueChanged.AddListener(UpdateBlinkySpeed);
        pinkySlider.onValueChanged.AddListener(UpdatePinkySpeed);
        clydeSlider.onValueChanged.AddListener(UpdateClydeSpeed);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
            
        }

        if (paused)
        {
            Time.timeScale = 0;
            pause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pause.SetActive(false);
        }
    }

    void UpdatePlayerSpeed(float value)
    {
        PlayerSpeed = value;
        playerText.text = value.ToString("F1");
    }

    void UpdateInkySpeed(float value)
    {
        InkySpeed = value;
        inkyText.text = value.ToString("F1");
    }

    void UpdateBlinkySpeed(float value)
    {
        BlinkySpeed = value;
        blinkyText.text = value.ToString("F1");
    }

    void UpdatePinkySpeed(float value)
    {
        PinkySpeed = value;
        pinkyText.text = value.ToString("F1");
    }

    void UpdateClydeSpeed(float value)
    {
        ClydeSpeed = value;
        clydeText.text = value.ToString("F1");
    }
}