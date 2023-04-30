using BepInEx;
using SpaceWarp;
using SpaceWarp.API.Mods;
using SpaceWarp.API.Game;
using Prometheus;
using KSP.Sim.impl;
using BepInEx.Configuration;

namespace KSP2PrometheusExporter;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
public class KSP2PrometheusExporterPlugin : BaseSpaceWarpPlugin
{
    public static KSP2PrometheusExporterPlugin Instance { get; set; }

    private static readonly Prometheus.Gauge ActiveVesselVehicleAltitudeFromSeaLevel = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_altitude_from_sea_level",
        "Altitude from sea level of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleGeeForce = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_gee_force",
        "G force of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleHorizontalSpeed = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_horizontal_speed",
        "Horizontal speed of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleMainThrottle = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_main_throttle",
        "Main throttle of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleOrbitalSpeed = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_orbital_speed",
        "Orbital speed of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehiclePitch = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_pitch",
        "Pitch of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleRoll = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_roll",
        "Roll of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleVerticalSpeed = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_vertical_speed",
        "Vertical speed of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleYaw = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_yaw",
        "Yaw of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );

    /// <summary>
    /// Runs when the mod is first initialized.
    /// </summary>
    public override void OnInitialized()
    {
        base.OnInitialized();

        Instance = this;

        ConfigEntry<int> portConfigEntry = Config.Bind<int>("Prometheus Settings",
            "port",
            9102,
            new BepInEx.Configuration.ConfigDescription(
                "Port where the metrics will be served under /metrics (defaults to 9102)")
            );
        ConfigEntry<String> pushEndpointConfigEntry = Config.Bind<string>("Prometheus Settings",
            "push-engpoint",
            null,
            new ConfigDescription("The address of the Prometheus push gateway to send metrics to (defaults to none)")
            );


        Metrics.DefaultRegistry.AddBeforeCollectCallback(() =>
        {
            VesselVehicle activeVesselVehicle = Vehicle.ActiveVesselVehicle;
            if (activeVesselVehicle != null)
            {
                ActiveVesselVehicleOrbitalSpeed.Publish();
                ActiveVesselVehicleOrbitalSpeed.Set(activeVesselVehicle.OrbitalSpeed);
                ActiveVesselVehicleGeeForce.Publish();
                ActiveVesselVehicleGeeForce.Set(activeVesselVehicle.GeeForce);
                ActiveVesselVehicleHorizontalSpeed.Publish();
                ActiveVesselVehicleHorizontalSpeed.Set(activeVesselVehicle.HorizontalSpeed);
                ActiveVesselVehicleMainThrottle.Publish();
                ActiveVesselVehicleMainThrottle.Set(activeVesselVehicle.mainThrottle);
                ActiveVesselVehicleAltitudeFromSeaLevel.Publish();
                ActiveVesselVehicleAltitudeFromSeaLevel.Set(activeVesselVehicle.AltitudeFromSeaLevel);
                ActiveVesselVehiclePitch.Publish();
                ActiveVesselVehiclePitch.Set(activeVesselVehicle.pitch);
                ActiveVesselVehicleRoll.Publish();
                ActiveVesselVehicleRoll.Set(activeVesselVehicle.roll);
                ActiveVesselVehicleVerticalSpeed.Publish();
                ActiveVesselVehicleVerticalSpeed.Set(activeVesselVehicle.VerticalSpeed);
                ActiveVesselVehicleYaw.Publish();
                ActiveVesselVehicleYaw.Set(activeVesselVehicle.yaw);
            }
            else
            {
                ActiveVesselVehicleAltitudeFromSeaLevel.Unpublish();
                ActiveVesselVehicleGeeForce.Unpublish();
                ActiveVesselVehicleHorizontalSpeed.Unpublish();
                ActiveVesselVehicleMainThrottle.Unpublish();
                ActiveVesselVehicleOrbitalSpeed.Unpublish();
                ActiveVesselVehiclePitch.Unpublish();
                ActiveVesselVehicleRoll.Unpublish();
                ActiveVesselVehicleVerticalSpeed.Unpublish();
                ActiveVesselVehicleYaw.Unpublish();
            }
        });

        Metrics.SuppressDefaultMetrics();

        new MetricServer(portConfigEntry.Value).Start();

        Logger.LogInfo("Started Prometheus exporter on port 9102");

        if (pushEndpointConfigEntry.Value != null)
        {
            new MetricPusher(new MetricPusherOptions
            {
                Endpoint = pushEndpointConfigEntry.Value,
                Job = "ksp2",
                ReplaceOnPush = true,
            }).Start();

            Logger.LogInfo($"Started Prometheus metrics pusher to push metrics to {pushEndpointConfigEntry.Value}");
        }
    }
}
