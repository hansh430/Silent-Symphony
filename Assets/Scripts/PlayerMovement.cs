using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Camera and settings")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float lookXLimit = 75f;
    [SerializeField] private float cameraRotationSmooth = 5f;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpPower = 0f;
    [SerializeField] private float gravity = 10f;

    [Header("Ground Sounds")]
    [SerializeField] AudioClip[] woodFootstepSounds;
    [SerializeField] AudioClip[] tileFootstepSounds;
    [SerializeField] AudioClip[] carpetFootstepSounds;
    [SerializeField] Transform footstepAudioPosition;
    [SerializeField] AudioSource audioSource;
  
    [Header("Camera Zoom Settings")]
    [SerializeField] int ZoomFOV = 35;
    [SerializeField] int initialFOV;
    [SerializeField] float cameraZoomSmooth = 1;

    [Header("UI")]
    [SerializeField] private GameObject pauseScreen;
    private bool isWalking = false;
    private bool isFootstepCoroutineRunning = false;
    private AudioClip[] currentFootstepSounds;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private float rotationY = 0;
    private bool isZoomed = false;
    private bool canMove = true;
    private bool isPaused;
    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        SetCursor(CursorLockMode.Locked, false);
        currentFootstepSounds =woodFootstepSounds;
    }
    private void Update()
    {
        PlayerWalkAndMove();
        CameraZoom();
        CameraMovement();
        EnablePauseScreen();
    }

    private void EnablePauseScreen()
    {
        if(Input.GetKeyDown (KeyCode.Escape) && !isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseScreen.SetActive(true);
            SetCursor(CursorLockMode.None,true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Time.timeScale= 1;
            isPaused = false;
            pauseScreen.SetActive(false);
            SetCursor(CursorLockMode.Locked, false);
        }
    }
    private void SetCursor(CursorLockMode lockMode, bool isVisible)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = isVisible;
    }
    private void PlayerWalkAndMove()
    {
        //walkin/Running
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float moveDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        //jump
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = moveDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);


        // Play footstep sounds when walking
        if ((curSpeedX != 0f || curSpeedY != 0f) && !isWalking && !isFootstepCoroutineRunning)
        {
            isWalking = true;
            StartCoroutine(PlayFootstepSounds(1.3f / (isRunning ? runSpeed : walkSpeed)));
        }
        else if (curSpeedX == 0f && curSpeedY == 0f)
        {
            isWalking = false;
        }
    }

    private void CameraZoom()
    {
        // zoom 
        if (Input.GetButtonDown("Fire2"))
        {
            isZoomed = true;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            isZoomed = false;
        }
        if (isZoomed)
        {
            playerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerCam.fieldOfView, ZoomFOV, Time.deltaTime * cameraZoomSmooth);
        }
        else
        {
            playerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerCam.fieldOfView, initialFOV, Time.deltaTime * cameraZoomSmooth);
        }
    }

    private void CameraMovement()
    {
        //camera movement
        if (canMove)
        {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            rotationY += Input.GetAxis("Mouse X") * lookSpeed;

            Quaternion targetRotationX = Quaternion.Euler(rotationX, 0, 0);
            Quaternion targetRotationY = Quaternion.Euler(0, rotationY, 0);
            playerCam.transform.localRotation = Quaternion.Slerp(playerCam.transform.localRotation, targetRotationX, Time.deltaTime * cameraRotationSmooth);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationY, Time.deltaTime * cameraRotationSmooth);
        }
    }

    private IEnumerator PlayFootstepSounds(float footstepDelay)
    {
        isFootstepCoroutineRunning = true;
        while (isWalking)
        {
            if (currentFootstepSounds.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, currentFootstepSounds.Length);
                audioSource.transform.position = footstepAudioPosition.position;
                audioSource.clip = currentFootstepSounds[randomIndex];
                audioSource.Play();
                yield return new WaitForSeconds(footstepDelay);
            }
            else
            {
                yield break;
            }
        }
        isFootstepCoroutineRunning = false;
    }
    // Detect ground surface and set the current footstep sounds array accordingly
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wood"))
        {
            currentFootstepSounds = woodFootstepSounds;
        }
        else if (other.CompareTag("Tile"))
        {
            currentFootstepSounds = tileFootstepSounds;
        }
        else if (other.CompareTag("Carpet"))
        {
            currentFootstepSounds = carpetFootstepSounds;
        }
    }
}
