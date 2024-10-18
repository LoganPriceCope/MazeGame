using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Player
{


    public class PlayerScript : MonoBehaviour
    {

        // EEE
        public Transform[] points;
        public NavMeshAgent nav;
        public int destPoint;

        public int maxDistanceOk = 100;

        public RaycastHit hit;

        public Canvas canvas;
        public GameObject player;
        //EEE

        public LayerMask playerLayer;

            public float sightRange, killRange;
            public bool playerInSightRange, playerInKillRange;

        // variables holding the different player states
        public AttackState attackState;
        public PartolState partolState;

        public StateMachine sm;



        // Start is called before the first frame update
        void Start()
        {
            nav = GetComponent<NavMeshAgent>();

            sm = gameObject.AddComponent<StateMachine>();

            // add new states here
            attackState = new AttackState(this, sm);
            partolState = new PartolState(this, sm);

            // initialise the statemachine with the default state
            sm.Init(partolState);
        }

        // Update is called once per frame
        public void Update()
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            playerInKillRange = Physics.CheckSphere(transform.position, killRange, playerLayer);

            sm.CurrentState.LogicUpdate();

            //output debug info to the canvas
            string s;
            s = string.Format("last state={0}\ncurrent state={1}", sm.LastState, sm.CurrentState);

            //UIscript.ui.DrawText(s);

            //UIscript.ui.DrawText("Press I for idle / R for run");

        }



        void FixedUpdate()
        {
            sm.CurrentState.PhysicsUpdate();
            Debug.Log(sm.CurrentState);
            Attack();
            Kill();
        }



        public void CheckForPartol()
        {
            if (!playerInSightRange)
            {
                sm.ChangeState(partolState);
            }
        }

        public void CheckForAttack(Vector3 center, float radius)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            foreach(var hitCollider in hitColliders)
            {
                hitCollider.SendMessage("E");
            }
        }

        public void Attack()
        {
            if (playerInSightRange == true)
            {
                sm.ChangeState(attackState);
            }
        }

        public void Kill()
        {
            if (playerInKillRange)
            {
                Debug.Log("Enters");
                canvas.enabled = true;
                Invoke("Restart", 5f);
            }
        }

        public void Restart()
        {
            SceneManager.LoadScene("SampleScene");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, sightRange);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, killRange);
        }
    }

}