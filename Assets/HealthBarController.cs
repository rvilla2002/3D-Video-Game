using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    private Image healthBarFill;

    void Start()
    {
        healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, transform);
        healthBarFill = healthBarInstance.transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    public void UpdateHealthBar(float percentage)
    {
        healthBarFill.rectTransform.localScale = new Vector3(percentage, 1, 1);
    }
}

