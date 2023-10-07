using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem.Processors;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera basic2Dcamera;
    public CinemachineVirtualCamera waterCamera;
    public CinemachineFramingTransposer camSettings;
    public CinemachineBasicMultiChannelPerlin camNoise;
    public float JumpDeadZone = 2f;

    // Start is called before the first frame update
    void Start()
    {
        basic2Dcamera = GetComponent<CinemachineVirtualCamera>();

        camSettings = basic2Dcamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        camNoise = basic2Dcamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void DeadZoneOn()
    {
        camSettings.m_DeadZoneHeight = JumpDeadZone;
    }

    public void DeadZoneOff()
    {
        camSettings.m_DeadZoneHeight = 0;
    }

    public void ScreenShake(float myFreq, float myDuration, float myAmplitude)
    {
        StartCoroutine(Shake(myFreq, myDuration, myAmplitude));
    }

    public IEnumerator Shake(float myFreq, float myDuration, float myAmplitude)
    {
        print("shake");
        camNoise.m_AmplitudeGain = myAmplitude;
        camNoise.m_FrequencyGain = myFreq;
        yield return new WaitForSeconds(myDuration);
        camNoise.m_AmplitudeGain = 0;
        camNoise.m_FrequencyGain = 0;

    }

    public void FindPlayerAtSceneLoad()
    {
        GameObject player = GameObject.Find("Player");
        GameObject cameraTarget = player.transform.Find("CameraTarget").gameObject;

        // Check if the camera target exists before setting it as the follow target
        if (cameraTarget != null)
        {
            basic2Dcamera.Follow = cameraTarget.transform;
        }
        else
        {
            Debug.LogWarning("CameraTarget not found!");
        }
    }

}
