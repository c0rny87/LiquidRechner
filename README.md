# LiquidRechner

`LiquidRechner` ist eine interaktive Blazor-Anwendung (.NET 10, C# 14), die Hobby-Mixern ein präzises Werkzeug an die Hand gibt, um Mischungen aus Nikotin-Shots, Aromen und Basen zusammenzustellen. Das Projekt nutzt die interaktive Server-Render-Mode-Architektur und richtet sich primär an Anwender, die schnell verschiedenste Zielvorgaben (Menge, Stärke, Aromaanteil, VG/PG-Mischung) durchrechnen möchten.

## Features
- Live-reactive Formularfelder mit Bindung an `LiquidRecipe`
- Anzeige von Resultaten in ml und geschätzten Gramm
- Gewichtsschätzung basierend auf VG/PG-Dichte
- Darstellung von Basen mit unterschiedlichen VG-Anteilen sowie optionalen Shot-Einstellungen
- Fehlerbehandlung bei unmöglichen Rezepturen

## Voraussetzungen
- .NET 10 SDK
- Visual Studio 2026 oder vergleichbare Entwicklungsumgebung mit Blazor-Unterstützung

## Projekt starten
1. Repository klonen und in das Projektverzeichnis wechseln.
2. Abhängigkeiten laden:
	- dotnet restore
3. Anwendung starten:
	- dotnet run --project LiquidRechner/LiquidRechner.csproj

4. Browser öffnet automatisch die App auf `https://localhost:5001` (ggf. HTTPS-Zertifikat akzeptieren).

## Architektur
- `Program.cs` richtet die minimalistische Middleware-Pipeline ein, nutzt Razor-Komponenten und bindet interaktives Server-Rendering ein.
- `Home.razor` enthält die UI mit Eingabefeldern, Validierung und Ergebnisdarstellung.
- `MainLayout.razor` liefert das Grundlayout mit Sidebar (NavMenu) und Content-Bereich.
- Die Logik ist in `LiquidRecipe` gekapselt (nicht gezeigt), die Rechnungen zentral übernimmt.

## Erweiterungsmöglichkeiten
- Weitere Basen/Shot-Presets hinzufügen
- Export-Feature für Rezepte in CSV oder PDF
- Internationalisierung für Benutzeroberfläche und Beschriftungen

So bietet `LiquidRechner` eine klare Basis für ein Blazor-Geschäftslogik-Dashboard und lässt sich gut modular erweitern.
