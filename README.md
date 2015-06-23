# Unity-ThreadTask
Simple threaded tasks for Unity (5.x)
Runs a task in a different thread, then sends the data back to a callback method on the main thread when complete.

I use it to generate Marching Cube terrain in the background.

# How to use

	public ThreadTask threadTask;
	
	void Awake()
    {
		threadTask = gameObject.AddComponent<ThreadTask>();
		threadTask.StartTask(CalculateSomeData, DoTaskWithResult, data);
	}

	object CalculateSomeData(object o)
	{
		Data data = (Data)o;
		
		// RUNNING ON DIFFERENT THREAD
		// do heavy calculation here on data...
		// cannot use Unity functions here
	}

	void DoTaskWithResult(object o)
	{
		Data data = (Data)o;

		// RUNNING ON MAIN THREAD
		// do calculation here on data...
		// can use Unity functions here
	}
