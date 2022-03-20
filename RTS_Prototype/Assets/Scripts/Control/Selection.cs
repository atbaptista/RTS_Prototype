using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selection : MonoBehaviour
{
    private List<GameObject> prevSelected = new List<GameObject>();
    [SerializeField] private RectTransform boxImage;

    [SerializeField] private GameObject boxSelector;

    private Vector3 selectStart;
    private Vector3 selectEnd;

    private Vector3 startPos;
    private Vector3 endPos;

    private bool _aPressed;
    private bool _shiftPressed;

    [SerializeField] private Camera cam;

    private void Start()
    {
        //gui stuff
        boxImage.gameObject.SetActive(false);
        Invoke("SelectEverything", 0.5f);

        _aPressed = false;
        _shiftPressed = false;
    }

    void Update()
    {
        Checks();

        Lmb();
        RmbMove();
    }

    private void Checks() {
        if (Input.GetKeyDown(KeyCode.A)) {
            _aPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            _shiftPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Tab)) {
            SelectEverything();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Lmb() {
        int ignoreUIMask = 1 << 5;
        ignoreUIMask = ~ignoreUIMask;

        Collider[] newlySelected;
        Selectable.unitTypes selectedType;

        if (Input.GetMouseButtonDown(0)) {
            //mouse start pos
            startPos = Input.mousePosition;

            RaycastHit hit;

            //point in game where i click
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreUIMask)) {

                //make sure game always has background or something idk
                selectStart = hit.point;
            }

            //left click on non moveable object
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Selectable")) {
                bool isRobot = false;

                //attack move
                if (_aPressed) {
                    foreach (GameObject i in prevSelected) {
                        //null check
                        if (!i.Equals(null)) {
                            isRobot = i.GetComponent<Selectable>().unitType.Equals(Selectable.unitTypes.Robot);

                            if (isRobot) {
                                //IMPLEMENT ATTACK MOVE STATE
                                print("all attack move");
                            }
                        }
                    }
                    _aPressed = false;
                    return;
                } else {
                    ClearPrevSelected();
                    return;
                }
            }

            selectedType = hit.collider.GetComponent<Selectable>().unitType;
            if (_aPressed && selectedType == Selectable.unitTypes.Dinosaur) {
                //IMPLEMENT ATTACK MOVE STATE
                //loop thru each thing and change state or something idk
                print("attack dino!");
                _aPressed = false;
                return;
            }

            //if lmb on a robot
            if (hit.collider.CompareTag("MoveObject") && selectedType == Selectable.unitTypes.Robot) {
                //can add code to check if shift clicking l8er maybe

                //clear previously selected objects
                ClearPrevSelected();

                //if not already selected, select it
                if (!hit.collider.gameObject.GetComponent<Selectable>().isSelected) {
                    hit.collider.gameObject.GetComponent<Selectable>().isSelected = true;
                    prevSelected.Add(hit.collider.gameObject);
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            //gui
            boxImage.gameObject.SetActive(false);
        }

        if (Input.GetMouseButton(0)) {
            //GUI stuff
            //object not already active, make it active
            if (!boxImage.gameObject.activeInHierarchy) {
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
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreUIMask)) {
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
            foreach (Collider i in newlySelected) {
                //if not already selected, select it
                if (!i.gameObject.GetComponent<Selectable>().isSelected) {
                    i.gameObject.GetComponent<Selectable>().isSelected = true;
                    prevSelected.Add(i.gameObject);
                }
            }
        }
    }

    private void RmbMove() {
        if (!Input.GetMouseButtonDown(1)) {
            return;
        }

        bool isRobot = false;

        //Bit shift the index of the ground layer (8) to get a bit mask
        int layerMask = 1 << 8;

        //idk need a maxdistance to input layermask
        float maxDistance = 100f;

        RaycastHit hit;

        //point in game where i click
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //raycast and only hit things in layer 8 (ground)

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask)) {
            foreach (GameObject i in prevSelected) {
                //null check
                if (!i.Equals(null)) {
                    isRobot = i.GetComponent<Selectable>().unitType.Equals(Selectable.unitTypes.Robot);

                    if (isRobot) {
                        i.GetComponent<Moveable>().GoTo(hit.point);
                    }
                }
            }
        }
    }

    private void SelectEverything()
    {
        //get all things in selectable layermask
        Collider[] all = Physics.OverlapSphere(cam.transform.position, 1000f, 1 << 7);
        for (int i = 0; i < all.Length; i++) 
        {
            if (all[i].gameObject.GetComponent<Selectable>().unitType == Selectable.unitTypes.Robot)
            {
                //if not already selected, select it
                if (!all[i].gameObject.GetComponent<Selectable>().isSelected) {
                    all[i].gameObject.GetComponent<Selectable>().isSelected = true;
                    prevSelected.Add(all[i].gameObject);
                }
            }   
        }
    }

    private void ClearPrevSelected() {
        //clear previously selected objects
        for (int i = 0; i < prevSelected.Count; i++) {
            //null check if enemies die while selected
            if (!prevSelected[i].Equals(null)) {
                prevSelected[i].GetComponent<Selectable>().isSelected = false;
            }
        }
        prevSelected.Clear();
    }
}
