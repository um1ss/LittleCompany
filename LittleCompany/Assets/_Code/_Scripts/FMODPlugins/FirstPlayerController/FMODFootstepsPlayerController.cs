using FMODUnity;
using UnityEngine;

public class FMODFootstepsPlayerController : MonoBehaviour
{
    [field: Header("Player SFX")]
    [field: SerializeField]
    private EventReference PlayerJump { get; set; }
    [field: SerializeField] private EventReference PlayerMetalHit { get; set; }
    [field: SerializeField] private EventReference PlayerWoodHit { get; set; }
    [field: SerializeField] private EventReference PlayerGrassHit { get; set; }


    [field: Header("Player Steps")]
    [field: SerializeField]
    private EventReference PlayerMetalFootsteps { get; set; }
    [field: SerializeField] private EventReference PlayerWoodFootsteps { get; set; }
    [field: SerializeField] private EventReference PlayerGrassFootsteps { get; set; }

    private FirstPlayerController _firstPlayerController;

    private void OnEnable()
    {
        _firstPlayerController.OnJump += HandleJump;
    }

    private void OnDisable()
    {
        _firstPlayerController.OnJump -= HandleJump;
    }

    private void Awake()
    {
        _firstPlayerController = GetComponent<FirstPlayerController>();
    }

    private void Update()
    {
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        if (!_firstPlayerController._characterController.isGrounded)
        {
            return;
        }

        if (_firstPlayerController._currentInput == Vector2.zero)
        {
            return;
        }

        _firstPlayerController._footstepTimer -= Time.deltaTime;

        if (_firstPlayerController._footstepTimer <= 0)
        {
            if (Physics.Raycast(_firstPlayerController._playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/Wood":
                        FMODEvents.Instance.PlayOneShot(PlayerWoodFootsteps, transform.position);
                        break;

                    case "Footsteps/Metal":
                        FMODEvents.Instance.PlayOneShot(PlayerMetalFootsteps, transform.position);
                        break;

                    case "Footsteps/Grass":
                        FMODEvents.Instance.PlayOneShot(PlayerGrassFootsteps, transform.position);
                        break;

                    default:
                        FMODEvents.Instance.PlayOneShot(PlayerMetalFootsteps, transform.position);
                        break;
                }

                _firstPlayerController._footstepTimer = _firstPlayerController.GetCurrentOffset;
            }
        }
    }

    private void HandleJump()
    {           
        FMODEvents.Instance.PlayOneShot(PlayerJump, transform.position);
    }

    private void HandleLanding()
    {
        if (Physics.Raycast(_firstPlayerController._playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
        {
            switch (hit.collider.tag)
            {
                case "Footsteps/Wood":
            
                    FMODEvents.Instance.PlayOneShot(PlayerWoodHit,
                        transform.position);
                    break;
                case "Footsteps/Metal":
            
                    FMODEvents.Instance.PlayOneShot(PlayerMetalHit,
                        transform.position);
                    break;
                case "Footsteps/Grass":
            
                    FMODEvents.Instance.PlayOneShot(PlayerGrassHit,
                        transform.position);
                    break;
                default:
            
                    FMODEvents.Instance.PlayOneShot(PlayerMetalHit,
                        transform.position);
                    break;
            }
        }
    }
    
    // private void WoodPlayBack()
    // {
    //     PLAYBACK_STATE WoodplaybackState;
    //
    //     playerWoodFootsteps.getPlaybackState(out WoodplaybackState);
    //     if (WoodplaybackState.Equals(PLAYBACK_STATE.STOPPED))
    //     {
    //         playerWoodFootsteps.start();
    //     }
    //     else
    //     {
    //         playerWoodFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //     }
    // }
    //
    // private void GrassPlayBack()
    // {
    //     PLAYBACK_STATE GrassplaybackState;
    //
    //     playerGrassFootsteps.getPlaybackState(out GrassplaybackState);
    //     if (GrassplaybackState.Equals(PLAYBACK_STATE.STOPPED))
    //     {
    //         playerGrassFootsteps.start();
    //     }
    //     else
    //     {
    //         playerGrassFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //     }
    // }
    //
    // private void MetalPlayBack()
    // {
    //     PLAYBACK_STATE MetallaybackState;
    //
    //     playerMetalFootsteps.getPlaybackState(out MetallaybackState);
    //     if (MetallaybackState.Equals(PLAYBACK_STATE.STOPPED))
    //     {
    //         playerMetalFootsteps.start();
    //     }
    //     else
    //     {
    //         playerMetalFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //     }
    // }
    //
    // private void DefaultPlayBack()
    // {
    //     PLAYBACK_STATE DefaultplaybackState;
    //
    //     playerDefaultFootsteps.getPlaybackState(out DefaultplaybackState);
    //     if (DefaultplaybackState.Equals(PLAYBACK_STATE.STOPPED))
    //     {
    //         playerDefaultFootsteps.start();
    //     }
    //     else
    //     {
    //         playerDefaultFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //     }
    // }
}
