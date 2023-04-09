using UnityEngine;
using Spine;
using Spine.Unity;

namespace Framework
{
    [DisallowMultipleComponent, RequireComponent(typeof(SkeletonGraphic))]
    public class SpineUIEffect : MonoBehaviour
    {
        public static SpineUIEffect GetByPath(GameObject container, string path)
        {
            GameObject foundGameObject = container.transform.Find(path).gameObject;
            if (foundGameObject == null) return null;

            return Get(foundGameObject);
        }

        public static SpineUIEffect Get(GameObject gameObject)
        {
            if (gameObject == null) return null;
            if (gameObject.GetComponent<SkeletonGraphic>() == null)
            {
                return null;
            }

            SpineUIEffect ta = gameObject.AddComponent<SpineUIEffect>();
            ta.Init();
            return ta;
        }

        private SkeletonGraphic skeletonGraphic;
        private float customSpeed = -1f;

        private bool isInited = false;

        private void Init()
        {
            if (isInited)
            {
                return;
            }

            skeletonGraphic = GetComponent<SkeletonGraphic>();
            if (skeletonGraphic != null)
            {
                skeletonGraphic.Initialize(false);

                if(customSpeed >= 0f)
                {
                    skeletonGraphic.timeScale = customSpeed;
                }
            }

            isInited = true;
        }

        private void Awake()
        {
            Init();
        }

        public float GetSpeed()
        {
            if(customSpeed < 0f)
            {
                if(skeletonGraphic != null)
                {
                    return skeletonGraphic.timeScale;
                }
                return 0f;
            }
            return customSpeed;
        }

        public void SetSpeed(float speed)
        {
            customSpeed = speed;
            if (skeletonGraphic != null)
            {
                skeletonGraphic.timeScale = customSpeed;
            }
        }

        public void Play(string animationName, bool isLoop = false)
        {
            Init();
            if (skeletonGraphic != null)
            {
                skeletonGraphic.AnimationState.SetAnimation(0, animationName, isLoop);
            }
        }

        public void Stop()
        {
            if (skeletonGraphic != null)
            {
                skeletonGraphic.AnimationState.ClearTracks();
            }
        }

        private void OnDestroy()
        {
            skeletonGraphic = null;
        }
    }
}