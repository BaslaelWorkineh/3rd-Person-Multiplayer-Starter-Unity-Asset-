using UnityEngine;
using UnityEngine.UI;
public class QualitySetting : MonoBehaviour
{
    public Slider qualitySlider;

    void Start()
    {
        // Add listener for the slider value change
        qualitySlider.onValueChanged.AddListener(ChangeQualityLevel);
    }

    void ChangeQualityLevel(float value)
    {
        int qualityLevel = Mathf.RoundToInt(value * 6); // Convert slider value to an integer between 0 and 6
        QualitySettings.SetQualityLevel(qualityLevel, true); // Set the quality level
        Debug.Log(qualityLevel);
    }
}
