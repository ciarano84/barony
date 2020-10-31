using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;

public class TacticsMovement : Unit
{
    public bool turn = false;
    bool moveGate = false;

    //Debug
    public bool test = false;

    //Made public so that the weapons can grab it. 
    public List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    public Tile currentTile;

    //Used to ensure the first tile doesn't count against movement.
    Tile firstTileInPath;

    //Needed for A*
    public Tile actualTargetTile;

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
    //bool mouseOver = false;
    public Vector3 dodgeTarget = new Vector3();
    bool dodging = false;

    public delegate void OnEnterSquareDelegate(Unit mover);
    public static OnEnterSquareDelegate OnEnterSquare;

    public delegate void OnDodgeDelegate(Unit attacker, Result result);
    public static OnDodgeDelegate OnDodge;

    public void InitTacticsMovement() {
        if (unitInfo.faction == Factions.enemies)
        {
            GetComponent<MonsterConstructor>().SetUpMonster();
            SetStats();
        }
        SetActions();
        //tiles = GameObject.FindGameObjectsWithTag("tile");
        halfHeight = GetComponent<Collider>().bounds.extents.y;
        unitAnim = rig.GetComponent<Animator>();
        CheckInitiative();
        remainingMove = unitInfo.currentMove;
        remainingActions = 1;
        focusSwitched = false;
        if (RostaInfo.encounter)
        {
            Initiative.AddUnit(this);
        }
    }

    public void AllocateTile()
    {
        GetCurrentTile();
        currentTile.occupant = this;
    }

    public void GetCurrentTile() {
        currentTile = GetTargetTile(gameObject);

        //trying to catch the original fucker of a bug:
        if (currentTile == null)
        {
            Debug.DrawRay(transform.position, -Vector3.up, Color.blue, 50f);
            Debug.Log("Bug no 1 found on " + this);
        }

        if (turn) currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target) {
        RaycastHit hit;
        int layerMask = 1 << 9;

        Tile tile = null;

        if (Physics.Raycast(target.transform.position + new Vector3(0, 1, 0), -Vector3.up, out hit, 2, layerMask))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeAdjacencyList(float jumpHeight, Tile targetTile)
    {
        //required if the map is to change size after initializing. 
        //tiles = GameObject.FindGameObjectsWithTag("tile");

        foreach (GameObject tile in ArenaBuilder.tiles)
        {
            Tile t = tile.GetComponent<Tile>();

            //Debug
            if (t != null)
            {
                //t.FindNeighbours(jumpHeight, targetTile);
                t.CheckNeighbours(jumpHeight, targetTile);
            }
            else
            {
                Debug.LogError("Tile tag set without tile component");
                tile.transform.position = new Vector3(0, 10, 0);
            }
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyList(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        firstTileInPath = currentTile;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();
            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < remainingMove)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    float distanceToNextTile = 1;
                    if (tile.difficultTerrain) distanceToNextTile += 1;

                    if (!tile.visited && ((remainingMove - t.distance) >= distanceToNextTile))
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = distanceToNextTile + t.distance;
                        process.Enqueue(tile);
                    }
                }

                //Now go through the diagonals, which use up 'distance' quicker
                foreach (Tile tile in t.diagonalAdjacencyList)
                {
                    float distanceToNextTile = 1.4f;
                    if (tile.difficultTerrain) distanceToNextTile += 1.4f;

                    if (!tile.visited && ((remainingMove - t.distance) >= distanceToNextTile))
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.diagonal = true;
                        tile.distance = distanceToNextTile + t.distance;
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
        unitAnim.SetBool("moving", true);

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            //if (next.parent != null)
            //{
            //    firstTileInPath = next;
            //}
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
        unitAnim.SetBool("moving", true);

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

            //Here is my part which establishes the current tiles, and those occupants of those tiles.
            currentTile.occupant = null;
            t.occupant = this;
            currentTile = t;

            Vector3 target = t.transform.position;

            //Calculate the unit's position on top of target tile. 
            target.y += /*halfHeight  + */ t.GetComponent<Collider>().bounds.extents.y + 0.02f;

            //Failsafe I've put in to catch unit's who's movement has gone wrong
            if (Vector3.Distance(transform.position, target) >= 1.6f)
            {
                transform.position = target;
            }

            if (Vector3.Distance(transform.position, target) >= 0.2f)
            {
                bool jump = false;

                //This next line was...
                //bool jump = transform.position.y != target.y;

                if (transform.position.y >= target.y + 0.05 || transform.position.y <= target.y - 0.05) jump = true;

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
                        if (t.difficultTerrain) remainingMove -= 2.8f;
                        else remainingMove -= 1.41f;
                    }
                    else
                    {
                        if (t.difficultTerrain) remainingMove -= 2f;
                        else remainingMove--;
                    }
                }
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();

            if (focus != null)
            {
                if (RangeFinder.LineOfSight(this, focus) == true)
                {
                    FaceDirection(focus.transform.position);
                }
            }

            if (turnRequired)
            {
                FaceDirection(tileToFace);
                turnRequired = false;
            }
            moving = false;
            unitAnim.SetBool("moving", false);
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
        bool firstTurn = false;
        if (!turn)
        {
            //put stuff that should only happen at the start of their turn in here (as opposed to the beginning of each time they choose a new action/move). 
            firstTurn = true;
            CheckFocus(false);
        }

        turn = true;
        FindSelectableTiles();
        mainWeapon.GetTargets();

        //this needs to not get checked every time it comes to the end of an action AND has to be after turn is set to work. 
        if (firstTurn)
        {
            if (gameObject.GetComponent<AI>() != null)
            {
                gameObject.GetComponent<AI>().SetTask();
            }
        }
    }

    public void EndTurn()
    {
        AutoSetFocus();
        turn = false;
        remainingMove = unitInfo.currentMove;
        remainingActions = 1;

        //This needs to be altered to stop focus just being lost at the end of the turn. 
        CheckFocus(true);
        focusSwitched = false;
        canFocusSwitch = false;
    }

    void CheckInitiative()
    {
        currentInitiative = (Random.Range(1, 20) + initiativeMod);
    }

    void OnMouseOver()
    {
        mousedOverUnit = this;
        if (RostaInfo.encounter) ActionUIManager.SetCursor();
    }

    void OnMouseExit()
    {
        mousedOverUnit = null;
        if (RostaInfo.encounter) ActionUIManager.SetCursor();
    }

    public void FaceDirection(Vector3 target)
    {
        target.y = transform.position.y;
        transform.LookAt(target);
        if (aimingBow) transform.rotation *= Quaternion.Euler(0, 90f, 0);
    }

    //The A* formula. 
    public Tile FindPath(Tile targetTile)
    {
        ComputeAdjacencyList(jumpHeight, targetTile);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        //currentTile.parent = ??

        currentTile.h = Vector3.Distance(currentTile.transform.position, targetTile.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);
            closedList.Add(t);

            if (t == targetTile)
            {
                return FindEndTile(t);
            }

            //Here in order to put both adjacency and diagonal into one. 
            List<Tile> combinedAdjacencyList = new List<Tile>();
            if (combinedAdjacencyList.Count > 0) Debug.LogError("combined adjacency list needs resetting as it still has members.");
            foreach (Tile x in t.adjacencyList) combinedAdjacencyList.Add(x);
            foreach (Tile x in t.diagonalAdjacencyList) combinedAdjacencyList.Add(x);

            foreach (Tile tile in combinedAdjacencyList)
            {
                if (closedList.Contains(tile))
                {
                    //Do Nothing.
                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }

                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, targetTile.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }


        //todo: come up with a way of handling when their is no path, or when the tile next to the target tile is blocked. 
        Debug.LogWarning("NPC path not found");
        return null;
    }

    protected void NPCMove()
    {
        Initiative.queuedActions++;
        MoveToTile(actualTargetTile);
    }

    //Needed for A*
    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }
        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= remainingMove)
        {
            return t.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= remainingMove; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;
    }

    public void Dodge(Result _result)
    {
        if (_result == Result.PARTIAL)
        {
            UpdateBreath(-1, true);
        }  
        dodging = true;
        currentTile.occupant = null;
        OnDodge(this, _result);
        Initiative.queuedActions++;
        unitAnim.SetTrigger("dodge");
    }

    private void Update()
    {
        //dodging
        if (dodging)
        {
            if (Vector3.Distance(transform.position, dodgeTarget) >= 0.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, dodgeTarget, Time.deltaTime * 5f);
            }
            else
            {
                transform.position = dodgeTarget;
                dodging = false;
                AllocateTile();
                Initiative.EndAction();
            }
        }
    }
}
