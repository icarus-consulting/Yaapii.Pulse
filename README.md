# Yaapii.Pulse

A library for object oriented event handling.

## Content

1. [Basic Principles](#basic-principles)
2. [Signals](#signals)
    - [Signal Examples](#signal-examples)
3. [Sensors](#sensors)
    - [Routing](#routing)
    - [Sensor Examples](#sensor-examples)
4. [Pulse](#pulse)
    - [Pulse Examples](#pulse-examples)

## Basic Principles

```ISignal``` represents information about an event.

```ISensor``` reacts to events.

```IPulse``` bundles sensors and sends signals to them.

```IAsyncPulse``` sends signals to sensors asynchronously.

To allow your app to react to specific events, add a pulse, create sensors to process those events and connect the sensors to your pulse. To trigger an event, send an appropriate signal over the pulse.

## Signals

```ISignal``` has two methods returning string dictionaries, ```Head()``` and ```Props()```.

The ```Head``` dictionary is used for routing information, based on which the appropriate sensor is selected, i.e. info on what kind of event the signal represents. For this purpose, all signals in your app should use the same keys in this dictionary, for example "entity", "category" and "event". The head of a signal is also a good place for metadata present in all your signals, like timestamps.

The ```Props``` dictionary contains event-specific information for the appropriate sensor to work with. What this looks like may be different for each type of event.

### Signal Examples

```csharp
// signal with only routing information:
new SignalOf(
    new SigHead(
        "example-entity",
        "example-category",
        "example-event",
        new SigScope("example-scope"), // this is optional
        new SigTimestamp() // this is also optional
    )
)

// signal with props:
new SignalOf(
    new SigHead(
        "example-entity",
        "example-category",
        "example-event"
    ),
    new SigProps(
        "example-key-1", "example-value-1",
        "example-key-2", "example-value-2"
    )
)
```

## Sensors

```ISensor``` can process signals. In addition, there are two status methods, ```IsActive()``` and ```IsDead()```. If the sensor is not currently active, it should be ignored and no signals should be sent to it. An inactive sensor may become active again, it is only temporarily disabled. If the sensor is dead, it is permanently unuseable and should be removed from the pulse.

### Routing

```SnsChain``` can forward a signal to multiple other sensors.

```ConditionalSensor``` will forward a signal to another sensor, if the signal matches the given conditions. Conditions can be ```Func<ISignal, bool>``` or ```ICondition```.

### Sensor Examples

```csharp
// example routing with entity, category and action:
new SnsChain(
    new ConditionalSensor(
        new SnsChain(
            new ConditionalSensor(
                new SnsChain(
                    new ConditionalSensor(
                        new SnsAct((signal) => { /* do something */ }),
                        new HeadCondition("event", "example-event-1")
                    ),
                    new ConditionalSensor(
                        new SnsAct((signal) => { /* do something */ }),
                        new HeadCondition("event", "example-event-2")
                    )
                ),
                new HeadCondition("category", "example-category-1")
            ),
            new ConditionalSensor(
                new SnsChain(
                    new ConditionalSensor(
                        new SnsAct((signal) => { /* do something */ }),
                        new HeadCondition("event", "example-event-3")
                    )
                ),
                new HeadCondition("category", "example-category-2")
            )
        ),
        new HeadCondition("entity", "example-entity-1")
    ),
    new ConditionalSensor(
        new SnsChain(
            new ConditionalSensor(
                new SnsChain(
                    new ConditionalSensor(
                        new SnsAct((signal) => { /* do something */ }),
                        new HeadCondition("event", "example-event-4")
                    )
                ),
                new HeadCondition("category", "example-category-3")
            )
        ),
        new HeadCondition("entity", "example-entity-2")
    )
).Trigger(
    new SignalOf(
        new SigHead("example-entity-1", "example-category-2", "example-event-3")
    )
);
```

## Pulse

```IPulse``` and ```IAsyncPulse``` will send signals to all connected sensors that are currently active. Sensors can be passed to the ctors of ```SnycPulse``` and ```ParallelPulse``` or added later vie the ```Connect(ISensor sensor)``` method common to both pulse interfaces. Dead sensors will be removed.

### Pulse Examples

```csharp
var pulse =
    new ParallelPulse(
        ConditionalSensor(
            SnsAct((signal) => { /* do something */ }),
            new HeadCondition("event", "example-event-1")
        )
    );
pulse.Connect(
    ConditionalSensor(
        SnsAct((signal) => { /* do something */ }),
        new HeadCondition("event", "example-event-2")
    )
);
pulse.Send(
    new SignalOf(
        new SigHead(
            new KvpOf("event", "example-event-1")
        )
    )
);
```
