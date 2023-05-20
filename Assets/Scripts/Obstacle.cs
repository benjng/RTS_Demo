using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{   
    private bool isBlockingRay = false;
    private bool hasCompletedReplacing = false;
    private MeshRenderer meshRenderer;
    private List<Material> originalMaterials = new List<Material>();
    private int numOfMaterials;
    // public Material transparentMaterial;

    public bool IsBlockingRay { get => isBlockingRay; set => isBlockingRay = value; }

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        numOfMaterials = meshRenderer.materials.Length;
        foreach (Material material in meshRenderer.materials){
            originalMaterials.Add(material);
        }
    }

    void Update(){
        if (isBlockingRay && !hasCompletedReplacing){
            for (int i = 0; i < numOfMaterials; i++){
                // Debug.Log("Updating materials: " + meshRenderer.materials[i]);

                // meshRenderer.materials[i].
                meshRenderer.materials[i].SetOverrideTag("RenderType", "Transparent");
                meshRenderer.materials[i].SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                meshRenderer.materials[i].SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                meshRenderer.materials[i].SetFloat("_ZWrite", 0.0f);
                meshRenderer.materials[i].DisableKeyword("_ALPHATEST_ON");
                meshRenderer.materials[i].DisableKeyword("_ALPHABLEND_ON");
                meshRenderer.materials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
                meshRenderer.materials[i].renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;

                meshRenderer.materials[i].color = new Color(meshRenderer.materials[i].color.r, meshRenderer.materials[i].color.g, meshRenderer.materials[i].color.b, 0.3f);
            }
            hasCompletedReplacing = true;
        } 
        if (!isBlockingRay){
            for (int i = 0; i < numOfMaterials; i++){
                meshRenderer.materials[i].SetOverrideTag("RenderType", "");
                meshRenderer.materials[i].SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
                meshRenderer.materials[i].SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
                meshRenderer.materials[i].SetInt("_ZWrite", 1);
                meshRenderer.materials[i].DisableKeyword("_ALPHATEST_ON");
                meshRenderer.materials[i].DisableKeyword("_ALPHABLEND_ON");
                meshRenderer.materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                meshRenderer.materials[i].renderQueue = -1;
            }
            hasCompletedReplacing = false;
        }
    }
}
