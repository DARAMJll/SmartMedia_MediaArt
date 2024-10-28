using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtons : MonoBehaviour
{
    public DrawingCanvas drawingCanvas;
    public Color penColor;  // Inspector���� ������ ����
    private Button colorButton;


    // Start is called before the first frame update
    void Start()
    {
        // ��ư ������Ʈ ��������
        colorButton = GetComponent<Button>();

        // DrawingCanvas ���� Ȯ��
        if (drawingCanvas == null)
        {
            Debug.LogError("DrawingCanvas reference is not set in the ColorButton!");
            return;
        }

        // ��ư ������Ʈ Ȯ��
        if (colorButton != null)
        {
            colorButton.onClick.AddListener(OnColorButtonClick);

            // ��ư�� �̹��� ������ penColor�� ����
            Image buttonImage = colorButton.GetComponent<Image>();
            if (buttonImage != null)
            {

                Color buttonColor = penColor;
                buttonColor.a = 1f;
                buttonImage.color = buttonColor;//���İ� ����
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
