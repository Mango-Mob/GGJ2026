using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace Audio
{
    public class Player
    {
        public Player( GameObject owner )
        {
            source = owner.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.volume = 0.0f;
        }

        public VolumeChannel channel;
        public AudioClip current_clip { get { return source.clip; } set { Stop(); source.clip = value; } }
        public AudioSource source;
        protected bool was_playing = false; //Caches the result of is_playing between frames
        public float progress
        {
            get { return is_playing ? source.time / source.clip.length : 0.0f; }
            set
            {
                if ( !source )
                    return;

                source.time = source.clip.length * Mathf.Clamp( value, 0.0f, 1.0f );
            }
        }

        public float time_left { 
            get 
            {
                if ( !is_playing )
                    return 0.0f;

                if ( _time_left >= 0.0f )
                    return _time_left;

                return source.clip.length - source.time; 
            } 
            set
            {
                _time_left = value;
            }
        }
        private float _time_left = -1.0f;

        public void Update()
        {
            if ( !is_playing || paused )
                return;

            float fade_in_volume = IsFadingIn() ? Mathf.Clamp( source.time / GetFadeIn(), 0.0f, 1.0f ) : 1.0f;
            float fade_out_volume = IsFadingOut() ? Mathf.Clamp( time_left / GetFadeOut(), 0.0f, 1.0f ) : 1.0f;

            source.volume = _volume * Audio.AudioManager.Instance.GetVolume( channel ) * fade_in_volume * fade_out_volume;

            if ( _time_left > 0.0f )
            {
                _time_left -= Time.deltaTime;
                if ( _time_left <= 0.0f )
                    source.Stop();
            }
        }

        public void LateUpdate() { was_playing = is_playing; }
        public void OnDestroy() { Stop(); }
        public bool mute { get { return _volume <= 0.0f; } set { _volume = ( value ? 0.0f : 1.0f ); } }

        public float volume { get { return source.volume; } set { _volume = value; } }
        private float _volume = 1.0f;

        public float pitch { get { return source.pitch; } set { source.pitch = value; } }

        public bool looping { get { return source.loop; } set { source.loop = value; } }
        public bool is_playing { get { return source && source.isPlaying; } }
        public bool has_stopped_this_frame { get { return source && !paused && !source.isPlaying && was_playing; } }

        public bool paused
        {
            get { return _paused; }
            set
            {
                if ( value )
                    source.Pause();
                else
                    source.UnPause();

                _paused = value;
            }
        }
        private bool _paused = false;

        public void Play() 
        {
            if ( is_playing )
                return;

            source.Play();
            was_playing = true; 
            volume = 1.0f;
            time_left = -1.0f;

            Audio.AudioManager.Instance.playing_audio.Add( this );
            paused = Audio.AudioManager.Instance.paused;
            mute = Audio.AudioManager.Instance.muted;
        }
        
        public void Stop() 
        { 
            source.Stop();
            source.volume = 0.0f;

            time_left = -1.0f;
            _paused = false;
            mute = false;
            Audio.AudioManager.Instance?.playing_audio.Remove( this );
        }

        public int CompareTo( ref Player other )
        {
            if ( looping != other.looping )
                return looping ? 1 : -1;

            if ( time_left != other.time_left ) 
                return time_left < other.time_left ? -1 : 1;

            return 0;
        }
#region Fade
        private Vector2 fade_value; //x in, y out percentages

        public float GetFadeIn() { return source && source.clip ? fade_value.x * source.clip.length : 0.0f; }
        public void SetFadeIn( float duration )
        {
            fade_value.x = Mathf.Clamp( duration / source.clip.length, 0.0f, 1.0f );
        }
        public void PlayFadeIn( float duration ) { SetFadeIn( duration ); SetFadeOut( 0.0f ); Play(); }
        public float GetFadeOut() { return source && source.clip ? source.clip.length - fade_value.y * source.clip.length : 0.0f; }
        public void SetFadeOut( float duration )
        {
            fade_value.y = Mathf.Clamp( 1.0f - duration / source.clip.length, 0.0f, 1.0f );
        }
        public void StopFadeOut( float duration ) { SetFadeOut( duration ); SetFadeIn( 0.0f ); time_left = duration; }

        public bool IsFadingIn() { return GetFadeIn() >= source.time; }
        public bool IsFadingOut() { return GetFadeOut() <= source.time; }
#endregion
    }
}