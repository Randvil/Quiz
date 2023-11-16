using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnswerUI : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Button answerButton;

    public Image Background => background;
    public TextMeshProUGUI TextField => textField;

    public UnityEvent<AnswerUI> AnswerSelectEvent { get; } = new();

    private void Awake()
    {
        answerButton.onClick.AddListener(() => AnswerSelectEvent.Invoke(this));
    }
}
