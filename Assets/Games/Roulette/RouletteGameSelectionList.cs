using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Luna.Core.Pool;
using Luna.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace USEN.MiniGames.Roulette
{
    
    public class RouletteGameSelectionList : StatefulWidget<RouletteGameSelectionList.State>, IEventSystemHandler
    {
        public List<RouletteSectors> rouletteData;
        public GameObject cellPrefab;
        
        protected ScrollRect listView;
        
        void Awake()
        {
            listView = GetComponent<ScrollRect>();
            CreateCells();
        }
            
        async Task OnEnable()
        {
            // Select first cell
            var firstCell = listView.content.GetChild(0).gameObject;
            if (firstCell != null)
            {
                await UniTask.NextFrame();
                EventSystem.current.SetSelectedGameObject(firstCell);
            }
        }
        
        void Start()
        {
            
        }
        
        void Update()
        {
            
        }
        
        void CreateCells()
        {
            foreach (var data in rouletteData)
            {
                GameObject cell = ObjectPool.Get(cellPrefab);
                cell.SetActive(true);
                cell.transform.SetParent(listView.content, false);
                cell.GetComponent<RouletteGameSelectionListCell>().rouletteData = data;
            }
        }
        public void SnapTo(RectTransform target)
        {
            var y = -target.anchoredPosition.y - ((RectTransform)listView.transform).rect.height;
            y = Mathf.Clamp(y, 0, listView.content.rect.height);
            var pos = new Vector2(listView.content.anchoredPosition.x, y);
            DOTween.To(() => listView.content.anchoredPosition, v => listView.content.anchoredPosition = v, pos, 0.5f);
        }
        
        public class State : Luna.UI.State
        {
            public override void InitState()
            {
                base.InitState();
                
                
            }
        }

    }

}
