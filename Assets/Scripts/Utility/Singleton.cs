using UnityEngine;

/*
 * Singleton by Tarodev
 * File: Singleton.cs
 * Description:
 *		An abstract class representation of a singleton class, which would be used as a class parent.
 *		This code was adapted from the video: https://www.youtube.com/watch?v=tE1qH8OxO2Y
 */

namespace Utility
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if ( _instnace == null )
                {
                    if ( quitting )
                        return null;

                    GameObject g = new GameObject();
                    _instnace = g.AddComponent<T>();
                    g.name = _instnace.GetType().Name;
                }
                return _instnace;
            }
            set { _instnace = value; }
        }

        private static T _instnace;
        private static bool quitting = false;

        protected virtual void Awake()
        {
            if ( !HasInstance() )
                _instnace = this as T;

            if ( _instnace != this )
                Destroy( gameObject );
        }

        protected virtual void OnDestroy()
        {
            if ( _instnace == this )
                _instnace = null;
        }

        protected void OnApplicationQuit()
        {
            quitting = true;
            _instnace = null;
            Destroy( gameObject );
        }

        public static bool HasInstance()
        {
            return _instnace != null;
        }
    }

    public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();

            if ( transform.parent != null && transform.parent.gameObject != null )
                DontDestroyOnLoad( transform.parent.gameObject );
            else
                DontDestroyOnLoad( gameObject );
        }
    }
}