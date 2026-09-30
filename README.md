# DosingCalculator

A Windows desktop dosing calculator for **Red Sea Complete Reef Care 4-Part** reef aquarium supplements.

**The calculator computes doses using the conversion factors and dosing ratios in Red Sea's official Complete Reef Care documentation.** See the [manufacturer's manual (EN / DE / FR, V24A)](https://g1.redseafish.com/wp-content/uploads/2024/03/24964_Complete-Reef-Care-Manual_FED_V24A-WEB.pdf), especially PDF page 5 for the correction factors and profile table.

It calculates calcium (Ca) and alkalinity (KH) correction doses, derives proportional doses for Parts #3 and #4, and spreads the results over a chosen number of days. This is an independent community project, not an official Red Sea application.

## Features

- Enter your system's water volume in liters and current/target Ca and KH values.
- Select a Mixed Reef, SPS Dominant, Frags, or ULNS profile.
- View total doses in ml and daily doses in ml/day for all four parts.
- Accept either a decimal point or a decimal comma in Ca, KH, and volume inputs.
- Show a warning when a target Ca or KH value is lower than its current value; the corresponding correction dose is zero.
- Run locally without an account, internet connection, or dosing-pump integration.

## Requirements and build

- Windows with **.NET Framework 4.7.2 or later** installed.
- To build: Visual Studio with the **.NET desktop development** workload and the **.NET Framework 4.7.2 targeting pack**.

```powershell
git clone https://github.com/pdonat01/DosingCalculator.git
cd DosingCalculator
```

Open `DosingCalculator.sln` in Visual Studio, select **Release**, and choose **Build Solution**. Run:

```text
DosingCalculator\bin\Release\DosingCalculator.exe
```

Alternatively, from a Visual Studio Developer Command Prompt:

```powershell
msbuild DosingCalculator.sln /p:Configuration=Release /p:Platform=AnyCPU
```

The project uses C# and Windows Forms, targets .NET Framework 4.7.2, and has no external NuGet dependencies. The source ZIP downloaded from GitHub must be built before it can be run.

## How to use

1. Enter your actual water volume: display tank plus sump, accounting for displacement by rock and equipment.
2. Choose a reef profile. The two objectives in its name are **growth / coloration**; the numbers are the profile's Part #1–#4 values from the manufacturer's initial-dose table.
3. Enter your measured **Ca (ppm)** and **KH (dKH)**, then enter their target values.
4. Enter a positive whole number of days over which to spread the correction.
5. Click **Calculate** to view the total and daily doses.

The Mg fields are currently displayed but are not read or used by the calculator. Changing them does not change the results. Selecting a profile also does not update the target fields automatically.

## Calculations

For water volume `V` in liters, the app uses the manufacturer's correction factors:

| Supplement | Factor per 100 L | Total dose (ml) |
| --- | --- | --- |
| Part #1 — Calcium & Magnesium | 1 ml raises Ca by 1.4 ppm | `max(targetCa - currentCa, 0) / 1.4 * V / 100` |
| Part #2 — KH/Alkalinity | 1 ml raises KH by 0.1 dKH | `max(targetKh - currentKh, 0) / 0.1 * V / 100` |
| Part #3 — Iodine & Potassium | Proportional to Part #1 | `part1Ml * profilePart3 / profilePart1` |
| Part #4 — Iron & Bioactive Elements | Proportional to Part #1 | `part1Ml * profilePart4 / profilePart1` |

Each daily dose is the total divided by the entered number of days. Results are displayed to two decimal places.

All included profiles have the same relative dosing ratio: **1 : 2 : 0.5 : 0.5**. Therefore, switching profiles currently leaves the calculated correction doses unchanged. Part #2 is calculated independently from the KH deficit, rather than being set to twice the Part #1 dose.

### Example

For **100 L**, Ca **436 → 450 ppm**, KH **8.0 → 8.5 dKH**, and **4 days**:

| Supplement | Total | Per day |
| --- | --- | --- |
| Part #1 | 10.00 ml | 2.50 ml/day |
| Part #2 | 5.00 ml | 1.25 ml/day |
| Part #3 | 5.00 ml | 1.25 ml/day |
| Part #4 | 5.00 ml | 1.25 ml/day |

## Scope and dosing guidance

This version calculates a correction from a single set of measurements. It does not measure ongoing consumption, calculate a maintenance regimen, control a pump, or enforce maximum daily increases. The prefilled targets are editable examples, not automatically selected recommendations for your reef profile.

Use the manufacturer's instructions when applying the results:

- Red Sea's daily-increase limits are **20 ppm Ca** and **1.4 dKH**. Choose enough days to stay within them and re-test during adjustments.
- Add the parts in numerical order, leaving **10 minutes** between additions.
- The 4-part program is intended for systems without a refugium or algae scrubber; consult Red Sea's guidance for those systems.

## Official references

- [Red Sea Complete Reef Care 4-Part product information](https://redseafish.com/reef-care-program/supplements/complete-4-part/)
- [Complete Reef Care manual — EN / DE / FR, V24A (PDF)](https://g1.redseafish.com/wp-content/uploads/2024/03/24964_Complete-Reef-Care-Manual_FED_V24A-WEB.pdf) — English dosing factors and profile table on PDF page 5; daily-increase limits on PDF page 6.

## Issues and contributions

Report bugs or suggest improvements through [GitHub Issues](https://github.com/pdonat01/DosingCalculator/issues). For a calculation issue, include your inputs, selected profile, actual result, expected result, and the relevant manufacturer reference. For application issues, include your Windows version and any error message or screenshot.

Pull requests are welcome. Build the solution on Windows, describe the change, and include an example that demonstrates its behavior. Changes to dosing factors should cite the relevant official documentation.

## License

This repository does not currently contain a `LICENSE` file. No open-source license has been declared; check with the author before redistributing the project or using its code in another project.
