public class PlainObservable<T> : ObservableBase<T>
{
	public PlainObservable(T init = default(T)) : base()
	{
		_value = init;
	}

	protected override void ValueChanged()
	{
		base.Notify();
		return;
	}
}