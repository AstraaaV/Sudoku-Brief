#include "DiagonalRules.h"

bool DiagonalRules::isValid(const Grid& grid, int row, int col, int value) const
{
	// Standard Sudoku checks
	if (!Rules::isValid(grid, row, col, value))
		return false;

	// This checks the main diagonal (top-left to bottom-right)
	if (row == col)
	{
		for (int i = 0; i < 9; i++)
		{
			// If same value already exists here -> nope
			if (grid.get(i, i) == value) return false;
		}
	}

	// This checks the anti-diagonal (top-right to bottom-left)
	if (row + col == 8)
	{
		for (int i = 0; i < 9; i++)
		{
			// Same logic as above, but mirrored
			if (grid.get(i, 8 - i) == value) return false;
		}
	}
	// If all checks are successful, then we're good
	return true;
}
