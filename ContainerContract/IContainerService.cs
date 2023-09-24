namespace Services;


/// <summary>
/// Car descriptor.
/// </summary>
public class CarDesc
{
	/// <summary>
	/// Car ID.
	/// </summary>
	public int CarId { get; set; }

	/// <summary>
	/// Car number.
	/// </summary>
	public string CarNumber { get; set; }

	/// <summary>
	/// Driver name and surname.
	/// </summary>
	public string DriverNameSurname { get; set; }
}

public class ContainerDesc
{
	public double Mass {  get; set; }

	public double Temperature { get; set; }
	public double Pressure { get; set;}

    public double GasConstant => 8.314;

    public double Volume => 1;	
}

/// <summary>
/// Descriptor of pass atempt result
/// </summary>
public class PassAttemptResult
{
	/// <summary>
	/// Indicates if pass attempt has succeeded.
	/// </summary>
	public bool IsSuccess { get; set; }

	/// <summary>
	/// If pass attempt has failed, indicates crash reason.
	/// </summary>
	public string CrashReason { get; set; }
}

public class ContainerLimits
{
    public double lowerLimit => 80000;
    public double upperLimit => 95000;
    public double implosionLimit => 50000;
    public double explosionLimit => 250000;
}



/// <summary>
/// Light state.
/// </summary>
public enum LightState : int
{
	Red,
	Green
}


/// <summary>
/// Service contract.
/// </summary>
public interface IContainerService
{
	/// <summary>
	/// Get next unique ID from the server. Is used by cars to acquire client ID's.
	/// </summary>
	/// <returns>Unique ID.</returns>
	int GetUniqueId();

	/// <summary>
	/// Get current light state.
	/// </summary>
	/// <returns>Current light state.</returns>				
	LightState GetLightState();

	ContainerLimits GetContainerLimits();

	ContainerDesc ContainerInfo();

	void SetMass(double mass);

	/// <summary>
	/// Queue give car at the light. Will only succeed if light is red.
	/// </summary>
	/// <param name="car">Car to queue.</param>
	/// <returns>True on success, false on failure.</returns>
	bool Queue(CarDesc car);

	/// <summary>
	/// Tell if car is first in line in queue.
	/// </summary>
	/// <param name="carId">ID of the car to check for.</param>
	/// <returns>True if car is first in line. False if not first in line or not in queue.</returns>
	bool IsFirstInLine(int carId);

	/// <summary>
	/// Try passing the traffic light. If car is in queue, it will be removed from it.
	/// </summary>
	/// <param name="car">Car descriptor.</param>
	/// <returns>Pass result descriptor.</returns>
	PassAttemptResult Pass(CarDesc car);
}
