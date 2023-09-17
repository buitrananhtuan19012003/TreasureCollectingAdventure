using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterLocomotion : MonoBehaviour
{
    public Animator rigController;
    public Volume postProcessVolume;

    private Animator animator;
    private CharacterController characterController;
    private ActiveWeapon activeWeapon;
    private WeaponReload reloadWeapon;
    private CharacterAiming characterAiming;
    private Vector2 userInput;
    private Vector3 rootMotion;
    private Vector3 velocity;
    private float jumpHeight;
    private float gravity;
    private float stepDown;
    private float airControl;
    private float jumpDamp;
    private float groundSpeed;
    private float pushPower;
    private bool isJumping;
    private int isSprintingParam = Animator.StringToHash("IsSprinting");

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        activeWeapon = GetComponent<ActiveWeapon>();
        reloadWeapon = GetComponent<WeaponReload>();
        characterAiming = GetComponent<CharacterAiming>();
        if (DataManager.HasInstance)
        {
            jumpHeight = DataManager.Instance.GlobalConfig.jumpHeight;
            gravity = DataManager.Instance.GlobalConfig.gravity;
            stepDown = DataManager.Instance.GlobalConfig.stepDown;
            airControl = DataManager.Instance.GlobalConfig.airControl;
            jumpDamp = DataManager.Instance.GlobalConfig.jumpDamp;
            groundSpeed = DataManager.Instance.GlobalConfig.groundSpeed;
            pushPower = DataManager.Instance.GlobalConfig.pushPower;
        }
    }

    void Update()
    {
        userInput.x = Input.GetAxis("Horizontal");
        userInput.y = Input.GetAxis("Vertical");

        animator.SetFloat("InputX", userInput.x);
        animator.SetFloat("InputY", userInput.y);

        UpdateIsSprinting();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            UpdateInAir();
        }
        else
        {
            UpdateOnGround();
        }
    }

    private bool IsSprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = activeWeapon.IsFiring();
        bool isReloading = reloadWeapon.isReloading;
        bool isChangingWeapon = activeWeapon.isChangingWeapon;
        bool isAiming = characterAiming.isAiming;
        return isSprinting && !isFiring && !isReloading && !isChangingWeapon && !isAiming;
    }

    private void UpdateIsSprinting()
    {
        bool isSprinting = IsSprinting();
        animator.SetBool(isSprintingParam, isSprinting);
        rigController.SetBool(isSprintingParam, isSprinting);
        if (userInput.x != 0)
        {
            //if (postProcessVolume.profile.TryGet(out ChromaticAberration chromaticAberration))
            //{
            //    chromaticAberration.active = isSprinting;
            //}
        }
    }

    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;
        characterController.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;
        if (!characterController.isGrounded)
        {
            SetInAir(0);
        }
    }

    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 airDisplacement = velocity * Time.fixedDeltaTime;
        airDisplacement += CalculateAircontrol();
        characterController.Move(airDisplacement);
        isJumping = !characterController.isGrounded;
        rootMotion = Vector3.zero;
        animator.SetBool("IsJumping", isJumping);
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity * jumpDamp * groundSpeed;
        velocity.y = jumpVelocity;
        animator.SetBool("IsJumping", true);
    }

    private Vector3 CalculateAircontrol()
    {
        return ((transform.forward * userInput.y) + (transform.right * userInput.x)) * (airControl / 100);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        if (hit.moveDirection.y < -0.3f)
            return;
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }

    public void OnFootStep()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_FOOTSTEP);
        }
    }

    public void OnJump()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_JUMP);
        }
    }
}
