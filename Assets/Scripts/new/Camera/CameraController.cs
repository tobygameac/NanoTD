using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

  public float speed;

  public float scrollingSpeed;
  public float maxCameraSize;
  public float minCameraSize;

  public int maxEdgeDistanceToMove = 5;
  public float edgeMovingModifier = 0.5f;

  public Vector2 minBoundPostion;
  public Vector2 maxBoundPostion;

  void Update () {
    
    float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
    float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * speed;
    
    // Moving when mouse is close to the edge of the screen
    
    if (Input.mousePosition.x <= maxEdgeDistanceToMove) {
      deltaX = -edgeMovingModifier * speed * Time.deltaTime;
    }

    if (Input.mousePosition.y <= maxEdgeDistanceToMove) {
      deltaY = -edgeMovingModifier * speed * Time.deltaTime;
    }

    if (Input.mousePosition.x >= Screen.width - maxEdgeDistanceToMove) {
      deltaX = edgeMovingModifier * speed * Time.deltaTime;
    }

    if (Input.mousePosition.y >= Screen.height - maxEdgeDistanceToMove) {
      deltaY = edgeMovingModifier * speed * Time.deltaTime;
    }

    // Boundary

    Vector3 newPosition = transform.position + new Vector3(deltaX, deltaY, 0);

    newPosition.x = Mathf.Max(newPosition.x, minBoundPostion.x);
    newPosition.y = Mathf.Max(newPosition.y, minBoundPostion.y);

    newPosition.x = Mathf.Min(newPosition.x, maxBoundPostion.x);
    newPosition.y = Mathf.Min(newPosition.y, maxBoundPostion.y);

    Camera.main.transform.Translate(newPosition - transform.position);

    // Scroll

    if (Input.GetAxis("Mouse ScrollWheel") != 0) {
      Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollingSpeed;
    }

    Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, minCameraSize);
    Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize, maxCameraSize);

  }

}
