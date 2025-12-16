#pragma once
#include "Grid.h"
#include "Rules.h"

/*
	This is the ruleset used for any Sudoku rule system
	aka every variant such as Diagonal, Jigsaw, etc will
	inherit from this class and override isValid()

	This is easier when adding new puzzles later on as
	a new class can easily inherit from this class
*/
class VariantRules : public Rules
{
public:
	// Checks if a number is allowed here according to the selected ruleset
	virtual bool isValid(const Grid& grid, int row, int col, int value) const = 0;
	
	// Destructor as polymorphism is used, basically avoids issues
	virtual ~VariantRules() {}
};