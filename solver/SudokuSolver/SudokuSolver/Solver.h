#pragma once
#include "Grid.h"

class Solver
{
public:
	bool solve(Grid& grid);
private:
	bool findEmpty(const Grid& grid, int& row, int& col);
};