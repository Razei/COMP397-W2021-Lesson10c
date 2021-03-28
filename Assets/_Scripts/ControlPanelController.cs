using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlPanelController : MonoBehaviour
{
    public RectTransform rectTransform;

    public Vector2 offScreenPosition;
    public Vector2 onScreenPosition;

    [Range(0.1f, 10.0f)] 
    public float speed = 1.0f;
    public float timer = 0.0f;
    public bool isOnScreen = false;

    [Header("Player Settings")]
    public CameraController playerCamera;
    public PlayerBehaviour player;

    public Pauseable pausable;

    [Header("Scene Data")]
    public SceneDataScriptableObject sceneData;

    public GameObject gameStateLabel;

    private void Awake()
    {
        // deserialize data from player preferences
        var data = PlayerPrefs.GetString("playerData");

        if (data != null)
        {
            /*JsonUtility.FromJsonOverwrite(data, sceneData);*/
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        pausable = FindObjectOfType<Pauseable>();
        playerCamera = FindObjectOfType<CameraController>();
        rectTransform = GetComponent<RectTransform>();
        player = FindObjectOfType<PlayerBehaviour>();
        rectTransform.anchoredPosition = offScreenPosition;
        timer = 0.0f;

        LoadFromPlayerPrefs();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    ToggleControlPanel();
        //}

        if (isOnScreen)
        {
            MoveControlPanelDown();

        }
        else
        {
            MoveControlPanelUp();

        }

        gameStateLabel.SetActive(pausable.isGamePaused);
    }

    void ToggleControlPanel()
    {
        isOnScreen = !isOnScreen;
        timer = 0.0f;

        if (isOnScreen)
        {
            //Cursor.lockState = CursorLockMode.None;
            playerCamera.enabled = false;
        }
        else
        {

            //Cursor.lockState = CursorLockMode.Locked;
            playerCamera.enabled = true;
        }
    }

    private void MoveControlPanelDown()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(offScreenPosition, onScreenPosition, timer);
        if (timer < 1.0f)
        {
            timer += Time.deltaTime * speed;
        }
    }

    private void MoveControlPanelUp()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(onScreenPosition, offScreenPosition, timer);
        if (timer < 1.0f)
        {
            timer += Time.deltaTime * speed;
        }

        if (pausable.isGamePaused)
        {
           pausable.TogglePause();
        }
    }

    public void OnControlButtonPressed()
    {
        ToggleControlPanel();
    }

    public void OnLoadButtonPressed()
    {
        LoadFromPlayerPrefs();

        player.controller.enabled = false;
        player.transform.position = sceneData.playerPosition;
        player.transform.rotation = sceneData.playerRotation;
        player.controller.enabled = true;

        int health = sceneData.playerHealth;
        player.health = health;
        player.healthBar.SetHealth(health);
        PlayerPrefs.SetString("playerData", JsonUtility.ToJson(sceneData));
    }

    public void OnSaveButtonPressed()
    {
        sceneData.playerPosition = player.transform.position;
        sceneData.playerRotation = player.transform.rotation;
        sceneData.playerHealth = player.health;

        /*// save data to player preferences dictionary / db
        PlayerPrefs.SetString("playerData",JsonUtility.ToJson(sceneData));*/
        SaveToPlayerPrefs();

    }

    public void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetFloat("playerTransformX", sceneData.playerPosition.x);
        PlayerPrefs.SetFloat("playerTransformY", sceneData.playerPosition.y);
        PlayerPrefs.SetFloat("playerTransformZ", sceneData.playerPosition.z);

        PlayerPrefs.SetFloat("playerRotationX", sceneData.playerRotation.x);
        PlayerPrefs.SetFloat("playerRotationY", sceneData.playerRotation.y);
        PlayerPrefs.SetFloat("playerRotationZ", sceneData.playerRotation.z);
        PlayerPrefs.SetFloat("playerRotationW", sceneData.playerRotation.w);

        PlayerPrefs.SetInt("playerHealth", sceneData.playerHealth);
    }

    public void LoadFromPlayerPrefs()
    {
        sceneData.playerPosition.x = PlayerPrefs.GetFloat("playerTransformX");
        sceneData.playerPosition.y = PlayerPrefs.GetFloat("playerTransformY");
        sceneData.playerPosition.z = PlayerPrefs.GetFloat("playerTransformZ");

        sceneData.playerRotation.x = PlayerPrefs.GetFloat("playerRotationX");
        sceneData.playerRotation.y = PlayerPrefs.GetFloat("playerRotationY");
        sceneData.playerRotation.z = PlayerPrefs.GetFloat("playerRotationZ");
        sceneData.playerRotation.w = PlayerPrefs.GetFloat("playerRotationW");

        sceneData.playerHealth = PlayerPrefs.GetInt("playerHealth");
    }
}
