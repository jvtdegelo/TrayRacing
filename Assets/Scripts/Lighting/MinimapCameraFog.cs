using UnityEngine;
using UnityEngine.Rendering;

public class MinimapCameraFog : MonoBehaviour
{
    public bool IsThereFogInScene;

    private void Start()
    {
        IsThereFogInScene = RenderSettings.fog;
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
        if (camera.TryGetComponent<MinimapCameraFog>(out _))
            RenderSettings.fog = false;
        else
            RenderSettings.fog = IsThereFogInScene;

    }
    void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        RenderSettings.fog = IsThereFogInScene;
    }
}