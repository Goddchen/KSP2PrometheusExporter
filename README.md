# KSP2 Prometheus Exporter

A Prometheus exporter for all kind of KSP2 telemetry data and metrics.

## Installation

* Download from [SpaceDock](https://spacedock.info/mod/3370/KSP2%20Prometheus%20Exporter)
* Install via [CKAN](https://github.com/KSP-CKAN/CKAN)

## Configurable settings

* port: The port where you metrics will be served (defaults to 9102)
* push-endpoint: The endpoint of a Prometheus push gateway where you can send your metrics to (defaults to none)

## Docker setup

I created a nice like Docker-Compose configuration that will get you up and running with Prometheus and Grafana to visualize your data.

If you want to login to Grafana to edit stuff, the default user/password is just `admin:admin`.

Run by calling `docker-compose up` in the [Docker](Docker/) folder. Prometheus will then be available at http://localhost:9090 and Grafana will be available at http://localhost:3000.

## Demo

Explanation: The mod simply exports ingame telemetry and other metrics in the Prometheus data format.
Prometheus servers can then scrape this data. Once that is done, you can use whatever visualization tool you like (I chose Grafana).

The output that this mod gives you looks something like this:

```prometheus
# HELP ksp2_active_vessel_vehicle_yaw Yaw of the active vessel vehicle
# TYPE ksp2_active_vessel_vehicle_yaw gauge
ksp2_active_vessel_vehicle_yaw 0
# HELP ksp2_active_vessel_vehicle_horizontal_speed Horizontal speed of the active vessel vehicle
# TYPE ksp2_active_vessel_vehicle_horizontal_speed gauge
ksp2_active_vessel_vehicle_horizontal_speed 1853.70796604418
# HELP ksp2_active_vessel_vehicle_orbital_speed Orbital speed of the active vessel vehicle
# TYPE ksp2_active_vessel_vehicle_orbital_speed gauge
ksp2_active_vessel_vehicle_orbital_speed 2089.24847469182
# HELP ksp2_active_vessel_vehicle_main_throttle Main throttle of the active vessel vehicle
# TYPE ksp2_active_vessel_vehicle_main_throttle gauge
ksp2_active_vessel_vehicle_main_throttle 0
# HELP ksp2_active_vessel_vehicle_vertical_speed Vertical speed of the active vessel vehicle
# TYPE ksp2_active_vessel_vehicle_vertical_speed gauge
ksp2_active_vessel_vehicle_vertical_speed 4.08596516962251
# HELP ksp2_active_vessel_vehicle_pitch Pitch of the active vessel vehicle
# TYPE ksp2_active_vessel_vehicle_pitch gauge
ksp2_active_vessel_vehicle_pitch 0
# HELP ksp2_active_vessel_vehicle_roll Roll of the active vessel vehicle
# TYPE ksp2_active_vessel_vehicle_roll gauge
ksp2_active_vessel_vehicle_roll 0
# HELP ksp2_active_vessel_vehicle_altitude_from_sea_level Altitude from sea level of the active vessel vehicle
# TYPE ksp2_active_vessel_vehicle_altitude_from_sea_level gauge
ksp2_active_vessel_vehicle_altitude_from_sea_level 207936.061219515
# HELP ksp2_active_vessel_vehicle_gee_force G force of the active vessel vehicle
# TYPE ksp2_active_vessel_vehicle_gee_force gauge
ksp2_active_vessel_vehicle_gee_force 0.000368174444844003
```

[![](https://img.youtube.com/vi/3SCYy1J_Pqc/0.jpg)](https://youtu.be/3SCYy1J_Pqc)