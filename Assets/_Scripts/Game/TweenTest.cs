using UnityEngine;
using Moonvalk.Animation;

public class TweenTest : MonoBehaviour
{
    [SerializeField]
    public Easing.Types EasingType1;

    [SerializeField]
    public Easing.Types EasingType2;

    [SerializeField]
    public Easing.Types EasingType3;

    protected Vector3 _position;
    protected float _rotation;
    protected float _scale;

    /// <summary>
    /// Unity Event - Occurs when this object first starts.
    /// </summary>
    private void Start()
    {
        _position = new Vector3();

        MVTween tween1 = new MVTween(() => ref this._position.x, () => ref this._position.y, () => ref this._rotation, () => ref this._scale);
        tween1.To(1f, 2f, -100f, 1.2f).Duration(2f).Delay(1f)
            .Ease(Easing.Functions[EasingType1], Easing.Functions[EasingType2], Easing.Functions[EasingType3])
            .OnComplete(() => { 
                MVTween tween2 = new MVTween(() => ref this._position.x, () => ref this._position.y, () => ref this._rotation, () => ref this._scale);
                tween2.To(-1f, -2f, 100f, 0.5f).Duration(2f)
                    .Ease(Easing.Functions[EasingType1], Easing.Functions[EasingType2], Easing.Functions[EasingType3])
                    .OnComplete(() => { tween1.Start(); })
                    .OnUpdate(() => { updateTransform(); })
                    .Start();
            })
            .OnUpdate(() => { updateTransform(); })
            .Start();
    }

    protected void updateTransform()
    {
        transform.position = this._position;
        transform.rotation = Quaternion.Euler(0f, 0f, this._rotation);
        transform.localScale = this._scale * Vector3.one;
    }

    /// <summary>
    /// Unity Event - Occurs each game tick as this object is updated.
    /// </summary>
    private void Update()
    {

    }
}
