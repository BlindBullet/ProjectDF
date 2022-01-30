//public static class ObservableExtension
//{
//	public static IMonoObservable EveryUpdate<T>(this IMonoObservable observable)
//	{
//		return new UpdateObservable<T>();
//	}
//}
/// <summary>
/// ObservingBehaviour는 매 프레임 Notify를 시도하는데, PlainObservable과 달리 이를 막지 않는다. 물리 등 기능 일부가 유니티에 시스템에
/// 종속되어 있는 한 일시적으로 필요할 것임.
/// </summary>
/// <typeparam name="T"></typeparam>
public class UpdateObservable<T> : ObservableBase<T>
{
	private bool _isNotifyInNextFrame;

	public UpdateObservable(T init = default(T)) : base()
	{
		_value = init;
	}

	public override void Notify()
	{
		if (_isNotifyInNextFrame)
		{
			base.Notify();
			_isNotifyInNextFrame = false;
		}
	}

	protected override void ValueChanged()
	{
		_isNotifyInNextFrame = true;
	}
}