using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    private List<Collider> prevSelected = new List<Collider>();
    [SerializeField] private RectTransform boxImage;

    [SerializeField] private GameObject boxSelector;

    private Vector3 selectStart;
    private Vector3 selectEnd;

    private Vector3 startPos;
    private Vector3 endPos;

    [SerializeField] private Camera cam;

    private void Start()
    {
        //gui stuff
        boxImage.gameObject.SetActive(false);
    }

    void Update()
    {
        int ignoreUIMask = 1 << 5;
        ignoreUIMask = ~ignoreUIMask;

        Collider[] newlySelected;

        if (Input.GetMouseButtonDown(0))
        {
            //mouse start pos
            startPos = Input.mousePosition;

            RaycastHit hit;

            //point in game where i click
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreUIMask))
            {

                //make sure game always has background or something idk
                selectStart = hit.point;
                Debug.Log(hit.transform.name);

            //clear previously selected objects
            }
            for (int i = 0; i < prevSelected.Count; i++)
            {
                prevSelected[i].gameObject.GetComponent<Movement>().isSelected = false;
            }
            prevSelected.Clear();
        }

        if (Input.GetMouseButtonUp(0))
        {
            //gui
            boxImage.gameObject.SetActive(false);
        }

        if (Input.GetMouseButton(0))
        {
            //GUI stuff
            //object not already active, make it active
            if (!boxImage.gameObject.activeInHierarchy)
            {
                boxImage.gameObject.SetActive(true);
            }

            endPos = Input.mousePosition;

            Vector3 boxStart = Camera.main.WorldToScreenPoint(startPos);
            boxStart.z = 0f;

            Vector3 center = (startPos + endPos) / 2f;

            boxImage.position = center;

            float sizeX = Mathf.Abs(startPos.x - endPos.x);
            float sizeY = Mathf.Abs(startPos.y - endPos.y);

            boxImage.sizeDelta = new Vector2(sizeX, sizeY);


            //selector code
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreUIMask))
            {
                selectEnd = hit.point;
            }

            //Selectable layermask (7)
            int layerMask = 1 << 7;

            //midpoint formula
            Vector3 centerOfOverlap;
            centerOfOverlap.x = (selectStart.x + selectEnd.x) / 2;
            centerOfOverlap.y = 0.25f;
            centerOfOverlap.z = (selectStart.z + selectEnd.z) / 2;

            //half extent does half a square idk lol
            Vector3 halfExtents = (selectStart - selectEnd) / 2;
            halfExtents.x = Mathf.Abs(halfExtents.x);
            halfExtents.z = Mathf.Abs(halfExtents.z);

            //spawn overlapbox to detect everything inside of the mouse click/drag
            newlySelected = Physics.OverlapBox(centerOfOverlap, halfExtents,
                Quaternion.identity, layerMask);
            
            //add new selected to prev selected
            foreach (Collider i in newlySelected)
            {
                if (!i.gameObject.GetComponent<Movement>().isSelected)
                {
                    i.gameObject.GetComponent<Movement>().isSelected = true;
                    prevSelected.Add(i);
                }
            }
        }
    }
}
