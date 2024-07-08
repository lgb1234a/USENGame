// Created by LunarEclipse on 2024-7-5 18:37.

using System;
using System.Collections.Generic;
using DG.Tweening;
using Luna.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Modules.UI.Widgets
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class ListView<T, U> : Widget where T : ListViewCell<U>
    {
        [SerializeField] 
        private List<U> data;
        
        public virtual List<U> Data
        {
            get => data;
            set
            {
                data = value;
                Initialize();
            }
        }
        
        public T cell;
        public bool snapToCellWhenSelected = true;
        
        protected ScrollRect _scrollRect;
        protected readonly List<T> _cells = new();

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
            Initialize();
        }
        

        public void Initialize()
        {
            // Clear existing cells
            foreach (var cell in _cells)
            {
                cell.OnCellSelected -= OnCellSelected;
                cell.OnCellDeselected -= OnCellDeselected;
                cell.OnCellSubmitted -= OnCellSubmitted;
                Destroy(cell.gameObject);
            }
            _cells.Clear();
            
            // Create new cells
            CreateCells();
        }
        
        private void CreateCells()
        {
            var content = _scrollRect.content;
            for (int i = 0; i < Data.Count; i++)
            {
                var newCell = Instantiate(cell, content);
                _cells.Add(newCell);
                newCell.Index = i;
                newCell.Data = Data[i];
                newCell.OnCellSelected += OnCellSelected;
                newCell.OnCellDeselected += OnCellDeselected;
                newCell.OnCellSubmitted += OnCellSubmitted;
                if (snapToCellWhenSelected)
                {
                    newCell.OnCellSelected += (index, listViewCell) =>
                    {
                        SnapTo(listViewCell.transform as RectTransform);
                    };
                }
                newCell.gameObject.SetActive(true);
            }
        }

        protected abstract void OnCellSubmitted(int index, ListViewCell<U> listViewCell);

        protected abstract void OnCellDeselected(int index, ListViewCell<U> listViewCell);

        protected abstract void OnCellSelected(int index, ListViewCell<U> listViewCell);

        private void OnScrollValueChanged(Vector2 normalizedPosition)
        {
            // UpdateVisibleItems();
        }
        
        public void SnapTo(RectTransform target)
        {
            var y = -target.anchoredPosition.y - ((RectTransform)_scrollRect.transform).sizeDelta.y;
            y = Mathf.Clamp(y, 0, _scrollRect.content.sizeDelta.y);
            var pos = new Vector2(_scrollRect.content.anchoredPosition.x, y);
            DOTween.To(() => _scrollRect.content.anchoredPosition, v => _scrollRect.content.anchoredPosition = v, pos, 0.5f);
        }
        
        // public void SnapTo(RectTransform target)
        // {
        //     var newPos =
        //         (Vector2)_scrollRect.transform.InverseTransformPoint(_scrollRect.content.position)
        //         - (Vector2)_scrollRect.transform.InverseTransformPoint(target.position);
        //     var y = Mathf.Clamp(newPos.y - 80f, 0, _scrollRect.content.sizeDelta.y);
        //     var pos = new Vector2(_scrollRect.content.anchoredPosition.x, y);
        //     Debug.Log("SnapTo: " + pos);
        //     DOTween.To(() => _scrollRect.content.anchoredPosition, v => _scrollRect.content.anchoredPosition = v, pos, 0.5f);
        // }

        // private void UpdateVisibleItems()
        // {
        //     float scrollHeight = scrollRect.viewport.rect.height;
        //     float scrollPos = scrollRect.content.anchoredPosition.y;
        //
        //     startIndex = Mathf.FloorToInt(scrollPos / itemHeight);
        //     endIndex = Mathf.CeilToInt((scrollPos + scrollHeight) / itemHeight);
        //
        //     startIndex = Mathf.Clamp(startIndex, 0, itemCount - 1);
        //     endIndex = Mathf.Clamp(endIndex, 0, itemCount - 1);
        //
        //     for (int i = 0; i < content.childCount; i++)
        //     {
        //         content.GetChild(i).gameObject.SetActive(i >= startIndex && i <= endIndex);
        //     }
        // }
        //
        // private void CalculateItemHeight()
        // {
        //     if (content.childCount > 0)
        //     {
        //         RectTransform firstItem = content.GetChild(0) as RectTransform;
        //         RectTransform lastItem = content.GetChild(content.childCount - 1) as RectTransform;
        //
        //         itemHeight = Mathf.Abs(firstItem.anchoredPosition.y - lastItem.anchoredPosition.y) / (content.childCount - 1);
        //     }
        // }
    }
    
}


public class ListViewCell<T> : Widget, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    virtual public T Data { get; set; }
    
    public int Index { get; set; }
        
    public event Action<int, ListViewCell<T>> OnCellSelected;
    public event Action<int, ListViewCell<T>> OnCellDeselected;
    public event Action<int, ListViewCell<T>> OnCellSubmitted;

    public virtual void OnSelect(BaseEventData eventData)
    {
        OnCellSelected?.Invoke(Index, this);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        OnCellDeselected?.Invoke(Index, this);
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        OnCellSubmitted?.Invoke(Index, this);
    }
}
