using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    void Start()
    {
        _slider.value = PlayerPrefs.GetFloat("FlagTapTime",0.25f);
    }
    public void ChangeFlagTapTime(float newTapTime)
    {
        PlayerPrefs.SetFloat("FlagTapTime",newTapTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
