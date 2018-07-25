package md5ba8cb0599a934ca092814c857c1e4308;


public class CustomVerticalViewScroller
	extends android.widget.ScrollView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"n_onLayout:(ZIIII)V:GetOnLayout_ZIIIIHandler\n" +
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Calendar.CustomVerticalViewScroller, Syncfusion.SfCalendar.Android", CustomVerticalViewScroller.class, __md_methods);
	}


	public CustomVerticalViewScroller (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomVerticalViewScroller.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Calendar.CustomVerticalViewScroller, Syncfusion.SfCalendar.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public CustomVerticalViewScroller (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomVerticalViewScroller.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Calendar.CustomVerticalViewScroller, Syncfusion.SfCalendar.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomVerticalViewScroller (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomVerticalViewScroller.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Calendar.CustomVerticalViewScroller, Syncfusion.SfCalendar.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public CustomVerticalViewScroller (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == CustomVerticalViewScroller.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Calendar.CustomVerticalViewScroller, Syncfusion.SfCalendar.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public boolean onTouchEvent (android.view.MotionEvent p0)
	{
		return n_onTouchEvent (p0);
	}

	private native boolean n_onTouchEvent (android.view.MotionEvent p0);


	public void onLayout (boolean p0, int p1, int p2, int p3, int p4)
	{
		n_onLayout (p0, p1, p2, p3, p4);
	}

	private native void n_onLayout (boolean p0, int p1, int p2, int p3, int p4);

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
