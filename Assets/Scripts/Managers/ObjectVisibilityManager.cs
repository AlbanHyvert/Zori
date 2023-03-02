using UnityEngine;
using System.Collections.Generic;

public class ObjectVisibilityManager : Singleton<ObjectVisibilityManager>
{
    private List<Renderer> _rendererList = new List<Renderer>();
    private Camera _playerCamera = null;

    public void AddRenderer(Renderer renderer)
    => _rendererList.Add(renderer);

    public void RemoveRenderer(Renderer renderer)
    => _rendererList.Remove(renderer);

    public void SetCamera(Camera cam)
    {
        _playerCamera = cam;

        GameManager.Instance.OnUpdatePlayer += Tick;
    }

    private void Tick(float time)
    {
        foreach (var renderer in _rendererList)
        {
            if(renderer == null) return;

            if (!IsVisible(renderer))
            {
                renderer.gameObject.SetActive(false);
            }

            if(IsVisible(renderer))
                renderer.gameObject.SetActive(true);

            else
            {
                /*float distance = Vector3.Distance(renderer.transform.position, mainCamera.transform.position);
                LODGroup lodGroup = renderer.GetComponent<LODGroup>();
                if (lodGroup != null)
                {
                    LOD[] lods = lodGroup.GetLODs();
                    for (int i = 0; i < lods.Length; i++)
                    {
                        if (distance < lods[i].screenRelativeTransitionHeight * distanceThreshold)
                        {
                            lodGroup.ForceLOD(i);
                            break;
                        }
                    }
                }*/
            }
        }
    }
    
    bool IsVisible(Renderer renderer)
    {
        // Use occlusion culling to determine visibility
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(_playerCamera), renderer.bounds);
    }
}
