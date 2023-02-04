# Simple Health System for Unity

A simple Health System for Unity that can be used on a variety of types of games

## Features 

- Has a `HealthManager`component that can be attached to any GameObject;
- Event-based calls, like when the health gets changed or when it dies;
- Cooldown option, with a configurable value;
- Easy to use (simply call the methods `Damage` or `Heal` to change the health).

## Setup

Simply download and import the package into your project.

Optionally, you can include a **Samples** folder with Scenes that demonstrate how the component can be used.

![image](https://user-images.githubusercontent.com/1856860/216791755-97577ac5-1fe3-490d-aeeb-46de8a009e8c.png)

## Usage

### `HealthManager`

Attach the `HealthManager` component to the GameObject you want to have a Health System, for example, the Player, and adjust the attributes on the inspector according to your need.

![image](https://user-images.githubusercontent.com/1856860/216790567-cdf1fb29-c41e-4cbc-aa5e-39096b30efe8.png)

#### Attributes

- **Max Health**: The maximum amount of health the object can have;
- **Cooldown**: the cooldown time, in seconds. _Set '0' for no cooldown_;
- **Starting Health**: The amount of health the object will start with.

### Events

You can either set the events through the Inspector or the C# Code:

![image](https://user-images.githubusercontent.com/1856860/216790715-e727353a-c1b5-4e35-aa96-a1284c7c3690.png)

- **On Health Change**: Called when the health gets changed, because of a damage taken or health added.
_The method for this event must have two parameters: a `float` value indicating the current Health and another `float` with the amount of health changed (positive for Healing or negative for Damaging)_
- **On Die**: Called when the Health reaches 0.
- **On Cooldown Start**: Called when the cooldown starts, after taking damage.
- **On Cooldown End**: Called whe the cooldown ends.

## Examples

The `HealthManager` is a component, so it can be referenced in your code by dragging and dropping its reference in the Inpector or at runtime (for example using `GetComponent<HealthManager>()` method.

### Applying Damage

```csharp

...

private Start()
{
    healthManager = GetComponent<HealthManager>();
}

private DoApplyDamage()
{
    bool appliedDamange = healthManager.Damage(3);

    if (appliedDamage)
    {
        Debug.Log($"The damage had been applied to the Health Manager");
    }
}

...

```
This sample applies 3 of damage to the Health System and display a Debug message if the damage had been properly apply

_The `Damage` method returns `false` if the Health System is cooling down or already reached 0 health.

### Healing

```csharp

...

private Start()
{
    healthManager = GetComponent<HealthManager>();
}

private DoApplyDamage()
{
    healthManager.Heal(5);
}

...

```

This sample add 5 point of health to the Health System.

### Observing `OnHealthChanged` event

```csharp

...

private Start()
{
    healthManager = GetComponent<HealthManager>();
    healthManager.OnHealthChange += HealthChange;
}

private OnDestroy()
{
    healthManager.OnHealthChange -= HealthChange;
}

private HealthChange(float health, float amount)
{
    Debug.Log($"The health has changed by {amount}. Current Health is: {health}");
}

...

```

This sample adds a callback method to the `OnHealthChange` event that displays a bebug message every time the health gets changed
