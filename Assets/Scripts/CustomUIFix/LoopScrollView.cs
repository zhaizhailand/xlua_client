using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public enum ScrollType{
    Horizontal,
    Virtical
}

//可复用Item的ScrollView，只支持单向滑动（水平/垂直滑动）
public class LoopScrollView : MonoBehaviour
{
    [Header("固定Item数量")]
    public int fixedCount = 10;

    //所有item的数量
    public int totalCount = 20;

    [Header("Item预制")]
    public GameObject celltempObj;

    //item预制大小
    public Vector2 cellSize = new Vector2(100,100);

    private RectTransform contentRectTrans;
    private ScrollRect scrollRect;
    private ScrollType scrollType = ScrollType.Horizontal;
    private GridLayoutGroup gridLayout;

    //ScrollView内显示的第一行item的行数
    private int headRow = 0;
    //ScrollView内显示的第一行item的索引
    private int headIndex = 0;
    //最后一行item的行数
    private int tailRow = 0;
    //ScrollView内显示的最后一行item的索引
    private int tailIndex = 0;

    //每行/列显示rowCount个item
    [Header("每行/列显示的item个数")]
    public int constriantCount = 0;
    //最大行/列数
    private int maxRowColumn = 0;

    //所有item列表
    private List<RectTransform> itemsPool = new List<RectTransform>();

    private bool init_suc = true;

    //每行item的高度/每列item的长度
    private float sizeXY = 0;

    private Vector2 leftUpperAnchor = new Vector2(0, 1);
    private Vector2 rightUpperAnchor = new Vector2(1, 1);
    private Vector2 leftBottomAnchor = new Vector2(0, 0);


    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        if(scrollRect == null)
        {
            Debug.LogError("[loopscrollview] Not Find ScrollRect Component.");
            init_suc = false;
            return;
        }
        contentRectTrans = scrollRect.content;
        scrollRect.onValueChanged.AddListener((Vector2 vec) => OnScrollMove(vec));
        if (scrollRect.horizontal)
        {
            scrollType = ScrollType.Horizontal;

            contentRectTrans.anchorMin = leftBottomAnchor;
            contentRectTrans.anchorMax = leftUpperAnchor;
        }
        else
        {
            scrollType = ScrollType.Virtical;

            contentRectTrans.anchorMin = leftUpperAnchor;
            contentRectTrans.anchorMax = rightUpperAnchor;
        }
       // Debug.Log("[loopscrollview] scrollType = "+ scrollType.ToString());

        InitLayout();
        tailRow = Mathf.CeilToInt(fixedCount / constriantCount) - 1;
        maxRowColumn = Mathf.CeilToInt(totalCount / constriantCount) - 1;
        InitItems();
        InitContentSize();
    }


    //初始化fixedCount个item
    private void InitItems()
    {
        if(celltempObj == null)
        {
            Debug.LogError("[loopscrollview] celltempObj null");
            init_suc = false;
            return;
        }
        for (int i = 0; i < fixedCount; i ++)
        {
            GameObject obj = Instantiate(celltempObj, contentRectTrans);
            obj.SetActive(true);
            RectTransform trans = obj.GetComponent<RectTransform>();
            itemsPool.Add(trans);
            InitItem(trans, i);
        }
    }

    //初始化GridLayoutGroup配置
    private void InitLayout()
    {
        gridLayout = contentRectTrans.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            Debug.LogError("[loopscrollview] Content has no GridLayoutGroup Component.");
            init_suc = false;
            return;
        }
        gridLayout.cellSize = cellSize;
       
        if (scrollType == ScrollType.Horizontal)
        {
            gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
            gridLayout.childAlignment = TextAnchor.UpperLeft;

            gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            sizeXY = gridLayout.padding.left + cellSize.x + gridLayout.spacing.x;
        }
        else if (scrollType == ScrollType.Virtical)
        {
            gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
            gridLayout.childAlignment = TextAnchor.UpperLeft;

            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            sizeXY = gridLayout.padding.top + cellSize.y + gridLayout.spacing.y;
        }
        gridLayout.constraintCount = constriantCount;
    }

    //初始化Content size
    private void InitContentSize()
    {
        Vector2 size = new Vector2(0, 0);
        //计算行/列
        if (scrollType == ScrollType.Virtical)
        {
            
            size.y = gridLayout.padding.top + (maxRowColumn + 1) * (gridLayout.cellSize.y + gridLayout.spacing.y) + gridLayout.padding.bottom;
        }
        else
        {
            float width = gridLayout.padding.left + (maxRowColumn + 1) * (gridLayout.cellSize.x + gridLayout.spacing.x) + gridLayout.padding.right;
            size.x = width;
        }
        //Debug.Log("[loopscrollview] size = " + size.ToString());
        contentRectTrans.sizeDelta = size;
    }

    //item显示逻辑
    private void InitItem(RectTransform trans,int idx)
    {
        trans.name = (idx).ToString();
        trans.GetComponentInChildren<TextMeshProUGUI>().text = idx.ToString();

        float posx = 0;
        float posy = 0;
        if(scrollType == ScrollType.Virtical)
        {
            int n = idx % constriantCount;
            posx = gridLayout.padding.left + n * (gridLayout.cellSize.x + gridLayout.spacing.x);

            int row = Mathf.CeilToInt(idx / constriantCount);
            posy = gridLayout.padding.top + row * (gridLayout.cellSize.y + gridLayout.spacing.y);
            posy = -posy;
        }
        else
        {
            int n = idx % constriantCount;
            posy = gridLayout.padding.top + n * (gridLayout.cellSize.y + gridLayout.spacing.y);
            posy = -posy;

            int column = Mathf.CeilToInt(idx / constriantCount);
            posx = gridLayout.padding.left + column * (gridLayout.cellSize.x + gridLayout.spacing.x);
        }

        trans.anchoredPosition = new Vector2(posx, posy);
    }

    private void OnScrollMove(Vector2 vec)
    {
        if(!init_suc)
        {
            Debug.LogError("[loopscrollview] init failed.");
            return;
        }
        if(itemsPool.Count <= 0)
        {
            Debug.LogError("[loopscrollview] Item Pool empty");
            return;
        }
        if(scrollType == ScrollType.Virtical)
        {
            OnVirticleScrollMove(vec);
        }
        else
        {
            OnHorizontalScrollMove(vec);
        }
    }

    //上下滑动
    private void OnVirticleScrollMove(Vector2 vec)
    {
        // Debug.Log("[loopscrollview]  contentRectTrans.anchoredPosition = " + contentRectTrans.anchoredPosition.y.ToString());
        //向上滑
        while (contentRectTrans.anchoredPosition.y >= (headRow + 1) * sizeXY && tailRow < maxRowColumn)
        {
            //Debug.LogError("向上滑 headRow = "+ headRow.ToString());
            //将第headIndex行item移动到最后
            int itemCount = constriantCount;
            if (headRow >= maxRowColumn)
            {
                itemCount = totalCount - maxRowColumn * constriantCount;
            }

            int startIdx = (tailRow + 1) * constriantCount;
            for (int i = 0; i < itemCount; i ++)
            {
                RectTransform item = itemsPool[0];
                itemsPool.Remove(item);
                itemsPool.Add(item);
                InitItem(item, startIdx + i);
            }
            headRow = headRow + 1;
            tailRow = tailRow + 1;
            //Debug.Log("headRow = " + headRow.ToString() + "tailRow = "+ tailRow.ToString() + "maxRowColumn = "+maxRowColumn.ToString());
        }

        //向下滑
       while(contentRectTrans.anchoredPosition.y <= headRow * sizeXY && headRow > 0)
        {
            //Debug.LogError("向下滑");
            //将最后一行的item移动到前排
            int itemCount = constriantCount;
            if (tailRow >= maxRowColumn)
            {
                itemCount = totalCount - maxRowColumn * constriantCount;
            }
            int startIdx = (headRow - 1) * constriantCount;
            for (int i = 0; i < itemCount; i++)
            {
                RectTransform item = itemsPool[itemsPool.Count-1];
                itemsPool.Remove(item);
                itemsPool.Insert(0,item);
                InitItem(item, startIdx + i);
            }
            headRow = headRow - 1;
            tailRow = tailRow - 1;
        }
    }
    
    //左右滑动
   private void OnHorizontalScrollMove(Vector2 vec)
   {
        //向右滑
        while (-contentRectTrans.anchoredPosition.x >= (headRow + 1) * sizeXY && tailRow < maxRowColumn)
        {
            //Debug.LogError("向右滑 headRow = " + headRow.ToString());
            //将第headRow列item移动到最后
            int itemCount = constriantCount;
            if (headRow >= maxRowColumn)
            {
                itemCount = totalCount - maxRowColumn * constriantCount;
            }

            int startIdx = (tailRow + 1) * constriantCount;
            for (int i = 0; i < itemCount; i++)
            {
                RectTransform item = itemsPool[0];
                itemsPool.Remove(item);
                itemsPool.Add(item);
                InitItem(item, startIdx + i);
            }
            headRow = headRow + 1;
            tailRow = tailRow + 1;
            //Debug.Log("headRow = " + headRow.ToString() + "tailRow = " + tailRow.ToString() + "maxRowColumn = " + maxRowColumn.ToString());
        }

        //向左滑
        while (-contentRectTrans.anchoredPosition.x <= headRow * sizeXY && headRow > 0)
        {
            //Debug.LogError("向左滑");
            //将最后一列的item移动到前排
            int itemCount = constriantCount;
            if (tailRow >= maxRowColumn)
            {
                itemCount = totalCount - maxRowColumn * constriantCount;
            }
            int startIdx = (headRow - 1) * constriantCount;
            for (int i = 0; i < itemCount; i++)
            {
                RectTransform item = itemsPool[itemsPool.Count - 1];
                itemsPool.Remove(item);
                itemsPool.Insert(0, item);
                InitItem(item, startIdx + i);
            }
            headRow = headRow - 1;
            tailRow = tailRow - 1;
        }
    }

}
