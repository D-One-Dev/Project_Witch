using TMPro;
using UnityEngine;
using Zenject;

public class TextAnimator : ITickable, IInitializable
{
    [Inject(Id = "DialogueText")]
    private readonly TMP_Text _dialogueText;
    [Inject(Id = "Rainbow")]
    private readonly Gradient _rainbow;

    private Mesh _mesh;
    private Vector3[] _verticies;
    private Color[] _colors;
    private AnimationType _currentAnimation;
    private float _timeScale, _magnitude;

    public enum AnimationType 
    {
        None = 0,
        Wobble = 1,
        VerticalWobble = 2,
        RGB = 3
    }

    public void Tick()
    {
        if(_dialogueText.gameObject.activeInHierarchy && _currentAnimation != AnimationType.None)
            UpdateText(_timeScale, _magnitude);
    }

    private void UpdateText(float timeScale, float magnitude)
    {
        _dialogueText.ForceMeshUpdate();
        _mesh = _dialogueText.mesh;
        _verticies = _mesh.vertices;
        _colors = _mesh.colors;

        for (int i = 0; i < _dialogueText.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = _dialogueText.textInfo.characterInfo[i];
            int index = c.vertexIndex;
            if(_currentAnimation == AnimationType.RGB)
            {
                for (int j = index; j < index + 4; j++)
                {
                    _colors[j] = _rainbow.Evaluate(Mathf.Repeat(Time.time * timeScale + _verticies[j].x*.001f, 1f)*Time.timeScale);
                }
            }
            else
            {
                Vector3 offset;
                switch (_currentAnimation)
                {
                    case AnimationType.Wobble:
                        offset = Wobble(Time.unscaledTime + i, timeScale, magnitude);
                        break;
                    case AnimationType.VerticalWobble:
                        offset = VerticalWobble(Time.unscaledTime + i, timeScale, magnitude);
                        break;
                    default:
                        offset = Vector3.zero;
                        break;
                }
                offset *= Time.timeScale;
                for(int j = index; j < index + 4; j++)
                {
                    _verticies[j] += offset;
                }
                _verticies[i] += offset;
            }
        }

        _mesh.vertices = _verticies;
        _mesh.colors = _colors;
        _dialogueText.canvasRenderer.SetMesh(_mesh);
    }

    private Vector2 Wobble(float time, float timeScale, float magnitude)
    {
        return new Vector2(Mathf.Sin(time * timeScale) * magnitude, Mathf.Cos(time * timeScale) * magnitude);
    }

    private Vector2 VerticalWobble(float time, float timeScale, float magnitude)
    {
        return new Vector2(0f, Mathf.Sin(time * timeScale) * magnitude);
    }

    public void SetAnimation((AnimationType, float, float) parameters)
    {
        _currentAnimation = parameters.Item1;
        _timeScale = parameters.Item2;
        _magnitude = parameters.Item3;
    }

    public void Initialize()
    {
        SetAnimation((AnimationType.Wobble, 5f, 2f));
    }
}