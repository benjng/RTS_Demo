# RTS Demo (In progress)

A demonstration of a simple strategy game mechanics.

## Table of contents

- [Unit](#unit-)
- [Unit Selection](#unit-selection-system)
- [](#-)

## Unit

All units in game, including player controllable units, buildings, enemy units, are objects of Unit class.

### Unit class

1. GetTarget()
	- Retrieve updated target from TargetsDetector
2. DetectTarget()
	- Check if target is within attack range, or detect range
	- Set the unit to face towards target when in range
3. ShootTarget()
	- Only shoot when target is in attack range
	- When shooting, instantiate a bullet prefab
	- Get the starting and ending position, and move the bullet using Lerp

Deriving class: ArmyUnit, BuildingUnit, EnemyUnit

## Unit Selection System

Unit selection system consist of follow:

### UnitSelection class

1. ClickSelect() / ShiftClickSelect() / DragSelect() / DeselectAll()
	- Handle different selection ways
	- Add/remove the selected unit from the unitsSelected list accordingly
	- Enable/disable a unit's manual movement
2. UpdateControlUI() / ClearControlUI()
	- On unit(s) selected, inform Control UI Renderer for UI updates
3. SwitchMode()
	- Send selected unitSO to modelHandler for state control

### UnitDrag class 

1. Update(): Handles all mouse dragging inputs on 
2. DrawVisual()
	- Create a visible box from mouse click startPosition  to mouse click endPosition
	- Set the box in the middle of startPosition and endPosition
3. DrawSelection()
	- Draw a rect for logical selection in SelectUnits()
4. SelectUnits()
	- On mouse release, check if the units in the registered unitList is within the selectionBox area. If so, send the gameObject to UnitSelection

### UnitClick class

Handles click (non-drag) inputs, including shift click, normal click, and clicks on empty ground by raycasting on selectableUnits LayerMask.