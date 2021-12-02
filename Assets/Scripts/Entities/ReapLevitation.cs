using UnityEngine;
using System.Collections;
 
public class ReapLevitation : MonoBehaviour {
    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public float yOffset = 5f;
    public float lerpSpeed = 1.0f;
 
    private Vector3 _posOffset = new Vector3 ();
    private Vector3 _tempPos = new Vector3 ();
    private Vector3 _startMarker;
    private Vector3 _endMarker;
    
    private float _startTime;

    // Total distance between the markers.
    private float _journeyLength;

    private float _fractionOfJourney;
    // Use this for initialization
    private void Start () {
        // Lerp vars
        _startTime = Time.time;
        _startMarker = transform.position;
        _endMarker = transform.position + new Vector3(0, yOffset);
        _journeyLength = Vector3.Distance(_startMarker, _endMarker);
        
        // Floating
        _posOffset = transform.position;
        _posOffset += new Vector3(0, 5f, 0);
    }
     
    private void Update () {
        // Reached spot in air
        if (_fractionOfJourney >= 1)
        {
            // Spin object around Y-Axis
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
     
            // Float up/down with a Sin()
            _tempPos = _posOffset;
            _tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
     
            transform.position = _tempPos;
        }
        else
        {
            DoLerp();
        }
    }


    // https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
    private void DoLerp()
    { 
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - _startTime) * lerpSpeed;

        // Fraction of journey completed equals current distance divided by total distance.
        _fractionOfJourney = distCovered / _journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(_startMarker, _endMarker, _fractionOfJourney);
        
    }
}