using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Player : MonoBehaviour
{
    public float force;
    public bool isWin = false;
    public bool isLost = false;
    public Text text;
    

    private Rigidbody _rb;
    private Animator _animator;
    private float _touchStartTime = 0f;
    private GameObject _waterSurface;
    private Vector3 _currentCamPos;
    private bool _isJumping = false;
    [SerializeField]
    private float _maxForceThreshHold = 500;
    private bool _spinPlayed = false;
    

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _waterSurface = GameObject.FindGameObjectWithTag("Water Volume");

    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            _touchStartTime = Time.time;
            _animator.SetBool("isReady", true);

            if(_isJumping)
            {
                _animator.SetBool("isSpin", true);
                
            }
        }
        

        if (Input.GetMouseButtonUp(0) && !IsPointerOverUIObject())
        {
            float delta = Time.time - _touchStartTime;
            float adjustedForce = force * delta;
            if(adjustedForce > _maxForceThreshHold)
            {
                adjustedForce = _maxForceThreshHold;
            }
            
            Vector3 pos = new Vector3(0, 1, 1);
            _rb.AddForceAtPosition(pos * adjustedForce, Vector3.up * -5);
 

            force = 0;
            _isJumping = true;
            _animator.SetBool("isSpin", false);
            _animator.SetBool("isJumping", true); 
   
        }

        
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Spinning") && 
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && 
            !_animator.IsInTransition(0))
            {
                _spinPlayed = true;
            }

        _currentCamPos = Camera.main.transform.position;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _waterSurface)
        {
            Camera.main.GetComponent<CameraFollow>().smoothSpeed = 0;
            _isJumping = false;

            if(!isWin)
            {
                isLost = true;
                text.text = "Try Again..";
                _animator.SetBool("isFall", true);
            }

            if(isWin && !_spinPlayed)
            {
                ScoreManager.instance.AddPoint();
                _animator.SetBool("isVictory", true);
                text.text = "Success!";
            }

            if(isWin && _spinPlayed)
            {
                ScoreManager.instance.AddBonusPoint();
                _animator.SetBool("isVictory", true);
                text.text = "Bonus Point!";
            }
        }
        
    }


    public void FlipPlayer()
    {
        transform.localScale = new Vector3(transform.localScale.x, 
            transform.localScale.y, 
            -1 * transform.localScale.z);
    }


    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
