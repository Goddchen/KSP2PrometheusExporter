using BepInEx;
using HarmonyLib;
using KSP.UI.Binding;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.Game;
using SpaceWarp.API.Game.Extensions;
using SpaceWarp.API.UI;
using SpaceWarp.API.UI.Appbar;
using UnityEngine;
using Prometheus;
using KSP.Sim.impl;

namespace KSP2PrometheusExporter;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
public class KSP2PrometheusExporterPlugin : BaseSpaceWarpPlugin
{
    // These are useful in case some other mod wants to add a dependency to this one
    public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
    public const string ModName = MyPluginInfo.PLUGIN_NAME;
    public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

    private bool _isWindowOpen;
    private Rect _windowRect;

    private const string ToolbarFlightButtonID = "BTN-KSP2PrometheusExporterFlight";
    private const string ToolbarOABButtonID = "BTN-KSP2PrometheusExporterOAB";

    public static KSP2PrometheusExporterPlugin Instance { get; set; }

    private static readonly Prometheus.Gauge ActiveVesselVehicleOrbitalSpeed = Metrics.CreateGauge(
        "ksp2_active_vessel_vehicle_orbital_speed",
        "Orbital speed of the active vessel vehicle",
        new GaugeConfiguration { SuppressInitialValue = true });

    /// <summary>
    /// Runs when the mod is first initialized.
    /// </summary>
    public override void OnInitialized()
    {
        base.OnInitialized();

        Instance = this;

        Metrics.DefaultRegistry.AddBeforeCollectCallback(async (cancel) =>
        {
            VesselVehicle activeVesselVehicle = Vehicle.ActiveVesselVehicle;
            if (activeVesselVehicle != null)
            {
                ActiveVesselVehicleOrbitalSpeed.Publish();
                ActiveVesselVehicleOrbitalSpeed.Set(Vehicle.ActiveVesselVehicle.OrbitalSpeed);
                //ActiveVesselVehicleOrbitalSpeed.Set(12.4);
            }
            else
            {
                ActiveVesselVehicleOrbitalSpeed.Unpublish();
            }
        });

        Metrics.SuppressDefaultMetrics();

        new MetricServer(9102).Start();

        Logger.LogInfo("Started Prometheus exporter on port 9102");

        new MetricPusher(new MetricPusherOptions
        {
            Endpoint = "http://51.15.51.177:9091/metrics",
            Job = "ksp2",
            ReplaceOnPush = true,
        }).Start();

        Logger.LogInfo("Started Prometheus metrics pusher to push metrics to http://51.15.51.177:9091/metrics");

        //// Register Flight AppBar button
        //Appbar.RegisterAppButton(
        //    "KSP2 Prometheus Exporter",
        //    ToolbarFlightButtonID,
        //    AssetManager.GetAsset<Texture2D>($"{SpaceWarpMetadata.ModID}/images/icon.png"),
        //    isOpen =>
        //    {
        //        _isWindowOpen = isOpen;
        //        GameObject.Find(ToolbarFlightButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isOpen);
        //    }
        //);

        //// Register OAB AppBar Button
        //Appbar.RegisterOABAppButton(
        //    "KSP2 Prometheus Exporter",
        //    ToolbarOABButtonID,
        //    AssetManager.GetAsset<Texture2D>($"{SpaceWarpMetadata.ModID}/images/icon.png"),
        //    isOpen =>
        //    {
        //        _isWindowOpen = isOpen;
        //        GameObject.Find(ToolbarOABButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isOpen);
        //    }
        //);

        //// Register all Harmony patches in the project
        //Harmony.CreateAndPatchAll(typeof(KSP2PrometheusExporterPlugin).Assembly);

        //// Try to get the currently active vessel, set its throttle to 100% and toggle on the landing gear
        //try
        //{
        //    var currentVessel = Vehicle.ActiveVesselVehicle;
        //    if (currentVessel != null)
        //    {
        //        currentVessel.SetMainThrottle(1.0f);
        //        currentVessel.SetGearState(true);
        //    }
        //}
        //catch (Exception) { }

        //// Fetch a configuration value or create a default one if it does not exist
        //var defaultValue = "my_value";
        //var configValue = Config.Bind<string>("Settings section", "Option 1", defaultValue, "Option description");

        //// Log the config value into <KSP2 Root>/BepInEx/LogOutput.log
        //Logger.LogInfo($"Option 1: {configValue.Value}");
    }

    ///// <summary>
    ///// Draws a simple UI window when <code>this._isWindowOpen</code> is set to <code>true</code>.
    ///// </summary>
    //private void OnGUI()
    //{
    //    // Set the UI
    //    GUI.skin = Skins.ConsoleSkin;

    //    if (_isWindowOpen)
    //    {
    //        _windowRect = GUILayout.Window(
    //            GUIUtility.GetControlID(FocusType.Passive),
    //            _windowRect,
    //            FillWindow,
    //            "KSP2 Prometheus Exporter",
    //            GUILayout.Height(350),
    //            GUILayout.Width(350)
    //        );
    //    }
    //}

    ///// <summary>
    ///// Defines the content of the UI window drawn in the <code>OnGui</code> method.
    ///// </summary>
    ///// <param name="windowID"></param>
    //private static void FillWindow(int windowID)
    //{
    //    GUILayout.Label("KSP2 Prometheus Exporter - Prometheus exporter for telemetry data.");
    //    GUI.DragWindow(new Rect(0, 0, 10000, 500));
    //}
}
