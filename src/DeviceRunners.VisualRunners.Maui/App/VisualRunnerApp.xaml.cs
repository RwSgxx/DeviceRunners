namespace DeviceRunners.VisualRunners.Maui;

partial class VisualRunnerApp : Application
{
	public VisualRunnerApp(IExternalAppResourcesProvider? externalAppResourcesProvider = null)
	{
		InitializeComponent();

		if (externalAppResourcesProvider == null)
		{
			return;
		}

		foreach (var externalAppResource in externalAppResourcesProvider.ExternalAppResources)
		{
			Resources.MergedDictionaries.Add(externalAppResource);
		}
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		if (Windows.Any(w => w is VisualRunnerWindow))
			throw new InvalidOperationException("Only a single instance of the test runner window is supported.");

		return Handler!.MauiContext!.Services.GetRequiredService<VisualRunnerWindow>();
	}
}
