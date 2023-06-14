# RTS Demo (In progress)

A demonstration of a simple strategy game mechanics.

## Table of contents

- [Unit](#unit)
- [Unit Selection](#unit-selection-system)
- [Unit Movements](#unit-movements)

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

## Unit Movements

Each unit is an instance of UnitMovement class, and each has its own target detector as a proximity GameObject sensor.

### UnitMovement class

1. Update()
	- Constant state check to prevent unwanted unit movements
	- Prevent unit jittering
	- Prevent auto movements when in movement
	- Handles auto movements when targets are enlisted
	- Has lower priority than the manual player movements
2. ManualMovement()
	- Raycast to check what is being clicked
	- If enemy layermask __is__ clicked, update that clicked unit as the current target. Retrieve the position of the first target from targetList as destination
	- If enemy layermask __is not__ clicked, Raycast again in GetPosByRay() using the same ray on the ground layermask, and retrieve the hit point as destination
	- Move towards destination
3. GetPosByTarget()
	- Calculate distance between self and target
	- if calculated distance is within attack range, return the current self position (no movement)
	- if not, set the stoppingDistance of navmeshagent to attack range (take the farthest possible attack distance when attack action is assigned) and return the target position