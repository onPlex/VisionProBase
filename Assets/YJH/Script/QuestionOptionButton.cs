using UnityEngine;

public class QuestionOptionButton : MonoBehaviour
{
    private int optionIndex;
    public System.Action<int> OnOptionSelected;

    /// <summary>
    /// Sets the option text for the button (e.g., TextMesh or TextMeshPro).
    /// </summary>
    /// <param name="text">Option text.</param>
    public void SetOptionText(string text)
    {
        var textMesh = GetComponentInChildren<TextMesh>(); // Replace with TextMeshPro if needed
        if (textMesh != null)
        {
            textMesh.text = text;
        }
    }

    /// <summary>
    /// Sets the index of this option.
    /// </summary>
    /// <param name="index">Option index.</param>
    public void SetOptionIndex(int index)
    {
        optionIndex = index;
    }

    /// <summary>
    /// Called when the button is pressed via SpatialButton.
    /// </summary>
    public void Press()
    {
        OnOptionSelected?.Invoke(optionIndex);
    }
}
