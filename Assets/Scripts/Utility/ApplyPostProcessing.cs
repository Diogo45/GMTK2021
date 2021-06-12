using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPostProcessing : MonoBehaviour
{

    public Material PostProcessingMaterial;

    protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, PostProcessingMaterial);
    }
}
