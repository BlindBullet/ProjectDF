/// <summary>
/// Value가 한 프레임 안에서 여러번 바뀌었어도, 딱 한번만 Notify해줌. 일종의 Lazy인데, 변경이 다음프레임으로 밀릴수도 있음에 주의
/// <typeparam name="T"></typeparam>
public class ThrottleSubjectSingle<T> : SubjectSingleBase<T>
{
	private bool _isNotifyInNextFrame;

	public ThrottleSubjectSingle(T init = default(T)) : base()
	{
		_value = init;
	}

	public override void OnUpdate()
	{
		if (_isNotifyInNextFrame)
		{
			base.OnUpdate();
			_isNotifyInNextFrame = false;
		}
	}

	protected override void ValueChanged()
	{
		_isNotifyInNextFrame = true;
	}
}