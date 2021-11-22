# FluxWork

.NET WPF MVVM-C "Framework" (Eher: Spielwiese)

Ursprünglich: HitWork

## HitWork in a nutshell

### Initial

Das HitWork baut eigentlich alles zusammen, was eine WPF-App braucht. Man legt lediglich ein Konsolenprogramm in .NET Framework an und schreibt folgende Zeilen in die Main-Funktion:

```c#
var boot = Bootstrap.Singleton; // Erzeugt eine Instanz des HitWorks
boot.DependencyService.SetAssemblyNameFilter(@"Meine\.App$"); // Grenzt die Suche nach Services ein, RegEx
boot.Boot("Meine App"); // Titel des Hauptfensters

var ds = boot.DependencyService; // Holt den DependencyService
var windowService = ds.Find<IWindowService>(); // Holt den WindowService
windowService.Navigate(ds.Find<ErstePresentationController>()); // Setzt die 'ErstePresentation' als Einstieg

boot.Run(); // Registriert Dienste und zeigt das Hauptfenster an
```

### Controller

`TODO`

### ViewModel

`TODO`

### View

`TODO`

### Commands

`TODO`

### Data

`TODO`

## Geschichtsstunde

Das HitWork ist vor einigen Jahren enstanden. Damals suchte ich ein passendes MVVM-Framework für meine Projekte. Einige Frameworks habe ich ausprobiert.

Zu der Zeit suchte ich ein leichtes Framework, was mir standard-Abläufe automatisiert. Was ich in meiner Suche fand war eher schwergewichtig und hat mir die Arbeit nur erschwert oder war schlecht dokumentiert. So zumindest meine damalige subjektive Sichtweise...

### Codequalität und so weiter...

Naja, das HitWork ist enstanden, da "wusste ich es nicht besser". Es ist eher ein Work-in-progress-Teil, was ich immerwieder erweitert habe, wenn ich eine Funktion brauchte.

Seit einigen Jahren programmiere ich nun hauptsächlich Webapps in .NET und irgendwann habe ich das HitWork "gelöscht". Dennoch habe ich das HitWork in vielen Projekten weiterverwendet und das ging auch einige Zeit gut.

### Grenzen der DLL

Ich hatte also bis zu einem gewissen Zeitpunkt nurnoch die DLL-Datei und einige Projekte, die davon abhänig waren. Keine gute Kombination. Ich habe mir also jetzt einen Decompiler geschnappt und das HitWork entpackt. Das Ergebnis musste ich etwas anpassen, viel ist es nicht, aber viel Magie.

### FluxWork?

Hier muss ich erklären, der Name HitWork kommt von **H**einle-**IT**-Frame**work**. Die Firma und Kunden der Heinle IT wurden jedoch von der Fluxel UG übernommen, weshalb die Namensanpassung im selben Zuge durchgeführt wurde.
