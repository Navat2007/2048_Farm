using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Vector2 _firstPressPos;
    private Vector2 _secondPressPos;
    private Vector2 _currentSwipe;
    private bool _isInputsEnabled = true;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.UI.Pause.performed += OnPause;

        EventBus.GameEvents.OnGameStarted += OnStartLevel;
        EventBus.GameEvents.OnGameEnded += OnEndLevel;
        EventBus.GameEvents.OnPause += DisablePlayerInput;
        EventBus.GameEvents.OnUnPause += EnablePlayerInput;
        EventBus.UIEvents.OnMainMenuWindowShow += OnEndLevel;
    }

    private void OnDestroy()
    {
        _playerInput.UI.Pause.performed -= OnPause;

        EventBus.GameEvents.OnGameStarted -= OnStartLevel;
        EventBus.GameEvents.OnGameEnded -= OnEndLevel;
        EventBus.GameEvents.OnPause -= DisablePlayerInput;
        EventBus.GameEvents.OnUnPause -= EnablePlayerInput;
        EventBus.UIEvents.OnMainMenuWindowShow -= OnEndLevel;
    }

    private void Update()
    {
        if (_isInputsEnabled)
        {
            Vector2 inputVector = _playerInput.Player.Move.ReadValue<Vector2>();
            inputVector = new Vector2(Mathf.RoundToInt(inputVector.x), Mathf.RoundToInt(inputVector.y)).normalized;

            EventBus.InputEvents.OnInputMoveChange?.Invoke(inputVector);

            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Began)
                {
                    //save began touch 2d point
                    _firstPressPos = new Vector2(t.position.x, t.position.y);
                }

                if (t.phase == TouchPhase.Ended)
                {
                    //save ended touch 2d point
                    _secondPressPos = new Vector2(t.position.x, t.position.y);

                    //create vector from the two points
                    _currentSwipe = new Vector3(_secondPressPos.x - _firstPressPos.x,
                        _secondPressPos.y - _firstPressPos.y);

                    //normalize the 2d vector
                    _currentSwipe.Normalize();

                    //swipe upwards
                    if (_currentSwipe.y > 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                    {
                        EventBus.InputEvents.OnInputMoveChange?.Invoke(Vector2.up);
                    }

                    //swipe down
                    if (_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                    {
                        EventBus.InputEvents.OnInputMoveChange?.Invoke(Vector2.down);
                    }

                    //swipe left
                    if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                    {
                        EventBus.InputEvents.OnInputMoveChange?.Invoke(Vector2.left);
                    }

                    //swipe right
                    if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                    {
                        EventBus.InputEvents.OnInputMoveChange?.Invoke(Vector2.right);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                _firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }

            if (Input.GetMouseButtonUp(0))
            {
                //save ended touch 2d point
                _secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                //create vector from the two points
                _currentSwipe = new Vector2(_secondPressPos.x - _firstPressPos.x, _secondPressPos.y - _firstPressPos.y);

                //normalize the 2d vector
                _currentSwipe.Normalize();

                //swipe upwards
                if (_currentSwipe.y > 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                {
                    EventBus.InputEvents.OnInputMoveChange?.Invoke(Vector2.up);
                }

                //swipe down
                if (_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                {
                    EventBus.InputEvents.OnInputMoveChange?.Invoke(Vector2.down);
                }

                //swipe left
                if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                {
                    EventBus.InputEvents.OnInputMoveChange?.Invoke(Vector2.left);
                }

                //swipe right
                if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                {
                    EventBus.InputEvents.OnInputMoveChange?.Invoke(Vector2.right);
                }
            }
        }
    }

    private void OnPause(InputAction.CallbackContext obj)
    {
        if (ServiceLocator.GameManager.GetState == GameManager.GameState.PLAY)
        {
            EventBus.GameEvents.OnPause?.Invoke();
        }
        else
        {
            EventBus.GameEvents.OnUnPause?.Invoke();
        }
    }

    private void OnStartLevel()
    {
        EnablePlayerInput();
        _playerInput.UI.Enable();
    }

    private void OnEndLevel()
    {
        DisablePlayerInput();
        _playerInput.UI.Disable();
    }

    private void OnEndLevel(bool success)
    {
        DisablePlayerInput();
        _playerInput.UI.Disable();
    }

    private void EnablePlayerInput()
    {
        _playerInput.Player.Enable();
        _isInputsEnabled = true;
    }

    private void DisablePlayerInput()
    {
        _playerInput.Player.Disable();
        _isInputsEnabled = false;
        
        _firstPressPos = Vector2.zero;
        _secondPressPos = Vector2.zero;
        _currentSwipe = Vector2.zero;
    }
}