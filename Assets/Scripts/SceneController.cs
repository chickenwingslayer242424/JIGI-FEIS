using UnityEngine;

public class SceneActivationHandler : MonoBehaviour
{
    public CameraFollow cameraFollow;

    public Vector2 scene1MinBounds = new Vector2(0f, 0f);
    public Vector2 scene1MaxBounds = new Vector2(35.1f, 10f);

    public Vector2 scene3MinBounds = new Vector2(0f, 0f);
    public Vector2 scene3MaxBounds = new Vector2(27.8f, 10f);

    private void OnEnable()
    {
        string sceneName = gameObject.name;

        switch (sceneName)
        {
            case "Scene1":
                cameraFollow.SetSceneBounds(scene1MinBounds, scene1MaxBounds);
                break;
            case "Scene3":
                cameraFollow.SetSceneBounds(scene3MinBounds, scene3MaxBounds);
                break;
            default:
                Debug.LogWarning("Scene bounds not set for this scene: " + sceneName);
                break;
        }

        cameraFollow.ResetCameraPosition();
    }
}
