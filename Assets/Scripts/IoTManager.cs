using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class IoTManager : MonoBehaviour
{
    [Header("UI")]
    public Slider   tempSlider;
    public TMP_Text tempLabel;

    [Header("Scene")]
    public Transform mercuryTube;

    [Header("IoT HTTP")]
    [Tooltip("URL du webhook ou de ton serveur IoT")]
    public string  webhookUrl;  

    private Vector3 initialScale;
    private float   initialPosY;
    private float   currentTemp = 0f;

    // Classe pour sérialisation JSON
    [System.Serializable]
    private class TemperatureData
    {
        public float temperature;
    }

    void Start()
    {
        // Mémoriser l’échelle et la position de base du tube
        if (mercuryTube != null)
        {
            initialScale = mercuryTube.localScale;
            initialPosY  = mercuryTube.localPosition.y;
        }

        // Initialiser le slider
        tempSlider.minValue = 0f;
        tempSlider.maxValue = 40f;
        tempSlider.value    = currentTemp;
        tempSlider.onValueChanged.AddListener(OnTempChanged);

        // Affichage initial
        UpdateTempDisplay(currentTemp);
    }

    void OnTempChanged(float newTemp)
    {
        currentTemp = newTemp;
        UpdateTempDisplay(currentTemp);
        StartCoroutine(PostTemperature(currentTemp));
    }

    void UpdateTempDisplay(float temp)
    {
        tempLabel.text = $"Temp : {temp:0} °C";
        if (mercuryTube == null) return;

        // Calcul de la proportion (0→1)
        float t = Mathf.InverseLerp(0f, 40f, temp);
        float newY = Mathf.Lerp(0f, initialScale.y, t);

        // Appliquer la nouvelle échelle
        Vector3 scale = initialScale;
        scale.y = newY;
        mercuryTube.localScale = scale;

        // Calculer la position pour que la base reste fixe
        float baseY   = initialPosY - initialScale.y / 2f;
        float centerY = baseY + newY / 2f;
        var pos = mercuryTube.localPosition;
        pos.y = centerY;
        mercuryTube.localPosition = pos;
    }

    IEnumerator PostTemperature(float temp)
    {
        // Sérialiser via notre classe
        var data = new TemperatureData { temperature = temp };
        string json = JsonUtility.ToJson(data);

        using var req = new UnityWebRequest(webhookUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        req.uploadHandler   = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            Debug.LogError($"IoT HTTP Error: {req.error}");
        else
            Debug.Log($"IoT HTTP Posted: {json}");
    }
}
