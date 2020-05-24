using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEditor.Build;

public class TacticsMovement : Unit
{    
    public bool turn = false;

    //Made public so that the weapons can grab it. 
    public List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    public Tile currentTile;
    
    //Used to ensure the first tile doesn't count against movement.
    Tile firstTileInPath;

    public bool moving = false;
    public int move = 5;
    public float jumpHeight = 2f;
    public float moveSpeed = 2;
    public float jumpVelocity = 4.5f;
    public int initiativeMod = 0;
    public int currentInitiative = 0;
    Vector3 tileToFace;
    //Tells it that it will need to turn to face a certain point at the end. 
    bool turnRequired;

    //Required for Action/move economy
    public float remainingMove;
    public int remainingActions;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();
    Vector3 jumpTarget;

    float halfHeight = 0;

    bool jumpingUp = false;
    bool movingEdge = false;
    bool fallingDown = false;
    
    public void InitTacticsMovement() {
        tiles = GameObject.FindGameObjectsWithTag("tile");
        halfHeight = GetComponent<Collider>().bounds.extents.y;
        unitAnim = GetComponent<Animator>();
        CheckInitiative();
        remainingMove = move;
        remainingActions = 1;
        Initiative.AddUnit(this);
    }

    public void GetCurrentTile() {
        currentTile = GetTargetTile(gameObject);
        //Removing the below as I think I have it covered elsewhere. 
        //currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target) {
        RaycastHit hit;

        Tile tile = null; 

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {  
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }

    public void ComputeAdjacencyList() {
        //required if the map is to change size after initializing. 
        //tiles = GameObject.FindGameObjectsWithTag("tile");
        
        foreach (GameObject tile in tiles)
            {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbours(jumpHeight);
            }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyList();
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();
            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < remainingMove)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile) 
    {   
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            if (next.parent != null)
            {
                firstTileInPath = next;
            }
            next = next.parent;
        }
    }

    //overload for when there is a facing
    public void MoveToTile(Tile tile, Vector3 facing)
    {
        //My bit to work out if it should face a certain way at the end. 
        if (facing != null)
        {
            tileToFace = facing;
            turnRequired = true;
        }

        //the original tactics movement
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            if (next.parent != null)
            {
                firstTileInPath = next;
            }
            next = next.parent;
        }
    }

    public void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //Calculate the unit's position on top of target tile. 
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                bool jump = transform.position.y != target.y;

                if (jump)
                {
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }
                
                //Locomotion (and where we would add animation). 
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //Tile centre reached
                transform.position = target;
                
                //Take this move off remaining move IF it's not the first tile in the path.
                if (path.Peek() != firstTileInPath)
                {
                    remainingMove--;
                }
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            if (turnRequired)
            {
                tileToFace.y = transform.position.y;
                transform.LookAt(tileToFace);
                turnRequired = false;
            }

            moving = false;
            Initiative.CheckForTurnEnd(this);
        }
    }

    protected void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }
        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }
        selectableTiles.Clear();
    }

    public void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target) 
    {
        if (fallingDown)
        {
            FallDownward(target);
        }
        else if (jumpingUp)
        {
            JumpUpward(target);
        }
        else if (movingEdge)
        {
            MoveToEdge();
        }
        else 
        {
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target) {
        float targetY = target.y;
        target.y = transform.position.y;
        CalculateHeading(target);

        if (transform.position.y > targetY)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2;
        }
        else {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            velocity = heading * moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }

    void FallDownward(Vector3 target) {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y <= target.y)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target) {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    void MoveToEdge() {
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();
        }
        else 
        {
            movingEdge = false;
            fallingDown = true;
            velocity /= 5.0f;
            velocity.y = 1.5f;
        }
    }

    public void BeginTurn() 
    {
        turn = true;
        //I've put this in to stop it firing constantly. Don't know why original vid had it on update, but I'm sure there's a good reason I'll find out. 
        
        GetComponent<PlayerCharacter>().FindSelectableTiles();  
        GetComponent<PlayerCharacter>().weapon1.GetTargets();
    }

    public void EndTurn()
    {
        turn = false;
        remainingMove = move;
        remainingActions = 1;
    }

    void CheckInitiative()
    {
        currentInitiative = (Random.Range(1, 20) + initiativeMod);
    }

    private void OnMouseOver()
    {
        if (Initiative.currentUnit.remainingActions > 0)
        {
            foreach (Weapon.Target target in Initiative.currentUnit.GetComponent<PlayerCharacter>().weapon1.targets)
            {
                
                if (target.unitTargeted == this && (Initiative.currentUnit != this))
                {
                    //change mouse pointer.
                    ActionUIManager.GetAttackCursor();
                }
            }
        }
    }

    private void OnMouseExit()
    {
        ActionUIManager.SetStandardCursor();
    }
}
