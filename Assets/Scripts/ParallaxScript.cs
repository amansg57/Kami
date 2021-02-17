using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    
    private Transform[] backgrounds;
    private float[] parallaxScales;
    public float smoothing = 1f;

    private Transform cameraTransform;
    private Vector3 previousCameraPos;
    
    private void Awake() 
    {
         cameraTransform = Camera.main.transform;
    }

    private void Start() 
    {
        previousCameraPos = cameraTransform.position;
        
        backgrounds = new Transform[transform.childCount - 1];
        parallaxScales = new float[transform.childCount - 1];

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Transform bg = transform.GetChild(i);
            backgrounds[i] = bg;
            parallaxScales[i] = bg.position.z;
        }
    }

    private void Update() 
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (cameraTransform.position.x - previousCameraPos.x) * parallaxScales[i];
            float backgroundTargetX = backgrounds[i].position.x + parallax;

            Vector3 targetPos = new Vector3(backgroundTargetX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, targetPos, smoothing * Time.deltaTime);
        }

        previousCameraPos = cameraTransform.position;
    }
}
