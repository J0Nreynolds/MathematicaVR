using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FunctionClickListener : MonoBehaviour {
    void Start()
  {
      GameObject textField = GameObject.Find("InputField");
      Button myselfButton = GetComponent<Button>();
      myselfButton.onClick.AddListener(() => performClick(textField.GetComponent<InputField>().text));
  }
 
  void performClick(string fun)
  {
  }
}
