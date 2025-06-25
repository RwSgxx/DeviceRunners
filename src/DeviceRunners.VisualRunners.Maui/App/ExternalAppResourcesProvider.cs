namespace DeviceRunners.VisualRunners.Maui;

public interface IExternalAppResourcesProvider
{
	IReadOnlyCollection<ResourceDictionary> ExternalAppResources { get; }
}

public class ExternalAppResourcesProvider : IExternalAppResourcesProvider
{
	public ExternalAppResourcesProvider(IReadOnlyCollection<ResourceDictionary> externalAppResources)
	{
		ExternalAppResources = externalAppResources;
	}
	public IReadOnlyCollection<ResourceDictionary> ExternalAppResources { get; }
}
