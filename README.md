# ClassroomDigitalTwin

A Unity project for procedural generation of a 3D classroom “digital twin”.  
This README covers repository structure, features, setup, usage, and best practices.

---

## Table of Contents

1. [Overview](#overview)  
2. [Key Features](#key-features)  
3. [Architecture & Design](#architecture--design)  
4. [Setup & Installation](#setup--installation)  
5. [Usage](#usage)  
6. [Project Structure](#project-structure)  
7. [Configurable Parameters](#configurable-parameters)  
8. [Best Practices & Design Patterns](#best-practices--design-patterns)  
9. [Contributing](#contributing)  
10. [Resources & References](#resources--references)  
11. [License](#license)  

---

## Overview

This repository contains a Unity scene and C# scripts that dynamically generate geometry and place classroom elements (desks, chairs, blackboard, windows) to create a **digital twin** of a classroom. It enables:

- Rapid configuration of educational 3D spaces  
- Fine control via Unity Editor or ScriptableObjects  
- Easy extension to other environments (lab, auditorium, etc.)  

---

## Key Features

- **Procedural Generation**: Controlled random placement using a grid + jitter approach.  
- **Configurable Parameters**: Number of rows, spacing, orientation, materials.  
- **Modular Prefabs**: Each object (Desk, Chair, Blackboard, Window) is a standalone prefab.  
- **Editor & Runtime Support**: Generate or regenerate in Edit mode or Play mode.  
- **Custom Inspector UI**: One-click “Generate Classroom” button in the Editor.  

---

## Architecture & Design

- **Factory Pattern**  
  - `ClassroomFactory` handles all object creation, decoupling placement logic from prefab instantiation.  
- **ScriptableObject**  
  - `ClassroomConfig` stores parameters (dimensions, counts, materials) for easy tuning without code changes.  
- **Singleton (optional)**  
  - `ClassroomManager.Instance` provides global access if you integrate multiple classrooms or multi-scene workflows.  

---

## Setup & Installation

1. **Clone the repository**  
   ```bash
   git clone https://github.com/maximepianezi/SalleDeClasseDigitalTwin.git
   cd SalleDeClasseDigitalTwin
Open in Unity

Launch Unity Hub, add this folder, and open with Unity 2021.3 LTS or newer.

Import Required Packages

Go to Window > Package Manager and install:

Cinemachine

Post Processing (or URP/HDRP packages)

Open the Main Scene

Assets/Scenes/ClassroomScene.unity

Usage
Select the ClassroomGenerator GameObject in the Hierarchy.

In the Inspector, assign:

A ClassroomConfig ScriptableObject

Prefabs: DeskPrefab, ChairPrefab, BlackboardPrefab, WindowPrefab

Adjust parameters (rows, columns, spacing, etc.).

Click Generate Classroom to create or refresh the scene.

csharp
Copier
Modifier
// Runtime example: generate a 10×5 classroom at startup
public class RuntimeSpawner : MonoBehaviour
{
    [SerializeField] private ClassroomGenerator generator;

    void Start()
    {
        generator.Config.Rows = 10;
        generator.Config.Columns = 5;
        generator.Generate();
    }
}
Project Structure

Folder	Contents
Assets/Scripts/	All C# scripts (Generator, Factory, Config, Manager, etc.)
Assets/Prefabs/	Prefabs for desks, chairs, blackboard, windows
Assets/Scenes/	Unity scene files (.unity)
Assets/Materials/	Materials and textures
Assets/Editor/	Custom inspectors and editor tools (Generate button, etc.)
Logs/	Optional generation logs
ProjectSettings/	Unity project settings
Configurable Parameters
The ClassroomConfig ScriptableObject exposes:


Property	Description	Default
Rows	Number of desk rows	8
Columns	Number of desk columns	6
SpacingX, SpacingZ	Distance between desks (Unity units)	1.5, 1.2
DeskPrefab	Desk prefab reference	–
ChairPrefab	Chair prefab reference	–
BlackboardPrefab	Blackboard prefab reference	–
WindowPrefab	Window prefab reference	–
UseRandomRotation	Apply slight random rotation to objects (bool)	false
Best Practices & Design Patterns
Decouple Logic & Data

Use ScriptableObjects to separate parameters from code.

Factory Method

Centralize prefab instantiation in factory classes for cleaner code.

Object Pooling

When regenerating often, implement pooling to avoid frequent Instantiate()／Destroy() overhead.

Profiling

Use the Unity Profiler to monitor CPU and garbage collection impact during generation.

Contributing
Fork the repo.

Create a branch feature/your-feature-name.

Submit a Pull Request with clear change descriptions.

Follow C# conventions (PascalCase, XML comments on public methods).

Resources & References
Unity Manual – ScriptableObject
https://docs.unity3d.com/Manual/scriptable-objects.html

Unity Scripting API – Instantiate
https://docs.unity3d.com/ScriptReference/Object.Instantiate.html

Procedural Grid Tutorial (Catlike Coding)
https://catlikecoding.com/unity/tutorials/procedural-grid/

License
MIT © 2025 — See the LICENSE file for details.