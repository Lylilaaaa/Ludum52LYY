
using UnityEngine;
using UnityEngine.UI;

public class CursorRayCast : MonoBehaviour
{
    private Transform PlantUIGameObj;
    public GameObject lastSelectedField;
    public GameObject thisSelectedField;

    void Update()
    {
        PlantInfMouseRayCastTest();   //点一下之后UI会出现
        FieldMouseRayCastTest();
        WaterFieldRayCast();
    }

    private void PlantInfMouseRayCastTest()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Plant" )
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.gameObject.GetComponent<UIShownInformation>().intMatureCounter <
                        hit.collider.gameObject.GetComponent<UIShownInformation>().thisPlants.matureTime)
                    {
                        PlantUIGameObj = hit.collider.gameObject.transform.GetChild(0).GetChild(0).GetChild(0);
                        PlantUIGameObj.gameObject.SetActive(!PlantUIGameObj.gameObject.activeInHierarchy);
                    }
                }
            }
        }
    }
    private void FieldMouseRayCastTest()
    {
        Ray rayField = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitField;
        if (Physics.Raycast(rayField, out hitField))
        {
            //Debug.Log(hitField.collider.gameObject.name);
            if (hitField.collider.gameObject.tag == "PlantThePlant" )
            {
                thisSelectedField = hitField.collider.gameObject;
                thisSelectedField.GetComponent<Image>().enabled = true;

                if (Input.GetMouseButtonDown(0)) //choose to grow the plant
                {
                    float tempPosY = thisSelectedField.transform.localPosition.y-1f;    //a chosen lerp move
                    Vector3 tempTartgetPos = new Vector3(thisSelectedField.transform.localPosition.x, tempPosY,
                        thisSelectedField.transform.localPosition.z);
                    thisSelectedField.transform.localPosition =Vector3.Lerp(thisSelectedField.transform.localPosition,tempTartgetPos,Time.deltaTime*30) ;

                    Color tempChosenColor = new Color(60f / 255f, 50f / 255f, 30f / 255f);    //a chosen color change
                    thisSelectedField.GetComponent<Image>().color = tempChosenColor;
                    
                    //open the select panel
                    thisSelectedField.transform.GetChild(0).gameObject.SetActive(true);
                    Debug.Log("plantThePlant Planel Trigger");
                    
                }
                if (Input.GetMouseButtonUp(0)) //choose to grow the plant
                {
                    float tempPosY = thisSelectedField.transform.localPosition.y+1f;    //a chosen lerp move
                    Vector3 tempTartgetPos = new Vector3(thisSelectedField.transform.localPosition.x, tempPosY,
                        thisSelectedField.transform.localPosition.z);
                    thisSelectedField.transform.localPosition = Vector3.Lerp(thisSelectedField.transform.localPosition,tempTartgetPos,Time.deltaTime*30) ;

                    Color tempChosenColor = new Color(237f / 255f, 212f / 255f, 162f / 255f);    //a chosen color change
                    thisSelectedField.GetComponent<Image>().color = tempChosenColor;
                }
                
                
                
                if (lastSelectedField == null)
                {
                    lastSelectedField = thisSelectedField;
                }
                else if(lastSelectedField.name != thisSelectedField.name)
                {
                    lastSelectedField.GetComponent<Image>().enabled = false;
                }
                lastSelectedField = thisSelectedField;
                return;
            }
            else
            {
                if (lastSelectedField != null)
                {
                    lastSelectedField.GetComponent<Image>().enabled = false;
                }
                thisSelectedField = null;
                lastSelectedField = null;
                return;
            }
        

        }
    }
    private void WaterFieldRayCast()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "PlantThe5x5")
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<WaterFieldLauch>().WaterPlant();
                    hit.collider.gameObject.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }
}
