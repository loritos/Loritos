package md5c36c76e1ccfede8aadee04a8ef4f21fb;


public class GradientColorStackRenderer
	extends md5b60ffeb829f638581ab2bb9b1a7f4f3f.VisualElementRenderer_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_dispatchDraw:(Landroid/graphics/Canvas;)V:GetDispatchDraw_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("GradientColor.Droid.GradientColorStackRenderer, FishOnLine.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GradientColorStackRenderer.class, __md_methods);
	}


	public GradientColorStackRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == GradientColorStackRenderer.class)
			mono.android.TypeManager.Activate ("GradientColor.Droid.GradientColorStackRenderer, FishOnLine.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public GradientColorStackRenderer (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == GradientColorStackRenderer.class)
			mono.android.TypeManager.Activate ("GradientColor.Droid.GradientColorStackRenderer, FishOnLine.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public GradientColorStackRenderer (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == GradientColorStackRenderer.class)
			mono.android.TypeManager.Activate ("GradientColor.Droid.GradientColorStackRenderer, FishOnLine.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public void dispatchDraw (android.graphics.Canvas p0)
	{
		n_dispatchDraw (p0);
	}

	private native void n_dispatchDraw (android.graphics.Canvas p0);

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
