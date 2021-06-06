using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Pathfinding;
using Completed;

public class MoveToGoal : Agent
{
    //[SerializeField] private Transform targetPosition;
    /*[SerializeField] private Transform propPosition;
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private Transform foodPosition;*/
    public float moveSpeed = 1.0f;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private Material foodMaterial;
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material innerWallMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    
    //private AstarPath _astarPath;
    //private AIDestinationSetter _aiDestinationSetter;

    bool isReachedToFood = false;

    private BoardManager boardManager;

    private void Start()
    {
        boardManager = GetComponent<BoardManager>();
        //_astarPath = FindObjectOfType<AstarPath>();
        //_aiDestinationSetter = transform.parent.GetComponent<AIDestinationSetter>();
    }

    public override void OnEpisodeBegin()
    {
        isReachedToFood = false;
        boardManager.CanGenerateNewData();
        transform.localPosition = new Vector3(Random.Range(-4, 4), Random.Range(3.50f, -3.50f), 0.0f);

         //_astarPath.Scan();
        //transform.localPosition = new Vector3(Random.Range(-4, 4), Random.Range(3.50f, -3.50f), 0.0f);
        //targetPosition.localPosition = new Vector3(Random.Range(-4, 4), Random.Range(3.50f, -3.50f), 0.0f);
        /*foodPosition.localPosition = new Vector3(Random.Range(-4, 4), Random.Range(3.50f, -3.50f), 0.0f);
        propPosition.localPosition = new Vector3(Random.Range(-4, 4), Random.Range(3.50f, -3.50f), 0.0f);
        enemyPosition.localPosition = new Vector3(Random.Range(-4, 4), Random.Range(3.50f, -3.50f), 0.0f);*/
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);

        /* if (isReachedToFood == false)
         {
             sensor.AddObservation(foodPosition.localPosition);
             //Debug.Log("FOODddddddddddddddddddddddddddddddd");
         }
         else
         {
             sensor.AddObservation(targetPosition.localPosition);
             //Debug.Log("Targetttttttttttttttttttttttttttttttttttt");
         }

         sensor.AddObservation(propPosition.localPosition);
         sensor.AddObservation(enemyPosition.localPosition);*/

        //sensor.AddObservation(targetPosition.localPosition);
        sensor.AddObservation(DataHolder.exit.transform.localPosition);
        //_aiDestinationSetter.target = DataHolder.exit.transform;

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.DiscreteActions[0];
        float moveY = actions.DiscreteActions[1];
        
        switch (moveX)
        {
            case 0: // Dont Move
                transform.localPosition = transform.localPosition;
                break;
            case 1: // Move Left
                transform.localPosition += new Vector3(-1, 0, 0.0f) * Time.deltaTime * moveSpeed;
                break;
            case 2: // Move Right
                transform.localPosition += new Vector3(+1, 0.0f, 0.0f) * Time.deltaTime * moveSpeed;
                break;
            default:
                transform.localPosition = transform.localPosition;
                break;
        }
        switch (moveY)
        {
            case 0: // Dont Move
                transform.localPosition = transform.localPosition;
                break;
            case 1: // Move Back
                transform.localPosition += new Vector3(0.0f, -1, 0.0f) * Time.deltaTime * moveSpeed;
                break;
            case 2: // Move Forword
                transform.localPosition += new Vector3(0.0f, +1, 0.0f) * Time.deltaTime * moveSpeed;
                break;
            default:
                transform.localPosition = transform.localPosition;
                break;
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        /*ActionSegment<int> continuousAction = actionsOut.DiscreteActions;
        continuousAction[0] = (int)Input.GetAxisRaw("Horizontal");
        continuousAction[1] = (int)Input.GetAxisRaw("Vertical");*/

        ActionSegment<int> discreateAction = actionsOut.DiscreteActions;

        switch (Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")))
        {
            case  0: discreateAction[0] = 0; break;
            case -1: discreateAction[0] = 1; break; 
            case +1: discreateAction[0] = 2; break; 
        }

        switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical")))
        {
            case  0: discreateAction[1] = 0; break;
            case -1: discreateAction[1] = 1; break;
            case +1: discreateAction[1] = 2; break;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tagName = collision.gameObject.transform.tag;
        if (tagName == "OuterWall")
        {
            SetReward(-5f);
            floorMeshRenderer.material = wallMaterial;
            EndEpisode();
        }
        if (tagName == "Exit")
        {
            SetReward(10f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (tagName == "Food")
        {
            SetReward(5f);
            floorMeshRenderer.material = foodMaterial;
            //Destroy(collision.gameObject);
            isReachedToFood = true;
        }
        if (tagName == "Enemy")
        {
            SetReward(-10f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }

        if (tagName == "BreakableWall")
        {
            SetReward(-3f);
            floorMeshRenderer.material = innerWallMaterial;
            //EndEpisode();
        }

    }

}
