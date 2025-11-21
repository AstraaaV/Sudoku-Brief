#pragma once
#include "VariantRules.h"
#include "Rules.h"

/*
	This is the default Sudoku ruleset

	This ruleset is plugged into the Solver if its a standard Sudoku puzzle,
	otherwise an alternative puzzle ruleset will be plugged in
*/
class StandardRules : public VariantRules
{
public:
	// This just uses the existing ruleset check system
	bool isValid(const Grid& grid, int row, int col, int value) const override
	{
		return Rules::isValid(grid, row, col, value);
	}
};