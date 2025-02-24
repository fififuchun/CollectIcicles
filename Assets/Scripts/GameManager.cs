using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Canvas canvas;
    private RectTransform canvasRectTransform;

    [SerializeField] private BoxCollider2D[] boxColliders = new BoxCollider2D[Const.maxIcicle];
    private Rigidbody2D[] rigidbodies = new Rigidbody2D[Const.maxIcicle];

    void Start()
    {
        // CanvasのRectTransformを事前に取得
        canvasRectTransform = canvas.GetComponent<RectTransform>();

        for (int i = 0; i < Const.maxIcicle; i++)
        {
            rigidbodies[i] = boxColliders[i].GetComponent<Rigidbody2D>();
            rigidbodies[i].gravityScale = 0;
        }
    }

    void Update()
    {
        // タップまたはマウス左クリックをしている間
        if (Input.GetMouseButton(0))
        {
            // スクリーン座標を取得
            Vector2 screenPosition = Input.mousePosition;

            // スクリーン座標をCanvasのローカル座標に変換
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform, // 事前に取得したRectTransformを使用
                screenPosition, // スクリーン座標
                canvas.worldCamera, // カメラを指定
                out localPosition // ローカル座標を格納
            );

            // 画像オブジェクトのColliderを確認
            for (int i = 0; i < Const.maxIcicle; i++)
            {
                // Colliderのワールド座標に変換
                // Vector2 worldPosition = boxColliders[i].transform.TransformPoint(boxColliders[i].offset);

                // localPositionをワールド座標に変換
                Vector2 worldLocalPosition = canvasRectTransform.TransformPoint(localPosition);

                // タップ位置がColliderの範囲内に含まれているかチェック
                if (boxColliders[i].bounds.Contains(worldLocalPosition))
                {
                    // Debug.Log(boxColliders[i].gameObject.name);
                    boxColliders[i].enabled = false;
                    rigidbodies[i].gravityScale = 20;
                }
            }
        }
    }
}
