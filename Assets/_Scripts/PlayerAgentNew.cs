using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Completed;
using Pathfinding;

public class PlayerAgentNew : Agent
{
    public float moveSpeed = 1.0f;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private Material foodMaterial;
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material innerWallMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    //[SerializeField] private AstarPath _astarPath;


    bool isReachedToFood = false;
    bool isEpisodeBegin = false;

    private BoardManager boardManager;

    //private AIDestinationSetter _aiDestinationSetter;

    private void Start()
    {
        boardManager = GetComponent<BoardManager>();

       //_aiDestinationSetter = transform.parent.GetComponent<AIDestinationSetter>();
    }


    public override void OnEpisodeBegin()
    {
        isReachedToFood = false;
        boardManager.CanGenerateNewData();
        //_astarPath.Scan();
        //transform.parent.localPosition
        transform.localPosition = new Vector3(Random.Range(boardManager.xAxisMovement.x, boardManager.xAxisMovement.y), Random.Range(boardManager.yAxisMovement.x, boardManager.yAxisMovement.y), 0.0f);

    }

    public override void CollectObservations(VectorSensor sensor)
    {

        sensor.AddObservation(transform.localPosition);

        /*if(DataHolder.foodTilesList != null && DataHolder.foodTilesList.Count != 0) 
        {
            if (isReachedToFood == false)
            {
                for(int i = 0; i < DataHolder.foodTilesList.Count; i++)
                {
                    sensor.AddObservation(DataHolder.foodTilesList[i].localPosition);
                    //_aiDestinationSetter.target = DataHolder.foodTilesList[i].transform;
                    break;
                }

            }
            else
            {
                sensor.AddObservation(DataHolder.exit.transform.localPosition);
                //_aiDestinationSetter.target = DataHolder.exit.transform;
            }
        }
        else
        {*/
            sensor.AddObservation(DataHolder.exit.transform.localPosition);
            //_aiDestinationSetter.target = DataHolder.exit.transform;
        //}

        /*if (DataHolder.enemyTilesList != null && DataHolder.enemyTilesList != null && DataHolder.enemyTilesList.Count != 0)
        {
            for (int i = 0; i < DataHolder.enemyTilesList.Count; i++)
            {
                sensor.AddObservation(DataHolder.enemyTilesList[i].localPosition);
            }
        }*/

        /*if (DataHolder.innerTilesList != null && DataHolder.innerTilesList.Count != 0)
        {
            for (int i = 0; i < DataHolder.innerTilesList.Count; i++)
            {
                sensor.AddObservation(DataHolder.innerTilesList[i].localPosition);
            }
        }*/

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.DiscreteActions[0];
        float moveY = actions.DiscreteActions[1];

        transform.localPosition += new Vector3(moveX, moveY, 0.0f) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<int> continuousAction = actionsOut.DiscreteActions;

        continuousAction[0] = (int)Input.GetAxisRaw("Horizontal");
        continuousAction[1] = (int)Input.GetAxisRaw("Vertical");

       // Debug.Log("HELLEOOE");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tagName = collision.gameObject.transform.tag;
        if(tagName == "OuterWall")
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
