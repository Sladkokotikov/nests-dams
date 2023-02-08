using System;
using DG.Tweening;

public class SwitchTween
{
    private bool _movingForward, _movingBackward;

    private Tween _forward, _backward;
    private Action _killForward, _killBackward;

    public SwitchTween(Tween forward, Action killForward, Tween backward, Action killBackward)
    {
        _forward = forward;
        _killForward = killForward;
        _backward = backward;
        _killBackward = killBackward;

    }

    public void Forward()
    {
        if (_movingForward)
            return;
        if (_movingBackward)
        {
            _backward.Kill();
            _killBackward();
            _movingBackward = false;
        }

        _movingForward = true;
        _forward.OnComplete(() => _movingForward = false).Play();

    }

    public void Backward()
    {
        if (_movingBackward)
            return;
        if (_movingForward)
        {
            _forward.Kill();
            _killForward();
            _movingForward = false;
        }

        _movingBackward = true;
        _backward.OnComplete(() => _movingBackward = false).Play();
    }
}