using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public sealed class DandelionBuilder : MonoBehaviour
{
    public DandelionSettings settings = new DandelionSettings();

    private Mesh mesh;

    private void OnEnable()
    {
        Rebuild();
    }

    private void OnValidate()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        if (mesh == null)
        {
            mesh = new Mesh { name = "Dandelion", hideFlags = HideFlags.DontSave };
        }

        MeshBuffer buffer = new MeshBuffer();
        DandelionGeometry.Build(buffer, settings, DandelionPalette.Meadow);
        buffer.WriteTo(mesh);
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
