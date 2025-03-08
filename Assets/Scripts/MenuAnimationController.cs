using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimationController : MonoBehaviour
{
    [SerializeField] private List<GameObject> menuObject;
    [SerializeField] private GameObject activeObject;


    public void SetActiveNewMenu(int id)
    {
        if (menuObject[id] == activeObject) return;
        activeObject.GetComponent<Animation>().Play("close");

        menuObject[id].GetComponent<Animation>().Play("open");
        activeObject = menuObject[id];
    }
}
