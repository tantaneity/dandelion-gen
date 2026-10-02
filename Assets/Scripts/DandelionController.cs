using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public sealed class DandelionController : MonoBehaviour
{
    public DandelionBuilder builder;

    public float orbitRadius = 2.4f;
    public float focusHeight = 0.3f;
    public float yaw = 20.0f;
    public float pitch = 8.0f;

    private void OnEnable()
    {
        PlaceCamera();
    }

    private void OnValidate()
    {
        PlaceCamera();
    }

    public void PlaceCamera()
    {
        Camera view = GetComponent<Camera>();
        view.fieldOfView = DandelionCamera.FieldOfView;
        DandelionCamera.Place(view, yaw, pitch, orbitRadius);
        view.transform.position += Vector3.up * focusHeight;
    }
}
