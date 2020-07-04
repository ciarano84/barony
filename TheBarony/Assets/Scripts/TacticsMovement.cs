using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class TacticsMovement : Unit
{    
    public bool turn = false;
    bool moveGate = false;

    //Made public so that the weapons can grab it. 
    public List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    public Tile currentTile;
    
    //Used to ensure the first tile doesn't count against movement.
    Tile firstTileInPath;

    public bool moving = false;
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

    public float halfHeight = 0;

    bool jumpingUp = false;
    bool movingEdge = false;
    bool fallingDown = false;
    bool mouseOver = false;

    public delegate void OnEnterSquareDelegate(Unit mover);
    public static OnEnterSquareDelegate OnEnterSquare;

    public void InitTacticsMovement() {
        if (unitInfo.faction == Factions.enemies)
        {
            GetComponent<MonsterConstructor>().SetUpMonster();
            SetStats();
        }
        tiles = GameObject.FindGameObjectsWithTag("tile");
        halfHeight = GetComponent<Collider>().bounds.extents.y;
        unitAnim = GetComponent<Animator>();
        CheckInitiative();
        remainingMove = unitInfo.currentMove;
        remainingActions = 1;
        focusSwitched = false;
        Initiative.AddUnit(this);
    }

    public void GetCurrentTile() {
        currentTile = GetTargetTile(gameObject);
        if (turn) currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target) {
        RaycastHit hit;

        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {  
            tile = hit.collider.GetComponent<Tile>();
        }
        if ((tile == null) && turn) 
        {
            Debug.Log("no current tile found"); //This to help track down bug 1. 
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

                //Now go through the diagonals, which use up 'distance' quicker
                foreach (Tile tile in t.diagonalAdjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.diagonal = true;
                        tile.distance = 1.41f + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    //HAS AN OVERLOAD!!!! (should likely just solve this by setting the additional parameter as a default. 
    public void MoveToTile(Tile tile) 
    {
        path.Clear();
        moveGate = true;
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
        moveGate = true;
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
        if (!moveGate)
        {
            return;
        } 
        else if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //Calculate the unit's position on top of target tile. 
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            //Failsafe I've put in to catch unit's who's movement has gone wrong
            if (Vector3.Distance(transform.position, target) >= 1.6f)
            {
                transform.position = target;
            }
            
            if (Vector3.Distance(transform.position, target) >= 0.2f)
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

                //The following replaces "transform.position += velocity * Time.deltaTime"
                transform.forward = heading;
                if (movingEdge)
                {
                    transform.position = Vector3.MoveTowards(transform.position, jumpTarget, moveSpeed * Time.deltaTime);
                }
                else
                {
                    transform.position += velocity * Time.deltaTime;
                }

            }
            else
            {
                //Tile centre reached
                transform.position = target;
                OnEnterSquare(this); //Alert that a unit has entered a square.

                //Take this move off remaining move IF it's not the first tile in the path.
                if (path.Peek() != firstTileInPath)
                {
                    if (t.diagonal)
                    {
                        remainingMove -= 1.41f;
                    } 
                    else remainingMove--;
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
            moveGate = false;
            Initiative.EndAction();
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

            velocity = heading * moveSpeed / 4.0f;

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
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.2f)
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
        GetComponent<PlayerCharacter>().FindSelectableTiles();  
        GetComponent<PlayerCharacter>().mainWeapon.GetTargets();
    }

    public void EndTurn()
    {
        AutoSetFocus();
        turn = false;
        remainingMove = unitInfo.currentMove;
        remainingActions = 1;
    }

    void CheckInitiative()
    {
        currentInitiative = (Random.Range(1, 20) + initiativeMod);
    }

    private void OnMouseOver()
    {
        mousedOverUnit = this;
        UnitMouseOverView.Display(this);
        if (Initiative.queuedActions > 0)
        {
            ActionUIManager.SetStandardCursor();
        }
        else if ((Initiative.currentUnit.remainingActions > 0) && (!Initiative.currentUnit.moving))
        {
            foreach (Weapon.Target target in Initiative.currentUnit.GetComponent<PlayerCharacter>().mainWeapon.targets)
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
        UnitMouseOverView.Hide();
        mousedOverUnit = null;
    }

    public void FaceDirection(Vector3 target)
    {
        target.y = transform.position.y;
        transform.LookAt(target);
    }
}
