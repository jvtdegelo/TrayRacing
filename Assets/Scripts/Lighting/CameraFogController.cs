using UnityEngine;
using UnityEngine.Rendering;

public class CameraFogController : MonoBehaviour
{
    private bool IsThereFogInScene;

    private void Start()
    {
        IsThereFogInScene = RenderSettings.fog;
        Debug.Log("fog in scene: " + IsThereFogInScene);
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }
    void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.beginCameraRendering -= OnEndCameraRendering;
    }

    void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera.TryGetComponent<NoFogCamera>(out _))
        {
            Debug.Log(camera.gameObject.name + " is NoFogCamera");
            RenderSettings.fog = false;
        }
        else
        {
            Debug.Log(camera.gameObject.name + " is not NoFogCamera");
            RenderSettings.fog = IsThereFogInScene;
        }
    }
    void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        RenderSettings.fog = IsThereFogInScene;
    }
}