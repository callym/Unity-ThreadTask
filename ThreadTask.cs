/* * * * * * * * * * * * * * * * * * * * * *
 * Simple Multithreading in Unity
 * callym 2015
 * * * * * * * * * * * * * * * * * * * * * *
 * Add this as a component on your object
 * 
 * TODO:
 * * try making a ThreadTaskPool so you don't can use this outside of a gameobject?
 * * * * * * * * * * * * * * * * * * * * * */

using UnityEngine;
using System;
using System.Threading;
using System.Collections;

public class ThreadTask : MonoBehaviour
{
	delegate object ThreadTaskObject(object o);
	ThreadTaskObject t;
	object results;
	Action<object> callback;

	bool done = false;
	object handle = new object();

	bool IsDone
	{
		get
		{
			bool tmp;
			lock (handle)
			{
				tmp = done;
			}
			return tmp;
		}
		set
		{
			lock (handle)
			{
				done = value;
			}
		}
	}

	/// <summary>
	/// Starts a task in a new thread, and runs a task in the main thread after it has finished.
	/// </summary>
	/// <param name="calculate">Function that will be run on the new thread. 
	/// Has to take in and return an object.</param>
	/// <param name="callback">Function that is run in the main thread after the calculation has finished
	/// Takes an object parameter (the results of the calculation). Returns void.</param>
	/// <param name="o">An object that is passed to the calculation function.</param>
	public void StartTask(Func<object, object> calculate, Action<object> callback, object o = null)
	{
		t = new ThreadTaskObject(calculate);
		this.callback = callback;
		t.BeginInvoke(o, new AsyncCallback(ThreadCallback), t);

		StartCoroutine(WaitForCallback());
	}

	public IEnumerator WaitForCallback()
	{
		while (!IsDone)
		{
			yield return null;
		}
		callback(results);
	}

	void ThreadCallback(IAsyncResult ar)
	{
		ThreadTaskObject task = (ThreadTaskObject)ar.AsyncState;
		results = task.EndInvoke(ar);
		IsDone = true;
	}
}
