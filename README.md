# 🚀 Chest System – Modular & Scalable Reward System for Unity

Welcome to my latest Unity project—a **fully modular and scalable Chest System**, inspired by mobile games like *Clash Royale*. This system isn't just about unlocking rewards—it's an architectural showcase that balances fun gameplay with powerful extensibility. 🎮

---

## Key Features & Functionality

### Randomized Chest Generation
Each chest is generated with randomized properties (from **Common** to **Legendary**), bringing unpredictability and excitement to every session.

###  Timed Unlocking with a Twist
Chests use real-time countdowns to build anticipation. Players can either wait or spend **gems** to unlock instantly—introducing strategy and choice into progression. Plus, with the **Undo** feature implemented via the Command Pattern, players can revert an instant unlock if they change their mind.

### Dynamic UI Feedback
Chests transition through visual states—**Locked**, **Unlocking**, **Unlocked**, and **Collected**—each with engaging animations and satisfying sound feedback.

### Inventory & Reward Management
Players can manage their expanding inventory with ease. Rewards like **coins** and **gems** are delivered through delightful animations to enhance the user experience.

---

## Architectural & Design Patterns

### Model-View-Controller (MVC)
Separation of data, UI, and logic improves clarity, maintainability, and testability of the codebase.

### Service Locator Pattern
A centralized `GameService` singleton ensures streamlined access to core systems, reducing coupling and simplifying dependency management.

### State Machine
Robust handling of chest states ensures smooth transitions between different phases like **Locked**, **Unlocking**, **Unlocked**, etc.

### Observer Pattern
Event-driven design enables clean communication across game components, making the system modular and scalable.

### Command Pattern
Encapsulates actions—such as spending gems for an instant unlock—into command objects, allowing execution and **undo** operations. This design supports reverting an instant chest unlock, improving user control and maintainability.

### Object Pooling
Efficient use of object pooling minimizes instantiation overhead—crucial for maintaining smooth mobile performance.

---

## Unity & C# Enhancements

### Interfaces & Enums
Type-safe and extensible system design to define chest behaviors, rarity types, and state handling.

### ScriptableObjects for Configuration
Flexible, data-driven design lets you configure chest properties and reward tables directly from the editor.

### Generics & Extension Methods
Reusable and flexible code components help keep everything DRY (*Don’t Repeat Yourself*).

### Coroutines & Tweening
Smooth, time-based effects and transitions for chest animations, countdowns, and UI feedback.

### Custom Editor Tools
Custom inspectors and tools simplify data entry and speed up iteration during development.

### Canvas Groups for UI Transitions
Professional-level UI transitions using canvas groups to create smooth fades and visibility control.

---

## Software Engineering Best Practices

### SOLID Principles
Each class follows the Single Responsibility Principle, making it easy to extend, test, and maintain.

### Dependency Injection
Loose coupling of components makes the system adaptable and easier to integrate new features.

### Namespaces & Abstraction
Structured code organization using namespaces and interfaces ensures flexibility and scalability.

---

## Personal Reflections

Creating this Chest System has been a blend of **gameplay design** and **software architecture**. I’ve applied principles from enterprise software development to build a feature-rich system that’s fun to use and easy to expand.

The modular design opens the door for future ideas—think loot tables, limited-time chests, or even player progression integrations.

---

## Let’s Connect!

What challenging game systems have *you* built recently? I’d love to hear your stories and exchange ideas!

---

## Play Link

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
