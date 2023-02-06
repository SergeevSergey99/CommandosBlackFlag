using Micosmo.SensorToolkit;
using UnityEngine;

public class ConeOfSightRenderer : MonoBehaviour
{
    private static readonly int sViewDepthTexturedID = Shader.PropertyToID("_ViewDepthTexture");
    private static readonly int sViewSpaceMatrixID = Shader.PropertyToID("_ViewSpaceMatrix");

    public Camera ViewCamera;
    public float ViewDistance;
    public float ViewDistance2;
    public float ViewAngle;
    private Material mMaterial;
    
    private void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        mMaterial = renderer.material;  // This generates a copy of the material
        renderer.material = mMaterial;

        RenderTexture depthTexture = new RenderTexture(ViewCamera.pixelWidth, ViewCamera.pixelHeight, 24, RenderTextureFormat.Depth);
        ViewCamera.targetTexture = depthTexture;


        mMaterial.SetTexture(sViewDepthTexturedID, ViewCamera.targetTexture);
        UpdateParameters();
    }

    void UpdateParameters()
    {
        ViewCamera.farClipPlane = ViewDistance + ViewDistance2;
        ViewCamera.fieldOfView = ViewAngle;
        transform.localScale = new Vector3((ViewDistance + ViewDistance2) * 2, transform.localScale.y, (ViewDistance + ViewDistance2) * 2);
        mMaterial.SetFloat("_ViewAngle", ViewAngle);
        mMaterial.SetFloat("_ViewDistance", 0.5f * ViewDistance/(ViewDistance + ViewDistance2));
        ViewCamera.GetComponent<LOSSensor>().MaxHorizAngle = ViewAngle / 2;
        ViewCamera.GetComponent<LOSSensor>().MaxDistance = ViewDistance + ViewDistance2;
    }
    private void LateUpdate()
    {
#if UNITY_EDITOR
        UpdateParameters();
#endif
        ViewCamera.Render();
        mMaterial.SetMatrix(sViewSpaceMatrixID, ViewCamera.projectionMatrix * ViewCamera.worldToCameraMatrix);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1f, 0f, 1f));
        Gizmos.DrawWireSphere(Vector3.zero, ViewDistance);
        Gizmos.DrawWireSphere(Vector3.zero, ViewDistance + ViewDistance2);
        Gizmos.matrix = Matrix4x4.identity;
    }

#endif
}