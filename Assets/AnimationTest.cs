using UnityEngine;
using Moonvalk.Animation;
using Moonvalk.Utility;

public class AnimationTest : MonoBehaviour
{
    protected Vector3 _rotation;

    private void Start()
    {
        this._rotation = new Vector3();

        MVTween tween = new MVTween(()=>ref this._rotation.x, ()=>ref this._rotation.y, ()=>ref this._rotation.z);
        tween.To(360f, 360f, 360f).Duration(8f)
            .Ease(Easing.Cubic.InOut, Easing.Exponential.InOut, Easing.Linear.None)
            .OnComplete(() => {
                this._rotation = Vector3.zero;
                tween.Start();
            })
            .OnUpdate(() => {
                transform.rotation = Quaternion.Euler(this._rotation);
            });

        MVTimer timer = new MVTimer();
        timer.Duration(5f).OnComplete(() => { tween.Start(); }).Start();
    }

    private void Update()
    {
    }
}
