using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PolygonCollider2D))]
public class CountryHandler : MonoBehaviour
{
    private SpriteRenderer sprite;
    public int indexInMap;
    public int numberOfF;
    public int rulerOfState;
    /// <summary>
    /// define the variables 
    /// </summary>
    void Awake()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        this.indexInMap= int.Parse(gameObject.name)-1;
        if (sprite)
        {
            sprite.sortingOrder = 1;
            sprite.sortingLayerName = "countries";
            this.sprite.color = Color.blue;
        }
    }
    /// <summary>
    /// cange the ruler and the number of forces every time
    /// </summary>
    private void Update()
    {
        //update the virables 
        this.numberOfF = Global.map.map[this.indexInMap].Info.Num_of_forces;
        this.rulerOfState = (int)Global.map.map[this.indexInMap].Info.Ruled_by;
        //color of ruler
        this.sprite.color= Global.colors[rulerOfState];
        //change number of forces
        if (transform.childCount > 0)
        {
            gameObject.GetComponentsInChildren<Text>()[0].text = this.numberOfF.ToString();
        }
       
    }
    /// <summary>
    /// get the input
    /// </summary>
    private void OnMouseDown()
    {
        CountryManager.insteance.getInput(int.Parse(gameObject.name) - 1);//send the wanted country
    }
}
