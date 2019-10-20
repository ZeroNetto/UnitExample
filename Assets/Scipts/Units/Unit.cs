using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.PackageManager;
using UnityEngine;

public class Unit : MonoBehaviour
{
    float moveSpeed = 5;
    float rotSpeed = 5;
    bool isFocused = false;
    bool isMoving = false;
    Color basicColor = Color.black;
    Color focusedColor = Color.yellow;
    Vector3 lookAtTarget;
    Quaternion playerRot;
    Vector3 targetPosition;
    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.color = basicColor;
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isFocused)
        {
            SetTargetPosition();
        }

        if (Input.GetMouseButtonDown(1) && isFocused)
        {
            RaycastHit hitInfo;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, 100) && hitInfo.collider.gameObject.CompareTag("Unit"))
            {
                Destroy(gameObject);
            }
        }

        if (isMoving)
        {
            Move();
        }
    }

    void ChangeState()
    {
        isFocused = !isFocused;
        renderer.material.color = isFocused ? focusedColor : basicColor;
    }

    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            targetPosition = hit.point;
            targetPosition.y = 0;
            lookAtTarget = new Vector3(targetPosition.x - transform.position.x,
                transform.position.y,
                targetPosition.z - transform.position.z);
            playerRot = Quaternion.LookRotation(lookAtTarget);
            isMoving = true;
        }
    }

    void Move()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
            playerRot,
            rotSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMoving = false;
        }
    }

    void OnMouseDown() => ChangeState();
}
