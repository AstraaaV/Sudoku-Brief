#pragma once
#include "Grid.h"
#include "VariantRules.h"

class Solver
{
public:
	Solver(const VariantRules* rules) : rules(rules) {}
	bool solve(Grid& grid);
private:
	bool findEmpty(const Grid& grid, int& row, int& col);
	const VariantRules* rules;
};