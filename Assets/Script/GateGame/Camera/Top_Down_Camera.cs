using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sun.Camera
{
    public class Top_Down_Camera : MonoBehaviour
    {
        #region Variables
        public Transform m_target;

        [SerializeField]
        private float m_Height = 10f;

        [SerializeField]
        private float m_Distance = 20f;

        [SerializeField]
        private float m_Angle = 45f;

        [SerializeField]
        private float m_SmoothSpeed = 0.5f;

        private Vector3 refVelocity;
        #endregion

        #region Main Methods
        // Start is called before the first frame update
        void Start()
        {
            HandleCamera();
        }

        // Update is called once per frame
        void Update()
        {
            HandleCamera();
        }

        #endregion

        #region Helper Methods
        protected virtual void HandleCamera()
        {
            if (!m_target)
            {
                return;
            }

            Vector3 worldPosition = (Vector3.forward * -m_Distance) + (Vector3.up * m_Height);
            Vector3 rotatedVector = Quaternion.AngleAxis(m_Angle, Vector3.up) * worldPosition;

            Vector3 flatTargetPoristion = m_target.position;
            flatTargetPoristion.y = 0f;
            Vector3 finalPosition = flatTargetPoristion + rotatedVector;

            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, m_SmoothSpeed);
            //transform.position = finalPosition;
            transform.LookAt(m_target.position);

        }
        #endregion
    }
}

