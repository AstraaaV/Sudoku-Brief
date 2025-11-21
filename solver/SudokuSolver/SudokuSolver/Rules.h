#pragma once
#include "Grid.h"

class Rules
{
public:
	static bool rowValid(const Grid& grid, int row, int value);
	static bool colValid(const Grid& grid, int col, int value);
	static bool boxValid(const Grid& grid, int row, int col, int value);

	static bool isValid(const Grid& grid, int row, int col, int value);
};