#include "DiagonalRules.h"

bool DiagonalRules::isValid(const Grid& grid, int row, int col, int value) const
{
	if (!Rules::isValid(grid, row, col, value))
		return false;

	if (row == col)
	{
		for (int i = 0; i < 9; i++)
		{
			if (grid.get(i, i) == value) return false;
		}
	}

	if (row + col == 8)
	{
		for (int i = 0; i < 9; i++)
		{
			if (grid.get(i, 8 - i) == value) return false;
		}
	}
	return true;
}
