using UnityEngine;

[ExecuteInEditMode]
public class TextureScaling : MonoBehaviour
{
    private Vector3 _currentScale;

    public float size = 1f;

    private Renderer _renderer;
    private MeshFilter _meshFilter;

    private void Start()
    {
        _currentScale = transform.localScale;
        _renderer = GetComponent<Renderer>();
        _meshFilter = GetComponent<MeshFilter>();
        Calculate();
        _currentScale = Vector3.zero; // Forces Calculate to run no matter what
    }

    private void Update()
    {
        Calculate();
    }

    public void Calculate()
    {
        if (_currentScale != transform.localScale)
        {
            _currentScale = transform.localScale;

            Mesh mesh = GetMesh();

            if (mesh.uv.Length != 24)
            {
                Debug.LogWarning("UV remapping only supported on Unity's default cube (24 vertices).");
                return;
            }

            Mesh newMesh = Instantiate(mesh);
            newMesh.uv = SetupUvMap(newMesh.uv);
            newMesh.name = "Cube Instance";
            _meshFilter.sharedMesh = newMesh;

            if (_renderer.sharedMaterial.mainTexture.wrapMode != TextureWrapMode.Repeat)
            {
                _renderer.sharedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
            }
        }
    }

    private Vector2[] SetupUvMap(Vector2[] meshUVs)
    {
        float x = _currentScale.x * size;
        float y = _currentScale.y * size;
        float z = _currentScale.z * size;

        meshUVs[0] = new Vector2(0f, 0f);
        meshUVs[1] = new Vector2(x, 0f);
        meshUVs[2] = new Vector2(0f, y);
        meshUVs[3] = new Vector2(x, y);

        meshUVs[4] = new Vector2(x, 0f);
        meshUVs[5] = new Vector2(0f, 0f);
        meshUVs[8] = new Vector2(x, z);
        meshUVs[9] = new Vector2(0f, z);

        meshUVs[7] = new Vector2(0f, 0f);
        meshUVs[6] = new Vector2(x, 0f);
        meshUVs[11] = new Vector2(0f, y);
        meshUVs[10] = new Vector2(x, y);

        meshUVs[12] = new Vector2(x, z);
        meshUVs[13] = new Vector2(x, 0f);
        meshUVs[14] = new Vector2(0f, 0f);
        meshUVs[15] = new Vector2(0f, z);

        meshUVs[16] = new Vector2(0f, 0f);
        meshUVs[17] = new Vector2(0f, y);
        meshUVs[18] = new Vector2(z, y);
        meshUVs[19] = new Vector2(z, 0f);

        meshUVs[20] = new Vector2(0f, 0f);
        meshUVs[21] = new Vector2(0f, y);
        meshUVs[22] = new Vector2(z, y);
        meshUVs[23] = new Vector2(z, 0f);

        return meshUVs;
    }
    private Mesh GetMesh()
    {
#if UNITY_EDITOR
    if (!Application.isPlaying)
        return GetComponent<MeshFilter>().sharedMesh;
#endif
        return GetComponent<MeshFilter>().mesh;
    }

}
