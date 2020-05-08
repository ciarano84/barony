using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour
{
    /*public LayerMask whatCanBeClickedOn;
    public NavMeshAgent troopAgent;
    Troop troopScript;
    float troopMoveSpeed;
    
    //likely not needed. 
    public TurnManager turnManager;
    public GameObject highlight;
    public Vector3 highlightedPosition;
    public Renderer highlightRenderer;
    GameObject highlightVisualGO;

    bool troopMoving = false;
    bool eligibleMove = false;
    Ray ray;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start() {
        highlightVisualGO = highlight.transform.GetChild(0).gameObject;
        highlightRenderer = highlightVisualGO.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //selects the highlighted square based on what your mousing over
        ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, whatCanBeClickedOn))
        {
            highlightedPosition = new Vector3(Mathf.Round(hit.point.x), 0, Mathf.Round(hit.point.z));
            highlight.transform.position = highlightedPosition;
            if ((troopMoving == false) && (Vector3.Distance(troopAgent.transform.position, highlightedPosition) < troopMoveSpeed))
            {
                highlightRenderer.material.color = Color.blue;
                eligibleMove = true;
            }
            else
            {
                highlightRenderer.material.color = Color.gray;
                eligibleMove = false;
            }
        }
        
        // The basic click to relocate a troop. 
        if (Input.GetMouseButtonDown(0))
        {
            Ray myRay = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if ((Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn)) && eligibleMove)
            {
                troopMoving = true;
                troopAgent.SetDestination(highlightedPosition);
            }
        }

        //Check to see if a movement is complete
        if (Vector3.Distance(highlightedPosition, troopAgent.transform.position) < 0.2) 
        {
            OnMovementFinished();
        };
    }

    public void UpdateTroop(GameObject troop) {
        troopAgent = troop.GetComponent<NavMeshAgent>();
        troopScript = troop.GetComponent<Troop>();
        troopMoveSpeed = troopScript.moveSpeed;
    }

    void OnMovementFinished()
    {
        turnManager.NewTurn();
        troopMoving = false;
    }*/
}
