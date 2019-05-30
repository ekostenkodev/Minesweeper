using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour
{

    public bool IsTouchable;
    private GameSystem _gameSystem;
    [SerializeField]
    private float _flagTapTime;
    private Vector3 _touchStart,_cameraTouchStartPosition;
    private float _movingOffsetValue = Screen.width * 0.0005f;
    private float _startTouchTime,_realTime;
    private float _defaultSize;
    private float _zoomOutMin,_zoomOutMax;

    private bool _movingTouch = false, _flagTouch = false;
    [SerializeField]
    [Range(0,1)]
    private float _movingSpeed = 0.15f;

    [Space]
    [SerializeField]
    private float _tilesPadding = 0;


	
    private void Awake() 
    {
        _gameSystem = GetComponent<GameSystem>();
        _gameSystem.GameStartEvent+=ChangeScreenResolution;
        _gameSystem.GameStartEvent+= () => IsTouchable = true;
        _gameSystem.EndGameEvent+= (result) => IsTouchable = false;

        _flagTapTime = PlayerPrefs.GetFloat("FlagTapTime",0.25f);

    }

    private void ChangeScreenResolution()
    {
        int _tilesInBeginOnScreen = 9;

        float tileSize = _gameSystem.TileFabric.TilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        _defaultSize = _tilesInBeginOnScreen * (_gameSystem.TileFabric.Offset+tileSize) * Screen.height / Screen.width * 0.5f + _tilesPadding;
        _zoomOutMin = _tilesInBeginOnScreen * tileSize * 0.5f;
        _zoomOutMax = _gameSystem.TileFabric.TotalSize * Screen.height / Screen.width * 0.5f + _tilesPadding;

        SetCameraInDefaultPosition();
    }


    public void SetCameraInDefaultPosition()
    {
        Camera.main.orthographicSize = _defaultSize;
        Camera.main.transform.position = new Vector3(0,0,-50);
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
	void Update () 
    {
        
        if(Input.touchCount==0 || !IsTouchable || IsPointerOverUIObject())
            return;
        if(Input.touchCount == 2)
        {
            float difference = calcDifferenceMove();
            ZoomCamera(difference * 0.01f);
            _movingTouch = true;
            return;
        }
        
        Touch touch = Input.GetTouch(0);

        if(touch.phase == TouchPhase.Began)
        {

            // Debug.Log("Beg");
            _flagTouch = false;
            _movingTouch = false;
            _startTouchTime = Time.time;
            _touchStart = Camera.main.ScreenToWorldPoint(touch.position);
            _cameraTouchStartPosition = Camera.main.transform.position;
        }
        
        if(touch.phase == TouchPhase.Moved)
        {
            Vector3 direction = _touchStart - Camera.main.ScreenToWorldPoint(touch.position);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,Camera.main.transform.position+direction,_movingSpeed);

            if(Vector3.Distance(Camera.main.transform.position,_cameraTouchStartPosition) >= _movingOffsetValue)
            {

                _movingTouch = true;
                Debug.Log(_cameraTouchStartPosition +"  "+Camera.main.transform.position+"   =   " +Vector3.Distance(Camera.main.transform.position,_cameraTouchStartPosition));
            }
        }

        

        if(touch.phase == TouchPhase.Stationary) 
        {
            if(_movingTouch || _flagTouch)
                return;

            _realTime = Time.time - _startTouchTime;
            _flagTouch = _realTime > _flagTapTime;

            if(_flagTouch)
            {
                TileComponent tile;
                if((tile = CastRay(touch)) == null)
                    return;

                tile.OnClick(true);
            }
        }

        if(touch.phase == TouchPhase.Ended) 
        {

            // Debug.Log("end");
            if(_movingTouch || _flagTouch)
                return;
            
            TileComponent tile;
            if((tile = CastRay(touch)) == null)
                return;

            tile.OnClick(false);
        }  
        
	}


    TileComponent CastRay(Touch touch)
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector2 touchPos2D = new Vector2(touchPos.x, touchPos.y);
        
        RaycastHit2D hit = Physics2D.Raycast(touchPos2D, Vector2.zero);

        if (hit.collider != null) 
        {
            return hit.transform.GetComponent<TileComponent>();
        }

        return null;
    }

    private float calcDifferenceMove()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;
        return difference;
    }

    void ZoomCamera(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, _zoomOutMin, _zoomOutMax);
    }
    
}
