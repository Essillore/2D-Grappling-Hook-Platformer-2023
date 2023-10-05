using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MovingWaveColorCurve : MonoBehaviour
{
    // Reference to the URP Volume component on your camera
    public Volume postProcessingVolume;

    private ColorCurves colorCurves;
    private TextureCurve movingWaveCurve;
    private AnimationCurve waveCurve;
    private float waveSpeed = 0.5f;
    private float waveAmplitude = 1.5f;

    private void Start()
    {
        // Check if the postProcessingVolume is assigned
        if (postProcessingVolume == null)
        {
            Debug.LogError("Post Processing Volume is not assigned!");
            return;
        }

        // Try to get the ColorCurves effect from the post-processing profile
        if (postProcessingVolume.profile.TryGet(out ColorCurves cc))
        {
            colorCurves = cc;
        }
        else
        {
            Debug.LogError("Color Curves effect not found in the post-processing profile.");
            return;
        }

        // Initialize the wave curve
        InitializeWaveCurve();

        // Start the wave movement
        StartCoroutine(MoveWave());
    }

    // Initialize the wave curve
    private void InitializeWaveCurve()
    {
        waveCurve = new AnimationCurve();
        // waveCurve.AddKey(new Keyframe(0.0f, 0.0f));
        // waveCurve.AddKey(new Keyframe(1.0f, 0.0f));

        // Adjust the keyframes to emphasize blue values
        waveCurve.AddKey(new Keyframe(0.0f, 0.0f));
        waveCurve.AddKey(new Keyframe(0.141f, 0.710f));         // Transition from black to blue
        waveCurve.AddKey(new Keyframe(0.355f, 0.240f));          // Blue
        waveCurve.AddKey(new Keyframe(0.609f, 0.683f));         // Transition from blue to black
        waveCurve.AddKey(new Keyframe(0.801f, 0.240f));          // End with black

    }

    // Coroutine to move the wave over time
    private IEnumerator MoveWave()
    {
        float time = 0.0f;

        while (true)
        {
            // Update the curve's keyframes to create the moving wave
            for (int i = 0; i < waveCurve.length; i++)
            {
                Keyframe key = waveCurve[i];
                key.value = Mathf.Sin((key.time + time) * waveSpeed) * waveAmplitude;
                waveCurve.MoveKey(i, key);
            }

            Vector2 bounds = new Vector2(0.0f, 1.0f);
            movingWaveCurve = new TextureCurve(waveCurve, 0.5f, true, in bounds);


            // Assign the updated curve to the blue channel
            colorCurves.blue.value = movingWaveCurve;

            // Increment time for the next frame
            time += Time.deltaTime;

            yield return null;
        }
    }
}