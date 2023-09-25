namespace Servers;

using Microsoft.AspNetCore.Routing.Constraints;
using Services;

/// <summary>
/// Service
/// </summary>
public class ContainerService : IContainerService
{
	//NOTE: non-singleton service would need logic to be static or injected from a singleton instance
	private readonly ContainerLogic mLogic = new();

    public void SetMass(double mass)
    {
        mLogic.updatecontainer(mass);
    }

    public ContainerLimits GetContainerLimits()
    {
        return mLogic.GetLimits();
    }

    public ContainerDesc ContainerInfo()
    {
        return mLogic.ContainerDetails();
    }

    public int ActiveClient()
    {
        return mLogic.ContainerControl();
    }
}