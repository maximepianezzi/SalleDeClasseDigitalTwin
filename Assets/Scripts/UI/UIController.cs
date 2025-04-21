using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    public Button startButton;
    public TMP_Text statusText;
    public ProfessorBehavior professorBehavior;

    void Start()
    {
        startButton.onClick.AddListener(OnStartClicked);
    }

    void OnStartClicked()
    {
        statusText.text = "Enseignement en cours…";
        StartCoroutine(RunLesson());
    }

    IEnumerator RunLesson()
    {
        yield return StartCoroutine(professorBehavior.TeachRoutine());
        statusText.text = "Terminé";
    }
}
