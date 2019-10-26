using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : StateMachineBehaviour {
    GameObject enemy;
    GameObject player;
    int currentWayPoint;
    [SerializeField]
    float radius;
    [SerializeField]
    int numberOfWaypoints;
    Vector2 initialPosition;
    Vector2[] wayPoints;
    int currentWaypoint;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        enemy = animator.gameObject;
        initialPosition = enemy.transform.position;
        wayPoints = new Vector2[numberOfWaypoints];
        currentWaypoint = 0;

        generateWaypoints();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        Vector2 currentPosition = (Vector2)enemy.transform.position;

        currentWaypoint += (Vector2.Distance(currentPosition, wayPoints[currentWaypoint]) < 0.2) ? 1 : 0;

        if (currentWaypoint >= numberOfWaypoints) {
            currentWaypoint = 0;
            generateWaypoints();
        }

        if (currentPosition != wayPoints[currentWaypoint]) {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, wayPoints[currentWaypoint], 3f * Time.deltaTime);
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}

    public void generateWaypoints()
    {
        for (int i = 0; i < numberOfWaypoints; i++)
        {
            wayPoints[i] = initialPosition + Random.insideUnitCircle * radius;
        }
    }

    //public IEnumerator moveEnemy(Transform enemyTransform)
    //{
    //    while(true)
    //    {
    //        Vector2 destination = initialPosition + Random.insideUnitCircle * 5.0f;

    //        while ((Vector2)enemy.transform.position != destination)
    //        {
    //            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, destination, 5.0f * Time.deltaTime);
    //            yield return null;
    //        }
    //        yield return new WaitForSeconds(3.0f);
    //    }
    //}


    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
