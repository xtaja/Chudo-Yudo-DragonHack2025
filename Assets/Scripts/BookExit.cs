using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCameraOut : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private string chapterSceneName = "Environment";
    [SerializeField] private string transferSceneName = "MainMenu";
    private string levelCameraTag = "LevelCamera";

    [SerializeField] private Vector3 targetCameraPosition;
    [SerializeField] private Vector3 targetCameraRotation; 
    
    [SerializeField] private float animationDuration = 3f;
    
    [SerializeField] private GameObject freeLookCamera;
    
    private bool isZooming = false;
    private bool transitionTriggered = false;
    
    Vector3 startPosition;
    Quaternion startRotation;
    Quaternion targetRotation;

    float elapsedTime = 0f;
    
    private string currentSceneName;
    
    void Update()
    {
        bool isInActiveScene = gameObject.scene == SceneManager.GetActiveScene();
        if (!isZooming && Input.GetKeyDown(KeyCode.Z) && isInActiveScene)
        {
            freeLookCamera.gameObject.SetActive(false);
            isZooming = true;
            
            startPosition = gameObject.transform.position;
            startRotation = gameObject.transform.rotation;
            
            targetCameraPosition = transform.position - transform.forward * 3f;


            targetRotation = Quaternion.Euler(targetCameraRotation);
            
            Debug.Log("Going back");
        }

        if (isZooming && !transitionTriggered)
        {
            transitionTriggered = true;
            StartCoroutine(HandleSceneTransition()); 
        }
    }
    
    private float EaseInOutQuint(float x)
    {
        return x < 0.5f ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }
    
    private IEnumerator HandleSceneTransition()
    {
        
        // Load Transfer scene additively
        AsyncOperation loadTransfer = SceneManager.LoadSceneAsync(transferSceneName, LoadSceneMode.Additive);
        while (!loadTransfer.isDone)
            yield return null;
        
        Camera chapterCam = GameObject.FindWithTag("LevelCamera").GetComponent<Camera>();
        if (chapterCam != null)
        {
            Debug.LogError("No chapter camera found");
            chapterCam.targetTexture = renderTexture;
            chapterCam.tag = "Untagged";       
            chapterCam.enabled = true;    
            yield return new WaitForEndOfFrame(); 
            chapterCam.targetTexture = null;
            chapterCam.enabled = false;
        }
        
        // Set Transfer scene as active
        Scene transferScene = SceneManager.GetSceneByName(transferSceneName);
        SceneManager.SetActiveScene(transferScene);

        // Find and configure the Transfer camera
        Camera transferCam = null;
        foreach (GameObject root in transferScene.GetRootGameObjects())
        {
            transferCam = root.GetComponentInChildren<Camera>();
            if (transferCam != null)
            {
                transferCam.tag = "MainCamera";   // Set as main camera
                transferCam.targetTexture = null; // Render to screen
                transferCam.enabled = true;
                break;
            }
        }
        
        
        if (transferCam == null)
        {
            Debug.LogError("No camera found in Transfer scene!");
        }
        
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("Environment");
        if (unloadOp == null)
        {
            yield break;
        }
        
        transferCam.GetComponent<MoveCameraIntoChapter>().ZoomOut();
        GameObject book = GameObject.FindWithTag("Book");
        var bookScript = book.GetComponent<OpenOnKeyPress>();
        bookScript.openBook();
    }
    
}
