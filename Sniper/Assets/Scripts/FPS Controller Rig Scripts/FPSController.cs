using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reliant on mouselook and crossplatforminput maybe from standard assets

public enum PlayerMoveStatus{NotMoving, Crouching, Walking, Running, NotGrounded, Landing }
public enum CurveControlledBobCallbackType { Horizontal, Vertical}

public delegate void CurveControlledBobCallback();

[System.Serializable]
public class CurveControlledBobEvent
{
    public float Time = 0f;
    public CurveControlledBobCallback Function = null;
    public CurveControlledBobCallbackType Type = CurveControlledBobCallbackType.Vertical;
}

[System.Serializable]
public class CurveControlledBob
{
    [SerializeField] AnimationCurve _bobcurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(.5f, 1f),
                                                                    new Keyframe(1f, 0f), new Keyframe(1.5f, -1f),
                                                                    new Keyframe(2f, 0f));

    [SerializeField] float _horizontalMultiplier = .01f;
    [SerializeField] float _verticalMultiplier = .02f;
    [SerializeField] float _verticaltoHorizontalSpeedRatio = 2.0f;
    [SerializeField] float _baseInterval = 1f;

    private float _prevXPlayHead;
    private float _prevYPlayHead;
    private float _xPlayHead;
    private float _yPlayHead;
    private float _curveEndTime;
    private List<CurveControlledBobEvent> _events = new List<CurveControlledBobEvent>();

    public void Initialize()
    {
        
        _curveEndTime = _bobcurve[_bobcurve.length - 1].time;
        _xPlayHead = 0f;
        _yPlayHead = 0f;
        _prevXPlayHead = 0f;
        _prevYPlayHead = 0f;
    }

    public void RegisterEventCallback(float time, CurveControlledBobCallback function, CurveControlledBobCallbackType type)
    {
        CurveControlledBobEvent ccbeEvent = new CurveControlledBobEvent();
        ccbeEvent.Time = time;
        ccbeEvent.Function = function;
        ccbeEvent.Type = type;
        _events.Add(ccbeEvent);
        _events.Sort(
            delegate (CurveControlledBobEvent t1, CurveControlledBobEvent t2)
            {
                return (t1.Time.CompareTo(t2.Time));
            }
         );
    }

    public Vector3 GetVectorOffset(float speed)
    {
        _xPlayHead += (speed * Time.deltaTime)/_baseInterval;
        _yPlayHead += ((speed * Time.deltaTime) / _baseInterval)*_verticaltoHorizontalSpeedRatio;

        if (_xPlayHead > _curveEndTime)
            _xPlayHead -= _curveEndTime;
        if (_yPlayHead > _curveEndTime)
            _yPlayHead -= _curveEndTime;

        for(int i = 0; i < _events.Count; i++)
        {
            CurveControlledBobEvent ev = _events[i];
            if(ev != null)
            {
                if(ev.Type == CurveControlledBobCallbackType.Vertical)
                {
                    if((_prevYPlayHead < ev.Time && _yPlayHead >= ev.Time) || 
                       (_prevYPlayHead > _yPlayHead && (ev.Time > _prevYPlayHead || ev.Time <= _yPlayHead)))
                    {
                        ev.Function();
                    }
                }
                else
                {
                    if ((_prevXPlayHead < ev.Time && _xPlayHead >= ev.Time) ||
                       (_prevXPlayHead > _xPlayHead && (ev.Time > _prevXPlayHead || ev.Time <= _xPlayHead)))
                    {
                        ev.Function();
                    }
                }
            }
        }

        float xPos = _bobcurve.Evaluate(_xPlayHead) * _horizontalMultiplier;
        float yPos = _bobcurve.Evaluate(_yPlayHead) * _verticalMultiplier;

        _prevXPlayHead = _xPlayHead;
        _prevYPlayHead = _yPlayHead;

        return new Vector3(xPos, yPos, 0f);


    }

}

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour {

    public List<AudioSource> AudioSources = new List<AudioSource>();
    private int _audioToUse = 0;

    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _runSpeed = 4.5f;
    [SerializeField] private float _jumpSpeed = 7.5f;
    [SerializeField] private float _crouchSpeed = 1f;
    [SerializeField] private float _stickToGroundForce = 5f;
    [SerializeField] private float _gravityMultiplier = 2.5f;
    [SerializeField] private float _runStepLengthen = 0.75f;
    [SerializeField] private UnityStandardAssets.Characters.FirstPerson.MouseLook _mouseLook;
    [SerializeField] private CurveControlledBob _headBob = new CurveControlledBob();
    [SerializeField] private GameObject _flashlight = null;

    //private internals
    private Camera _camera = null;
    private bool _jumpButtonPressed = false;
    private Vector2 _inputVector = Vector2.zero;
    private Vector3 _moveDirection = Vector3.zero;
    private bool _previouslyGrounded = false;
    private bool _isWalking = true;
    private bool _isJumping = false;
    private bool _isCrouching = false;
    private Vector3 _localSpaceCameraPos = Vector3.zero;
    private float _controllerHeight = 0f;
    float timer = 0;

    //timers
    private float _fallingTimer = 0f;
    private CharacterController _characterController = null;
    private PlayerMoveStatus _movementStatus = PlayerMoveStatus.NotMoving;

    //Public Properties
    public PlayerMoveStatus movementStatus { get { return _movementStatus; } }
    public float walkSpeed { get { return _walkSpeed; } }
    public float runSPeed { get { return _runSpeed; } }
    public float sprintCoolDown = 3f;
    private StaminaScript stamina;
    public Crosshair crossHair;
    private gunScript gun;
    bool canUseStamina = true;

    public bool applyPhysics = true;

    // Use this for initialization
    //Start
    void Start (){
        _characterController = GetComponent<CharacterController>();
        _controllerHeight = _characterController.height;

        _camera = Camera.main;
        _localSpaceCameraPos = _camera.transform.localPosition;
        _movementStatus = PlayerMoveStatus.NotMoving;

        _fallingTimer = 0f;
        _mouseLook.Init(transform, _camera.transform);

        _headBob.Initialize();

        _headBob.RegisterEventCallback(1.5f, PlayFootStepSound, CurveControlledBobCallbackType.Vertical);

        stamina = GetComponent<StaminaScript>();
        gun = GetComponentInChildren<gunScript>();
        crossHair = FindObjectOfType<Crosshair>().GetComponent<Crosshair>();

        if (_flashlight)
            _flashlight.SetActive(false);
	}
    

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool wasWalking = _isWalking;
        timer += Time.deltaTime;
        //_isWalking = !Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKey(KeyCode.LeftShift) && timer >= 1.5f)
        {
            _isWalking = !Input.GetKey(KeyCode.LeftShift);
            crossHair.gap = 10 + gun.runAccuracy;
            stamina.UseStamina(1);
        }

        else
        {
            crossHair.gap = 10 + gun.walkAccuracy;
            _isWalking = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            timer = 0f;
        }

        float speed = _isCrouching ? _crouchSpeed : _isWalking ? _walkSpeed : _runSpeed;
        //Debug.Log(canUseStamina);

        /*if (speed == _runSpeed && canUseStamina == true)
        {

        }

        else if (canUseStamina == false)
        {
            if(stamina.maxStamina * 0.10 <= stamina.currentStamina)
            {
                canUseStamina = true;
            }
            else
            {
                canUseStamina = false;
                speed = walkSpeed;
                _movementStatus = PlayerMoveStatus.Walking;
            }
        }*/


        if (stamina.currentStamina <= 0) //Why does the current stamina go to like -6 instead of 0 ?
        {
            canUseStamina = false;
            speed = walkSpeed;
            _movementStatus = PlayerMoveStatus.Walking;
            crossHair.gap = 10 + gun.walkAccuracy;
            
        }


        _inputVector = new Vector2(horizontal, vertical);

        if (_inputVector.sqrMagnitude > 1) _inputVector.Normalize();

        Vector3 desiredMove = transform.forward * _inputVector.y + transform.right * _inputVector.x;

        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, _characterController.radius, Vector3.down, out hitInfo, _characterController.height / 2f, 1))
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        _moveDirection.x = desiredMove.x * speed;
        _moveDirection.z = desiredMove.z * speed;

        if (_characterController.isGrounded)
        {
            _moveDirection.y = -_stickToGroundForce;

            if (_jumpButtonPressed)
            {
                _moveDirection.y = _jumpSpeed;
                _jumpButtonPressed = false;
                _isJumping = true;
                AudioSources[4].Play();//this is working
                //play jumping sound
            }
        }
        else
        {
            if(applyPhysics)
                _moveDirection += Physics.gravity * _gravityMultiplier * Time.fixedDeltaTime;
        }

        _characterController.Move(_moveDirection * Time.fixedDeltaTime);

        Vector3 speedXZ = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z);
        if (speedXZ.magnitude > .01f)
            _camera.transform.localPosition = _localSpaceCameraPos + _headBob.GetVectorOffset(speedXZ.magnitude * (_isCrouching ||_isWalking? 1.0f:_runStepLengthen));
        else
            _camera.transform.localPosition = _localSpaceCameraPos;

        ////added for headbob increase with zoom:
        //if (Input.GetMouseButton(1))
        //{
        //    _camera.transform.localPosition += _localSpaceCameraPos + _headBob.GetVectorOffset(2f);
        //}
        //else if (Input.GetKeyUp(KeyCode.Mouse0))
        //{
            
        //}
        ////end added
    }

    void PlayFootStepSound()
    {
        if (_isCrouching)
        {
            AudioSources[_audioToUse].Play();//Debug.Log("Playing FootStep Sound");
            _audioToUse = (_audioToUse == 2) ? 3 : 2;
        }

        AudioSources[_audioToUse].Play();//Debug.Log("Playing FootStep Sound");
        _audioToUse = (_audioToUse == 0) ? 1 : 0;
    }

    // Update is called once per frame

    public Animator animator;//place in inspector
    private bool isRunning = false;//this is for the animator
    private bool isShooting = false;

    IEnumerator WaitAfterShootToReturnToRunning()
    {
        yield return new WaitForSeconds(2f);
    }

    void Update () {


        //if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))     //probably could combine this with the speed being increased with left shift but its not quite as simple as that? maybe do if _movementStatus == PlayerMoveStatus.Running, had some problems with that though
        //{
        //    isRunning = true;
        //    animator.SetBool("Running", isRunning);
        //}
        //else if(Input.GetKeyUp(KeyCode.LeftShift) || isShooting == true || Input.GetKeyUp(KeyCode.W))
        //{
        //    isRunning = false;
        //    animator.SetBool("Running", isRunning);
        //}

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    isShooting = true;
        //    animator.SetBool("Shooting", isShooting);
        //}
        //else if (Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    isShooting = false;
        //    animator.SetBool("Shooting", isShooting);
        //    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) 
        //    {
        //        isRunning = true;
        //        animator.SetBool("Running", isRunning);
        //    }
        //}//the above animator code is extremely convoluted and there is probably a much simpler way of getting animations to run at the proper time

        
        if (_movementStatus == PlayerMoveStatus.Running && isShooting == false)//i've been messing around with this logic and the animation state times and offsets and exit time and so on, the problem was that the shooting didnt happen fast enough while running, i think now the animation returns to running too quickly if you only single click
        {
            isRunning = true;
            animator.SetBool("Running", isRunning);     
        }
        else if(_movementStatus == PlayerMoveStatus.Walking || _movementStatus == PlayerMoveStatus.NotMoving /*|| isShooting == true*/)
        {
            isRunning = false;
            animator.SetBool("Running", isRunning);
        }
        if (Input.GetMouseButtonDown(0))
        {
            transform.BroadcastMessage("Fire");
        }
        /*else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //WaitAfterShootToReturnToRunning();//wanted to put a delay after firing so that it doesn't go immediately to the running state after firing, didn't work
            isShooting = false;
            animator.SetBool("Shooting", isShooting);
        }*/



            if (_characterController.isGrounded) _fallingTimer = 0f;
        else                                 _fallingTimer += Time.deltaTime;

        if (Time.timeScale > Mathf.Epsilon)
            _mouseLook.LookRotation(transform, _camera.transform);

        if (Input.GetButtonDown("Flashlight"))
        {
            if (_flashlight)
                _flashlight.SetActive(!_flashlight.activeSelf);
        }

        if (!_jumpButtonPressed && !_isCrouching)
            _jumpButtonPressed = Input.GetButtonDown("Jump");

        if (Input.GetButtonDown("Crouch"))
        {
            _isCrouching = !_isCrouching;
            _characterController.height = _isCrouching == true ? _controllerHeight / 2.0f : _controllerHeight;
        }

        if (!_previouslyGrounded && _characterController.isGrounded)
        {
            if (_fallingTimer > .5f)
            {
                //this is where you are supposed to play landing audio i think
                //AudioSources[5].Play();//landing audio is index 5, this isn't working though
            }
            //AudioSources[5].Play();//it works playing it here, but if you jump while moving forward footsteps continue to play, and it plays when you crouch and standup
            _moveDirection.y = 0f;
            _isJumping = false;
            _movementStatus = PlayerMoveStatus.Landing;
            crossHair.gap = gun.StillAccuracy;
        }
        else if (!_characterController.isGrounded)
        {
            _movementStatus = PlayerMoveStatus.NotGrounded;
            crossHair.gap = 10 + gun.runAccuracy;
        }

        else if (_characterController.velocity.sqrMagnitude < .01f)
        {
            _movementStatus = PlayerMoveStatus.NotMoving;
            crossHair.gap = 10 + gun.StillAccuracy;
        }
        else if (_isCrouching)
        {
            _movementStatus = PlayerMoveStatus.Crouching;
            crossHair.gap = 10 + gun.StillAccuracy;
        }
        else if (_isWalking)
        {
            _movementStatus = PlayerMoveStatus.Walking;
            crossHair.gap = 10 + gun.walkAccuracy;
        }
        else
        {
            _movementStatus = PlayerMoveStatus.Running;
            crossHair.gap = 10 + gun.runAccuracy;
        }

        _previouslyGrounded = _characterController.isGrounded;

	}
}
