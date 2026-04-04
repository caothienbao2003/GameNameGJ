using UnityEngine;
using TMPro; // Remove this if using standard UI Text
using System.Text;

public class PerformanceUIDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI statsText; // If using standard Text, use: public UnityEngine.UI.Text statsText;

    private float deltaTime = 0.0f;
    private StringBuilder sb = new StringBuilder();

    void Update()
    {
        // Calculate frame time
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Convert to metrics
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        long heapSize = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() / (1024 * 1024);

        // Update the UI Text
        sb.Clear();
        sb.AppendLine($"{Mathf.Ceil(fps)} FPS");
        sb.AppendLine($"{msec:0.0} ms");
        sb.AppendLine($"{heapSize} MB Heap");

        if (statsText != null)
        {
            statsText.text = sb.ToString();
        }
    }
}