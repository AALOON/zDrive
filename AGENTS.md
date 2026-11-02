# AGENTS.md

Guidance for AI coding agents working in this repository.

## Project Overview

**zDrive** is a compact Windows desktop widget written in **WPF (.NET 10, `net10.0-windows`)**.
It shows storage drive usage, RAM/CPU widgets, and a primary-display switcher.
Single-project solution: `zDrive.sln` → `zDrive/zDrive.csproj`. Windows-only.

## Build & Run

```powershell
dotnet build zDrive.sln                                  # debug build
dotnet run --project zDrive/zDrive.csproj                # run the app
dotnet publish zDrive/zDrive.csproj -p:PublishProfile=SingleFile   # self-contained win-x64 single exe (~59 MB)
```

- Publish profile lives at `zDrive/Properties/PublishProfiles/SingleFile.pubxml`.
- SDK is pinned in `global.json` (`.NET 10.0.4xx`, `rollForward: latestFeature`).
- There are **no automated tests** in the solution; verification = build (warnings are errors-prone style rules, see `.editorconfig`) + manual smoke run.

## Architecture

MVVM without external MVVM/DI frameworks. Everything is hand-rolled:

- **`ViewModels/ViewModelLocator.cs`** — the composition root. Static constructor registers all services in the custom IoC container. This is where new registrations go.
- **`Services/Ioc/`** — tiny custom service locator (`SimpleIoc`), supports singleton / per-dependency, interface+implementation keys.
- **`Interfaces/`** — contracts for services and view models. ViewModels implement interfaces (`IDriveViewModel`, `IInfoViewModel`, `IMainViewModel`); `ViewModelBase` provides `Set`/`RaisePropertyChanged`.
- **`Services/`** — domain services:
  - `DriveInfoService` — enumerates drives, updates view models.
  - `DriveDetectionService` — hot-plug detection via `WM_DEVICECHANGE` (implements `IWndProc`; `MainWindow.OnSourceInitialized` hooks all `IWndProc` implementations into the window's `HwndSource`).
  - `WidgetsService` — adds/removes info widgets (`InfoWidget`: RamDisk, Cpu, Displays).
  - `RegistryService` — persists **all settings to Windows Registry** (keys = `nameof(Property)`).
  - `TimerService` — UI timer driving periodic updates.
- **`Native/`** — Win32 interop by area: `Display` (DeviceContext + DisplayConfig structs for display switching), `Kernel` (Kernel32), `Pdh` (performance counters for CPU/RAM), `Ps` (PsApi), `Shell` (ShellExecute). P/Invoke only — keep native structs here, not in services/VMs.
- **`Mvvm/`** — `RelayCommand`, `AsyncRelayCommand`, attached behaviors.
- **`Skins/`** — XAML resource dictionaries: `DarkSkin.xaml`, `GraySkin.xaml`, `ModernSkin.xaml`, shared `ControlTemplates.xaml`. Switched at runtime via `App.ChangeSkin(Uri)`; `Theme.Default` maps to Gray.
- **Localization** — `Properties/Resources.resx` (ru, neutral) + `Resources.en-US.resx`. `SatelliteResourceLanguages=en-US;ru` is deliberately limited in the csproj to shrink publish output.
- **`WpfSingleInstance.cs`** — single-instance guard via mutex.
- Logging: `Microsoft.Extensions.Logging` → Windows EventLog (see `EventIds.cs`). Startup failures are logged best-effort in `App.OnStartup`.

## Constraints & Gotchas

- **No WinForms.** It was deliberately removed (saves ~22 MB of the single-file bundle). Screen enumeration goes through `MonitorsService` (`EnumDisplayDevices` interop), not `System.Windows.Forms.Screen`.
- **Single-file publish:** `Assembly.Location` is empty at runtime — use `Environment.ProcessPath` (see `MainViewModel.GenerateLocation`).
- Only NuGet dependency is `Microsoft.Extensions.Logging.EventLog`; prefer the standard library and existing code over adding packages.
- `zDrive.pfx` files exist for signing — do not delete or regenerate them.
- The app writes settings to the registry on property change; property names are the storage keys — renaming public VM properties breaks persisted settings.

## Code Style (enforced by .editorconfig, severities up to warning/error)

- 4 spaces, UTF-8, max line length 120, file-scoped rules: trim trailing whitespace, final newline.
- Always qualify member access with `this.` for fields/properties/methods/events.
- Prefer `var`, expression-bodied members, pattern matching, switch expressions.
- Braces required even for single statements (`csharp_prefer_braces`).
- Naming: private fields `camelCase`, constants and static readonly fields `PascalCase`, no public instance fields, interfaces `I`-prefixed, type parameters `T`-prefixed.
- `using` directives outside namespace; `System.*` first.
- XML doc comments (`///`) on public/visible members; use `<inheritdoc />` on interface implementations.
- Keep new code consistent with the surrounding StyleCop-flavored conventions.

## Commits & PRs

Commit messages in English, imperative mood, describing the change (see `git log` examples: `add self-contained single-file publish and shrink the bundle`, `fix drive widget info: remove raw disk read speed value`).
