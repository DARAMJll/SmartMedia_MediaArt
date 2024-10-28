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

        // 펜 버튼 설정
        if (penButton != null)
        {
            penButton.onClick.AddListener(OnPenButtonClick);
        }
        else
        {
            Debug.LogError("Pen button is not assigned in the inspector!");
        }

        // 지우개 버튼 설정
        if (eraserButton != null)
        {
            eraserButton.onClick.AddListener(OnEraserButtonClick);
        }
        else
        {
            Debug.LogError("Eraser button is not assigned in the inspector!");
        }

        // 전체 지우기 버튼 설정
        if (eraseAllButton != null)
        {
            eraseAllButton.onClick.AddListener(OnEraseAllButtonClick);
        }
        else
        {
            Debug.LogError("Erase All button is not assigned in the inspector!");
        }

        // 초기 상태 설정 (펜 모드로 시작)
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

    // 버튼의 시각적 상태 업데이트
    private void UpdateButtonVisuals(bool penSelected, bool eraserSelected)
    {
        // 선택된 버튼의 알파값을 낮추어 시각적으로 구분
        Color penColor = penButton.image.color;
        Color eraserColor = eraserButton.image.color;

        penColor.a = penSelected ? 0.5f : 1f;
        eraserColor.a = eraserSelected ? 0.5f : 1f;

        penButton.image.color = penColor;
        eraserButton.image.color = eraserColor;
    }

    // 초기 버튼 상태 설정
    private void SetInitialButtonState()
    {
        UpdateButtonVisuals(true, false);
    }

    // OnDestroy에서 리스너 제거
    void OnDestroy()
    {
        if (penButton != null) penButton.onClick.RemoveListener(OnPenButtonClick);
        if (eraserButton != null) eraserButton.onClick.RemoveListener(OnEraserButtonClick);
        if (eraseAllButton != null) eraseAllButton.onClick.RemoveListener(OnEraseAllButtonClick);
    }
}
