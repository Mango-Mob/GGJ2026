using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    public enum VolumeChannel : int { SoundEffects, Music, NumVolumeTypes };

    public class Constants
    {
        public const bool PAUSE_ON_LOSE_FOCUS = true;
        public const bool MUTE_ON_LOSE_FOCUS = false;
    }

    [DefaultExecutionOrder(-80)] 
    public class AudioManager : Utility.SingletonPersistent<AudioManager>
    {
        public List<Audio.Player> playing_audio = new List<Audio.Player>();
        [ReadOnlyAttribute(true)] public int playing = 0;

        public float master_volume { get; protected set; } = 1.0f;
        private float[] volumes = new float[ ( int )VolumeChannel.NumVolumeTypes ];

        private AudioAgent global_agent;

        public bool paused
        {
            get { return is_paused; }
            set
            {
                is_paused = value;
                foreach ( var audio in playing_audio )
                    audio.paused = is_paused;
            } 
        }
        private bool is_paused = false;

        public bool muted
        {
            get { return is_muted; }
            set
            {
                is_muted = value;
                foreach ( var audio in playing_audio )
                    audio.mute = is_muted;
            }
        }
        private bool is_muted = false;

        protected override void Awake()
        {
            base.Awake();

            master_volume = PlayerPrefs.GetFloat( $"master_volume", 1.0f );
            for ( int i = 0; i < ( int )VolumeChannel.NumVolumeTypes; i++ ) 
            {
                volumes[ i ] = PlayerPrefs.GetFloat( $"volume{i}", 1.0f );
            }

            global_agent = gameObject.AddComponent<AudioAgent>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            PlayerPrefs.SetFloat( $"master_volume", master_volume );
            for ( int i = 0; i < volumes.Length; i++ )
            {
                PlayerPrefs.SetFloat( $"volume{i}", volumes[ i ] );
            }
        }

        protected void Update()
        {
            playing = playing_audio.Count;
        }

        protected void OnApplicationFocus( bool focus )
        {
            if ( Audio.Constants.PAUSE_ON_LOSE_FOCUS )
                paused = !focus;
            if ( Audio.Constants.MUTE_ON_LOSE_FOCUS )
                muted = !focus;
        }

        public bool Play( AudioClip clip, bool looping = false, float fade_in = 0.0f )
        {
            global_agent.loop = looping;
            return global_agent.Play( clip, fade_in ); ;
        }

        public float GetVolume( VolumeChannel type )
        {
            return Mathf.Clamp( master_volume * volumes[ ( int )type ], 0.0f, 1.0f );
        }

        public float GetVolumeValue( VolumeChannel type ) { return Mathf.Clamp( volumes[ ( int )type ], 0.0f, 1.0f ); }
        public void SetVolumeValue( VolumeChannel type, float value ) { volumes[ ( int )type ] = Mathf.Clamp( value, 0.0f, 1.0f ); }
    }
}