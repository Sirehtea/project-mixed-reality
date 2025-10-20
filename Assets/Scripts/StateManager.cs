using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState
{
    TVOn,
    TVOff,
    RadioOn,
    RadioOff
}

public class StateManager : MonoBehaviour
{
    public TVControllerXR tv;
    public SimpleRadio radio;
    private GameState _currentState;
    public InputActionReference actionReference;
    public void NextState()
    {
        if (_currentState != GameState.RadioOff) _currentState++;
        EvaluateState();
    }
    public void PreviousState()
    {
        if (_currentState != GameState.TVOn) _currentState--;
        EvaluateState();
    }
    private void EvaluateState()
    {
        print(_currentState);
        switch (_currentState)
        {
            case GameState.TVOn:
                tv.StartTV();
                break;
            case GameState.TVOff:
                tv.StopTV();
                radio.StopRadio(); // Temporary to prevent overlapping
                break;
            case GameState.RadioOn:
                radio.StartRadio();
                break;
            case GameState.RadioOff:
                radio.StopRadio();
                break;
        }
    }
}