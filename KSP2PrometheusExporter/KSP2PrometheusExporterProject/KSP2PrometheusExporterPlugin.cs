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

    private static readonly Prometheus.Gauge ActiveVesselVehicleAltitudeFromRadius = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_altitude_from_radius",
        "Altitude from radius of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleAltitudeFromScenery = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_altitude_from_scenery",
        "Altitude from scenery of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleAltitudeFromSeaLevel = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_altitude_from_sea_level",
        "Altitude from sea level of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleAltitudeFromTerrain = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_altitude_from_terrain",
        "Altitude from terrain of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleAtmosphericDensity = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_atmospheric_density",
        "Atmospheric density of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleAtmosphericDensityNormalized = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_atmospheric_density_normalized",
        "Normalized atmospheric density of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleAtmosphericTemperature = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_atmospheric_temperature",
        "Atmospheric temperature of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleDynamicPressureKPa = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_dynamic_pressure_kpa",
        "Dynamic pressure (kPa) of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleExternalTemperature = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_external_temperature",
        "External temperature of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleGeeForce = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_gee_force",
        "G force of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleHeading = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_heading",
        "Heading of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleHorizontalSpeed = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_horizontal_speed",
        "Horizontal speed of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleMachNumber = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_mach_number",
        "Mach number of the active vessel vehicle",
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
    private static readonly Prometheus.Gauge ActiveVesselVehicleSoundSpeed = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_sound_speed",
        "Sound speed of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleSpecificAcceleration = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_specific_acceleration",
        "Specific acceleration of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleStaticPressureKPa = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_static_pressure_kpa",
        "Static pressure (kPa) of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleSurfaceSpeed = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_surface_speed",
        "Surface speed of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleTargetSpeed = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_target_speed",
        "Target speed of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleVerticalSpeed = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_vertical_speed",
        "Vertical speed of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleWheelSteer = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_wheel_steer",
        "Wheel steer of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleWheelSteerTrim = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_wheel_steer_trim",
        "Wheel steer trim of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleWheelThrottle = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_wheel_throttle",
        "Wheel throttle of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true }
        );
    private static readonly Prometheus.Gauge ActiveVesselVehicleWheetThrottleTrim = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_wheel_trottle_trim",
        "Wheel throttle trim of the active vessel vehicle",
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
                ActiveVesselVehicleAltitudeFromRadius.Publish();
                ActiveVesselVehicleAltitudeFromRadius.Set(activeVesselVehicle.AltitudeFromRadius);
                ActiveVesselVehicleAltitudeFromScenery.Publish();
                ActiveVesselVehicleAltitudeFromScenery.Set(activeVesselVehicle.AltitudeFromScenery);
                ActiveVesselVehicleAltitudeFromSeaLevel.Publish();
                ActiveVesselVehicleAltitudeFromSeaLevel.Set(activeVesselVehicle.AltitudeFromSeaLevel);
                ActiveVesselVehicleAltitudeFromTerrain.Publish();
                ActiveVesselVehicleAltitudeFromTerrain.Set(activeVesselVehicle.AltitudeFromTerrain);
                ActiveVesselVehicleAtmosphericDensity.Publish();
                ActiveVesselVehicleAtmosphericDensity.Set(activeVesselVehicle.AtmosphericDensity);
                ActiveVesselVehicleAtmosphericDensityNormalized.Publish();
                ActiveVesselVehicleAtmosphericDensityNormalized.Set(activeVesselVehicle.AtmosphericDensityNormalized);
                ActiveVesselVehicleAtmosphericTemperature.Publish();
                ActiveVesselVehicleAtmosphericTemperature.Set(activeVesselVehicle.AtmosphericTemperature);
                ActiveVesselVehicleDynamicPressureKPa.Publish();
                ActiveVesselVehicleDynamicPressureKPa.Set(activeVesselVehicle.DynamicPressurekPa);
                ActiveVesselVehicleExternalTemperature.Publish();
                ActiveVesselVehicleExternalTemperature.Set(activeVesselVehicle.ExternalTemperature);
                ActiveVesselVehicleGeeForce.Publish();
                ActiveVesselVehicleGeeForce.Set(activeVesselVehicle.GeeForce);
                ActiveVesselVehicleHeading.Publish();
                ActiveVesselVehicleHeading.Set(activeVesselVehicle.Heading);
                ActiveVesselVehicleHorizontalSpeed.Publish();
                ActiveVesselVehicleHorizontalSpeed.Set(activeVesselVehicle.HorizontalSpeed);
                ActiveVesselVehicleMachNumber.Publish();
                ActiveVesselVehicleMachNumber.Set(activeVesselVehicle.MachNumber);
                ActiveVesselVehicleMainThrottle.Publish();
                ActiveVesselVehicleMainThrottle.Set(activeVesselVehicle.mainThrottle);
                ActiveVesselVehicleOrbitalSpeed.Publish();
                ActiveVesselVehicleOrbitalSpeed.Set(activeVesselVehicle.OrbitalSpeed);
                ActiveVesselVehiclePitch.Publish();
                ActiveVesselVehiclePitch.Set(activeVesselVehicle.pitch);
                ActiveVesselVehicleRoll.Publish();
                ActiveVesselVehicleRoll.Set(activeVesselVehicle.roll);
                ActiveVesselVehicleSoundSpeed.Publish();
                ActiveVesselVehicleSoundSpeed.Set(activeVesselVehicle.SoundSpeed);
                ActiveVesselVehicleSpecificAcceleration.Publish();
                ActiveVesselVehicleSpecificAcceleration.Set(activeVesselVehicle.SpecificAcceleration);
                ActiveVesselVehicleStaticPressureKPa.Publish();
                ActiveVesselVehicleStaticPressureKPa.Set(activeVesselVehicle.StaticPressurekPa);
                ActiveVesselVehicleSurfaceSpeed.Publish();
                ActiveVesselVehicleSurfaceSpeed.Set(activeVesselVehicle.SurfaceSpeed);
                ActiveVesselVehicleTargetSpeed.Publish();
                ActiveVesselVehicleTargetSpeed.Set(activeVesselVehicle.TargetSpeed);
                ActiveVesselVehicleVerticalSpeed.Publish();
                ActiveVesselVehicleVerticalSpeed.Set(activeVesselVehicle.VerticalSpeed);
                ActiveVesselVehicleWheelSteer.Publish();
                ActiveVesselVehicleWheelSteer.Set(activeVesselVehicle.wheelSteer);
                ActiveVesselVehicleWheelSteerTrim.Publish();
                ActiveVesselVehicleWheelSteerTrim.Set(activeVesselVehicle.wheelSteerTrim);
                ActiveVesselVehicleWheelThrottle.Publish();
                ActiveVesselVehicleWheelThrottle.Set(activeVesselVehicle.wheelThrottle);
                ActiveVesselVehicleWheetThrottleTrim.Publish();
                ActiveVesselVehicleWheetThrottleTrim.Set(activeVesselVehicle.wheelThrottleTrim);
                ActiveVesselVehicleYaw.Publish();
                ActiveVesselVehicleYaw.Set(activeVesselVehicle.yaw);
            }
            else
            {
                ActiveVesselVehicleAltitudeFromRadius.Unpublish();
                ActiveVesselVehicleAltitudeFromScenery.Unpublish();
                ActiveVesselVehicleAltitudeFromSeaLevel.Unpublish();
                ActiveVesselVehicleAtmosphericDensity.Unpublish();
                ActiveVesselVehicleAtmosphericDensityNormalized.Unpublish();
                ActiveVesselVehicleAtmosphericTemperature.Unpublish();
                ActiveVesselVehicleDynamicPressureKPa.Unpublish();
                ActiveVesselVehicleExternalTemperature.Unpublish();
                ActiveVesselVehicleGeeForce.Unpublish();
                ActiveVesselVehicleHeading.Unpublish();
                ActiveVesselVehicleHorizontalSpeed.Unpublish();
                ActiveVesselVehicleMachNumber.Unpublish();
                ActiveVesselVehicleMainThrottle.Unpublish();
                ActiveVesselVehicleOrbitalSpeed.Unpublish();
                ActiveVesselVehiclePitch.Unpublish();
                ActiveVesselVehicleRoll.Unpublish();
                ActiveVesselVehicleSoundSpeed.Unpublish();
                ActiveVesselVehicleSpecificAcceleration.Unpublish();
                ActiveVesselVehicleStaticPressureKPa.Unpublish();
                ActiveVesselVehicleSurfaceSpeed.Unpublish();
                ActiveVesselVehicleTargetSpeed.Unpublish();
                ActiveVesselVehicleVerticalSpeed.Unpublish();
                ActiveVesselVehicleWheelSteer.Unpublish();
                ActiveVesselVehicleWheelSteerTrim.Unpublish();
                ActiveVesselVehicleWheelThrottle.Unpublish();
                ActiveVesselVehicleWheetThrottleTrim.Unpublish();
                ActiveVesselVehicleYaw.Unpublish();
            }
        });

        Metrics.SuppressDefaultMetrics();

        new MetricServer(portConfigEntry.Value).Start();

        Logger.LogInfo("Started Prometheus exporter on port 9102");

        if (pushEndpointConfigEntry.Value != null && !pushEndpointConfigEntry.Value.IsNullOrWhiteSpace())
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
