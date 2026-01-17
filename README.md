# Game System/Framework | Unity | C#

A chest unlock system with state-driven gameplay, command pattern for undo functionality, and event-based architecture. Features object pooling for performance optimization and service locator pattern for dependency management.

**Tech Stack:** Unity 3D | C# | State Pattern | Command Pattern | Observer Pattern | Object Pool Pattern

---

## Development Approach

I structured the system around MVC with a service layer managing cross-cutting concerns. GameService acts as a service locator, providing access to ChestService, PlayerService, SoundService, and CommandInvoker. Each chest maintains its own state machine, with ChestController coordinating between ChestView (UI) and ChestModel (data). When a player interacts with a chest, the state machine handles transitions while EventService broadcasts changes to registered listeners.

The challenge was managing multiple chests unlocking over real-time while maintaining only one active unlock timer. I solved this by having ChestService track `currentlyUnlockingChest` and rejecting new unlock attempts until the current chest completes. States handle their own logic - LockedState checks `CanStartUnlocking()` before transitioning, while UnlockingState runs a coroutine that decrements `RemainingUnlockTime` every second.

```mermaid
flowchart TD
    GameService[GameService]
    
    GameService --> ChestService[ChestService]
    GameService --> PlayerService[PlayerService]
    GameService --> CommandInvoker[CommandInvoker]
    GameService --> SoundService[SoundService]
    
    ChestService --> ChestController[ChestController]
    ChestService --> ChestPool[ChestPool]
    ChestService --> EmptySlotPool[EmptySlotPool]
    
    ChestController --> ChestView[ChestView]
    ChestController --> ChestModel[ChestModel]
    
    ChestController --> ChestStateMachine[ChestStateMachine]
    ChestPool --> ChestStateMachine
    ChestModel --> ChestStateMachine
    
    ChestStateMachine --> LockedState[LockedState]
    ChestStateMachine --> UnlockingState[UnlockingState]
    ChestStateMachine --> UnlockedState[UnlockedState]
    ChestStateMachine --> CollectedState[CollectedState]
    
    PlayerService --> PlayerController[PlayerController]
    
    PlayerController --> PlayerView[PlayerView]
    PlayerController --> PlayerModel[PlayerModel]
    
    CommandInvoker --> CommandStack[Command Stack]
    
    classDef serviceStyle fill:#5B9BD5,stroke:#333,stroke-width:2px,color:#fff
    classDef chestServiceStyle fill:#70AD47,stroke:#333,stroke-width:2px
    classDef stateMachineStyle fill:#F4B183,stroke:#333,stroke-width:2px
    classDef commandStyle fill:#E57373,stroke:#333,stroke-width:2px
    
    class GameService serviceStyle
    class ChestService chestServiceStyle
    class ChestStateMachine stateMachineStyle
    class CommandInvoker commandStyle
```

---

## Key Technical Systems

* ### State Machine with Dynamic Unlock Timing
    - Chests transition through four states using a state machine that implements `IState`. The critical challenge was handling real-time unlocking with instant unlock interruptions. UnlockingState runs a coroutine that updates `ChestModel.RemainingUnlockTime` every second, recalculating gem cost dynamically.
    - The gem cost formula uses `MINUTES_PER_GEM = 10f`, so a 1-hour chest costs 6 gems initially but drops to 3 gems after 30 minutes. I implemented `UpdateGemCost()` to recalculate on every timer tick, ensuring accurate pricing. The edge case was chests with < 10 minutes remaining - I added a minimum cost check to prevent 0-gem unlocks.
    - When a player instant-unlocks, `InstantChestUnlockCommand` executes, storing `previousState`, `previousUnlockTime`, and `previousPlayerGems` for undo functionality. The command pattern needed to restore the UnlockingState's coroutine, so I used `ChestStateMachine.ChangeState()` which calls `OnStateEnter()` - this restarts the timer coroutine with the restored `RemainingUnlockTime`.

* ### Command Pattern for Undoable Actions
    - The undo system needed to reverse gem spending and restore the unlocking timer. I implemented `ICommand` with `Execute()` and `Undo()` methods, storing all state before modifications. The tricky part was accessing private fields in ChestModel during undo.
    - I used reflection to set `remainingUnlockTime` and `chestSprite` directly:
 
```csharp
var timeField = typeof(ChestModel).GetField(
    "remainingUnlockTime",
    System.Reflection.BindingFlags.NonPublic |
    System.Reflection.BindingFlags.Instance
);

timeField.SetValue(model, previousUnlockTime);
```

    - This approach is fragile but necessary since ChestModel doesn't expose setters. For production, I'd add `RestoreState(float time, Sprite sprite)` to ChestModel to avoid reflection. CommandInvoker maintains a `Stack<ICommand>` and shows an undo notification after execution, hooking into `NotificationPanel.OnNotificationClosed` event to trigger the undo.

### Object Pooling for Dynamic Chest Management

The system spawns/despawns chests frequently, so I built `GenericObjectPool<T>` using a `List<PooledItem<T>>` that tracks `isUsed` flags. When requesting a chest, `GetItem()` searches for an unused instance before calling `CreateItem()`.

The challenge was managing sibling indices when removing chests. `RemoveChestAndMaintainMinimumSlots()` needed to preserve visual order while ensuring at least 4 slots remain. I store the deleted chest's siblingIndex, then shift all subsequent elements down by calling `SetSiblingIndex(index - 1)`. If total slots drop below the minimum, I spawn an EmptySlotView at the deleted position.

ChestPool and EmptySlotPool both extend `GenericObjectPool<T>` but implement different `CreateItem()` methods - ChestPool instantiates ChestView prefabs, while EmptySlotPool creates EmptySlotView instances. When returning to pool, chests call `OnReturnToPool()` which triggers `Cleanup()` to unregister from `ChestService.currentlyUnlockingChest` if needed.

### Event-Driven Sound System

SoundService registers listeners for all game events in a single method. Each event type triggers a corresponding sound:

```csharp
EventService.Instance.OnChestSpawned.AddListener(
    chest => PlaySoundEffects(SoundType.CHEST_CLICK)
);
```

The problem was memory leaks - if SoundService didn't unregister listeners before destruction, event subscriptions persisted. I implemented `UnregisterSoundEventListeners()` called in `GameService.OnDestroy()`, removing all registered callbacks. The event system uses `EventController<T>` with generic `Action<T>` delegates, allowing type-safe event invocations.

### Notification System with Conditional UI

NotificationPanel handles two display modes: standard notifications and undo-enabled notifications. `ShowNotificationWithUndo()` activates an extra `undoButtonContainer` GameObject that's hidden in normal notifications.

The animation system uses a CanvasGroup for fade effects and `RectTransform.localScale` for popup scaling. I implemented coroutines that interpolate alpha and scale values using an AnimationCurve over `fadeInDuration`. The tricky part was handling notification interruptions - if a new notification appears while fading, I stop the current coroutine before starting a new one to prevent overlapping animations.

Static event `Action OnNotificationClosed` allows UnlockedState to defer chest collection until the player dismisses the reward notification. I subscribe in `ShowRewardsNotification()` and unsubscribe in `CollectChestAfterNotification()` to avoid duplicate callbacks.

### Service Locator for Dependency Management

GameService implements `GenericMonoSingleton<T>` and initializes all services in `Awake()`. This creates a global access point: `GameService.Instance.chestService`. The advantage is loose coupling - LockedState doesn't need constructor injection of ChestService, it simply calls `GameService.Instance`.

The downside is hidden dependencies and harder testing. For production, I'd use a DI framework like Zenject. The singleton pattern prevents multiple GameService instances by checking `if (instance == null)` in `Awake()` and destroying duplicates.

---

## Technical Challenges

- **Dynamic Slot Management:** Maintaining visual order when removing chests required tracking sibling indices and shifting all subsequent UI elements. I had to handle the edge case where removing a chest would leave fewer than the minimum slots, conditionally spawning EmptySlotViews to maintain the required slot count.

- **Coroutine Lifecycle Management:** UnlockingState's timer coroutine needed to stop cleanly when instant-unlocking. I stored the Coroutine reference and called `StopCoroutine()` in `OnStateExit()`, preventing multiple timers from running simultaneously.

- **Reflection for State Restoration:** Undo functionality required modifying private ChestModel fields. Using reflection was necessary but adds fragility - if field names change, the code breaks at runtime. I added null checks on FieldInfo to prevent crashes if fields are renamed.

---

## What I Learned

The State pattern simplified chest behavior by isolating state-specific logic - adding new states like "Repairing" would just require implementing `IState`. Command pattern's reversibility made undo trivial but required careful state capture before execution. Object pooling significantly reduced GC pressure, but managing pooled object lifecycle (cleanup, reinitialization) added complexity. Using a service locator created global coupling, making unit testing harder - dependency injection would improve testability but adds initialization complexity for small projects.

---

## Play Link
https://sayannandi.itch.io/chest-system

[![Watch the video](https://img.youtube.com/vi/ZzMUREyAbMA/maxresdefault.jpg)](https://youtu.be/ZzMUREyAbMA)
### [Gameplay Video](https://youtu.be/ZzMUREyAbMA)

![Image](https://github.com/user-attachments/assets/bf126721-4c44-45cc-b0b7-fa6715ad5c25)

![Image](https://github.com/user-attachments/assets/66b9c297-0c29-49c3-93cd-b0fd20344ee7)

![Image](https://github.com/user-attachments/assets/2f8bb228-4678-459b-b218-b9db52b89c64)

![Image](https://github.com/user-attachments/assets/85bfbd53-f868-41f8-b6ae-dfd7a40aff08)

![Image](https://github.com/user-attachments/assets/0343078e-d749-4836-8250-fa7efb5f98b8)

![Image](https://github.com/user-attachments/assets/f622cb4f-710c-4052-a3e2-21ea1b45803d)

![Image](https://github.com/user-attachments/assets/ebb9b99a-5fce-44e2-b772-7d9a339cf042)

![Image](https://github.com/user-attachments/assets/f2049160-71c2-4615-a18e-2b883529c9fa)

![Image](https://github.com/user-attachments/assets/cc0dc3c7-ca93-4127-bc80-e0ffe2f538ac)

![Image](https://github.com/user-attachments/assets/b17b7bef-c961-4f49-bd7c-ccb098dd411c)

![Image](https://github.com/user-attachments/assets/9923de97-993e-4ab6-be74-f95f0675e1b5)

![Image](https://github.com/user-attachments/assets/acc0b8a4-b5a7-4848-b079-b73486845424)

![Image](https://github.com/user-attachments/assets/b54c3863-b7e0-4c2c-9180-d4bbc5a1593f)
