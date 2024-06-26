using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 玩家 Transform
    public Vector3 offset;   // 摄像机偏移量
    public Vector2 minPosition; // 最小摄像机位置
    public Vector2 maxPosition; // 最大摄像机位置
    public float edgeOffset = 1f; // 边缘偏移量

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;

            // 限制摄像机位置
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x + Camera.main.orthographicSize * Camera.main.aspect, maxPosition.x - Camera.main.orthographicSize * Camera.main.aspect);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y + Camera.main.orthographicSize, maxPosition.y - Camera.main.orthographicSize);

            transform.position = targetPosition;
        }
    }

    public void ResetCameraPosition()
    {
        if (player != null)
        {
            Vector3 resetPosition = player.position + offset;
            resetPosition.x = Mathf.Clamp(resetPosition.x, minPosition.x + Camera.main.orthographicSize * Camera.main.aspect, maxPosition.x - Camera.main.orthographicSize * Camera.main.aspect);
            resetPosition.y = Mathf.Clamp(resetPosition.y, minPosition.y + Camera.main.orthographicSize, maxPosition.y - Camera.main.orthographicSize);

            transform.position = resetPosition;
        }
    }
}

