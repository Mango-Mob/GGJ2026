using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Audio
{
    public class AudioAgent : MonoBehaviour
    {
        public AudioClip clip;
        public VolumeChannel channel;

        public bool playOnAwake = false;
        public bool loop = false;

        [SerializeField]
        [Range( 0.0f, 1.0f )]
        private float PlayingProgress = 0.0f;

        private Player player;
        private Player back_player;

        private const float random_pitch_min = 0.75f;
        private const float random_pitch_max = 1.25f;

        public void Awake()
        {
            player = new Player( gameObject );
            back_player = new Player( gameObject );
            player.current_clip = clip;
            player.channel = channel;
            player.looping = loop;

            if ( playOnAwake )
                player.Play();
        }

        public void Update()
        {
            player.Update();
            back_player.Update();

            if ( player.paused || back_player.paused )
                return;

            if ( back_player.has_stopped_this_frame )
            {
                StopBuffer( ref back_player );
                return;
            }
            else if ( !back_player.is_playing && back_player.current_clip != null )
            {
                //Check if queued audio should play
                if ( !player.is_playing )
                {
                    SwapBuffers();
                    player.Play();
                    StopBuffer( ref back_player );
                }
                else if ( player.time_left <= back_player.GetFadeIn() )
                {
                    back_player.PlayFadeIn( Mathf.Min( back_player.GetFadeIn(), player.time_left ) );
                }
            }

            if ( !player.is_playing && back_player.is_playing )
            { 
                SwapBuffers();
                StopBuffer( ref back_player );
            }
        }
        public void OnValidate()
        {
            if ( player is null )
                return;

            if ( PlayingProgress == player.progress )
                return;

            if ( back_player.is_playing )
                return; //Don't update if the back player is fading in

            player.progress = PlayingProgress;
        }

        public void LateUpdate()
        {
            player.LateUpdate();
            back_player.LateUpdate();

            PlayingProgress = player.progress;
        }
        public void OnDestroy()
        {
            player.OnDestroy();
            back_player.OnDestroy();
        }

        public void ForcePlay( AudioClip clip, float fade_duration = 0.0f, bool random_pitch = false )
        {
            StopBuffer( ref player );
            player.current_clip = clip;
            player.pitch = random_pitch ? Random.Range( random_pitch_min, random_pitch_max ) : 1.0f;
            player.PlayFadeIn( fade_duration );
        }

        public bool Play( AudioClip clip, float fade_duration = 0.0f, bool random_pitch = false )
        {
            if ( clip is null )
                return false;

            if ( fade_duration > 0.0f )
            {
                player.StopFadeOut( Mathf.Min( fade_duration, player.time_left ) );
                StopBuffer( ref back_player );

                SwapBuffers();
                player.current_clip = clip;
                player.pitch = random_pitch ? Random.Range( random_pitch_min, random_pitch_max ) : 1.0f;
                player.PlayFadeIn( fade_duration );
                back_player.StopFadeOut( fade_duration );
            }
            else
                ForcePlay( clip, fade_duration, random_pitch );

            return true;
        }

        public bool Queue( AudioClip clip, float fade_duration = 0.0f, bool random_pitch = false )
        {
            if ( back_player.is_playing )
                return false; //Can't interupt the queue as we are fading in already

            player.looping = false;
            back_player.looping = loop;
            back_player.current_clip = clip;
            back_player.pitch = random_pitch ? Random.Range( random_pitch_min, random_pitch_max ) : 1.0f;
            player.SetFadeOut( Mathf.Min( fade_duration, player.time_left ) );
            back_player.SetFadeIn( Mathf.Min( fade_duration, player.time_left ) );
            return true;
        }

        public void Stop()
        {
            StopBuffer( ref back_player );
            StopBuffer( ref player );
        }

        private void StopBuffer( ref Player player )
        {
            player.Stop();
            player.current_clip = null;
        }

        private void SwapBuffers() 
        { 
            (player, back_player) = (back_player, player); 
        }
    }
}
