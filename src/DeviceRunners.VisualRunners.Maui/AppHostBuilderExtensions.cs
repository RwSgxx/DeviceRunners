﻿using CommunityToolkit.Maui;

using DeviceRunners.Core;
using DeviceRunners.VisualRunners.Maui;
using DeviceRunners.VisualRunners.Maui.Pages;

namespace DeviceRunners.VisualRunners;

public static class AppHostBuilderExtensions
{
	// default to true as it is always supported
	internal static bool IsUsingVisualRunner { get; set; } = true;

	public static MauiAppBuilder UseVisualTestRunner(
		this MauiAppBuilder appHostBuilder, 
		Action<VisualTestRunnerConfigurationBuilder> configurationBuilder,
		IReadOnlyCollection<ResourceDictionary>? externalAppResources = null)
	{
		var configBuilder = new VisualTestRunnerConfigurationBuilder(appHostBuilder);
		configurationBuilder?.Invoke(configBuilder);
		var configuration = configBuilder.Build();

		if (externalAppResources != null)
		{
			appHostBuilder.Services.AddSingleton<IExternalAppResourcesProvider>(new ExternalAppResourcesProvider(externalAppResources));
		}

		// register runner components
		appHostBuilder.Services.AddSingleton<IVisualTestRunnerConfiguration>(configuration);
		appHostBuilder.Services.AddSingleton<IDiagnosticsManager, DiagnosticsManager>();
		appHostBuilder.Services.AddSingleton<IAppTerminator, DefaultAppTerminator>();
		appHostBuilder.Services.AddSingleton<IResultChannelManager, DefaultResultChannelManager>();

		// only register the "root" view models and the others are created by the ITestRunner
		appHostBuilder.Services.AddSingleton<HomeViewModel>();
		appHostBuilder.Services.AddSingleton<DiagnosticsViewModel>();
		appHostBuilder.Services.AddSingleton<CreditsViewModel>();

		// register app components
		appHostBuilder.Services.AddSingleton<VisualRunnerWindow>();
		appHostBuilder.Services.AddSingleton<VisualRunnerAppShell>();

		// register pages
		appHostBuilder.Services.AddTransient<HomePage>();
		appHostBuilder.Services.AddTransient<TestAssemblyPage>();
		appHostBuilder.Services.AddTransient<TestResultPage>();
		appHostBuilder.Services.AddTransient<CreditsPage>();
		appHostBuilder.Services.AddTransient<DiagnosticsPage>();

		appHostBuilder.UseMauiCommunityToolkit();

		if (configBuilder.RunnerUsage == VisualTestRunnerUsage.Always ||
			(configBuilder.RunnerUsage == VisualTestRunnerUsage.Automatic && IsUsingVisualRunner))
		{
			Console.WriteLine("Registering the visual runner app as the test runner app.");

			appHostBuilder.UseMauiApp<VisualRunnerApp>();
		}

		return appHostBuilder;
	}

	public static TBuilder SetTestRunnerUsage<TBuilder>(this TBuilder builder, VisualTestRunnerUsage usage)
		where TBuilder : VisualTestRunnerConfigurationBuilder
	{
		builder.RunnerUsage = usage;
		return builder;
	}
}
