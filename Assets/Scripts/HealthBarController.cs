using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarController : MonoBehaviour
{
    private float maxHP;
    public EnemyController enemyController;
    public Image bar;
    private float healthPercent;
    private float currentHPBarPercent;
    private float smoothVelocity;
    private float smoothTime = 0.3f;

    private void Start()
    {
        enemyController = this.GetComponentInParent<EnemyController>();
        maxHP = enemyController.MaxHealth;
    }

    private void Update()
    {
        healthPercent = enemyController.Health / maxHP;
        currentHPBarPercent = Mathf.SmoothDamp(currentHPBarPercent, healthPercent, ref smoothVelocity, smoothTime);
        bar.fillAmount = currentHPBarPercent;
    }
}
