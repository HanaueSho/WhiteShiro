/*
    LevelEditorSelectBlock.cs
    20260802  arai eito
    レベルエディターのセレクトしているブロックを表示する・取得可能にしたりする
 */
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class LevelEditorSelectBlock : MonoBehaviour
{

    // ==================================================
    // ----- Priority -----
    // ==================================================
    [SerializeField, Attribute_ReadOnly] private Block _selectBlock;

    // プレビュー
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private Camera _previewCamera;
    private RenderTexture _renderTexture;
    private GameObject _previewObject;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public Block SelectBlock
    {   get { return _selectBlock; }
        set
        {
            if(_selectBlock == value)
            {
                return;
            }

            // 
            _selectBlock = value;

            // プレビューオブジェクト
            CreatePreviewObject(_selectBlock?.gameObject);            
        }
    }

    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void OnEnable()
    {
        // レンダーテクスチャ
        if (_renderTexture == null)
        {
            CreateRenderTexture();
        }

        OnValidate();
    }
    private void OnValidate()
    {

        if (_renderTexture == null)
        {
            CreateRenderTexture();
        }

        if (_selectBlock != null && _previewObject == null)
        {
            CreatePreviewObject(_selectBlock.gameObject);
        }

        // UI Image
        if (_rawImage != null)
        {
            _rawImage.texture = _renderTexture;
        }

        // Preview Camera
        if(_previewCamera != null)
        {
            _previewCamera.transform.SetParent(transform, false);

            _previewCamera.targetTexture = _renderTexture;
            _previewCamera.allowHDR = false;
            _previewCamera.allowMSAA = false; 
            _previewCamera.clearFlags = CameraClearFlags.SolidColor;
            _previewCamera.backgroundColor = Color.clear;


            int layer = LayerMask.NameToLayer("Preview");
            if (layer != -1)
            {
                _previewCamera.cullingMask = 1 << layer;
            }
        }
    }
    private void OnDisable()
    {
        if (_previewObject != null)
        {
            if (Application.isPlaying)
                Destroy(_previewObject);
            else
                DestroyImmediate(_previewObject);

            _previewObject = null;
        }


        if (_previewCamera != null)
        {
            _previewCamera.targetTexture = null;
        }

        if(_rawImage != null)
        {
            _rawImage.texture = null;
        }


        // レンダーテクスチャ
        if (_renderTexture != null)
        {

            _renderTexture.Release();

            if (Application.isPlaying)
            {
                Destroy(_renderTexture);
            }
            else
            {
                DestroyImmediate(_renderTexture);
            }

            _renderTexture = null;

        }
    }

    private void Update()
    {


        if(Application.isPlaying)
        {
            // Game
            if (Camera.main == null)
                return;

            UpdatePreviewCamera(Camera.main);
        }
        else
        {
            // Scene
#if UNITY_EDITOR
            Camera sceneCamera = SceneView.lastActiveSceneView?.camera;

            if (sceneCamera != null)
            {
                UpdatePreviewCamera(sceneCamera);
            }
#endif
        }
    }

    // ==================================================
    // ----- Preview Event -----
    // ==================================================  
    private void UpdatePreviewCamera(Camera targetCamera)
    {
        if (targetCamera == null || _previewCamera == null)
        {
            return;
        }


        if (_previewObject == null)
        {
            return;
        }

        Renderer[] renderers = _previewObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            return;
        }

        
        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        float distance = bounds.extents.magnitude * 2.5f;

        Vector3 forward = targetCamera.transform.forward;

        _previewCamera.transform.position =
            bounds.center - forward * distance;

        _previewCamera.transform.LookAt(bounds.center);
    }


    private void CreatePreviewObject(GameObject obj)
    {


        // プレビューオブジェクトを削除
        if (_previewObject != null)
        {
            if (Application.isPlaying)
            {
                Destroy(_previewObject);
            }
            else
            {
                DestroyImmediate(_previewObject);
            }

            _previewObject = null;
        }

        if (obj == null)
        {
            return ;
        }

        // 生成
        GameObject go = Instantiate(obj); 
        go.transform.SetParent(transform, false);
        go.transform.localPosition = Vector3.zero;

        // レイヤー設定
        int layer = LayerMask.NameToLayer("Preview");
        if (layer != -1)
        {
            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = layer;
            }
        }

        _previewObject = go;
    }

    private void CreateRenderTexture()
    {
        if (_renderTexture != null)
        {
            return;
        }

        var rt = new RenderTexture(
            512,
            512,
            24,
            RenderTextureFormat.ARGB32
        );
        rt.Create();


        _renderTexture = rt;
    }
}
