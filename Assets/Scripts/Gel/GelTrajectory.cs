﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelTrajectory : MonoBehaviour {
    public GameObject dotPrefab;
    public GameObject arrowPrefab;
    public GameObject[] points;
    public GameObject prefabContainer;
    public int numberOfPoints;

    Rigidbody2D rb;

    Vector2 trajectoryDirection;
    float lastLaunchForce;
    float lastGravityScale;
    bool shouldRender = false;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        PlayerShoot.OnAim += SetupRender;
    }

    private void OnDisable() {
        PlayerShoot.OnAim -= SetupRender;
    }

    private void Start() {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++) {
            GameObject instance;
            if (i % 3 == 0) {
                instance = Instantiate(arrowPrefab, prefabContainer.transform);
            } else {
                instance = Instantiate(dotPrefab, prefabContainer.transform);
            }
            points[i] = instance;
        }
        points[0].SetActive(false);
    }

    private void Update() {
        if (shouldRender) {
            RenderTrajectory();
        }
    }

    void RenderTrajectory() {
        for (int i = 0; i < points.Length; i++) {
            float time = i * 0.1f;
            points[i].transform.position = PointPosition(time);
            if (i - 1 > 0) {
                RotateWithTrajectory(points[i - 1], points[i], time);
            }
        }
    }

    void SetupRender(Vector2? direction, float force, float gravityScale) {
        if (direction == null) {
            prefabContainer.SetActive(false);
            shouldRender = false;
        } else {
            prefabContainer.SetActive(true);
            shouldRender = true;
            trajectoryDirection = (Vector2)direction;
        }
        lastLaunchForce = trajectoryDirection.magnitude * force;
        lastGravityScale = gravityScale;
    }

    Vector2 PointPosition(float time) {
        Vector2 startVelocity = trajectoryDirection.normalized * lastLaunchForce;
        Vector2 currentPosition = (Vector2)transform.position + (startVelocity * time) + (Physics2D.gravity * lastGravityScale) * time * time * 0.5f;
        return currentPosition;
    }

    Vector2 Abs(Vector2 v2) {
        return new Vector2(Mathf.Abs(v2.x), Mathf.Abs(v2.y));
    }

    void RotateWithTrajectory(GameObject prevObj, GameObject obj, float time) {
        Vector3 dir = (prevObj.transform.position - obj.transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
}
