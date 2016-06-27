using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LineVisualizerScript : MonoBehaviour {

    private float[] samples = new float[256];   //power of 2
    private LineRenderer lineRenderer;
    private float stepSize;

    public FFTWindow fftWindow;
    public float size = 10.0f;
    public float amplitude = 1.0f;
    public int CutOffSample = 32;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(CutOffSample);
        stepSize = size / CutOffSample;
    }

    void Update() {
        AudioListener.GetSpectrumData(samples, 0, fftWindow);

        for (int i = 0; i < CutOffSample; i++) {
            Vector3 position = new Vector3(i * stepSize - size/2.0f, samples[i] * amplitude, 0.0f);
            lineRenderer.SetPosition(i, position);
        }
    }
}
