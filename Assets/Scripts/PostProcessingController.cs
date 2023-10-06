using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PostProcessingController : MonoBehaviour
{
    // Reference to the URP Volume component on your camera
    public Volume postProcessingVolume;

    private ColorAdjustments colorAdjustments;
    private ColorCurves colorCurves;
    private AnimationCurve blueCurve;
    private TextureCurve blueTextureCurve;

    bool loop = true;
    public Vector2 bounds = new Vector2(0.0f, 1.0f);
    public TextureCurve masterTextureCurve;
    public AnimationCurve masterCurve;
    public AnimationCurve luminosityBonfireCurve;
    public TextureCurve lumVsSatCurve;


    public Image iceOverLay;
    private Color targetColor; // The target color with the desired alpha.
    public float newAlpha;

    void Start()
    {

        iceOverLay = GameObject.Find("IceOver").GetComponent<Image>();
        targetColor = iceOverLay.color; // Store the initial color
        // Check if the postProcessingVolume is assigned
        if (postProcessingVolume == null)
        {
            Debug.LogError("Post Processing Volume is not assigned!");
            return;
        }

        // Try to get the ColorAdjustments effect from the post-processing profile
        if (postProcessingVolume.profile.TryGet(out ColorAdjustments ca))
        {
            colorAdjustments = ca;
        }
        else
        {
            Debug.LogError("Color Adjustments effect not found in the post-processing profile.");
        }

        if (postProcessingVolume.profile.TryGet(out ColorCurves cc))
        {
            colorCurves = cc;
        }
        else
        {
            Debug.LogError("Color Adjustments effect not found in the post-processing profile.");
        }

        MasterCurveAdjustment();

    }
    public void Update()
    {
        // Update the color with the new alpha value.
        targetColor.a = newAlpha;
        iceOverLay.color = targetColor;
    }

    public void BlueCurveAdjustment()
    {
        newAlpha = 0.0125f;
        // Set the Color Curves for the blue channel
        if (colorCurves != null)
        {
            // Create a new curve for the blue channel
            blueCurve = new AnimationCurve(
                new Keyframe(0.0f, 0.0f),
                new Keyframe(0.239f, 0.532f),
                new Keyframe(0.651f, 0.290f),
                new Keyframe(1.0f, 1.0f)

            );

            blueTextureCurve = new TextureCurve(blueCurve, 0.5f, loop, in bounds);

            // Set the blue channel curve
            colorCurves.blue.value = blueTextureCurve;

            float evaluationPoint = 0.239f; // Change this to your desired point
            float blueCurveValue = colorCurves.blue.value.Evaluate(evaluationPoint);


            // Print the evaluated value to the console
            Debug.Log("Blue Channel Curve Value at " + evaluationPoint + ": " + blueCurveValue);
        }
    }

    public void BlueAdjustmentUnder30()
    {
        newAlpha = 0.0075f;
        // Set the Color Curves for the blue channel
        if (colorCurves != null)
        {
            // Create a new curve for the blue channel
            blueCurve = new AnimationCurve(
                new Keyframe(0.0f, 0.0f),
                new Keyframe(1.0f, 1.0f)

            );

            blueTextureCurve = new TextureCurve(blueCurve, 0.5f, loop, in bounds);

            // Set the blue channel curve
            colorCurves.blue.value = blueTextureCurve;
        }
    }


    public void BlueAdjustmentUnder10()
    {
        newAlpha = 0.038f;
        // Set the Color Curves for the blue channel
        if (colorCurves != null)
        {
            // Create a new curve for the blue channel
            blueCurve = new AnimationCurve(
                new Keyframe(0.0f, 0.0f),
                new Keyframe(0.131f, 0.532f),
                new Keyframe(0.351f, 0.390f),
                new Keyframe(1.0f, 1.0f)

            );

            blueTextureCurve = new TextureCurve(blueCurve, 0.5f, loop, in bounds);

            // Set the blue channel curve
            colorCurves.blue.value = blueTextureCurve;
        }
    }


    public void BlueAdjustmentUnder5()
    {
        newAlpha = 0.076f;
        // Set the Color Curves for the blue channel
        if (colorCurves != null)
        {
            // Create a new curve for the blue channel
            blueCurve = new AnimationCurve(
                new Keyframe(0.0f, 0.0f),
                new Keyframe(0.086f, 0.797f),
                new Keyframe(0.250f, 0.283f),
                new Keyframe(1.0f, 1.0f)

            );

            blueTextureCurve = new TextureCurve(blueCurve, 0.5f, loop, in bounds);

            // Set the blue channel curve
            colorCurves.blue.value = blueTextureCurve;
        }
    }

    public void MasterCurveAdjustment()
    {
        if (colorCurves != null)
        {
            // Create a new curve for
            masterCurve = new AnimationCurve(
                new Keyframe(0.0f, 0.0f),
                new Keyframe(0.325f, 0.305f),
                new Keyframe(0.6f, 0.716f),
                new Keyframe(1.0f, 1.0f)
            );
            //Convert animation curve to a texturecurve
            masterTextureCurve = new TextureCurve(masterCurve, 0.5f, !loop, in bounds);

            //set the curve
            colorCurves.master.value = masterTextureCurve;
        }
    }

    public void LuminosityAtBonfire()
    {
        if (colorCurves != null)
        {
            // Create a new curve for
            luminosityBonfireCurve = new AnimationCurve(
                new Keyframe(0.0f, 0.0f),
                new Keyframe(0.146f, 0.809f),
                new Keyframe(0.298f, 0.1f),
                new Keyframe(0.6f, 0.5f),
                new Keyframe(0.6f, 0.716f),
                new Keyframe(1.0f, 1.0f)
            ); ; ;
            lumVsSatCurve = new TextureCurve(luminosityBonfireCurve, 0.5f, !loop, in bounds);
            // Assign the square wave to the LumVsSat channel curve
            colorCurves.lumVsSat.value = lumVsSatCurve;
        }
    }
    public void LuminosityReset()
    {
        if (colorCurves != null)
        {
            // Create a new curve for
            luminosityBonfireCurve = new AnimationCurve(
                new Keyframe(0.0f, 0.5f),
                new Keyframe(1.0f, 0.5f)
            );
            lumVsSatCurve = new TextureCurve(luminosityBonfireCurve, 0.5f, !loop, in bounds);
            // Assign the square wave to the LumVsSat channel curve
            colorCurves.lumVsSat.value = lumVsSatCurve;
        }
    }

    public void LuminosityVsSatCurveWave()
    {
        // Define the properties of the square wave
        float frequency = 3.0f; // Adjust the frequency of the square wave
        float amplitude = 2.0f; // Adjust the amplitude of the square wave
        float offset = 0.5f;   // Adjust the offset of the square wave

        // Create a new AnimationCurve for the square wave
        AnimationCurve squareWave = new AnimationCurve();
        squareWave.AddKey(new Keyframe(0.0f, offset));
        squareWave.AddKey(new Keyframe(0.5f / frequency, offset + amplitude));
        squareWave.AddKey(new Keyframe(1.0f / frequency, offset));

        lumVsSatCurve = new TextureCurve(squareWave, 0.5f, !loop, in bounds);
        //        lumVsSatCurve = new TextureCurve(squareWave, 0.5f, !loop, in bounds);
        // Assign the square wave to the LumVsSat channel curve
        colorCurves.lumVsSat.value = lumVsSatCurve;
    }

   
}