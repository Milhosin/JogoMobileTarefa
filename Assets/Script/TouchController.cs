using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    [Header("Movimentação Lateral")]
    public float sensitivity = 0.5f;
    public float smoothness = 15f;

    [Header("Limites da Pista")]
    public float minX = -4f;
    public float maxX = 4f;

    private Vector2 pastPosition;
    private float targetX;

    void Start()
    {
        targetX = transform.position.x;
    }

    void Update()
    {
        if (PlayerController.Instance != null && !PlayerController.Instance.CanRun)
        {
            pastPosition = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            pastPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            float deltaX = Input.mousePosition.x - pastPosition.x;
            targetX += deltaX * sensitivity * 0.1f;
            targetX = Mathf.Clamp(targetX, minX, maxX);
        }

        pastPosition = Input.mousePosition;

        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Lerp(currentPosition.x, targetX, smoothness * Time.deltaTime);
        transform.position = currentPosition;
    }
}
