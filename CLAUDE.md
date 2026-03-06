# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Game Design Document

The living GDD is maintained in Google Docs and is the source of truth for game mechanics, content, and narrative:
https://docs.google.com/document/d/1R7DD0sgwv62BPvMhKz4mxv4hkTpERIXbCr1uZoUXx90/edit?tab=t.0#heading=h.nkmim77anjg

Always check this document for the latest design decisions before implementing new features.

## Project Overview

CEO decision-making game built for a game jam. The player reviews employee requests and makes Yes/No decisions that affect Budget, Morale, and Headcount. Built with Unity 6000.3.3f1 using URP 2D.

## Repository Structure

- `GameJam_CEO/` — The actual Unity project (open this folder in Unity)
- `Assets/Scripts/` — Game scripts (currently outside the Unity project at repo root; should be under `GameJam_CEO/Assets/`)
- `Assets/ScriptableObjects/Employees/` and `Assets/ScriptableObjects/Requests/` — ScriptableObject asset folders

**Note:** The `Assets/` folder at repo root is separate from `GameJam_CEO/Assets/`. Scripts need to live under `GameJam_CEO/Assets/` for Unity to compile them.

## Architecture

All game data uses **ScriptableObjects** with direct SO references (no ID system). Everything is in the `CEOGame.Data` namespace.

**Data layer** (`Assets/Scripts/Data/`):
- `Enums.cs` — Team, Position, RelationshipType, RequestCategory, HiddenTrait
- `Structs.cs` — `Relationship` (colleague ref + type), `DecisionOutcome` (budget/morale/people changes, position changes, delayed effects, outcome text)
- `EmployeeData.cs` — ScriptableObject: identity, team/position/salary, happiness (0-100), relationships array, hidden traits
- `RequestData.cs` — ScriptableObject: employee ref, category, dialogue, scheduling (earliestTurn/priority), approve/deny outcomes, prerequisite chains (requiresApproved/requiresDenied)

**Design decisions:**
- SOs are mutated at runtime (acceptable for game jam scope; resets on play mode stop)
- Flat structs over nested SOs so all data is editable in one Inspector window
- Sprite references for portraits (works directly with Unity UI)
- Create assets via: Right-click → Create → CEO Game → Employee Data / Request Data

## Unity Specifics

- Unity version: 6000.3.3f1
- Render pipeline: URP 2D
- No test framework configured yet
- No custom build scripts — use Unity's default build (File → Build Settings)
