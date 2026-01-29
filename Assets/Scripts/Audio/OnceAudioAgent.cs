using Audio;
using UnityEngine;

namespace Audio
{
    public class OnceAudioAgent : MonoBehaviour
    {
        public AudioClip clip;
        public bool deleteOnEnd = true;
        public Vector2 randomPitch = Vector2.one;
        public float fade_in = 0.0f;
        public VolumeChannel channel = VolumeChannel.SoundEffects;

        [SerializeField]
        [Range( 0.0f, 1.0f )]
        private float PlayingProgress = 0.0f;

        private Player player;

        void Awake()
        {
            player = new Player( gameObject );
            player.channel = channel;
            player.current_clip = clip;
            player.pitch = Random.Range( randomPitch.x, randomPitch.y );
            player.PlayFadeIn( fade_in );
        }

        void Update()
        {
            player.Update();
            if ( deleteOnEnd && !player.is_playing && !player.paused )
                Destroy( gameObject );
        }

        private void LateUpdate()
        {
            player.LateUpdate();

            PlayingProgress = player.progress;
        }

        public void OnValidate()
        {
            if ( player is null )
                return;

            if ( PlayingProgress == player.progress )
                return;

            player.progress = PlayingProgress;
        }

        public void OnDestroy()
        {
            player.OnDestroy();
        }
    }
}
