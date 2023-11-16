using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnswersUI : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private AnswerUI answerPrefab;
    [SerializeField] private GridLayoutGroup gridGroup;
    [SerializeField] private float relativeSpacing = 0.1f;

    private Dictionary<AnswerUI, Answer> answersUIAndReal = new();

    public UnityEvent<Answer> AnswerSelectEvent { get; } = new();

    public void CreateAnswers(List<Answer> answers)
    {
        ClearAnswers();

        SetGridSettings(answers);

        foreach (Answer answer in answers)
        {
            AnswerUI answerUI = Instantiate(answerPrefab, gridGroup.transform);
            answerUI.TextField.text = answer.text;
            answersUIAndReal.Add(answerUI, answer);
            answerUI.AnswerSelectEvent.AddListener(OnAnswerSelect);
        }
    }

    public void ClearAnswers()
    {
        answersUIAndReal.Clear();

        AnswerUI[] lastAnswers = gridGroup.GetComponentsInChildren<AnswerUI>();

        foreach (AnswerUI lastAnswer in lastAnswers)
        {
            Destroy(lastAnswer.gameObject);
        }
    }

    private void SetGridSettings(List<Answer> answers)
    {
        int answersCount = answers.Count;
        RectTransform gridTransform = gridGroup.GetComponent<RectTransform>();
        Vector2 resolusion = canvasScaler.referenceResolution;

        int columnCount;
        int rowCount;
        if (gridGroup.constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
            rowCount = gridGroup.constraintCount;
            columnCount = Mathf.CeilToInt(answersCount / (float)rowCount);
        }
        else if (gridGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            columnCount = gridGroup.constraintCount;
            rowCount = Mathf.CeilToInt(answersCount / (float)columnCount);
        }
        else
        {
            return;
        }

        Vector2 elementSize = new(resolusion.x * (gridTransform.anchorMax.x - gridTransform.anchorMin.x) / columnCount,
                                  resolusion.y * (gridTransform.anchorMax.y - gridTransform.anchorMin.y) / rowCount);
        Vector2 spacing = elementSize * relativeSpacing;
        gridGroup.spacing = spacing;
        Vector2 answerSize = elementSize - spacing + new Vector2(spacing.x / columnCount, spacing.y / rowCount);
        gridGroup.cellSize = answerSize;
    }

    private void OnAnswerSelect(AnswerUI answerUI)
    {
        foreach (KeyValuePair<AnswerUI, Answer> answerUIAndReal in answersUIAndReal)
        {
            answerUIAndReal.Key.Background.color = answerUIAndReal.Value.correct ? Color.green : Color.red;
        }

        AnswerSelectEvent.Invoke(answersUIAndReal[answerUI]);
    }
}
