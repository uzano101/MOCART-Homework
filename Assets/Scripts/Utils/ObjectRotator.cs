using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 15f;

    private void Start()
    {
        if (int.Parse(gameObject.tag) >= ShopManager.Instance.GetProducts().Length)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
