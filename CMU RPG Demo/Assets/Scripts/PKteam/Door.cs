using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public CanvasGroup overlay;
    public float transitionSpeed = 1f;
    public string sceneToLoad; // Scene name to load
    private bool warping = false;

    private void Awake()
    {
        overlay.alpha = 0;
    }

    void Update()
    {
        if (DoorInteractor.hitDoor || warping)
        {
            warping = true;
            overlay.alpha += Time.deltaTime * transitionSpeed;

            if (overlay.alpha >= 1)
            {
                SceneManager.LoadScene(sceneToLoad); // Load the specified scene
            }
        }
    }
}
