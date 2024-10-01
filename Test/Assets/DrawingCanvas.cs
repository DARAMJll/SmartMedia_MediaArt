using UnityEngine;
using UnityEngine.UI;

public class DrawingCanvas : MonoBehaviour
{
    public RawImage drawingSurface; // Inspector에서 할당
    public Texture2D canvasTexture;
    public Color drawColor = Color.black;
    public int brushSize = 5;

    private bool isDrawing = false;
    private Vector2 previousMousePos; // 이전 프레임에서의 마우스 위치 저장

    void Start()
    {
        // 빈 텍스처 생성
        canvasTexture = new Texture2D(512, 512);
        drawingSurface.texture = canvasTexture;

        // 캔버스를 하얀색 배경으로 초기화
        ClearCanvas();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
            previousMousePos = GetMousePosition(); // 마우스를 클릭한 순간 위치 저장
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }

        if (isDrawing)
        {
            Draw();
        }
    }

    void Draw()
    {
        // 현재 마우스 위치 얻기
        Vector2 currentMousePos = GetMousePosition();

        // 마우스가 이전 프레임에서 움직인 경우에만 선을 그리기
        if (previousMousePos != currentMousePos)
        {
            // 이전 위치와 현재 위치 사이에 선을 그리기
            DrawLine(previousMousePos, currentMousePos);

            // 현재 위치를 이전 위치로 업데이트
            previousMousePos = currentMousePos;
        }

        // 텍스처 적용
        canvasTexture.Apply();
    }

    Vector2 GetMousePosition()
    {
        // 마우스 위치를 월드 좌표에서 로컬 좌표로 변환
        Vector2 mousePos = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            drawingSurface.rectTransform, mousePos, null, out Vector2 localPoint);

        // 로컬 좌표를 텍스처 좌표로 변환
        float x = (localPoint.x + drawingSurface.rectTransform.rect.width / 2) * 
                  (canvasTexture.width / drawingSurface.rectTransform.rect.width);
        float y = (localPoint.y + drawingSurface.rectTransform.rect.height / 2) * 
                  (canvasTexture.height / drawingSurface.rectTransform.rect.height);

        return new Vector2(x, y);
    }

    void DrawLine(Vector2 start, Vector2 end)
    {
        // 선형 보간(Lerp)을 사용하여 두 지점 사이에 선을 그린다
        float distance = Vector2.Distance(start, end);
        Vector2 direction = (end - start).normalized;

        for (float i = 0; i <= distance; i += 0.1f)
        {
            Vector2 point = start + direction * i;

            // 브러쉬 크기만큼 점을 찍기
            for (int dx = -brushSize; dx <= brushSize; dx++)
            {
                for (int dy = -brushSize; dy <= brushSize; dy++)
                {
                    canvasTexture.SetPixel((int)point.x + dx, (int)point.y + dy, drawColor);
                }
            }
        }
    }

    public void ClearCanvas()
    {
        // 캔버스를 하얀색으로 초기화
        for (int x = 0; x < canvasTexture.width; x++)
        {
            for (int y = 0; y < canvasTexture.height; y++)
            {
                canvasTexture.SetPixel(x, y, Color.white);
            }
        }
        canvasTexture.Apply();
    }
}