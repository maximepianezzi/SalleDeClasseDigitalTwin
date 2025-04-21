using UnityEngine;
using TMPro;
using System.Collections;

public class ProfessorBehavior : MonoBehaviour
{
    [Header("Références")]
    public Transform boardTransform;  // Position près du tableau
    public TMP_Text boardText;        // Référence au TextMeshPro sur le tableau

    [Header("Mouvement")]
    public float moveSpeed = 2f;      // Vitesse de déplacement

    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
        StartCoroutine(TeachRoutine());
    }

    public IEnumerator TeachRoutine() {
        // 1. Aller au tableau
        anim.SetTrigger("toWalk");
        while (Vector3.Distance(transform.position, boardTransform.position) > 0.1f) {
            transform.position = Vector3.MoveTowards(
                transform.position,
                boardTransform.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // 2. Passer à l’écriture
        anim.SetTrigger("toWrite");
        yield return new WaitForSeconds(
            anim.GetCurrentAnimatorStateInfo(0).length
        );

        // 3. Appeler l’API IA pour générer le contenu
        yield return StartCoroutine(GenerateChalkboardText());

        // 4. Retour à l’état Idle
        anim.ResetTrigger("toWrite");
    }

    IEnumerator GenerateChalkboardText()
    {
        boardText.text = "Génération IA…";
        yield return StartCoroutine(
            FindObjectOfType<GPTService>()
                .GetChalkboardText(result => boardText.text = result)
        );
    }
}
