using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _zoomSpeed;

    [SerializeField] private Vector2 _zoomMinMax;

    private Camera cam;

    [SerializeField] private Vector2 velocity;
    [SerializeField] private float scroll;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var move = x * Vector3.right + z * Vector3.forward;

        transform.Translate(move * _moveSpeed * Time.deltaTime);

        scroll = -Input.GetAxis("Mouse ScrollWheel");


        var orthosize = new Vector2(cam.orthographicSize, 0f);
        var orthosizeNew = new Vector2(cam.orthographicSize + (scroll * _zoomSpeed), 0f);


        cam.orthographicSize = Vector2.SmoothDamp(orthosize, orthosizeNew, ref velocity, 0.3f).x;



        



        //var zoom = cam.orthographicSize * scroll;

        //cam.orthographicSize = Mathf.Clamp(zoom, _zoomMinMax.x, _zoomMinMax.y);



    }
}
