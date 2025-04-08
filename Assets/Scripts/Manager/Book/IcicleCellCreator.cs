using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// [ExecuteAlways]
public class IcicleCellCreator : MonoBehaviour
{
    [SerializeField] private IcicleSO icicleSO;

    [SerializeField] private GameObject[] cellPrefabs = new GameObject[20]; // つららのセルの大元のObject
    [SerializeField] private TextMeshProUGUI[] icicleNames = new TextMeshProUGUI[20]; // つららの名前
    [SerializeField] private Image[] images = new Image[20]; // つららの画像
    [SerializeField] private Slider[] sliders = new Slider[20]; // つららをどれくらい収穫したか
    [SerializeField] private Image[,] rares = new Image[20, 6]; // つららのレア度
    [SerializeField] private TextMeshProUGUI[] coinTexts = new TextMeshProUGUI[20]; // つらら収穫時のコイン

    // debug用
    [SerializeField] private GameObject content;

    void Start()
    {
        // for (int i = 0; i < 20; i++)
        // {
        //     cellPrefabs[i] = content.transform.GetChild(i).gameObject;
        //     icicleNames[i] = cellPrefabs[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        //     images[i] = cellPrefabs[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();
        //     sliders[i] = cellPrefabs[i].transform.GetChild(2).GetComponent<Slider>();
        //     for (int j = 0; j < 6; j++)
        //     {
        //         rares[i, j] = cellPrefabs[i].transform.GetChild(3).GetChild(0).GetChild(j).GetComponent<Image>();
        //     }
        //     coinTexts[i] = cellPrefabs[i].transform.GetChild(3).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        // }

        for (int i = 0; i < 20; i++)
        {
            cellPrefabs[i].transform.localPosition = new Vector3(cellPrefabs[i].transform.localPosition.x, -i * 500, 0);
            icicleNames[i].text = icicleSO.icicles[i].icicleName;

            Sprite icicleSprite = icicleSO.icicles[i].image;
            if (icicleSprite != null) images[i].sprite = icicleSprite;

            coinTexts[i].text = icicleSO.icicles[i].iciclePoint.ToString();
        }
    }

    public void CreateIcicleCell(int index)
    {
    }
}
