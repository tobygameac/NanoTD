using UnityEngine;
using System.Collections;

public class RangeDisplayer : MonoBehaviour {

  public GameObject imageToRotate;

  public float rotatingSpeed = 90;

  private SphereCollider sphereCollider;

  private Transform transformToDisplayRange;

  void Start() {
    transformToDisplayRange = transform;
    while (transformToDisplayRange.parent != null) {
      transformToDisplayRange = transform.parent;
    }
    sphereCollider = transformToDisplayRange.GetComponent<SphereCollider>();
  }

  void Update() {
    transform.localScale = Vector3.one * sphereCollider.radius;
    imageToRotate.transform.Rotate(Vector3.up * Time.deltaTime * rotatingSpeed, Space.World);
  }

}
