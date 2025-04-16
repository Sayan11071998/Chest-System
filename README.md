# 🚀 Chest System – Modular & Scalable Reward System for Unity

Welcome to my latest Unity project—a **fully modular and scalable Chest System**, inspired by mobile games like *Clash Royale*. This system isn't just about unlocking rewards—it's an architectural showcase that balances fun gameplay with powerful extensibility. 🎮

---

## 🌟 Key Features & Functionality

### 🎲 Randomized Chest Generation
Each chest is generated with randomized properties (from **Common** to **Legendary**), bringing unpredictability and excitement to every session.

### ⏱️ Timed Unlocking with a Twist
Chests use real-time countdowns to build anticipation. Players can either wait or spend **gems** to unlock instantly—introducing strategy and choice into progression.

### 🎨 Dynamic UI Feedback
Chests transition through visual states—**Locked**, **Unlocking**, **Unlocked**, and **Collected**—each with engaging animations and satisfying sound feedback.

### 📦 Inventory & Reward Management
Players can manage their expanding inventory with ease. Rewards like **coins** and **gems** are delivered through delightful animations to enhance the user experience.

---

## 🛠️ Architectural & Design Patterns

### 📑 Model-View-Controller (MVC)
Separation of data, UI, and logic improves clarity, maintainability, and testability of the codebase.

### 🧭 Service Locator Pattern
A centralized `GameService` singleton ensures streamlined access to core systems, reducing coupling and simplifying dependency management.

### 🔄 State Machine
Robust handling of chest states ensures smooth transitions between different phases like **Locked**, **Unlocking**, **Unlocked**, etc.

### 📢 Observer Pattern
Event-driven design enables clean communication across game components, making the system modular and scalable.

### ♻️ Object Pooling
Efficient use of object pooling minimizes instantiation overhead—crucial for maintaining smooth mobile performance.

---

## 💻 Unity & C# Enhancements

### 🔗 Interfaces & Enums
Type-safe and extensible system design to define chest behaviors, rarity types, and state handling.

### 📜 ScriptableObjects for Configuration
Flexible, data-driven design lets you configure chest properties and reward tables directly from the editor.

### 🧩 Generics & Extension Methods
Reusable and flexible code components help keep everything DRY (*Don’t Repeat Yourself*).

### ⏳ Coroutines & Tweening
Smooth, time-based effects and transitions for chest animations, countdowns, and UI feedback.

### 🔧 Custom Editor Tools
Custom inspectors and tools simplify data entry and speed up iteration during development.

### 🎭 Canvas Groups for UI Transitions
Professional-level UI transitions using canvas groups to create smooth fades and visibility control.

---

## 🏗️ Software Engineering Best Practices

### 🛡️ SOLID Principles
Each class follows the Single Responsibility Principle, making it easy to extend, test, and maintain.

### 🔌 Dependency Injection
Loose coupling of components makes the system adaptable and easier to integrate new features.

### 📦 Namespaces & Abstraction
Structured code organization using namespaces and interfaces ensures flexibility and scalability.

---

## ❤️ Personal Reflections

Creating this Chest System has been a blend of **gameplay design** and **software architecture**. I’ve applied principles from enterprise software development to build a feature-rich system that’s fun to use and easy to expand.

The modular design opens the door for future ideas—think loot tables, limited-time chests, or even player progression integrations.

---

## 💬 Let’s Connect!

What challenging game systems have *you* built recently? I’d love to hear your stories and exchange ideas!

> Drop a ⭐ if you find this project interesting or useful, and feel free to fork it or open a PR with improvements or cool additions!

---
