using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.GPConquest.Player
{

    public class UMAController : MonoBehaviour
    {

        public Animator animator;
        public DestinationController destination;
        public CharacterController characterController;
        private Transform destinationTransform;
        [Range(0.2f, 2.5f)]
        public float speed = 2.0f;
        public string speedParamenter = "Forward";
        private bool dimension = false;
        private float SpeedDampTime = .00f;
        public uint playerNetworkId;

        // Use this for initialization
        void Start()
        {
            if (destination == null)
            {
                DestinationController[] playersInTheScene = FindObjectsOfType<DestinationController>();
                foreach (DestinationController dst in playersInTheScene)
                {
                    if (dst.networkObject.NetworkId.Equals(playerNetworkId))
                    {
                        destination = dst;
                        destinationTransform = destination.GetComponent<Transform>();
                    }

                }
            }
        }

        // Update is called once per frame
        void Update()
        {

            if (!ReferenceEquals(animator,null) || 
                (animator = GetComponent<Animator>()) && !ReferenceEquals(destinationTransform,null))
            {
                if (Vector3.Distance(destinationTransform.position, animator.rootPosition) > 5)
                {
                    animator.SetFloat(speedParamenter, speed, SpeedDampTime, Time.deltaTime);

                    Vector3 curentDir = animator.rootRotation * Vector3.forward;
                    Vector3 wantedDir = (destinationTransform.position - animator.rootPosition).normalized;

                    if (Vector3.Dot(curentDir, wantedDir) > 0)
                    {
                        transform.LookAt(destinationTransform);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    }
                    else
                    {
                        transform.LookAt(destinationTransform);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    }
                }
                else
                {
                    animator.SetFloat(speedParamenter, 0, SpeedDampTime, Time.deltaTime);
                }
            }

            //TODO : To Find a better way to scale up the UMA's scale
            if (dimension == false)
            {
                transform.localScale = new Vector3(1, 1, 1);
                dimension = true;
            }
        }

        void OnAnimatorMove()
        {
            if (characterController || (characterController = GetComponent<CharacterController>()))
            {
                characterController.Move(animator.deltaPosition);
                transform.rotation = animator.rootRotation;
            }
        }

    }

}
