using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{

    public float speed;
    public Species species;
    public DevelopmentStage devStage;
    public bool isFemale;
    public float growTime;
    private float timeToGrow;
    private MeshRenderer rend;
    private Color thisColor;
    public bool lookingForMate;
    private bool readyToMultiply;
    private GameObject prospectMate;
    public float timeToChange;
    public float timeToReadyMultiply;
    private Vector3 babySize, juvenileSize, adultSize;
    public enum Species { cube, cilinder, sphere }
    public enum DevelopmentStage {baby, juvenile, adult, elderly}

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        lookingForMate = false;
        readyToMultiply = false;
        devStage = DevelopmentStage.baby;
        adultSize = Vector3.one;
        babySize = adultSize/4.0f;
        juvenileSize = adultSize/2.0f;
        transform.localScale = babySize;

        if (RandomBool())
        {
            thisColor = Color.red;
        }
        else thisColor = Color.blue;

    }

    // Update is called once per frame
    void Update()
    {
        rend.material.color = thisColor;
        if (!isFemale) lookingForMate = false;    
        if (prospectMate!=null)
        {
            SeekMate();
        }
        else Roam();   
        if (Time.time - timeToGrow > growTime && devStage != DevelopmentStage.adult)
        {
            Grow();
            timeToGrow = Time.time;
        }
        //if (Time.time - timeToReadyMultiply > 15.0f)
        //{
        //    lookingForMate = true;
        //}

    }

    private void WasBorn(Color color)
    {
        thisColor = color;
        rend.material.color = thisColor;
    }

    private void Roam()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Time.time - timeToChange > 4.0f )
        {
            ChangeDirection();
            timeToChange = Time.time;
        }
        if (lookingForMate == true) FindMate(); 
    }

    private void ChangeDirection()
    {
        Vector3 tempRotation = new Vector3(0f, UnityEngine.Random.Range(-100f, 100f), 0f);

        gameObject.transform.Rotate(tempRotation, Space.Self);
    }

    void FindMate()
    {
        Debug.Log("Finding mate");
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f);
        foreach (Collider collider in colliders)
        {
            if (collider.transform.position != transform.position && collider.CompareTag("Animal"))
            {
                if (collider.transform.gameObject.GetComponent<Animal>().species == species && collider.transform.gameObject.GetComponent<Animal>().devStage == devStage  && collider.transform.gameObject.GetComponent<Animal>().isFemale == false)
                {
                    Debug.Log("Found Mate");
                    prospectMate = collider.transform.gameObject;
                    return;
                }
            }
        }
    }
    void SeekMate()
    {
        Debug.Log("Seeking");
        speed = 2.0f;
        transform.LookAt(prospectMate.transform.position);
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position,prospectMate.transform.position) <= 1.0f)
        {
            Mate();
        }
    }

    void Mate()
    {
        if (isFemale)
        {
            Debug.Log("Mating");
            Color babiesColor = Color.Lerp(thisColor, prospectMate.GetComponent<Animal>().thisColor, 0.5f);
            GameObject newAnimal = Instantiate(gameObject, (transform.position + new Vector3(1.0f, 0f, 1.0f)), Quaternion.identity);
            newAnimal.GetComponent<Animal>().isFemale = RandomBool();
            newAnimal.GetComponent<Animal>().WasBorn(babiesColor);
            prospectMate = null;
            lookingForMate = false;
        }
        

    }
    private void Grow()
    {
        if (devStage == DevelopmentStage.baby)
        {
            Debug.Log("Grow 1");
            gameObject.transform.localScale = juvenileSize;
            devStage = DevelopmentStage.juvenile;
        }
        else if (devStage == DevelopmentStage.juvenile)
        {
            Debug.Log("Grow 2");
            gameObject.transform.localScale = adultSize;
            devStage = DevelopmentStage.adult;
            lookingForMate = true;
            readyToMultiply = true;
        }
    }

    bool RandomBool()
    {
        if (UnityEngine.Random.value >= .5)
        {
            return true;
        }
        else return false;
    }
}
