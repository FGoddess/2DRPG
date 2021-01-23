using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCam;

    private void Start()
    {
        _virtualCam.Follow = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
}
