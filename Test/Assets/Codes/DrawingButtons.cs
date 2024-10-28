using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingButtons : MonoBehaviour
{
    public DrawingCanvas drawingCanvas;

    public Button penButton;
    public Button eraserButton;
    public Button eraseAllButton;

    // Start is called before the first frame update
    void Start()
    {
        if (drawingCanvas == null)
        {
            Debug.LogError("DrawingCanvas reference is not set in the inspector!");
            return;
        }

        // �� ��ư ����
        if (penButton != null)
        {
            penButton.onClick.AddListener(OnPenButtonClick);
        }
        else
        {
            Debug.LogError("Pen button is not assigned in the inspector!");
        }

        // ���찳 ��ư ����
        if (eraserButton != null)
        {
            eraserButton.onClick.AddListener(OnEraserButtonClick);
        }
        else
        {
            Debug.LogError("Eraser button is not assigned in the inspector!");
        }

        // ��ü ����� ��ư ����
        if (eraseAllButton != null)
        {
            eraseAllButton.onClick.AddListener(OnEraseAllButtonClick);
        }
        else
        {
            Debug.LogError("Erase All button is not assigned in the inspector!");
        }

        // �ʱ� ���� ���� (�� ���� ����)
        SetInitialButtonState();
    }

    private void OnPenButtonClick()
    {
        if (drawingCanvas != null)
        {
            drawingCanvas.SetPenMode();
            UpdateButtonVisuals(true, false);
        }
    }

    private void OnEraserButtonClick()
    {
        if (drawingCanvas != null)
        {
            drawingCanvas.SetEraserMode();
            UpdateButtonVisuals(false, true);
        }
    }

    private void OnEraseAllButtonClick()
    {
        if (drawingCanvas != null)
        {
            drawingCanvas.ClearCanvas();
        }
    }

    // ��ư�� �ð��� ���� ������Ʈ
    private void UpdateButtonVisuals(bool penSelected, bool eraserSelected)
    {
        // ���õ� ��ư�� ���İ��� ���߾� �ð������� ����
        Color penColor = penButton.image.color;
        Color eraserColor = eraserButton.image.color;

        penColor.a = penSelected ? 0.5f : 1f;
        eraserColor.a = eraserSelected ? 0.5f : 1f;

        penButton.image.color = penColor;
        eraserButton.image.color = eraserColor;
    }

    // �ʱ� ��ư ���� ����
    private void SetInitialButtonState()
    {
        UpdateButtonVisuals(true, false);
    }

    // OnDestroy���� ������ ����
    void OnDestroy()
    {
        if (penButton != null) penButton.onClick.RemoveListener(OnPenButtonClick);
        if (eraserButton != null) eraserButton.onClick.RemoveListener(OnEraserButtonClick);
        if (eraseAllButton != null) eraseAllButton.onClick.RemoveListener(OnEraseAllButtonClick);
    }
}
