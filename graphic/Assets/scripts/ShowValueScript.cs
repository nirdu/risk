using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// get the value from the scrollbar
/// </summary>
public class ShowValueScript : MonoBehaviour
{
    Text forcesNumber=null;

    // Start is called before the first frame update
    void Start()
    {
        this.forcesNumber = gameObject.GetComponent<Text>();   
    }

    /// <summary>
    /// show the value
    /// </summary>
    /// <param name="value">the value of the scrollbar</param>
  public void updateNumSoldiers(float value)
    {
        if (this.forcesNumber != null)
        {
            this.forcesNumber.text = Mathf.RoundToInt(value).ToString();
        }
    }

}
