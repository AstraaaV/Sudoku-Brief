#pragma once
#include "Grid.h"

// This is the basic Sudoku rules, separated from other rules
class Rules
{
public:
	// Checks the row to see if there's duplicate numbers
	static bool rowValid(const Grid& grid, int row, int value);
	
	// Same checks but for the column
	static bool colValid(const Grid& grid, int col, int value);
	
	// Checks the correct 3x3 box
	static bool boxValid(const Grid& grid, int row, int col, int value);

	// Full check aka row + col + box must allow the number
	static bool isValid(const Grid& grid, int row, int col, int value);
};