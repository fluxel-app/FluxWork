# FluxWork

.NET WPF MVVM-C "Framework" (Eher: Spielwiese)

![Nuget](https://img.shields.io/nuget/v/FluxWork)

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

#### `ControllerBase<TViewModel>`

Basisklasse aller Controller. Sollte nicht direkt geerbt werden.

Controller können ab Version 1.0.2 auf Änderungen einzelner Properties im ViewModel reagieren. Hierfür reicht eine Funktion ohne Rückgabe (`void`), mit keinem oder einem Parameter (Typen der `Property`) und dem Namensmuster `On*PropertyName*Changed`.

```csharp
// ViewModel
public virtual string Label { get; set; }

//Controller
public void OnLabelChanged(string label) {
    // TODO
}
```

#### `NavigationTargetController<TViewModel>`

Ziel einer Navigation im Hauptfenster.

```csharp
public class FirstController : NavigationTargetController<FirstViewModel>
{
    private readonly IWindowService _windowService;

    public FirstController(IWindowService windowService, DialogController dialogController)
    {
        _windowService = windowService;
    }

    public override void Initialized()
    {
        this.Title = "Hello!";
        this._windowService.BaseWindow.Height = 200;
        this._windowService.BaseWindow.Width = 300;
        this.ViewModel.Label = string.Empty;
    }

    public override void Loaded()
    {
        this.ViewModel.Label = "Foo";
    }
}
```

#### `DialogController<TViewModel>`

Ein Controller zu einem Dialog.

```csharp
public class FirstDialogController : DialogController<DialogViewModel>
{
    public override void Initialized()
    {
        this.Dimensions.SetHeights(200);
        this.Dimensions.SetWidths(300);
        this.Title = "Dialogtitel";

        this.ViewModel.Items = new List<ItemViewModel>();
    }

    public override void Loaded()
    {
        this.ViewModel.Items.AddRange(new []
        {
            new ItemViewModel(1),
            new ItemViewModel(2),
            new ItemViewModel(3),
            new ItemViewModel(4),
        });
    }
}
```

#### `ContentController<TViewModel>`

Ein Controller für tiefere Inhalte. Initialized und Loaded müssen vom einbettenden Controller aufgerufen werden.

### ViewModel

#### `ViewModelBase`

Ein reguläres ViewModel ohne weiterer Funktion.

#### `VirtualViewModelBase`

Ein ViewModel, wo jede `Property` ein `OnPropertyChanged` auslöst. Jede `Property` **muss** `virtual` sein.

### View

Jede View (WPF UserControl) muss das `Interface` `IViewFor<TViewModel>` implementieren. Das kann im Code Behind-Bereich passieren. Das `Interface` enthält keine Funktion und dient lediglich der Zuordnung zum **ViewModel**. Im UserControl-Xaml-Teil kann `d:DataContext="{d:DesignInstance TViewModel}"` verwendet werden, um Hinweise auf die `Properties` des ViewModels zu erhalten.

### Commands

Um Kommandos zu verarbeiten kann ein eigenes Kommando implementiert werden oder über `DelegateCommand` oder `DelegateCommand<TType>` ein Kommando in eine Funktion (`Action`/`Action<TType>`) umgeleitet werden.

Seit **Version 1.0.2** werden im ViewModel Properties vom Typ `ICommand` direkt an Funktionen des Controllers weitergeleitet. Die Funktion im Controller darf keinen Rückgabetypen haben, erwartet entweder keinen oder einen Parameter und muss folgenden Namen aufweisen: `On*PropertyName*`

```csharp
// ViewModel
public virtual ICommand Save { get; set; }

//Controller
public void OnSave() {
    // TODO
}
```

### Data

Die `ControllerBase` enthält zwei überschreibbare Funktionen.

#### `virtual void Initialized()`

In dieser Funktion sollten keine Daten geladen und lediglich der Controller und das ViewModel vorbereitet werden.

#### `virtual void Loaded()`

In dieser Funktion werden Daten geladen und aufbereitet.

#### `ObservableDataSource<TItem>`

Die `ObservableDataSource<TItem>` ist von der `ObservableCollection<TItem>` abgeleitet. Sie erwartet im Konstruktor eine Funktion, welche die Daten zurückgibt, die Si halten soll. Mit der Funktion `void Reload()` wird die Funktion aufgerufen und die DataSource gefüllt. So kann man in der Initialized-Funktion die DataSource vorbereiten und die Funktion angeben und in der Loaded-Funktion die DataSource befüllen lassen.

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
