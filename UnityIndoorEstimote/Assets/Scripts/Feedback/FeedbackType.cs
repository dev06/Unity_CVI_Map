using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public enum Feedback
{
	None, 
	Ping, 
	Voice,
	Haptic
}
public class FeedbackType : MonoBehaviour {

	public Feedback type; 

	public bool selected; 

	public Toggle toggle; 

}
