using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowMover : MonoBehaviour
{
    public Transform center; //Object to lerp to
    public Color p1, p2;

    private Image arrow;
    private Vector3 startPosition;
    private float lerpProg;

    public float speed; //Based off of song beats per second

    public void Initialize(Transform _center, float _lerpSpeed, bool _songBeat)
    {
        Destroy(gameObject);
        arrow = GetComponent<Image>();
        center = _center;
        speed = _lerpSpeed;
       
        transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);

        startPosition = gameObject.transform.position;

        if(!_songBeat) { ComboArrow(); }
    }

    // Update is called once per frame
    void Update()
    {
        lerpProg += Time.deltaTime / speed;

        transform.position = Vector3.Lerp(startPosition, center.position, lerpProg + 0.1f);

        if(lerpProg >= 0.9f) { Destroy(gameObject); }
    }

    void ComboArrow()
    {
        Vector3 newScale = new Vector3(1.2f, 1.2f, 1.2f);
        transform.localScale = newScale;
        //arrow.color = p1;
    }
}
