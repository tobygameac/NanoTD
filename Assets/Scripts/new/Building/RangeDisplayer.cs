using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SphereCollider))]
public class RangeDisplayer : MonoBehaviour {

  public Texture2D rangeDisplayTexture;
  public Texture2D arcTexture;

  public float rotatingSpeed = 90;
  private float angle;

  private SphereCollider sphereCollider;

  private Vector3 boundAdjuster;

  void Start() {
    sphereCollider = GetComponent<SphereCollider>();
    boundAdjuster = new Vector3(transform.localScale.x, 0, transform.localScale.z);
    angle = 0;
  }

  void Update() {
    angle += Time.deltaTime * rotatingSpeed;
    if (angle >= 360) {
      angle -= 360;
    }
  }

  void OnGUI() {
    Vector2 positionOnScreenLB = Camera.main.WorldToScreenPoint(Vector3.Scale(sphereCollider.bounds.min - boundAdjuster, new Vector3(1, 0, 1)));
    Vector2 positionOnScreenRU = Camera.main.WorldToScreenPoint(Vector3.Scale(sphereCollider.bounds.max + boundAdjuster, new Vector3(1, 0, 1)));
    Vector2 differenceOnScreen = positionOnScreenRU - positionOnScreenLB;
    
    Rect drawingArea = new Rect(positionOnScreenLB.x, Screen.height - positionOnScreenRU.y, differenceOnScreen.x, differenceOnScreen.y);
    
    GUI.color = new Color(1, 1, 1, 0.25f);
    GUI.DrawTexture(drawingArea, rangeDisplayTexture, ScaleMode.StretchToFill, true, 10.0f);

    Vector2 pivotPoint = new Vector2(positionOnScreenLB.x + differenceOnScreen.x / 2, Screen.height - positionOnScreenRU.y + differenceOnScreen.y / 2);
    GUIUtility.RotateAroundPivot(angle, pivotPoint);

    GUI.DrawTexture(drawingArea, arcTexture, ScaleMode.StretchToFill, true, 10.0f);
  }
}
