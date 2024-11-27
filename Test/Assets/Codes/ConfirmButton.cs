using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmButton : MonoBehaviour
{
    public DrawingCanvas drawingCanvas;
    public Button confirmButton;
	public AudioClip buttonSound;
	private AudioSource audioSource;

	// Start is called before the first frame update
	void Start()
    {
        confirmButton = GetComponent<Button>();
		audioSource = gameObject.AddComponent<AudioSource>();

		// 확인 버튼에 리스너 추가
		if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClick);
        }
        else
        {
            Debug.LogError("Confirm button is not assigned in the inspector!");
        }

        if (drawingCanvas == null)
        {
            Debug.LogError("DrawingCanvas reference is not set in the inspector!");
        }
    }

    void OnConfirmButtonClick()
    {
        if (drawingCanvas != null)
        {
			drawingCanvas.SaveCanvasAsTexture();
			audioSource.PlayOneShot(buttonSound);
		}
        else
        {
            Debug.LogError("Cannot save texture: DrawingCanvas reference is null!");
        }
    }
}
