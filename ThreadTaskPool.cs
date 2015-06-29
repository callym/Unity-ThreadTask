/* * * * * * * * * * * * * * * * * * * * * *
 * Simple Multithreading in Unity
 * callym 2015
 * * * * * * * * * * * * * * * * * * * * * *
 * Makes sure that you never run too many tasks
 * 
 * TODO:
 * * optimise...
 * * * * * * * * * * * * * * * * * * * * * */

using UnityEngine;
using System.Threading;
using System.Collections.Generic;

static public class ThreadTaskPool
{
	static Queue<ThreadTask> tasks = new Queue<ThreadTask>();
	static List<ThreadTask> runningTasks = new List<ThreadTask>();

	static int numRunningTasks = 0;
	static int maxRunningTasks = SystemInfo.processorCount - 1; // want to leave the main thread...

	static public void FinishTask(ThreadTask t)
	{
		numRunningTasks--;
		runningTasks.Remove(t);
		RunTask();
	}

	static public void QueueTask(ThreadTask t)
	{
		tasks.Enqueue(t);
		RunTask();
	}

	static public void RunTask()
	{
		if (numRunningTasks < maxRunningTasks && tasks.Count > 0)
		{
			ThreadTask t = tasks.Dequeue();

			ThreadPool.QueueUserWorkItem(
				new WaitCallback(delegate(object ob)
				{
					t.RunTask();
				}), null);
			t.StartCoroutine(t.WaitForCallback());
		}
	}
}