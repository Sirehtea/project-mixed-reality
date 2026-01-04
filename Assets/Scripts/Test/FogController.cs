using UnityEngine;
using System.Collections.Generic;

public class FogController : MonoBehaviour
{
    public static FogController Instance;

    [Header("Fog Values")]
    public float defaultFog = 0.1f;
    public float fadeSpeed = 2f;

    private readonly Dictionary<object, float> sources = new();

    private float targetFog;

    private void Awake()
    {
        Instance = this;
        RenderSettings.fog = true;
        targetFog = defaultFog;
        RenderSettings.fogDensity = defaultFog;

        Debug.Log($"[FOG] Awake — defaultFog={defaultFog}");
    }

    private void Start()
    {
        targetFog = defaultFog;
        RenderSettings.fog = true;
        RenderSettings.fogDensity = defaultFog;

        Debug.Log($"[FOG] Start — fogDensity forced to {RenderSettings.fogDensity}");
    }

    private void Update()
    {
        RenderSettings.fogDensity =
            Mathf.Lerp(RenderSettings.fogDensity, targetFog, fadeSpeed * Time.deltaTime);
    }

    public void RequestFog(object source, float fogValue)
    {
        sources[source] = fogValue;
        Debug.Log($"[FOG] Request FROM {source} — value={fogValue}");

        RecomputeFog();
    }

    public void ReleaseFog(object source)
    {
        if (sources.ContainsKey(source))
        {
            sources.Remove(source);
            Debug.Log($"[FOG] Release FROM {source}");
        }
        else
        {
            Debug.LogWarning($"[FOG] Release called BUT SOURCE NOT FOUND: {source}");
        }

        RecomputeFog();
    }

    private void RecomputeFog()
    {
        if (sources.Count == 0)
        {
            targetFog = defaultFog;
            Debug.Log($"[FOG] No sources — reverting to default={defaultFog}");
            return;
        }

        float max = 0f;
        foreach (var kv in sources)
            if (kv.Value > max) max = kv.Value;

        targetFog = max;

        Debug.Log($"[FOG] Sources={sources.Count}  TargetFog={targetFog}");
    }
}
