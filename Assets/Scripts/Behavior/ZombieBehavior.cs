
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA;
using System.Collections;
using System.Collections.Generic;

public enum ZombieState
{
    IDLE,
    WALKING,
    CHASING,
    ATTACKING,
    DEAD,
    PAUSED,
    REACHING,
};

public class ZombieBehavior : MonoBehaviour
{
    #region firstpart
    [Tooltip("Layers that line of sight will collide with.")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

    [Tooltip("Object of the model we want to animate.")]
    public GameObject modelObject;

    [Tooltip("Color of the zombie's eyes.")]
    public Color weakZombieColor, normalZombieColor, strongZombieColor;

    [Tooltip("Zombie Cornea")]
    public GameObject zombieCornea_L, zombieCornea_R;

    [Tooltip("Hitpoint threshold for eye lights")]
    public int greenThreshold = 50, yellowThreshold = 65, redThreshold = 80;

    [Tooltip("BloodFX component.")]
    public GameObject bloodFXObject;

    [Tooltip("Player layer so the zombie knows when it is close enough to attack.")]
    public int playerLayer;

    [Tooltip("Barrier layer so the zombie knows when to switch to a reaching state.")]
    public int barrierLayer = 12;

    [Tooltip("How often will agent check line of sight?")]
    public float reactionTimeInSeconds = 2.0f;

    [Tooltip("The number of hitpoints this zombie starts with.")]
    public int hitPoints = 20;

    [Tooltip("The number of points each hit denomination is worth")]
    public int headShotPoints = 100;
    public int bodyShotPoints = 35;
    public int limbShotPoints = 10;

    [Tooltip("The number of seconds to wait for game over screen after attack.")]
    public float gameOverDelay = 1.0f;

    [Tooltip("Movement speed per second.")]
    public float walkSpeed = 0.5f;
    public float runSpeed = 0.65f;

    [Tooltip("Attack speed in seconds.")]
    public float attackSpeed = 1f;

    [Tooltip("The number of death animations the animator has to choose from.")]
    public int numberOfDeathAnimations = 4;

    [Tooltip("The number of reach animations the animator has to choose from.")]
    public int numberOfReachAnimation = 2;

    [Tooltip("Dummies do not check path finder or line of sight. They remain idle.")]
    public bool isDummy = false;

    [Tooltip("Does model use root motion?")]
    public bool useRootMotion = false;


    [Tooltip("does this zombie have a BoomVest on?")]
    public bool isVest = false;


    [Tooltip("HP is not assigned by wave")]
    public bool isStaticHP = false;

    [HideInInspector]
    public ZombieState state { get; private set; }
    ZombieState lastState;

    GameManager manager;            // the scene's game manager script
    UAudioManager audioManager;     // the UAudioManager component attached to this game object
    PathFinder pathFinder;          // this object's pathfinder component
    Rigidbody rb;                   // this object's rigidbody component
    CapsuleCollider cc;             // this object's capsule collider
    Animator[] animator;            // the animator component of the model we want to animate
    BloodFX bloodFX;                // the BloodFX component
    TimerBehavior attackTimer;      // attack timer object

    Quaternion toRotation;          // destination rotation
    Stack<PathNode> path;           // the path that the agent is following
    GameObject currentPoint;        // current GridPoint on GridMap
    GameObject targetPoint;         // the GridPoint that the agent is heading toward currently
    GameObject hotspotPoint;
    Vector3 targetPosition;         // the position that the agent is heading for
    bool melting;                   // flag that triggers game object destruction
    bool hasFoundHotspot;
    Vector3 meltStartPosition;      // position of transform when melting begins          
    float reactionTime;             // private version of reactionTimeInSeconds
    int hp;                         // counter for hit points
    float moveSpeed;                // movement speed
    float multiplier = 1.0f;        // multiplier of animation speed
    int deathType = 0;              // deathType for animator
    int reachType = 0;              // reachType for animator


    VestBehavior _vb;               //ref to the exploding vest script

    void Start()
    {
        //meltStartPosition = new Vector3(0f, 0f, 0f);
        manager = GameManager.Instance;
        audioManager = GetComponent<UAudioManager>();
        rb = gameObject.GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        animator = GetComponentsInChildren<Animator>();
        bloodFX = bloodFXObject.GetComponent<BloodFX>();
        attackTimer = gameObject.AddComponent<TimerBehavior>();

        toRotation = transform.rotation;
        pathFinder = GetComponent<PathFinder>();

        //if (pathFinder == null)
        //    DebugConsole.print("there is no path finder");
        //else
        //    DebugConsole.print(" path finder found");

        path = new Stack<PathNode>();
        targetPoint = null;

        hasFoundHotspot = false;

        if (isVest)
        {
            _vb = GetComponentInChildren<VestBehavior>();
            // DebugConsole.print("Z: i am wearing a vest !");
        }


        lastState = ZombieState.IDLE;
        melting = false;
        reactionTime = reactionTimeInSeconds;
        hp = hitPoints;
        moveSpeed = walkSpeed;

        //SetHP(80);

        // spawn enemy to face player
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));

        // if the zombie is a dummy, turn it to face away from the player
        if (isDummy)
            transform.Rotate(Vector3.up, 180.0f);

        // start zombie moans immediately
        audioManager.PlayEvent("_Idle");

        SetHotspotLocation();
    }

 
    void Update()
    {
        CalculateMovement();
        UpdateAudio();
        UpdateAnimation();

        // update last state
        lastState = state;
        // printState();
    }

    #endregion
    void SetHotspotLocation()
    {
        List<GameObject> hotspots = manager.GetHotspots();
        int randIndex = Random.Range(0, hotspots.Count);
        hotspotPoint = manager.GetGridMap().GetClosestPoint(hotspots[randIndex]);
    }

    void CalculateMovement()
    {
  if (isDummy) return;

        // removed return on state == ZombieState.IDLE 
        if (state == ZombieState.PAUSED || state == ZombieState.REACHING)
            return;

        if (melting)
        {
            rb.MovePosition(transform.position + (Vector3.down * Time.deltaTime * 0.5f));

            float dis = Vector3.Distance(transform.position, meltStartPosition);
            if (dis > 2.5f)
                Destroy(gameObject);
        }


        if (state == ZombieState.DEAD || state == ZombieState.ATTACKING)
            return;


        if (currentPoint == null)
            currentPoint = manager.GetGridMap().GetClosestPoint(gameObject);

        //check reaction time
        reactionTime -= Time.deltaTime;
        if (reactionTime <= 0.0f)
        {
            reactionTime = reactionTimeInSeconds;
            targetPoint = null;
            state = ZombieState.CHASING;
            moveSpeed = runSpeed;//walkSpeed;
            multiplier = 1.0f;
        }
      
        // if not following and there is no target point, check path
        if (  targetPoint == null)
        {
            if (path == null)
            {
                state = ZombieState.IDLE;
                path = new Stack<PathNode>();
            }

            if (path.Count > 0)
            {
                targetPoint = path.Pop().gridPoint;
                targetPoint.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                if (manager.player.gridPosition == null)
                    manager.player.SetGridPosition();


                if (!pathFinder.isFinding)
                {
                    if (hasFoundHotspot || hotspotPoint == null)
                    {
                        pathFinder.FindPath(currentPoint, manager.player.gridPosition);
                    }
                    else
                    {
                        pathFinder.FindPath(currentPoint, hotspotPoint);
                        hasFoundHotspot = true;
                    }
                }
            }
        }

        // if no target point then don't move
        if (targetPoint == null)
        {
            state = ZombieState.IDLE;
            return;
        }
        else
        {
            
            // get rotation towards targetPoint
            Quaternion oldRotation = transform.rotation;
            transform.LookAt(new Vector3(targetPoint.transform.position.x, transform.position.y, targetPoint.transform.position.z));
            toRotation = transform.rotation;
            transform.rotation = oldRotation;

            // rotate towards targetPoint
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 500.0f * Time.deltaTime);
  
        }

        // move forward only if we do not use root motion and we are done rotating
        if (!useRootMotion && state != ZombieState.IDLE && Quaternion.Angle(transform.rotation, toRotation) <= 1.0f)
            rb.MovePosition(transform.position + (transform.forward * Time.deltaTime * moveSpeed));
    }
    

 
    void SetPath()
    {
        path = pathFinder.finalPath;
    }

    #region clean
    public void SetHP(int value)
    {
        if (isStaticHP) { hp = hitPoints; }
        else
            hp = hitPoints = value;

        if (zombieCornea_R == null || zombieCornea_L == null) return;

        // change eye to appropriate color
        if (hp >= redThreshold) SetEyeColor(strongZombieColor);
        else if (hp >= yellowThreshold) SetEyeColor(normalZombieColor);
        else if (hp >= greenThreshold) SetEyeColor(weakZombieColor);
        else ToggleEyeCornea(false);
    }

    void SetEyeColor(Color color)
    {
        ToggleEyeCornea(true);

        MeshRenderer zombieCorneaRender_R = zombieCornea_R.GetComponentInChildren<MeshRenderer>();
        MeshRenderer zombieCorneaRender_L = zombieCornea_L.GetComponentInChildren<MeshRenderer>();

        zombieCorneaRender_L.material.color = color;
        zombieCorneaRender_R.material.color = color;
    }


    void ToggleEyeCornea(bool toggle)
    {
        zombieCornea_L.SetActive(toggle);
        zombieCornea_R.SetActive(toggle);
    }


    void UpdateAudio()
    {
        // if state has changed this frame then change audio
        if (lastState != state)
        {
            audioManager.StopAllEvents();
            switch (state)
            {
                case ZombieState.IDLE:
                case ZombieState.WALKING:
                    audioManager.PlayEvent("_Idle");
                    break;
                case ZombieState.CHASING:
                case ZombieState.REACHING:
                    audioManager.PlayEvent("_Chasing");
                    break;
            }
        }
    }

    void UpdateAnimation()
    {
        for (int i = 0; i < animator.Length; i++)
        {
            animator[i].SetInteger("state", (int)state);
            animator[i].SetFloat("multiplier", multiplier);
            animator[i].SetInteger("deathType", deathType);
            animator[i].SetInteger("reachType", reachType);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (state == ZombieState.DEAD || state == ZombieState.PAUSED)
            return;

        if (other.CompareTag("GridPoint"))
        {
            currentPoint = other.gameObject;
            currentPoint.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.blue;
           
            if (targetPoint != null)
            {
                if (currentPoint == targetPoint) {
                    currentPoint.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                     
                    targetPoint = null;
                }
                   
            }
        }
        else if (other.gameObject.layer == playerLayer)
        {
                Attack();
        }
        else if (other.gameObject.layer == barrierLayer)
        {
            // if zombie has collided with barrier then switch to reach state
            Reach();
            if (isVest) { _vb.ExplodeAndDie(); GameManager.Instance.KillePlayer(); }
        }
    }

    void WriteToDebug(string s)
    {
        DebugConsole.print(s);
        Debug.Log(s + "debug");
    }


    void Reach()
    {
        if (state == ZombieState.REACHING)
            return;

        state = ZombieState.REACHING;

        // disable move collider
        cc.enabled = false;

        // get random reach animation
        reachType = Random.Range(0, numberOfReachAnimation - 1);

        // turn model to face camera
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));

        Attack();
    }

    void Attack()
    {
        if (state == ZombieState.PAUSED)
            return;

        GameManager.Instance.AttackPlayer();
        attackTimer.StartTimer(attackSpeed, Attack, false);
    }


    void HeadShot(Bullet bullet)
    {
        if (bullet == null) return;

        //Debug.Log("HEAD");
        // instantiate blood effect
        bloodFX.HeadShotFX(bullet.hitInfo);

        // score
        Score(headShotPoints);


        if (manager.isHeadShotKill)
            hp = -1;
        else
        hp -= ((bullet.damage * 2)+5);

        GameManager.Instance.IncrementCounter(3); //headShotCount

        foreach (Animator ator in animator) {
            ator.SetTrigger("trigHeadShot");
        }


        if (hp <= 0)
            Kill();
    }

    void TorsoShot(Bullet bullet)
    {
        if (bullet == null) return;

        // instantiate blood effect
        bloodFX.TorsoShotFX(bullet.hitInfo);

        // score
        Score(bodyShotPoints);

        // take torso damage
        hp -= bullet.damage;

        GameManager.Instance.IncrementCounter(4);//torsoShotCount

        if (hp <= 0)
            Kill();
    }

    void LimbShot(Bullet bullet)
    {
        if (bullet == null) return;

        // instantiate blood effect
        bloodFX.TorsoShotFX(bullet.hitInfo);

        // score
        Score(limbShotPoints);

        // take limb damage
        hp -= Mathf.RoundToInt(bullet.damage * 0.5f);

        GameManager.Instance.IncrementCounter(5);//limbShotCount

        if (hp <= 0)
            Kill();
    }

    void printState()
    {
        DebugConsole.print("stat=" + state.ToString());
    }
    public void Kill()
    {
        if (state == ZombieState.DEAD) return;

        // stop attacking
        attackTimer.StopTimer();

        if (zombieCornea_L != null) zombieCornea_L.SetActive(false);
        if (zombieCornea_R != null) zombieCornea_R.SetActive(false);

        // change state and disable movement collider
        state = ZombieState.DEAD;
        pathFinder.StopAllCoroutines();
        deathType = Random.Range(0, numberOfDeathAnimations - 1);

        rb.constraints = RigidbodyConstraints.FreezeAll;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        DisableBoxColliders(this.transform);

        UpdateAnimation();
        audioManager.StopAllEvents();
        audioManager.PlayEvent("_Die");
        GameManager.Instance.IncrementCounter(6);//killCount
       
        cc.enabled = false;
        manager.KillEnemy(gameObject);

    }

    void Score(int value)
    {
        // update score in game manager
        manager.AddPoints(value);
    }

    public void Activate()
    {
        isDummy = false;
    }

    public void Pause()
    {
        for (int i = 0; i < animator.Length; i++)
        {
            animator[i].speed = 0f;
        }

        audioManager.StopAllEvents();
        multiplier = 0.0f;
        state = ZombieState.PAUSED;
    }

    public void Melt()
    {
        // begin object destruction process
        melting = true;
        meltStartPosition = transform.position;
        rb.constraints = RigidbodyConstraints.None;
    }

    Transform DisableBoxColliders(Transform argTrans)
    {
        if (argTrans.gameObject.GetComponent<BoxCollider>())
            argTrans.gameObject.GetComponent<BoxCollider>().enabled = false;
        foreach (Transform c in argTrans)
        {
            var result = DisableBoxColliders(c);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public void TakeHit(Bullet bullet)
    {
        if (state == ZombieState.DEAD)
            return;

        string tag = bullet.hitInfo.collider.gameObject.tag;

        switch (tag)
        {
            case "ZombieHead":
                HeadShot(bullet);
                break;
            case "ZombieTorso":
                TorsoShot(bullet);
                break;
            case "ZombieLimb":
                LimbShot(bullet);
                break;
            default:
                bloodFX.LimbShotFX(bullet.hitInfo);
                break;
        }
    }
    #endregion
}