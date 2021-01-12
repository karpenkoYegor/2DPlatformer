using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMove : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform mainCameraTransform;
    private Transform[] layers;

    public float vievZone = 5f;
    private int leftIndex;
    private int rightIndex;
    public float backgroundSize = 19f;

    public float parralaxSpeed =0.3f;

    private float lastCameraX;
    private float lastCameraY;
    // Start is called before the first frame update
    void Start()
    {
        lastCameraX = mainCameraTransform.position.x;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
            leftIndex = 0;
            rightIndex = layers.Length - 1;
        }
    }

    void ScrollRight()
    {
        float lastLeft = leftIndex;
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
    }

    void ScrollLeft()
    {
        float lastindex = rightIndex;
        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0) rightIndex = layers.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, cameraTransform.position.y);
        layers[leftIndex].transform.position = new Vector2(layers[leftIndex].transform.position.x, cameraTransform.position.y);
        layers[rightIndex].transform.position = new Vector2(layers[rightIndex].transform.position.x, cameraTransform.position.y);

        float deltaX = mainCameraTransform.position.x - lastCameraX;
        lastCameraX = mainCameraTransform.position.x;
        


        transform.position += Vector3.right * (deltaX * parralaxSpeed);

        if (cameraTransform.position.x < layers[leftIndex].transform.position.x + vievZone)
        {
            ScrollLeft();
        }
        if (cameraTransform.position.x > layers[rightIndex].transform.position.x - vievZone)
        {
            ScrollRight();
        }
    }


}
