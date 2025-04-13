using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MoveCameraIntoChapter : MonoBehaviour
{
    [SerializeField] private string chapterSceneName = "Environment";
    [SerializeField] private string transferSceneName = "MainMenu";
    private string levelCameraTag = "LevelCamera";

    [SerializeField] private Vector3 targetCameraPosition;
    [SerializeField] private Vector3 targetCameraRotation; 
    [SerializeField] private Vector3 targetCameraZoomoutPosition;
    
    [SerializeField] private float animationDuration = 3f;
    [SerializeField] private Transform levelCameraTransform;
    
    private bool isZooming = false;
    private bool transitionTriggered = false;
    
    Vector3 startPosition;
    Quaternion startRotation;
    Quaternion targetRotation;
    
    public Animator animator;

    float elapsedTime = 0f;
    public bool zoomout = false;
    
    private string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        DisableExtraAudioListeners();
    }
    
    void Update()
    {
        if (!isZooming && Input.GetKeyDown(KeyCode.Space))
        {
            isZooming = true;
            
            startPosition = gameObject.transform.position;
            startRotation = gameObject.transform.rotation;

            targetRotation = Quaternion.Euler(targetCameraRotation);
        }

        if (isZooming && !transitionTriggered)
        {
            elapsedTime += Time.deltaTime;
            
            float t = Mathf.Clamp01(elapsedTime / animationDuration);
            float easedT = EaseInOutQuint(t);

            gameObject.transform.position = Vector3.Lerp(startPosition, targetCameraPosition, easedT);
            gameObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, easedT);

            if (easedT >= 1)
            {
                transitionTriggered = true;
                Debug.Log("Camera animation finished. Loading new scene...");
                LoadChapterScene();
            }
        }

        if (zoomout)
        {
            elapsedTime += Time.deltaTime;
            
            float t = Mathf.Clamp01(elapsedTime / animationDuration);
            float easedT = EaseInOutQuint(t);

            gameObject.transform.position = Vector3.Lerp(targetCameraPosition, targetCameraZoomoutPosition, easedT);
            //gameObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, easedT);

            if (easedT >= 1)
            {
                zoomout = false;
                Debug.Log("Camera animation finished. Loading new scene...");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //animator.SetBool("flip", true);
                }
            }
        }
        
    }


    private float EaseInOutQuint(float x)
    {
        return x < 0.5f ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }

    public void ZoomOut()
    {
        targetCameraZoomoutPosition = gameObject.transform.position;
        targetCameraPosition = levelCameraTransform.position;
        this.gameObject.transform.position = levelCameraTransform.position;
        this.zoomout = true;
    }
    
    private void LoadChapterScene()
    {
        Scene loadedScene = SceneManager.GetSceneByName(chapterSceneName);
        if (!loadedScene.IsValid() || !loadedScene.isLoaded)
        {
            Debug.LogError($"{chapterSceneName} is not loaded! You must preload it.");
            return;
        }

        SceneManager.SetActiveScene(loadedScene);
        Debug.Log($"Scene {chapterSceneName} is now active.");

        StartCoroutine(SwitchCameraAndUnloadPrevious());
    }


    private IEnumerator SwitchCameraAndUnloadPrevious()
    {
        yield return new WaitForEndOfFrame();

        // Switch to new camera
        GameObject newCamObj = GameObject.FindGameObjectWithTag(levelCameraTag);
        if (newCamObj == null)
        {
            Debug.LogError($"No camera with tag {levelCameraTag} found in the new scene.");
            yield break;
        }

        Camera newCamera = newCamObj.GetComponent<Camera>();
        if (newCamera == null)
        {
            Debug.LogError("Found tagged GameObject, but it has no Camera component.");
            yield break;
        }

        // Disable current camera (this one)
        Camera currentCam = GetComponent<Camera>();
        if (currentCam != null)
        {
            currentCam.enabled = false;
            var listener = currentCam.GetComponent<AudioListener>();
            if (listener != null) listener.enabled = false;
        }

        if (newCamera.targetTexture != null)
        {
            Debug.Log($"Clearing targetTexture on new camera: {newCamera.targetTexture.name}");
            newCamera.targetTexture = null;
        }
        
        // Enable new camera
        newCamera.enabled = true;
        var newListener = newCamera.GetComponent<AudioListener>();
        if (newListener != null) newListener.enabled = true;

        Debug.Log("Switched to new level camera.");

        // Unload previous scene
        if (currentSceneName != chapterSceneName)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentSceneName);
            if (unloadOp == null)
            {
                Debug.LogError($"Failed to start unloading {currentSceneName}.");
                yield break;
            }

            unloadOp.completed += (_) =>
            {
                Debug.Log($"{currentSceneName} successfully unloaded.");
            };
        }
    }

    private void DisableExtraAudioListeners()
    {
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        foreach (AudioListener listener in listeners)
        {
            if (listener != GetComponent<AudioListener>())
            {
                listener.enabled = false;
            }
        }
    }
    
}