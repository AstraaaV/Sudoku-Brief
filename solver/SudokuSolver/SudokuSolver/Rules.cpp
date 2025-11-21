#include "Rules.h"

bool Rules::rowValid(const Grid& grid, int row, int value)
{
    for (int col = 0; col < 9; col++)
    {
        if (grid.get(row, col) == value)
            return false;
    }
    return true;
}

bool Rules::colValid(const Grid& grid, int col, int value)
{
    for (int row = 0; row < 9; row++)
    {
        if (grid.get(row, col) == value)
            return false;
    }
    return true;
}

bool Rules::boxValid(const Grid& grid, int row, int col, int value)
{
    int startRow = row - (row % 3);
    int startCol = col - (col % 3);

    for (int r = 0; r < 3; r++)
    {
        for (int c = 0; c < 3; c++)
        {
            if (grid.get(startRow + r, startCol + c) == value)
                return false;
        }
    }
    return true;
}

bool Rules::isValid(const Grid& grid, int row, int col, int value)
{
    return rowValid(grid, row, value) &&
        colValid(grid, col, value) &&
        boxValid(grid, row, col, value);
}
