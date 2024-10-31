using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DrawingCanvas : MonoBehaviour
{
    public RawImage drawingSurface; // Inspector에서 할당
    public Texture2D canvasTexture;
<<<<<<< Updated upstream
    public Color drawColor = Color.black;
    public int brushSize = 5;

    private bool isDrawing = false;
    private Vector2 previousMousePos; // 이전 프레임에서의 마우스 위치 저장

    void Start()
=======
    public int brushSize = 3;

    public Slider brushsizeSlider;

    // 펜 색상과 지우개(흰색) 색상 정의
    //public Color penColor = Color.black;
    public Color eraserColor = Color.white;
    public Color currentColor = Color.black;
    public Color lastPenColor = Color.black; 

    private bool isDrawing = false;
    private Vector2 previousMousePos; // 이전 프레임에서의 마우스 위치 저장
    private DrawingMode currentMode = DrawingMode.Pen;

    private string GetPrefabPath()
    {
        // 현재 스크립트의 위치에서 상대 경로로 프리팹 찾기
        string scriptPath = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(this));
        string scriptDirectory = Path.GetDirectoryName(scriptPath);
        return Path.Combine(Path.GetDirectoryName(scriptDirectory), "Lamp/Lamp_prefab.prefab");
    }


    /// Lamps
    
    public Transform spawnPoint; // 램프가 생성될 위치
	//public GameObject lampPrefab;
	public GameObject[] lampPrefabs; // Inspector에서 5개의 램프 프리팹 할당

	void Start()
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
                    canvasTexture.SetPixel((int)point.x + dx, (int)point.y + dy, drawColor);
                }
=======
                    // 텍스처 범위 체크
                    int pixelX = (int)point.x + dx;
                    int pixelY = (int)point.y + dy;

					if (pixelX >= 0 && pixelX < canvasTexture.width &&
					    pixelY >= 0 && pixelY < canvasTexture.height)
					{
						Color drawColor = currentMode == DrawingMode.Eraser ? eraserColor : currentColor;
						canvasTexture.SetPixel(pixelX, pixelY, drawColor); // currentColor를 drawColor로 수정
					}
				}
>>>>>>> Stashed changes
            }
        }
    }

<<<<<<< Updated upstream
=======

    public void SetPenMode()
    {
        currentMode = DrawingMode.Pen;
        currentColor = lastPenColor;
        //brushSize = 3; // 펜 모드의 기본 브러시 크기
    }

    public void setBrushSize(){
        brushSize = (int)brushsizeSlider.value;
    }

    public void SetEraserMode()
    {
        currentMode = DrawingMode.Eraser;
        currentColor = eraserColor;
        //brushSize = 8; // 지우개 모드의 기본 브러시 크기
    }

    public void SetPenColor(Color newColor)
    {
        newColor.a = 1f; // 알파값을 1로 설정
        currentColor = newColor;
        lastPenColor = newColor;
        // 색상이 변경되면 자동으로 펜 모드로 전환
        currentMode = DrawingMode.Pen; // 펜 모드로 전환하되 색상은 유지
        SetPenMode();
    }


>>>>>>> Stashed changes
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

    //확인 버튼 누르면 이미지 파일로 저장되도록
    public void SaveCanvasAsTexture()
    {
        string folderPath = Path.Combine(Application.dataPath, "tmp_texture");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        //인덱스
        string fileName = "tmp_000";
        int fileIndex = 0;
        while (File.Exists(Path.Combine(folderPath, fileName + ".png")))
        {
            fileIndex++;
            fileName = $"tmp_{fileIndex:D3}";
        }

        string fullPath = Path.Combine(folderPath, fileName + ".png");
        byte[] pngData = canvasTexture.EncodeToPNG();
        File.WriteAllBytes(fullPath, pngData);

        Debug.Log($"Texture saved as: {fullPath}");

<<<<<<< Updated upstream
=======
        // 파일이 저장된 후 약간의 딜레이를 주어 AssetDatabase가 파일을 인식하도록 함
        AssetDatabase.Refresh();

        // 저장된 텍스처 파일 로드
        string texturePath = "Assets" + fullPath.Substring(Application.dataPath.Length);
        //Texture2D savedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        //if (savedTexture == null)
        //{
        //    Debug.LogError($"Failed to load texture at path: {texturePath}");
        //    return;
        //}


        TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;

        if (textureImporter != null)
        {
            textureImporter.textureType = TextureImporterType.Default;
            textureImporter.alphaIsTransparency = true;
            AssetDatabase.ImportAsset(texturePath);
        }


        // 램프 프리팹 생성 및 텍스처 적용
        //GameObject newLamp = Instantiate(lampPrefab, spawnPoint.position, spawnPoint.rotation);

        //// 램프의 메터리얼 찾기
        //Renderer lampRenderer = newLamp.GetComponent<Renderer>();
        //if (lampRenderer != null)
        //{
        //    // 새로운 메터리얼 인스턴스 생성
        //    Material newMaterial = new Material(lampRenderer.material);
        //    newMaterial.mainTexture = transparentTexture;
        //    lampRenderer.material = newMaterial;
        //}

        //newLamp.name = $"Lamp_{fileName}";
        //Debug.Log($"Lamp created with texture: {fullPath}");

        //ClearCanvas();




        // 저장된 텍스처 로드
        Texture2D savedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        if (savedTexture == null)
        {
            Debug.LogError($"Failed to load texture at path: {texturePath}");
            return;
        }


		// 새로운 Material 생성
		//Material newMaterial = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
		Material templateMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/tmp_texture/Materials/Default_Mat.mat");
		Material newMaterial = new Material(templateMaterial);
		newMaterial.mainTexture = savedTexture;

        // Material 저장
        string materialFolderPath = Path.Combine(folderPath, "Materials");
        if (!Directory.Exists(materialFolderPath))
        {
            Directory.CreateDirectory(materialFolderPath);
        }

        string materialFileName = $"Mat_{fileIndex:D3}";
        string materialPath = Path.Combine(materialFolderPath, materialFileName + ".mat");
        string materialAssetPath = "Assets" + materialPath.Substring(Application.dataPath.Length);
        AssetDatabase.CreateAsset(newMaterial, materialAssetPath);
        AssetDatabase.SaveAssets();

		if (lampPrefabs == null || lampPrefabs.Length == 0)
		{
			Debug.LogError("No lamp prefabs assigned!");
			return;
		}

		// 프리팹 배열 상태 확인을 위한 디버그 로그 추가
		Debug.Log($"Number of lamp prefabs: {lampPrefabs.Length}");

		// 랜덤으로 프리팹 선택
		int randomIndex = Random.Range(0, lampPrefabs.Length);
		GameObject selectedPrefab = lampPrefabs[randomIndex];

		// 선택된 프리팹으로 램프 생성
		GameObject newLamp = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);
		Transform lampCube = newLamp.transform.Find("Subdivision_Surface/lampCube");

		// 선택된 프리팹 확인을 위한 디버그 로그 추가
		Debug.Log($"Selected prefab index: {randomIndex}, Prefab name: {selectedPrefab.name}");

		if (lampCube != null)
        {
            Renderer cubeRenderer = lampCube.GetComponent<Renderer>();
            if (cubeRenderer != null)
            {
                cubeRenderer.material = newMaterial;
            }
            else
            {
                Debug.LogError("Cube Renderer not found!");
            }
        }
        else
        {
            Debug.LogError("Cube named 'lampcube' not found in lamp prefab!");
        }

        newLamp.name = $"Lamp_{fileName}";
        Debug.Log($"Lamp created with material: {materialFileName}");

>>>>>>> Stashed changes
        ClearCanvas();
    }

}