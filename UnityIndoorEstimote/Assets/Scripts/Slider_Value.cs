using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Text))]
public class Slider_Value : MonoBehaviour {
    public Slider mySlider;
    Text myText;
	// Use this for initialization
	void Start () {
        myText = GetComponent<Text>();
        mySlider.onValueChanged.AddListener(delegate { ValueChanged(); });
        ValueChanged();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ValueChanged()
    {
        myText.text = string.Format("{0:0.00}", mySlider.value);
    }
}
