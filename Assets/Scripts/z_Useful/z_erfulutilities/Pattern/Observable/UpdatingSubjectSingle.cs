/// <summary>
/// 그저 바인딩된 함수를 매 프레임 부름.
/// </summary>
public class UpdatingSubjectSingle<T> : SubjectSingleBase<T>
{
	public UpdatingSubjectSingle(T init = default(T)) : base()
	{
		_value = init;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
	}

	protected override void ValueChanged()
	{
		
	}
}