using Unity.XR.CoreUtils.Bindings;
using UnityEngine;

namespace Unity.VRTemplate
{
    /// <summary>
    /// This script is designed to be used on a camera to allow it to smoothly follow a target transform, while keeping the position updated 3 times in the frame.
    /// </summary>
    public class SmoothTrackingFollow : MonoBehaviour
    {
        [SerializeField]
        Transform m_TargetTransform = null;

        [Header("Smoothing config")]
        [SerializeField]
        [Tooltip("If the target is under the same relative root as this object, it's best to use local space. With XR Origin involved this helps resolve jumps during locomotion and snap turns.")]
        bool m_UseLocalSpace = true;

        [Header("Tween Settings")]

        [SerializeField]
        [Tooltip("The minimum angle offset threshold allowed before the target starts updating.")]
        float m_MinAngleOffsetAllowed = 0.1f;

        [SerializeField]
        [Tooltip("The maximum angle offset threshold allowed before the target starts updating.s")]
        float m_MaxAngleOffsetAllowed = 3f;

        [SerializeField]
        [Tooltip("The lower bound of speed used when the object is far from it's target.")]
        float m_LowerSpeed = 8f;

        [SerializeField]
        [Tooltip("The upper bounds of speed used when the object is close to it's target.")]
        float m_UpperSpeed = 12f;

        SmartFollowQuaternionTweenableVariable m_QuaternionTweenableVariable;
        BindingsGroup m_BindingsGroup = new BindingsGroup();

        void Awake()
        {
            m_QuaternionTweenableVariable = new SmartFollowQuaternionTweenableVariable(m_MinAngleOffsetAllowed, m_MaxAngleOffsetAllowed, 3f);
            Application.onBeforeRender += OnBeforeRender;
        }

        void OnDestroy()
        {
            Application.onBeforeRender -= OnBeforeRender;
        }

        void OnEnable()
        {
            if (m_TargetTransform == null)
            {
                Debug.LogError("Target transform is null. Please assign a target transform.");
                enabled = false;
                return;
            }

            m_BindingsGroup.AddBinding(m_QuaternionTweenableVariable.SubscribeAndUpdate(newRot =>
            {
                if (m_UseLocalSpace)
                    transform.localRotation = newRot;
                else
                    transform.rotation = newRot;
            }));
        }

        void OnDisable()
        {
            m_BindingsGroup.Clear();
        }

        void Update()
        {
            UpdatePosition();
            m_QuaternionTweenableVariable.SetTargetWithinThreshold(m_UseLocalSpace ? m_TargetTransform.localRotation : m_TargetTransform.rotation);
            m_QuaternionTweenableVariable.HandleSmartTween(Time.deltaTime, m_LowerSpeed, m_UpperSpeed);
        }

        void LateUpdate()
        {
            UpdatePosition();
        }

        void OnBeforeRender()
        {
            UpdatePosition();
        }

        void UpdatePosition()
        {
            if (m_UseLocalSpace)
                transform.localPosition = m_TargetTransform.localPosition;
            else
                transform.position = m_TargetTransform.position;
        }
    }
}
