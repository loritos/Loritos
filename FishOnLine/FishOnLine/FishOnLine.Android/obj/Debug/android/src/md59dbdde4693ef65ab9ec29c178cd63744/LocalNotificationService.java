package md59dbdde4693ef65ab9ec29c178cd63744;


public class LocalNotificationService
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("LocalNotification.Droid.LocalNotificationService, FishOnLine.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LocalNotificationService.class, __md_methods);
	}


	public LocalNotificationService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LocalNotificationService.class)
			mono.android.TypeManager.Activate ("LocalNotification.Droid.LocalNotificationService, FishOnLine.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
