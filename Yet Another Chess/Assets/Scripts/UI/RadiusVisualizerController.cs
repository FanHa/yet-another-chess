using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class RadiusVisualizerController : MonoBehaviour
{
    readonly List<GameObject> m_RadiusVisualizers = new List<GameObject>();
    public GameObject radiusVisualizerPrefab;
    public float radiusVisualizerHeight = 0.02f;
    public Vector3 localEuler;


    public void SetupRadiusVisualizers(Token token, Transform ghost = null)
    {
        // Create necessary affector radius visualizations
        List<ITokenRadiusProvider> providers =
            token.GetRadiusVisualizers();

        int length = providers.Count;
        for (int i = 0; i < length; i++)
        {
            if (m_RadiusVisualizers.Count < i + 1)
            {
                m_RadiusVisualizers.Add(Instantiate(radiusVisualizerPrefab));
            }

            ITokenRadiusProvider provider = providers[i];

            GameObject radiusVisualizer = m_RadiusVisualizers[i];
            radiusVisualizer.SetActive(true);
            radiusVisualizer.transform.SetParent(ghost == null ? token.transform : ghost);
            radiusVisualizer.transform.localPosition = new Vector3(0, radiusVisualizerHeight, 0);
            radiusVisualizer.transform.localScale = Vector3.one * provider.effectRadius * 2.0f;
            radiusVisualizer.transform.localRotation = new Quaternion { eulerAngles = localEuler };

            var visualizerRenderer = radiusVisualizer.GetComponent<Renderer>();
            if (visualizerRenderer != null)
            {
                visualizerRenderer.material.color = provider.effectColor;
            }
        }
    }
}
