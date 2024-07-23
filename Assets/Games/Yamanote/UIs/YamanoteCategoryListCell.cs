using Cysharp.Threading.Tasks;
using Luna.Extensions.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using USEN.Games.Yamanote;

public class YamanoteCategoryListCell : ListViewCell<YamanoteCategory>
{
    public TextMeshProUGUI text;
    public Image background;
    public CircleCollider2D ringCollider; 
    
    private YamanoteCategory _rouletteCategory;
    
    public override YamanoteCategory Data
    {
        get => _rouletteCategory;
        set
        {
            _rouletteCategory = value;
            text.text = value.title;
        }
    }

    protected async void OnValidate()
    {
        await UniTask.DelayFrame(5);
        
        if (ringCollider != null)
        {
            // Cast a horizontal ray to ring collider to get collision point
            var ray = new Ray( new Vector3(ringCollider.transform.position.x + 1000f, background.transform.position.y, 0), Vector3.left);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, LayerMask.GetMask("Editor"));
            Debug.Log(hit.point);
            
            if (hit.collider != null)
            {
                background.transform.SetX(hit.point.x);
            }
        }
    }
}


