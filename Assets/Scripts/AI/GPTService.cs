using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;

public class GPTService : MonoBehaviour
{
    [Header("Renseignez votre clé API OpenAI")]
    [SerializeField] private string apiKey;

    private const string endpoint = "https://api.openai.com/v1/chat/completions";

    public IEnumerator GetChalkboardText(System.Action<string> onResult)
    {
        // Préparer le payload JSON
        var body = new JObject
        {
            ["model"] = "gpt-4o-mini",
            ["messages"] = new JArray
            {
                new JObject { ["role"] = "system", ["content"] = "Tu es un professeur de maths." },
                new JObject { ["role"] = "user",   ["content"] = "Écris une équation simple. Ne donnes aucune explication textuels. On doit seulement voir l'équation et son résultat. Ne donne pas toujours la même équation. Ca peut être aussi un simple calcul. Varies." }
            }
        }.ToString();

        using (var req = new UnityWebRequest(endpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);
            req.uploadHandler   = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"GPT API Error: {req.error}");
                onResult("Erreur IA");
            }
            else
            {
                var json = JObject.Parse(req.downloadHandler.text);
                string content = json["choices"]?[0]?["message"]?["content"]?.ToString().Trim();
                onResult(string.IsNullOrEmpty(content) ? "Pas de réponse" : content);
            }
        }
    }
}
