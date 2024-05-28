// Created by LunarEclipse on 2024-5-28 3:23.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[ExecuteAlways]
public class Poker2D : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite front;
    public Sprite back;
    [SerializeField] private bool perspective = true;
    
    private Image _image;
    private UIFlippable _flippable;
    
    private Camera _mainCamera;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _flippable = GetComponent<UIFlippable>();
    }
    
    private void Start()
    {
        _image.sprite = front;
        _mainCamera = Camera.main;
    }
    
    private void Update()
    {
        var isBack = perspective ? !IsFacingCamera() : transform.rotation.eulerAngles.y is > 90 and < 270;
        if (isBack)
        {
            _image.sprite = back;
            _flippable.horizontal = true;
        }
        else
        {
            _image.sprite = front;
            _flippable.horizontal = false;
        }

        // if (Input.anyKey)
        // {
        //     transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y - 180f, 0), 2).OnUpdate(()=> {
        //         
        //     }).OnComplete(()=> {
        //         
        //     });
        // }
    }
    
    private bool IsFacingCamera()
    {
        Vector3 toCamera = (_mainCamera.transform.position - transform.position).normalized;
        var dotProduct = Vector3.Dot(transform.forward, toCamera);
        return dotProduct < 0;
    }
    
}
