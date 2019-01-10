using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class JumpingNumberTextComponent : MonoBehaviour
{
    [SerializeField]
    [Tooltip("From the highest order to loweset order, setting every number Text (Current Number)")]
    private List<Text> _numbers;
    [SerializeField]
    [Tooltip("From the highest order to loweset order, setting every number Text (Replaced Number)")]
    private List<Text> _unactiveNumbers;
    /// <summary>
    /// the animation duration time
    /// </summary>
    [SerializeField]
    private float _duration = 0.05f;
    /// <summary>
    /// Each Rolling Duration time of Number
    /// </summary>
    [SerializeField]
    private float _rollingDuration = 0.005f;
    /// <summary>
    /// The nubmer gap in each change
    /// </summary>
    private int _speed;
    /// <summary>
    /// rolling delay time (double when the number order increases）
    /// </summary>
    [SerializeField]
    private float _delay = 0.002f;

    /// <summary>
    /// Text Size Height and Width
    /// </summary>
    private Vector2 _numberSize;
    /// <summary>
    /// current number
    /// </summary>
    private int _curNumber = 1;
    /// <summary>
    /// initial number
    /// </summary>
    private int _fromNumber;
    /// <summary>
    /// then end number
    /// </summary>
    private int _toNumber;
    /// <summary>
    /// instance of tweener or each number 
    /// </summary>
    private List<Tweener> _tweener = new List<Tweener>();
    /// <summary>
    /// whether in the process of rolling
    /// </summary>
    private bool _isJumping;
    /// <summary>
    /// call-back function when finish rolling 
    /// </summary>
    public Action OnComplete;
    /// <summary>
    /// the difference between from number and to number
    /// </summary>

    private float _different;

    private void Awake()
    {
        if (_numbers.Count == 0 || _unactiveNumbers.Count == 0)
        {
            Debug.Log("[JumpingNumberTextComponent] 还未设置Text组件!");
            return;
        }
        _numberSize = _numbers[0].rectTransform.anchoredPosition;
    }

    public float duration
    {
        get { return _duration; }
        set
        {
            _duration = value;
        }
    }

    public float different
    {
        get { return _different; }
    }

    public void Change(int from, int to)
    {
        //if alreay in the process of changing
        bool isRepeatCall = _isJumping && _fromNumber == from && _toNumber == to;
        if (isRepeatCall) return;

        bool isContinuousChange = (_toNumber == from) && ((to - from > 0 && _different > 0) || (to - from < 0 && _different < 0));
        if (_isJumping && isContinuousChange)
        {
        }
        else
        {
            _fromNumber = from;
            _curNumber = _fromNumber;
        }
        _toNumber = to;

        _different = _toNumber - _fromNumber;
        _speed = (int)Math.Ceiling(_different / (_duration * (1 / _rollingDuration)));
        _speed = _speed == 0 ? (_different > 0 ? 1 : -1) : _speed;

        SetNumber(_curNumber, false);
        _isJumping = true;
        StopCoroutine("DoJumpNumber");
        StartCoroutine("DoJumpNumber");
    }

    public int number
    {
        get { return _toNumber; }
        set
        {
            if (_toNumber == value) return;
            Change(_curNumber, _toNumber);
        }
    }

    IEnumerator DoJumpNumber()
    {
        while (true)
        {
            if (_speed > 0)//add
            {
                _curNumber = Math.Min(_curNumber + _speed, _toNumber);            
            }
            else if (_speed < 0) //reduce
            {
                _curNumber = Math.Max(_curNumber + _speed, _toNumber);
            }
            SetNumber(_curNumber, true);

            if (_curNumber == _toNumber)
            {
                StopCoroutine("DoJumpNumber");
                _isJumping = false;
                //hide all the unactiveNumbers and show all the numbers
                ShowCurrentNumbers(true);
                ShowUnactiveNumbers(false);

                if (OnComplete != null) OnComplete();
                yield return null;
            }
            yield return new WaitForSeconds(_rollingDuration);
        }
    }

    
    /// <summary>
    /// set number in TEXT
    /// </summary>
    /// <param name="v"></param>
    /// <param name="isTween"></param>
    public void SetNumber(int v, bool isTween)
    {
        char[] c = v.ToString().ToCharArray();
        Array.Reverse(c);
        string s = new string(c);

        if (!isTween)
        {
            for (int i = 0; i < _numbers.Count; i++)
            {
                if (i < s.Count())
                    _numbers[i].text = s[i] + "";
                else
                    _numbers[i].text = "0";
            }
        }
        else
        {
            while (_tweener.Count > 0)
            {
                _tweener[0].Complete();
                _tweener.RemoveAt(0);
            }

            for (int i = 0; i < _numbers.Count; i++)
            {
                if (i < s.Count())
                {
                    _unactiveNumbers[i].text = s[i] + "";
                }
                else
                {
                    _unactiveNumbers[i].text = "0";
                }
              
                _unactiveNumbers[i].rectTransform.anchoredPosition = new Vector2(_unactiveNumbers[i].rectTransform.anchoredPosition.x, (_speed > 0 ? -0.5f : 0.5f) * _numberSize.y );
                //0.5f is the offset of the y position of _unactiveNumbers move
                _numbers[i].rectTransform.anchoredPosition = new Vector2(_unactiveNumbers[i].rectTransform.anchoredPosition.x, _numberSize.y);

                if (_unactiveNumbers[i].text != _numbers[i].text)
                {
                    _unactiveNumbers[i].gameObject.SetActive(true);
        
                    DoTween(_numbers[i], _numberSize.y, _delay * i);
                    DoTween(_unactiveNumbers[i], _numberSize.y, _delay * i);

                    Text tmp = _numbers[i];
                    _numbers[i] = _unactiveNumbers[i];
                    _unactiveNumbers[i] = tmp;
                }
                else
                {
                    //when _unactiveNumbers[i].text == _numbers[i].text, hide _unactiveNumbers[i]
                    _unactiveNumbers[i].gameObject.SetActive(false);

                }

            }
        }
    }

    public void DoTween(Text text, float endValue, float delay)
    {
        Tweener t = DOTween.To(() => text.rectTransform.anchoredPosition, (x) =>
        {
            text.rectTransform.anchoredPosition = x;
        }, new Vector2(text.rectTransform.anchoredPosition.x, endValue), _rollingDuration - delay).SetDelay(delay);
        _tweener.Add(t);
    }

    [ContextMenu("Test Nubmer changes")]
    public void TestChange( )
    {
        //active all the unactivenumbers
        ShowCurrentNumbers(true);
        ShowUnactiveNumbers(true);

        int ram = UnityEngine.Random.Range(1, 999999);
        Debug.Log(ram);
        if (_curNumber == 1)
        {
            Change(UnityEngine.Random.Range(1, 1), ram);
        }
        else
        {
          Change(_curNumber, ram);
        }
   

    }

    private void ShowCurrentNumbers(bool show)
    {
        for (int i = 0; i < _numbers.Count; i++)
        {
         
           _numbers[i].gameObject.SetActive(show);
      
        }
    }

    private void ShowUnactiveNumbers(bool show)
    {
        for (int i = 0; i < _unactiveNumbers.Count; i++)
        {

            _unactiveNumbers[i].gameObject.SetActive(show);

        }
    }
}