using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtons : MonoBehaviour
{
    public DrawingCanvas drawingCanvas;
    public Color penColor;  // Inspector에서 설정할 색상
    private Button colorButton;


    // Start is called before the first frame update
    void Start()
    {
        // 버튼 컴포넌트 가져오기
        colorButton = GetComponent<Button>();

        // DrawingCanvas 참조 확인
        if (drawingCanvas == null)
        {
            Debug.LogError("DrawingCanvas reference is not set in the ColorButton!");
            return;
        }

        // 버튼 컴포넌트 확인
        if (colorButton != null)
        {
            colorButton.onClick.AddListener(OnColorButtonClick);

            // 버튼의 이미지 색상을 penColor로 설정
            Image buttonImage = colorButton.GetComponent<Image>();
            if (buttonImage != null)
            {

                Color buttonColor = penColor;
                buttonColor.a = 1f;
                buttonImage.color = buttonColor;//알파값 고정
            }
        }
        else
        {
            Debug.LogError("Button component is missing!");
        }
    }

    private void OnColorButtonClick()
    {
        if (drawingCanvas != null)
        {
            drawingCanvas.SetPenColor(penColor);
        }
    }

    void OnDestroy()
    {
        if (colorButton != null)
        {
            colorButton.onClick.RemoveListener(OnColorButtonClick);
        }
    }
}
