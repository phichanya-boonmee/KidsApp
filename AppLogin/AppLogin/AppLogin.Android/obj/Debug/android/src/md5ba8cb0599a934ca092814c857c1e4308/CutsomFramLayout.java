package md5ba8cb0599a934ca092814c857c1e4308;


public class CutsomFramLayout
	extends android.widget.FrameLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_setBackgroundColor:(I)V:GetSetBackgroundColor_IHandler\n" +
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Calendar.CutsomFramLayout, Syncfusion.SfCalendar.Android", CutsomFramLayout.class, __md_methods);
	}


	public CutsomFramLayout (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CutsomFramLayout.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Calendar.CutsomFramLayout, Syncfusion.SfCalendar.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public CutsomFramLayout (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CutsomFramLayout.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Calendar.CutsomFramLayout, Syncfusion.SfCalendar.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CutsomFramLayout (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CutsomFramLayout.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Calendar.CutsomFramLayout, Syncfusion.SfCalendar.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public CutsomFramLayout (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == CutsomFramLayout.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Calendar.CutsomFramLayout, Syncfusion.SfCalendar.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void setBackgroundColor (int p0)
	{
		n_setBackgroundColor (p0);
	}

	private native void n_setBackgroundColor (int p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
