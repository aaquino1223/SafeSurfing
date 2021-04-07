using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBG : MonoBehaviour
{
    public float ScrollSpeed = 4f;
    private BoxCollider2D _Collider;
    private float height;
    public Vector3 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.localPosition;
        _Collider = GetComponent<BoxCollider2D>();
        height = _Collider.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * ScrollSpeed * Time.deltaTime);

        if(transform.localPosition.y < -(height * 3)){
            RepositionBG();
        }
    }

    private void RepositionBG(){
        Vector3 vector = new Vector3(0, height * 3f);
        transform.localPosition = StartPos;
    }
}
