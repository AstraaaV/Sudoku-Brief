#pragma once
#include "Grid.h"
#include "VariantRules.h"

/*
	This basically uses the rule system I state, e.g. StandardRules
	and tries to fill the grid using backtracking.

	I am using dependency injection essentially.
*/
class Solver
{
public:
	// Pass in what ruleset is wanting to be used
	Solver(const VariantRules* rules) : rules(rules) {}
	
	// Recursive backtracking
	bool solve(Grid& grid);
private:
	// Finds the next empty cell aka 0
	bool findEmpty(const Grid& grid, int& row, int& col);
	
	// Pointer to what ruleset is used
	const VariantRules* rules;
};