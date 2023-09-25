namespace Servers;

using NLog;

using Services;

public class TrafficLightState
{
	/// <summary>
	/// Access lock.
	/// </summary>
	public readonly object AccessLock = new object();

}

/// <summary>
/// Container state descritor.
/// </summary>
public class ContainerStuff
{
    /// <summary>
    /// Access lock.
    /// </summary>
    public readonly object AccessLock = new object();

    /// <summary>
    /// Access to Container limits
    /// </summary>
	public ContainerLimits ContainerLimits = new ContainerLimits();
}


/// <summary>
/// <para>Traffic light logic.</para>
/// <para>Thread safe.</para>
/// </summary>
class ContainerLogic
{
	/// <summary>
	/// Logger for this class.
	/// </summary>
	private Logger mLog = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Background task thread.
	/// </summary>
	private Thread mBgTaskThread;

	/// <summary>
	/// State descriptor.
	/// </summary>
	private TrafficLightState mState = new TrafficLightState();

	/// <summary>
	/// Container State Descriptor
	/// </summary>
	private ContainerStuff cState = new ContainerStuff();

    public ContainerDesc container = new ContainerDesc() { Mass = 0.00, Temperature = 273.15 };
    /// <summary>
    /// Constructor.
    /// </summary>
    public ContainerLogic()
	{
		//start the background task
		mBgTaskThread = new Thread(BackgroundTask);
		mBgTaskThread.Start();
	}
    /// <summary>
    /// Updates the container's mass.
    /// </summary>
    /// <param name="mass">The amount of mass to add.</param>
    public void updatecontainer(double mass)
	{
		lock ( mState.AccessLock )
		{
			container.Mass += mass;
		}
	}

    /// <summary>
    /// Gets the container's limits.
    /// </summary>
    /// <returns>The container's limits.</returns>
    public ContainerLimits GetLimits()
	{
		lock ( mState.AccessLock )
		{
			return cState.ContainerLimits;
		}
	}

    /// <summary>
    /// Resets the simulation.
    /// </summary>
    public void ResetSimulation()
    {
        lock (mState.AccessLock)
        {
            if (container.Pressure >= cState.ContainerLimits.explosionLimit)
			{
				mLog.Info($"Container Exploded. Simulation is Reset");
				container.Mass = 0;
				container.Temperature = 273.15;
                Console.Clear();
                mLog.Info("Server is about to start");
				Thread.Sleep(500 + new Random().Next(1500));
            }
            else if (container.Pressure <= cState.ContainerLimits.implosionLimit)
			{
                mLog.Info($"Container Imploded. Simulation is Reset");
                container.Mass = 0;
                container.Temperature = 273.15;
				Console.Clear();
                mLog.Info("Server is about to start");
                Thread.Sleep(500 + new Random().Next(1500));
            }
        }
    }

    /// <summary>
    /// Gets the container's details.
    /// </summary>
    /// <returns>The container's details.</returns>
    public ContainerDesc ContainerDetails()
	{
		lock (mState.AccessLock )
		{
			return container;
		}
	}

    /// <summary>
    /// Gets the container's control state.
    /// </summary>
    /// <returns>returns 1 for input component to work and 2 for output component to work</returns>
    public int ContainerControl()
	{
		lock (mState.AccessLock)
		{
			if (container.Pressure < cState.ContainerLimits.lowerLimit)
			{
                return 1;
			}
			else if (container.Pressure > cState.ContainerLimits.upperLimit)
			{
                return 2;
			}
            return 0;
		}
	}

	/// <summary>
	/// Background task for the container
	/// </summary>
	public void BackgroundTask()
	{      
        while (true)
        {
            Thread.Sleep(2000);
            lock (mState.AccessLock)
            {
                var random = new Random();
                double rnd = random.NextDouble() * 5 - 1;
                container.Temperature += rnd;
                mLog.Info($"New temperature is {container.Temperature:F2}\t\t\t\tChange: {rnd:F2}");
                container.Pressure = container.Mass * container.Temperature * container.GasConstant / container.Volume;
                mLog.Info($"New Mass is {container.Mass:F2}");
                mLog.Info($"New Pressure is {container.Pressure:F2}\n");
				ResetSimulation();
            }
            
        }
    }
}