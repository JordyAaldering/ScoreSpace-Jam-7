#pragma warning disable 0649
using System;
using TMPro;
using UnityEngine;

public class SetVariableText : MonoBehaviour
{
    [SerializeField] private FloatVariable var;
    
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.text = var.ToString();
    }
}
